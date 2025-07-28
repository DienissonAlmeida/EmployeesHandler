import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5151/api/Auth'; // ajuste para sua url backend

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<void> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => {
        localStorage.setItem('jwt_token', response.token);
      }),
      map(() => void 0) // só devolve void no subscribe
    );
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('jwt_token');
    return !!token;
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

    getUserId(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = token.split('.')[1]; // Parte do meio do JWT
    const decodedPayload = atob(payload); // Decodifica base64
    const parsed = JSON.parse(decodedPayload);

    return parsed['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || parsed['nameid'] || parsed['sub'] || null;
  }
}
