import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login';
import { KanbanComponent } from './tasks/kanban/kanban';
import { TaskListComponent } from './tasks/list/list';
import { TaskFormComponent } from './tasks/task-form/task-form';

import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },

  { path: 'kanban', component: KanbanComponent, canActivate: [authGuard] },
  { path: 'list', component: TaskListComponent, canActivate: [authGuard] },

  { path: 'tasks/create', component: TaskFormComponent, canActivate: [authGuard] },
  { path: 'tasks/edit/:id', component: TaskFormComponent, canActivate: [authGuard] },

  { path: '', redirectTo: 'kanban', pathMatch: 'full' },
  { path: '**', redirectTo: 'kanban' }
];
