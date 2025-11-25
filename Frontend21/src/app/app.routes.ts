import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { authGuard } from './core/auth.guard';
import { Component } from '@angular/core';

@Component({
  standalone: true,
  template: `
    <div style="padding: 24px">
      <h2>Bienvenido al panel</h2>
      <p>Aquí luego irá el Kanban / Lista de tareas.</p>
    </div>
  `
})
export class DashboardPlaceholderComponent {}

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: DashboardPlaceholderComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
