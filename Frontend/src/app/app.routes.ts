import { Routes } from '@angular/router';
import { KanbanComponent } from './tasks/kanban/kanban';
import { TaskListComponent } from './tasks/list/list';
import { TaskFormComponent } from './tasks/task-form/task-form.component';

export const routes: Routes = [
  { path: 'kanban', component: KanbanComponent },
  { path: 'list', component: TaskListComponent },
  { path: 'tasks/create', component: TaskFormComponent },
  { path: 'tasks/edit/:id', component: TaskFormComponent },
  { path: '', redirectTo: 'kanban', pathMatch: 'full' }
];
