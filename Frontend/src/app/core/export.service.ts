import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  private http = inject(HttpClient);
  private readonly API_URL = 'http://localhost:5208/api/Export';

  exportToExcel() {
    return this.http.get(`${this.API_URL}/excel`, {
      responseType: 'blob',
      observe: 'response'
    }).subscribe({
      next: (response) => {
        const contentDisposition = response.headers.get('content-disposition');
        let filename = `TaskFlow_Tareas_${this.getDateString()}.xlsx`;

        if (contentDisposition) {
          const matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(contentDisposition);
          if (matches != null && matches[1]) {
            filename = matches[1].replace(/['"]/g, '');
          }
        }

        if (response.body) {
          saveAs(response.body, filename);
        }
      },
      error: (error) => {
        console.error('Error al exportar Excel:', error);
        alert('Error al descargar el archivo Excel');
      }
    });
  }

  exportToPdf() {
    return this.http.get(`${this.API_URL}/pdf`, {
      responseType: 'blob',
      observe: 'response'
    }).subscribe({
      next: (response) => {
        const contentDisposition = response.headers.get('content-disposition');
        let filename = `TaskFlow_Tareas_${this.getDateString()}.pdf`;

        if (contentDisposition) {
          const matches = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(contentDisposition);
          if (matches != null && matches[1]) {
            filename = matches[1].replace(/['"]/g, '');
          }
        }

        if (response.body) {
          saveAs(response.body, filename);
        }
      },
      error: (error) => {
        console.error('Error al exportar PDF:', error);
        alert('Error al descargar el archivo PDF');
      }
    });
  }

  private getDateString(): string {
    const now = new Date();
    return `${now.getFullYear()}${(now.getMonth() + 1).toString().padStart(2, '0')}${now.getDate().toString().padStart(2, '0')}_${now.getHours().toString().padStart(2, '0')}${now.getMinutes().toString().padStart(2, '0')}${now.getSeconds().toString().padStart(2, '0')}`;
  }
}
