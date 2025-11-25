import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {

  private apiUrl = 'http://localhost:5208/api/tasks';

  constructor(private http: HttpClient) {}

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl);
  }

  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  createTask(task: Partial<Task>) {
    return this.http.post(this.apiUrl, task);
  }

  updateTask(id: number, task: Partial<Task>) {
    return this.http.put(`${this.apiUrl}/${id}`, task);
  }

  deleteTask(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  moveTask(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/${id}/move`, data);
  }
}
