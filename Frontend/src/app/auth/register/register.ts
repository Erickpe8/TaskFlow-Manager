import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './register.html',
  styleUrls: ['../login/login.css']
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  register() {
    this.errorMessage.set('');
    this.successMessage.set('');

    // Validaciones
    if (!this.name.trim()) {
      this.errorMessage.set('El nombre es obligatorio');
      return;
    }

    if (!this.email.trim()) {
      this.errorMessage.set('El email es obligatorio');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.email)) {
      this.errorMessage.set('El formato del email no es válido');
      return;
    }

    if (this.password.length < 6) {
      this.errorMessage.set('La contraseña debe tener al menos 6 caracteres');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.errorMessage.set('Las contraseñas no coinciden');
      return;
    }

    this.loading.set(true);

    this.authService.register(this.name, this.email, this.password).subscribe({
      next: () => {
        this.successMessage.set('¡Cuenta creada exitosamente! Redirigiendo al login...');
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (err) => {
        console.error('Error de registro:', err);
        if (err.error?.message) {
          this.errorMessage.set(err.error.message);
        } else if (err.status === 400) {
          this.errorMessage.set('El email ya está registrado o los datos son inválidos');
        } else {
          this.errorMessage.set('Error al crear la cuenta. Intenta nuevamente.');
        }
        this.loading.set(false);
      }
    });
  }
}
