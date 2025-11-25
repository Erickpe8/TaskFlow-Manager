import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ColumnDto } from './task';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private http = inject(HttpClient);
  private platformId = inject(PLATFORM_ID);

  private readonly baseUrl = 'http://localhost:5208';

  private getAuthHeaders() {

    const isBrowser = isPlatformBrowser(this.platformId);

    if (!isBrowser) {
      // 🚀 SSR: NO usar localStorage, retornar headers vacíos
      return {
        headers: new HttpHeaders({})
      };
    }

    const token = localStorage.getItem('token') ?? '';

    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  getBoard(): Observable<ColumnDto[]> {
    return this.http.get<ColumnDto[]>(`${this.baseUrl}/api/Columns`, this.getAuthHeaders());
  }

  moveTask(taskId: number, columnId: number, newOrder: number) {
    return this.http.put(
      `${this.baseUrl}/api/Tasks/${taskId}/move`,
      { ColumnId: columnId, NewOrder: newOrder },
      this.getAuthHeaders()
    );
  }
}
