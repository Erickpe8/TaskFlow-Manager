// tasks/tasks.service.ts
import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ColumnDto, TaskDto, PaginatedResult, TaskFilter, CreateTaskDto, UpdateTaskDto } from './task';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private http = inject(HttpClient);
  private readonly baseUrl = 'http://taskflow-backend.grupolimon.online/api';

  private getAuthHeaders() {
    if (typeof window === 'undefined') {
      return {};
    }

    const token = localStorage.getItem('token');
    if (!token) return {};

    return {
      headers: {
        Authorization: `Bearer ${token}`
      }
    };
  }

  // KANBAN
  getBoard() {
    return this.http.get<ColumnDto[]>(
      `${this.baseUrl}/Columns`,
      this.getAuthHeaders()
    );
  }

  moveTask(taskId: number, columnId: number, newOrder: number) {
    return this.http.put(
      `${this.baseUrl}/Tasks/${taskId}/move`,
      { ColumnId: columnId, NewOrder: newOrder },
      this.getAuthHeaders()
    );
  }

  // LISTA CON FILTROS
  getFiltered(filter: TaskFilter) {
    let params = new HttpParams()
      .set('page', filter.page.toString())
      .set('pageSize', filter.pageSize.toString());

    if (filter.search) {
      params = params.set('search', filter.search);
    }
    if (filter.columnId) {
      params = params.set('columnId', filter.columnId.toString());
    }
    if (filter.priority !== undefined) {
      params = params.set('priority', filter.priority.toString());
    }
    if (filter.sortBy) {
      params = params.set('sortBy', filter.sortBy);
    }
    if (filter.sortDesc !== undefined) {
      params = params.set('sortDesc', filter.sortDesc.toString());
    }

    return this.http.get<PaginatedResult<TaskDto>>(
      `${this.baseUrl}/Tasks/filtered`,
      { ...this.getAuthHeaders(), params }
    );
  }

  getAll() {
    return this.http.get<TaskDto[]>(
      `${this.baseUrl}/Tasks`,
      this.getAuthHeaders()
    );
  }

  getById(id: number) {
    return this.http.get<TaskDto>(
      `${this.baseUrl}/Tasks/${id}`,
      this.getAuthHeaders()
    );
  }

  createTask(dto: CreateTaskDto) {
    return this.http.post<TaskDto>(
      `${this.baseUrl}/Tasks`,
      dto,
      this.getAuthHeaders()
    );
  }

  updateTask(id: number, dto: UpdateTaskDto) {
    return this.http.put<TaskDto>(
      `${this.baseUrl}/Tasks/${id}`,
      dto,
      this.getAuthHeaders()
    );
  }

  deleteTask(id: number) {
    return this.http.delete(
      `${this.baseUrl}/Tasks/${id}`,
      this.getAuthHeaders()
    );
  }
}
