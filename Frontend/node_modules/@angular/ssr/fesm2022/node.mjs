import { renderApplication, renderModule, ɵSERVER_CONTEXT as _SERVER_CONTEXT } from '@angular/platform-server';
import * as fs from 'node:fs';
import { dirname, join, normalize, resolve } from 'node:path';
import { URL as URL$1, fileURLToPath } from 'node:url';
import { ɵInlineCriticalCssProcessor as _InlineCriticalCssProcessor, AngularAppEngine } from '@angular/ssr';
import { readFile } from 'node:fs/promises';
import { argv } from 'node:process';

function attachNodeGlobalErrorHandlers() {
  if (typeof Zone !== 'undefined') {
    return;
  }
  const gThis = globalThis;
  if (gThis.ngAttachNodeGlobalErrorHandlersCalled) {
    return;
  }
  gThis.ngAttachNodeGlobalErrorHandlersCalled = true;
  process.on('unhandledRejection', error => console.error('unhandledRejection', error)).on('uncaughtException', error => console.error('uncaughtException', error));
}

class CommonEngineInlineCriticalCssProcessor {
  resourceCache = new Map();
  async process(html, outputPath) {
    const beasties = new _InlineCriticalCssProcessor(async path => {
      let resourceContent = this.resourceCache.get(path);
      if (resourceContent === undefined) {
        resourceContent = await readFile(path, 'utf-8');
        this.resourceCache.set(path, resourceContent);
      }
      return resourceContent;
    }, outputPath);
    return beasties.process(html);
  }
}

const PERFORMANCE_MARK_PREFIX = '🅰️';
function printPerformanceLogs() {
  let maxWordLength = 0;
  const benchmarks = [];
  for (const {
    name,
    duration
  } of performance.getEntriesByType('measure')) {
    if (!name.startsWith(PERFORMANCE_MARK_PREFIX)) {
      continue;
    }
    const step = name.slice(PERFORMANCE_MARK_PREFIX.length + 1) + ':';
    if (step.length > maxWordLength) {
      maxWordLength = step.length;
    }
    benchmarks.push([step, `${duration.toFixed(1)}ms`]);
    performance.clearMeasures(name);
  }
  console.log('********** Performance results **********');
  for (const [step, value] of benchmarks) {
    const spaces = maxWordLength - step.length + 5;
    console.log(step + ' '.repeat(spaces) + value);
  }
  console.log('*****************************************');
}
async function runMethodAndMeasurePerf(label, asyncMethod) {
  const labelName = `${PERFORMANCE_MARK_PREFIX}:${label}`;
  const startLabel = `start:${labelName}`;
  const endLabel = `end:${labelName}`;
  try {
    performance.mark(startLabel);
    return await asyncMethod();
  } finally {
    performance.mark(endLabel);
    performance.measure(labelName, startLabel, endLabel);
    performance.clearMarks(startLabel);
    performance.clearMarks(endLabel);
  }
}
function noopRunMethodAndMeasurePerf(label, asyncMethod) {
  return asyncMethod();
}

const SSG_MARKER_REGEXP = /ng-server-context=["']\w*\|?ssg\|?\w*["']/;
class CommonEngine {
  options;
  templateCache = new Map();
  inlineCriticalCssProcessor = new CommonEngineInlineCriticalCssProcessor();
  pageIsSSG = new Map();
  constructor(options) {
    this.options = options;
    attachNodeGlobalErrorHandlers();
  }
  async render(opts) {
    const enablePerformanceProfiler = this.options?.enablePerformanceProfiler;
    const runMethod = enablePerformanceProfiler ? runMethodAndMeasurePerf : noopRunMethodAndMeasurePerf;
    let html = await runMethod('Retrieve SSG Page', () => this.retrieveSSGPage(opts));
    if (html === undefined) {
      html = await runMethod('Render Page', () => this.renderApplication(opts));
      if (opts.inlineCriticalCss !== false) {
        const content = await runMethod('Inline Critical CSS', () => this.inlineCriticalCss(html, opts));
        html = content;
      }
    }
    if (enablePerformanceProfiler) {
      printPerformanceLogs();
    }
    return html;
  }
  inlineCriticalCss(html, opts) {
    const outputPath = opts.publicPath ?? (opts.documentFilePath ? dirname(opts.documentFilePath) : '');
    return this.inlineCriticalCssProcessor.process(html, outputPath);
  }
  async retrieveSSGPage(opts) {
    const {
      publicPath,
      documentFilePath,
      url
    } = opts;
    if (!publicPath || !documentFilePath || url === undefined) {
      return undefined;
    }
    const {
      pathname
    } = new URL$1(url, 'resolve://');
    const pagePath = join(publicPath, pathname, 'index.html');
    if (this.pageIsSSG.get(pagePath)) {
      return fs.promises.readFile(pagePath, 'utf-8');
    }
    if (!pagePath.startsWith(normalize(publicPath))) {
      return undefined;
    }
    if (pagePath === resolve(documentFilePath) || !(await exists(pagePath))) {
      this.pageIsSSG.set(pagePath, false);
      return undefined;
    }
    const content = await fs.promises.readFile(pagePath, 'utf-8');
    const isSSG = SSG_MARKER_REGEXP.test(content);
    this.pageIsSSG.set(pagePath, isSSG);
    return isSSG ? content : undefined;
  }
  async renderApplication(opts) {
    const moduleOrFactory = this.options?.bootstrap ?? opts.bootstrap;
    if (!moduleOrFactory) {
      throw new Error('A module or bootstrap option must be provided.');
    }
    const extraProviders = [{
      provide: _SERVER_CONTEXT,
      useValue: 'ssr'
    }, ...(opts.providers ?? []), ...(this.options?.providers ?? [])];
    let document = opts.document;
    if (!document && opts.documentFilePath) {
      document = await this.getDocument(opts.documentFilePath);
    }
    const commonRenderingOptions = {
      url: opts.url,
      document
    };
    return isBootstrapFn(moduleOrFactory) ? renderApplication(moduleOrFactory, {
      platformProviders: extraProviders,
      ...commonRenderingOptions
    }) : renderModule(moduleOrFactory, {
      extraProviders,
      ...commonRenderingOptions
    });
  }
  async getDocument(filePath) {
    let doc = this.templateCache.get(filePath);
    if (!doc) {
      doc = await fs.promises.readFile(filePath, 'utf-8');
      this.templateCache.set(filePath, doc);
    }
    return doc;
  }
}
async function exists(path) {
  try {
    await fs.promises.access(path, fs.constants.F_OK);
    return true;
  } catch {
    return false;
  }
}
function isBootstrapFn(value) {
  return typeof value === 'function' && !('ɵmod' in value);
}

const HTTP2_PSEUDO_HEADERS = new Set([':method', ':scheme', ':authority', ':path', ':status']);
function createWebRequestFromNodeRequest(nodeRequest) {
  const {
    headers,
    method = 'GET'
  } = nodeRequest;
  const withBody = method !== 'GET' && method !== 'HEAD';
  const referrer = headers.referer && URL.canParse(headers.referer) ? headers.referer : undefined;
  return new Request(createRequestUrl(nodeRequest), {
    method,
    headers: createRequestHeaders(headers),
    body: withBody ? nodeRequest : undefined,
    duplex: withBody ? 'half' : undefined,
    referrer
  });
}
function createRequestHeaders(nodeHeaders) {
  const headers = new Headers();
  for (const [name, value] of Object.entries(nodeHeaders)) {
    if (HTTP2_PSEUDO_HEADERS.has(name)) {
      continue;
    }
    if (typeof value === 'string') {
      headers.append(name, value);
    } else if (Array.isArray(value)) {
      for (const item of value) {
        headers.append(name, item);
      }
    }
  }
  return headers;
}
function createRequestUrl(nodeRequest) {
  const {
    headers,
    socket,
    url = '',
    originalUrl
  } = nodeRequest;
  const protocol = getFirstHeaderValue(headers['x-forwarded-proto']) ?? ('encrypted' in socket && socket.encrypted ? 'https' : 'http');
  const hostname = getFirstHeaderValue(headers['x-forwarded-host']) ?? headers.host ?? headers[':authority'];
  if (Array.isArray(hostname)) {
    throw new Error('host value cannot be an array.');
  }
  let hostnameWithPort = hostname;
  if (!hostname?.includes(':')) {
    const port = getFirstHeaderValue(headers['x-forwarded-port']);
    if (port) {
      hostnameWithPort += `:${port}`;
    }
  }
  return new URL(`${protocol}://${hostnameWithPort}${originalUrl ?? url}`);
}
function getFirstHeaderValue(value) {
  return value?.toString().split(',', 1)[0]?.trim();
}

class AngularNodeAppEngine {
  angularAppEngine = new AngularAppEngine();
  constructor() {
    attachNodeGlobalErrorHandlers();
  }
  async handle(request, requestContext) {
    const webRequest = createWebRequestFromNodeRequest(request);
    return this.angularAppEngine.handle(webRequest, requestContext);
  }
}

function createNodeRequestHandler(handler) {
  handler['__ng_node_request_handler__'] = true;
  return handler;
}

async function writeResponseToNodeResponse(source, destination) {
  const {
    status,
    headers,
    body
  } = source;
  destination.statusCode = status;
  let cookieHeaderSet = false;
  for (const [name, value] of headers.entries()) {
    if (name === 'set-cookie') {
      if (cookieHeaderSet) {
        continue;
      }
      destination.setHeader(name, headers.getSetCookie());
      cookieHeaderSet = true;
    } else {
      destination.setHeader(name, value);
    }
  }
  if ('flushHeaders' in destination) {
    destination.flushHeaders();
  }
  if (!body) {
    destination.end();
    return;
  }
  try {
    const reader = body.getReader();
    destination.on('close', () => {
      reader.cancel().catch(error => {
        console.error(`An error occurred while writing the response body for: ${destination.req.url}.`, error);
      });
    });
    while (true) {
      const {
        done,
        value
      } = await reader.read();
      if (done) {
        destination.end();
        break;
      }
      const canContinue = destination.write(value);
      if (canContinue === false) {
        await new Promise(resolve => destination.once('drain', resolve));
      }
    }
  } catch {
    destination.end('Internal server error.');
  }
}

function isMainModule(url) {
  return url.startsWith('file:') && argv[1] === fileURLToPath(url);
}

export { AngularNodeAppEngine, CommonEngine, createNodeRequestHandler, createWebRequestFromNodeRequest, isMainModule, writeResponseToNodeResponse };
//# sourceMappingURL=node.mjs.map
