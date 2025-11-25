import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { TaskService } from '../services/task.service';
import { ColumnService } from '../services/column.service';

@Component({
  standalone: true,
  selector: 'app-task-form',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {

  task: any = {};
  columns: any[] = [];
  isEdit = false;
  id: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tasks: TaskService,
    private cols: ColumnService
  ) {}

  ngOnInit(): void {
    this.cols.getColumns().subscribe(c => this.columns = c);

    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.isEdit = !!this.id;

    if (this.isEdit) {
      this.tasks.getTaskById(this.id!).subscribe(t => this.task = t);
    }
  }

  save() {
    if (this.isEdit) {
      this.tasks.updateTask(this.id!, this.task).subscribe(() => {
        this.router.navigate(['/list']);
      });
    } else {
      this.tasks.createTask(this.task).subscribe(() => {
        this.router.navigate(['/list']);
      });
    }
  }
}
