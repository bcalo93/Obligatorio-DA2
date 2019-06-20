import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import { Credentials, User } from '../models';
import { environment } from '../environments/environment';
import { map, tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { UserRole } from 'src/enums';

export const TOKEN_NAME = 'token';
export const USER = 'user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private router: Router) {}

  getToken(): string {
    return localStorage.getItem(TOKEN_NAME);
  }

  getCurrentUser(): User {
    return JSON.parse(localStorage.getItem(USER));
  }

  isManagerLoggedIn() {
    return !!this.getCurrentUser() && this.getCurrentUser().role === UserRole.MANAGER;
  }

  isAdminLoggedIn() {
    return !!this.getCurrentUser() && this.getCurrentUser().role === UserRole.ADMIN;
  }

  setDataIntoLocalStorage(data: any): void {
    const currentUser = data.user as User;
    localStorage.setItem(USER, JSON.stringify(currentUser));
    localStorage.setItem(TOKEN_NAME, data.token);
  }

  login(credentials: Credentials): Observable<string> {
    const url = `${environment.apiEndpoint}/login`;
    const body = JSON.stringify(credentials);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers };

    return this.http
      .post(url, body, options)
      .pipe(
        map((data) => data),
        tap(this.setDataIntoLocalStorage),
        catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error'))
      );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_NAME);
    localStorage.removeItem(USER);
    this.router.navigate(['/login']);
  }
}
