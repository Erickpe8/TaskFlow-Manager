import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
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
    this.loading.set(true);
    this.errorMessage.set('');

    this.http.post<any>('http://localhost:5208/api/Auth/login', {
      email: this.email,
      password: this.password
    }).subscribe({
      next: (response) => {
        localStorage.setItem('token', response.token);
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.errorMessage.set('Credenciales incorrectas.');
      },
      complete: () => this.loading.set(false)
    });
  }
}
