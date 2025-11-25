import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

import { ColumnService } from '../services/column.service';
import { TaskService } from '../services/task.service';

import { Column } from '../models/column.model';
import { Task } from '../models/task.model';

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
  ) {}

  ngOnInit(): void {
    this.loadColumns();
  }

  loadColumns() {
    this.columnService.getColumns().subscribe(cols => {
      this.columns = cols;
    });
  }

  onDrop(event: CdkDragDrop<Task[]>, column: Column) {

    const previousList = event.previousContainer.data;
    const currentList = event.container.data;

    const task = event.item.data;

    if (event.previousContainer === event.container) {
      moveItemInArray(currentList, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(previousList, currentList, event.previousIndex, event.currentIndex);
      task.columnId = column.id;
    }

    // Reordenar tasks
    column.tasks.forEach((t, i) => t.order = i + 1);

    // Guardar en backend
    this.taskService.moveTask(task.id, {
      columnId: task.columnId,
      newOrder: task.order
    }).subscribe(() => this.loadColumns());
  }
}
