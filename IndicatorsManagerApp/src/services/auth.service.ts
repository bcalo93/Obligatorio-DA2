import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Credentials } from '../models/credentials';
import { environment } from '../environments/environment';
import { map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { User } from 'src/models/user';

export const TOKEN_NAME = 'token';
export const USER = 'user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router) {}

  getToken(): string {
    return localStorage.getItem(TOKEN_NAME);
  }

  getCurrentUser(): User {
    return JSON.parse(localStorage.getItem(USER));
  }

  setDataIntoLocalStorage(data: any): void {
    const currentUser = data.user as User;
    localStorage.setItem(USER, JSON.stringify(currentUser));
    localStorage.setItem(TOKEN_NAME, data.token);
  }

  login(credentials: Credentials): Promise<string> {
    const url = `${environment.apiEndpoint}/login`;
    const body = JSON.stringify(credentials);
    console.log(credentials);
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers: headers };

    return this.http
      .post(url, body, options)
      .pipe(
        map((data) => data),
        tap(this.setDataIntoLocalStorage),
        tap(() => this.router.navigate(['/']))
      )
      .toPromise();
  }

  logout(): void {
    localStorage.removeItem(TOKEN_NAME);
    localStorage.removeItem(USER);
    this.router.navigate(['/login']);
  }
}
