import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

interface AuthResponse {
  Token: string;
  ExpiresAt: string;
  Email: string;
  FullName: string;
  Roles: string[];
  Permissions: string[];
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  private http = inject(HttpClient);
  private router = inject(Router);

  email: string = '';
  password: string = '';

  loading = signal(false);
  errorMessage = signal('');

  login() {
    // Limpiar mensaje previo
    this.errorMessage.set('');

    // Validaciones
    if (!this.email.trim()) {
      this.errorMessage.set('El email es obligatorio');
      return;
    }

    if (!this.password.trim()) {
      this.errorMessage.set('La contraseña es obligatoria');
      return;
    }

    this.loading.set(true);

    // IMPORTANTE: Backend espera propiedades con PascalCase
    this.http.post<AuthResponse>('http://localhost:5208/api/Auth/login', {
      Email: this.email,
      Password: this.password
    }).subscribe({
      next: (response) => {
        // Guardar token y datos del usuario
        localStorage.setItem('token', response.Token);
        localStorage.setItem('user', JSON.stringify({
          email: response.Email,
          fullName: response.FullName,
          roles: response.Roles,
          permissions: response.Permissions
        }));

        console.log('Login exitoso:', response);
        this.router.navigate(['/dashboard/kanban']);
      },
      error: (err) => {
        console.error('Error de login:', err);
        if (err.status === 401) {
          this.errorMessage.set('Credenciales incorrectas. Verifica tu email y contraseña.');
        } else {
          this.errorMessage.set('Error al iniciar sesión. Intenta nuevamente.');
        }
        this.loading.set(false);
      }
    });
  }
}
