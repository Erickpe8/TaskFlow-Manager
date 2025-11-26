
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "redirectTo": "/login",
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/login"
  },
  {
    "renderMode": 2,
    "route": "/register"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-XQSZOJHY.js"
    ],
    "redirectTo": "/dashboard/kanban",
    "route": "/dashboard"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-XQSZOJHY.js"
    ],
    "route": "/dashboard/kanban"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-XQSZOJHY.js"
    ],
    "route": "/dashboard/list"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 4772, hash: 'fed51a24e6336280fe2a876984bd0b28fca2d258e50c7abc465082b854454392', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1106, hash: 'ad6b7954466b95c4d9ad37e41141acc9ef945c6dd64bf7f84b11082c3cef5980', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'register/index.html': {size: 15866, hash: '03d391b00fe360f11b4842af596da1fa189bdbfe6cc239a36522dd25612ab601', text: () => import('./assets-chunks/register_index_html.mjs').then(m => m.default)},
    'login/index.html': {size: 14586, hash: 'e3fe7a824cfc83a15477a80fb1b4317073a5327cc8af102df3295174582434dd', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'dashboard/list/index.html': {size: 31165, hash: '22e4ed9da5e310cfeda2d98274d6665c5b593904fe2c404fcb389d1fab2acef0', text: () => import('./assets-chunks/dashboard_list_index_html.mjs').then(m => m.default)},
    'dashboard/kanban/index.html': {size: 28634, hash: 'e31a268a0f6dad718dda1b09d07113ee311e1ffef4aaef4d55581a669ccfa41f', text: () => import('./assets-chunks/dashboard_kanban_index_html.mjs').then(m => m.default)},
    'styles-NMQ2VH7M.css': {size: 9288, hash: 'Pmgko2YfdS0', text: () => import('./assets-chunks/styles-NMQ2VH7M_css.mjs').then(m => m.default)}
  },
};
