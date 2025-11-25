import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColumnDto, TaskDto } from '../task';
import { TasksService } from '../tasks.service';
import {
  CdkDropList,
  CdkDropListGroup,
  CdkDrag,
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-kanban',
  standalone: true,
  imports: [CommonModule, CdkDropListGroup, CdkDropList, CdkDrag],
  templateUrl: './kanban.html',
  styleUrls: ['./kanban.css']
})
export class KanbanComponent implements OnInit, OnDestroy {

  private tasksService = inject(TasksService);

  columns: ColumnDto[] = [];
  private refreshTimer?: any;
  private readonly REFRESH_MS = 5000;

  ngOnInit(): void {
    this.loadBoard();

    // refresco continuo
    this.refreshTimer = setInterval(() => {
      this.loadBoard();
    }, this.REFRESH_MS);
  }

  ngOnDestroy(): void {
    if (this.refreshTimer) {
      clearInterval(this.refreshTimer);
    }
  }

  get connectedDropIds(): string[] {
    return this.columns.map(c => `column-${c.Id}`);
  }

  loadBoard(): void {
    this.tasksService.getBoard().subscribe({
      next: cols => {
        this.columns = cols
          .sort((a, b) => a.Order - b.Order)
          .map(col => ({
            ...col,
            Tasks: [...col.Tasks].sort((t1, t2) => t1.Order - t2.Order)
          }));
      },
      error: err => console.error('Error cargando tablero', err)
    });
  }

  drop(event: CdkDragDrop<TaskDto[]>, targetColumn: ColumnDto): void {
    const previousColumn = this.columns.find(
      c => `column-${c.Id}` === (event.previousContainer.id)
    );
    const currentColumn = targetColumn;

    if (!previousColumn || !currentColumn) return;

    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }

    const movedTask = event.container.data[event.currentIndex];
    const newOrder = event.currentIndex + 1;
    const newColumnId = currentColumn.Id;

    this.tasksService.moveTask(movedTask.Id, newColumnId, newOrder)
      .subscribe({
        next: () => {
          this.loadBoard();
        },
        error: err => {
          console.error('Error moviendo tarea', err);
        }
      });
  }
}
