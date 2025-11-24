import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Task } from '../models/task.model';
import { TaskStateService } from './state.service';

@Injectable({ providedIn: 'root' })
export class TaskService {

  private apiUrl = 'http://localhost:5208/api/tasks';

  constructor(
    private http: HttpClient,
    private state: TaskStateService
  ) {}

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl).pipe(
      tap(tasks => this.state.setTasks(tasks))
    );
  }

  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  createTask(task: Partial<Task>) {
    return this.http.post<Task>(this.apiUrl, task).pipe(
      tap(newTask => this.state.addTask(newTask))
    );
  }

  updateTask(id: number, task: Partial<Task>) {
    return this.http.put<Task>(`${this.apiUrl}/${id}`, task).pipe(
      tap(updated => this.state.updateTask(updated))
    );
  }

  deleteTask(id: number) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      tap(() => this.state.removeTask(id))
    );
  }

  moveTask(id: number, data: { columnId: number; newOrder: number }) {
    return this.http.put<void>(`${this.apiUrl}/${id}/move`, data).pipe(
      tap(() => {
        this.getTasks().subscribe();
      })
    );
  }
}
