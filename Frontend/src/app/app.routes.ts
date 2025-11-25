import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { authGuard } from './core/auth';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },

  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./dashboard/dashboard').then(m => m.DashboardComponent),
    children: [
      {
        path: 'kanban',
        loadComponent: () =>
          import('./tasks/kanban/kanban').then(m => m.KanbanComponent),
      },
      {
        path: 'list',
        loadComponent: () =>
          import('./tasks/list/list').then(m => m.ListComponent),
      }
    ]
  }
];
