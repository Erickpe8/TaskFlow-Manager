import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskService } from '../services/task.service';
import { ColumnService } from '../services/column.service';
import { Task } from '../models/task.model';
import { Column } from '../models/column.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class TaskListComponent implements OnInit {

  tasks: Task[] = [];
  filteredTasks: Task[] = [];
  columns: Column[] = [];

  loading = false;
  errorMessage = '';

  constructor(
    private taskService: TaskService,
    private columnService: ColumnService
  ) {}

  ngOnInit(): void {
    this.loadColumns();
    this.loadTasks();
  }

  private loadColumns(): void {
    this.columnService.getColumns().subscribe({
      next: (cols) => {
        this.columns = cols;
      },
      error: (err: any) => {
        console.error('Error cargando columnas', err);
      }
    });
  }

  private loadTasks(): void {
    this.loading = true;
    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.filteredTasks = tasks;
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Error cargando tareas', err);
        this.errorMessage = 'No se pudieron cargar las tareas.';
        this.loading = false;
      }
    });
  }

  // Filtro simple por título / descripción
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

    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.tasks = this.tasks.filter(t => t.id !== id);
        this.filteredTasks = this.filteredTasks.filter(t => t.id !== id);
      },
      error: (err: any) => {
        console.error('Error al eliminar la tarea', err);
        alert('No se pudo eliminar la tarea.');
      }
    });
  }
}
