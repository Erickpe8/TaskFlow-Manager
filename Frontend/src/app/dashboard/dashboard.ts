import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ExportService } from '../core/export.service';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent {
  private router = inject(Router);
  private exportService = inject(ExportService);
  private authService = inject(AuthService);

  // Estado para el menú de exportación
  showExportMenu = signal(false);
  currentUser = this.authService.currentUser;

  logout() {
    this.authService.logout();
  }

  toggleExportMenu() {
    this.showExportMenu.update(v => !v);
  }

  exportToExcel() {
    this.showExportMenu.set(false);
    this.exportService.exportToExcel();
  }

  exportToPdf() {
    this.showExportMenu.set(false);
    this.exportService.exportToPdf();
  }
}
