import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { App } from './app';

@NgModule({
  imports: [
    ServerModule
  ],
  bootstrap: [App]
})
export class AppServerModule {}
