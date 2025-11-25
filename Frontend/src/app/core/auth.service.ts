import { Injectable, inject, PLATFORM_ID, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { Observable, tap } from 'rxjs';

interface AuthResponse {
  Token: string;
  ExpiresAt: string;
  Email: string;
  FullName: string;
  Roles: string[];
  Permissions: string[];
}

interface UserData {
  email: string;
  fullName: string;
  roles: string[];
  permissions: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  private readonly API_URL = 'http://localhost:5208/api/Auth';

  // Estado reactivo del usuario
  currentUser = signal<UserData | null>(null);
  isAuthenticated = signal(false);

  constructor() {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage() {
    if (isPlatformBrowser(this.platformId)) {
      const token = localStorage.getItem('token');
      const userJson = localStorage.getItem('user');

      if (token && userJson) {
        try {
          const user = JSON.parse(userJson);
          this.currentUser.set(user);
          this.isAuthenticated.set(true);
        } catch {
          this.logout();
        }
      }
    }
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/login`, {
      Email: email,
      Password: password
    }).pipe(
      tap(response => this.handleAuthResponse(response))
    );
  }

  register(fullName: string, email: string, password: string): Observable<any> {
    return this.http.post(`${this.API_URL}/register`, {
      FullName: fullName,
      Email: email,
      Password: password
    });
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }

  private handleAuthResponse(response: AuthResponse) {
    if (isPlatformBrowser(this.platformId)) {
      const user: UserData = {
        email: response.Email,
        fullName: response.FullName,
        roles: response.Roles,
        permissions: response.Permissions
      };

      localStorage.setItem('token', response.Token);
      localStorage.setItem('user', JSON.stringify(user));

      this.currentUser.set(user);
      this.isAuthenticated.set(true);
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('token');
    }
    return null;
  }

  hasPermission(permission: string): boolean {
    const user = this.currentUser();
    return user?.permissions.includes(permission) ?? false;
  }

  hasRole(role: string): boolean {
    const user = this.currentUser();
    return user?.roles.includes(role) ?? false;
  }
}
