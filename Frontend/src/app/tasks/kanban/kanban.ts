import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

interface Task {
  Id: number;
  Title: string;
  Description: string;
  ColumnId: number;
  ColumnName: string;
  Priority: number;
  DueDate: string | null;
  Order: number;
}

interface Column {
  Id: number;
  Name: string;
  Order: number;
  Tasks: Task[];
}

@Component({
  selector: 'app-kanban',
  standalone: true,
  imports: [CommonModule, FormsModule, DragDropModule],
  templateUrl: './kanban.html',
  styleUrls: ['./kanban.css']
})
export class KanbanComponent implements OnInit {
  private http = inject(HttpClient);
  private readonly API_URL = 'http://taskflow-backend.grupolimon.online/api';

  columns: Column[] = [];
  connectedDropIds: string[] = [];

  // Propiedades para creación rápida de tareas
  showQuickAdd = false;
  quickTitle = '';
  quickColumnId: number | null = null;

  ngOnInit() {
    this.loadColumns();
  }

  loadColumns() {
    this.http.get<Column[]>(`${this.API_URL}/Columns`).subscribe({
      next: (data) => {
        this.columns = data;
        this.connectedDropIds = this.columns.map(col => `column-${col.Id}`);

        // Establecer la primera columna como default
        if (this.columns.length > 0 && !this.quickColumnId) {
          this.quickColumnId = this.columns[0].Id;
        }
      },
      error: (err) => console.error('Error cargando columnas:', err)
    });
  }

  quickCreateTask() {
    if (!this.quickTitle.trim()) {
      alert('El título es obligatorio');
      return;
    }

    if (!this.quickColumnId) {
      alert('Debes seleccionar una columna');
      return;
    }

    const newTask = {
      Title: this.quickTitle,
      Description: '',
      ColumnId: this.quickColumnId,
      Priority: 1,
      DueDate: null
    };

    this.http.post<Task>(`${this.API_URL}/Tasks`, newTask).subscribe({
      next: (task) => {
        // Agregar la tarea a la columna correspondiente
        const column = this.columns.find(col => col.Id === this.quickColumnId);
        if (column) {
          column.Tasks.push(task);
        }

        // Limpiar formulario
        this.quickTitle = '';
        this.showQuickAdd = false;
      },
      error: (err) => {
        console.error('Error creando tarea:', err);
        alert('Error al crear la tarea');
      }
    });
  }

  drop(event: CdkDragDrop<Task[]>, targetColumn: Column) {
    const task = event.item.data || event.previousContainer.data[event.previousIndex];

    if (event.previousContainer === event.container) {
      // Mover dentro de la misma columna
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      this.updateTaskOrder(task, targetColumn.Id, event.currentIndex);
    } else {
      // Mover entre columnas
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      this.updateTaskOrder(task, targetColumn.Id, event.currentIndex);
    }
  }

  updateTaskOrder(task: Task, newColumnId: number, newOrder: number) {
    this.http.put(`${this.API_URL}/Tasks/${task.Id}/move`, {
      ColumnId: newColumnId,
      NewOrder: newOrder
    }).subscribe({
      next: () => console.log('Tarea movida exitosamente'),
      error: (err) => {
        console.error('Error moviendo tarea:', err);
        // Recargar columnas en caso de error
        this.loadColumns();
      }
    });
  }
}
