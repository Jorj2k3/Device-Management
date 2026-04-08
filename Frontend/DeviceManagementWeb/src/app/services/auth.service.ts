import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7177/api/Auth'; 

  login(credentials: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Login`, credentials).pipe(
      tap((response: any) => {
        // 2. If successful, save the token to the browser's Local Storage!
        if (response && response.token) {
          localStorage.setItem('jwt_token', response.token);
        }
      })
    );
  }

  register(userData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Register`, userData);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
  }
}