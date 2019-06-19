import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { User, Indicator } from '../models';
import { environment } from '../environments/environment';
import { Observable, throwError } from 'rxjs';
import { UtilsService } from './utils.service';
import { catchError } from 'rxjs/operators';
import { IndicatorConfig } from 'src/models/indicatorConfig';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(
    private utilsService: UtilsService,
    private http: HttpClient) {}

  getAllUsers(): Observable<Array<User>> {
    const url = `${environment.apiEndpoint}/users`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<User>>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getUser(userId: string): Observable<User> {
    const url = `${environment.apiEndpoint}/users/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<User>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  addUser(user: User): Observable<User> {
    const url = `${environment.apiEndpoint}/users`;
    const options = this.utilsService.getOptions();
    return this.http.post<User>(url, user, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  updateUser(user: User): Observable<User> {
    const url = `${environment.apiEndpoint}/users/${user.id}`;
    const options = this.utilsService.getOptions();
    return this.http.put<User>(url, user, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  deleteUser(userId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/users/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getManagerIndicators(): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/users/indicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getManagerActiveIndicators(): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/users/activeindicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  updateIndicatorConfiguration(indicatorsConfig: Array<IndicatorConfig> ): Observable<User> {
    const url = `${environment.apiEndpoint}/indicators/indicatorconfig`;
    const options = this.utilsService.getOptions();
    return this.http.post<User>(url, indicatorsConfig, options)
    .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }
}
