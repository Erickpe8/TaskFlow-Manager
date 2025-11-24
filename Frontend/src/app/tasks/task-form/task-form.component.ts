import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../services/task.service';
import { ColumnService } from '../services/column.service';
import { Task } from '../models/task.model';
import { Column } from '../models/column.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {

  isEdit = false;
  taskId!: number;

  task: Partial<Task> = {
    title: '',
    description: '',
    columnId: 1,
    priority: 1,
    dueDate: null,
  };

  columns: Column[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private columnService: ColumnService
  ) { }

  ngOnInit(): void {
    this.loadColumns();

    this.taskId = Number(this.route.snapshot.paramMap.get('id'));
    this.isEdit = !!this.taskId;

    if (this.isEdit) {
      this.taskService.getTaskById(this.taskId).subscribe(t => {
        this.task = t;
      });
    }
  }



  loadColumns(): void {
    this.columnService.getColumns().subscribe(cols => this.columns = cols);
  }

  save(): void {
    if (!this.task.title || this.task.title.trim().length === 0) {
      alert("El título es obligatorio");
      return;
    }
    if (this.isEdit) {
      this.taskService.updateTask(this.taskId, this.task).subscribe(() => {
        this.router.navigate(['/list']);
      });
    } else {
      this.taskService.createTask(this.task).subscribe(() => {
        this.router.navigate(['/list']);
      });
    }
  }
}
