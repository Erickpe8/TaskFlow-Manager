import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TasksService } from '../tasks.service';
import { TaskDto, TaskFilter, PaginatedResult, CreateTaskDto, UpdateTaskDto, ColumnDto } from '../task';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class ListComponent implements OnInit {
  tasks: TaskDto[] = [];
  columns: ColumnDto[] = [];
  loading = true;

  // Paginación
  totalCount = 0;
  totalPages = 0;
  currentPage = 1;
  pageSize = 10;
  pageSizes = [5, 10, 20, 50];

  // Filtros
  filter: TaskFilter = {
    search: '',
    columnId: undefined,
    priority: undefined,
    sortBy: 'title',
    sortDesc: false,
    page: 1,
    pageSize: 10
  };

  // Modales
  showCreateModal = false;
  showEditModal = false;
  selectedTask: TaskDto | null = null;

  // Formularios
  taskForm: CreateTaskDto = {
    Title: '',
    Description: '',
    ColumnId: 0,
    Priority: undefined,
    DueDate: undefined
  };

  editForm: UpdateTaskDto = {
    Title: '',
    Description: '',
    ColumnId: 0,
    Priority: undefined,
    DueDate: undefined,
    Order: 0
  };

  constructor(private tasksService: TasksService) { }

  ngOnInit(): void {
    this.loadColumns();
    this.loadTasks();
  }

  loadColumns() {
    this.tasksService.getBoard().subscribe({
      next: (cols) => {
        this.columns = cols;
      },
      error: (err) => console.error('Error cargando columnas', err)
    });
  }

  loadTasks() {
    this.loading = true;
    this.filter.page = this.currentPage;
    this.filter.pageSize = this.pageSize;

    this.tasksService.getFiltered(this.filter).subscribe({
      next: (result: PaginatedResult<TaskDto>) => {
        this.tasks = result.Items;
        this.totalCount = result.TotalCount;
        this.totalPages = result.TotalPages;
        this.currentPage = result.Page;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error cargando tareas', err);
        this.loading = false;
      }
    });
  }

  // BÚSQUEDA Y FILTROS
  onSearchChange() {
    this.currentPage = 1;
    this.loadTasks();
  }

  onFilterChange() {
    this.currentPage = 1;
    this.loadTasks();
  }

  clearFilters() {
    this.filter = {
      search: '',
      columnId: undefined,
      priority: undefined,
      sortBy: 'title',
      sortDesc: false,
      page: 1,
      pageSize: this.pageSize
    };
    this.currentPage = 1;
    this.loadTasks();
  }

  // ORDENAMIENTO
  sort(column: string) {
    if (this.filter.sortBy === column) {
      this.filter.sortDesc = !this.filter.sortDesc;
    } else {
      this.filter.sortBy = column;
      this.filter.sortDesc = false;
    }
    this.loadTasks();
  }

  getSortIcon(column: string): string {
    if (this.filter.sortBy !== column) return '⇅';
    return this.filter.sortDesc ? '↓' : '↑';
  }

  // PAGINACIÓN
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadTasks();
    }
  }

  onPageSizeChange() {
    this.currentPage = 1;
    this.loadTasks();
  }

  get pages(): number[] {
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxVisible / 2));
    let end = Math.min(this.totalPages, start + maxVisible - 1);

    if (end - start < maxVisible - 1) {
      start = Math.max(1, end - maxVisible + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  // CREAR TAREA
  openCreateModal() {
    this.taskForm = {
      Title: '',
      Description: '',
      ColumnId: this.columns[0]?.Id || 0,
      Priority: undefined,
      DueDate: undefined
    };
    this.showCreateModal = true;
  }

  closeCreateModal() {
    this.showCreateModal = false;
  }

  createTask() {
    if (!this.taskForm.Title.trim()) {
      alert('El título es obligatorio');
      return;
    }

    this.tasksService.createTask(this.taskForm).subscribe({
      next: () => {
        this.closeCreateModal();
        this.loadTasks();
      },
      error: (err) => {
        console.error('Error creando tarea', err);
        alert('Error al crear la tarea');
      }
    });
  }

  // EDITAR TAREA
  openEditModal(task: TaskDto) {
    this.selectedTask = task;
    this.editForm = {
      Title: task.Title,
      Description: task.Description || '',
      ColumnId: task.ColumnId,
      Priority: task.Priority,
      DueDate: task.DueDate,
      Order: task.Order
    };
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
    this.selectedTask = null;
  }

  updateTask() {
    if (!this.selectedTask || !this.editForm.Title.trim()) {
      alert('El título es obligatorio');
      return;
    }

    this.tasksService.updateTask(this.selectedTask.Id, this.editForm).subscribe({
      next: () => {
        this.closeEditModal();
        this.loadTasks();
      },
      error: (err) => {
        console.error('Error actualizando tarea', err);
        alert('Error al actualizar la tarea');
      }
    });
  }

  // ELIMINAR
  deleteTask(task: TaskDto) {
    if (!confirm(`¿Eliminar la tarea "${task.Title}"?`)) return;

    this.tasksService.deleteTask(task.Id).subscribe({
      next: () => {
        this.loadTasks();
      },
      error: (err) => {
        console.error('Error eliminando tarea', err);
        alert('Error al eliminar la tarea');
      }
    });
  }

  // UTILIDADES
  formatDate(date?: string): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('es-ES');
  }

  getPriorityLabel(priority?: number): string {
    if (priority === undefined || priority === null) return '-';
    const labels: { [key: number]: string } = {
      1: 'Baja',
      2: 'Media',
      3: 'Alta',
      4: 'Urgente'
    };
    return labels[priority] || priority.toString();
  }

  getPriorityClass(priority?: number): string {
    if (priority === undefined || priority === null) return '';
    const classes: { [key: number]: string } = {
      1: 'priority-low',
      2: 'priority-medium',
      3: 'priority-high',
      4: 'priority-urgent'
    };
    return classes[priority] || '';
  }
}
