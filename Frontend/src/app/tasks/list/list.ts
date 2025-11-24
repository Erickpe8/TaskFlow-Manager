import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { TaskService } from '../services/task.service';
import { ColumnService } from '../services/column.service';
import { TaskStateService } from '../services/state.service';

import { Task } from '../models/task.model';
import { Column } from '../models/column.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class TaskListComponent implements OnInit {

  tasks: Task[] = [];
  filteredTasks: Task[] = [];
  columns: Column[] = [];

  constructor(
    private taskService: TaskService,
    private columnService: ColumnService,
    private taskState: TaskStateService
  ) { }

  ngOnInit(): void {

    this.columnService.getColumns().subscribe(cols => {
      this.columns = cols;
    });

    this.taskState.tasks$.subscribe(tasks => {
      this.tasks = tasks;
      this.filteredTasks = [...tasks];
    });

    this.taskService.getTasks().subscribe();
  }

  applyFilter(term: string): void {
    const value = term.toLowerCase().trim();

    if (!value) {
      this.filteredTasks = [...this.tasks];
      return;
    }

    this.filteredTasks = this.tasks.filter(t =>
      (t.title?.toLowerCase().includes(value)) ||
      (t.description?.toLowerCase().includes(value))
    );
  }

  getColumnName(columnId: number): string {
    const col = this.columns.find(c => c.id === columnId);
    return col ? col.name : '—';
  }

  deleteTask(id: number): void {
    const ok = confirm('¿Seguro que deseas eliminar esta tarea?');
    if (!ok) return;

    this.taskService.deleteTask(id).subscribe();
  }
}
