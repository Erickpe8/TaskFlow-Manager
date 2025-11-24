import { Component } from '@angular/core';
import { KanbanComponent } from './tasks/kanban/kanban';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [KanbanComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {}
