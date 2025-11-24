import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DragDropModule, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

import { ColumnService } from '../services/column.service';
import { Column } from '../models/column.model';

import { TaskService } from '../services/task.service';

@Component({
  selector: 'app-kanban',
  standalone: true,
  imports: [CommonModule, DragDropModule],
  templateUrl: './kanban.html',
  styleUrls: ['./kanban.css']
})
export class KanbanComponent implements OnInit {

  columns: Column[] = [];

  constructor(
    private columnService: ColumnService,
    private taskService: TaskService
  ) { }

  connectedLists: string[] = [];

  ngOnInit(): void {
    this.columnService.getColumns().subscribe(cols => {
      this.columns = cols;
      this.connectedLists = cols.map(c => 'col-' + c.id); // ← SOLUCIÓN
      console.log("Columnas cargadas:", cols);
    });
  }

  onDrop(event: CdkDragDrop<any[]>, targetColumn: Column) {

    let movedTask = event.item.data;

    // 1. Si está en la misma columna → solo reordenar
    if (event.previousContainer === event.container) {
      moveItemInArray(
        targetColumn.tasks,
        event.previousIndex,
        event.currentIndex
      );
    }
    else {
      // 2. Si va a otra columna → transferir
      const prevColumn = this.columns.find(c => c.tasks === event.previousContainer.data)!;

      transferArrayItem(
        prevColumn.tasks,
        targetColumn.tasks,
        event.previousIndex,
        event.currentIndex
      );

      // 3. Cambiar la columna del task en memoria
      movedTask.columnId = targetColumn.id;
    }

    // 4. Recalcular los ordenes (order)
    targetColumn.tasks.forEach((task, index) => {
      task.order = index + 1;
    });

    // 5. Enviar la actualización al backend
    this.taskService.moveTask(movedTask.id, {
      columnId: movedTask.columnId,
      newOrder: movedTask.order
    }).subscribe({
      next: () => console.log("Movimiento guardado en BD"),
      error: err => console.error("Error al mover la tarea", err)
    });
  }
}
