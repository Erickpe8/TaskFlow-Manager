import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DragDropModule, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';

import { ColumnService } from '../services/column.service';
import { TaskService } from '../services/task.service';
import { TaskStateService } from '../services/state.service';

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
  connectedLists: string[] = [];

  constructor(
    private colService: ColumnService,
    private taskService: TaskService,
    private taskState: TaskStateService
  ) { }

  ngOnInit(): void {

    this.colService.getColumns().subscribe(cols => {
      this.columns = cols;

      this.connectedLists = cols.map(c => 'col-' + c.id);
    });

    this.taskState.tasks$.subscribe(tasks => {

      this.columns.forEach(col => {
        col.tasks = tasks.filter(t => t.columnId === col.id);
      });
    });

    this.taskService.getTasks().subscribe();
  }

  onDrop(event: CdkDragDrop<any[]>, targetColumn: Column) {

    let movedTask = event.item.data;

    if (event.previousContainer === event.container) {
      moveItemInArray(targetColumn.tasks, event.previousIndex, event.currentIndex);
    } else {
      const prevColumn = this.columns.find(c => c.tasks === event.previousContainer.data)!;

      transferArrayItem(
        prevColumn.tasks,
        targetColumn.tasks,
        event.previousIndex,
        event.currentIndex
      );

      movedTask.columnId = targetColumn.id;
    }

    targetColumn.tasks.forEach((t, i) => t.order = i + 1);

    this.taskService.moveTask(movedTask.id, {
      columnId: movedTask.columnId,
      newOrder: movedTask.order
    }).subscribe();
  }
}
