import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { TaskService } from '../services/task.service';
import { ColumnService } from '../services/column.service';

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
  filtered: Task[] = [];
  columns: Column[] = [];

  constructor(
    private taskService: TaskService,
    private columnService: ColumnService
  ) {}

  ngOnInit(): void {
    this.columnService.getColumns().subscribe(c => this.columns = c);
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe(t => {
      this.tasks = t;
      this.filtered = [...t];
    });
  }

  getColumnName(id: number) {
    return this.columns.find(c => c.id === id)?.name ?? '—';
  }
}
