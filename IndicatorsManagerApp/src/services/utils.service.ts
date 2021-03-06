import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  constructor(private authService: AuthService) {}

  getOptions() {
    const token = this.authService.getToken();
    let headers: HttpHeaders = new HttpHeaders();
    headers = headers.append('Authorization', token);
    headers = headers.append('Content-Type', 'application/json');
    return { headers };
  }
}
