import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { User, Indicator } from '../models';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { UtilsService } from './utils.service';

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
    return this.http.get<Array<User>>(url, options);
  }

  getUser(userId: string): Observable<User> {
    const url = `${environment.apiEndpoint}/users/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<User>(url, options);
  }

  addUser(user: User): Observable<User> {
    const url = `${environment.apiEndpoint}/users`;
    const options = this.utilsService.getOptions();
    return this.http.post<User>(url, user, options);
  }

  updateUser(user: User): Observable<User> {
    const url = `${environment.apiEndpoint}/users/${user.id}`;
    const options = this.utilsService.getOptions();
    return this.http.put<User>(url, user, options);
  }

  deleteUser(userId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/users/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options);
  }

  getManagerIndicators(): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/users/indicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options);
  }

  getManagerActiveIndicators(): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/users/activeindicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options);
  }

  addManagerIndicatorConfiguration(indicatorId: string, indicators: Array<Indicator> ): Observable<User> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}/userindicator`;
    const options = this.utilsService.getOptions();
    return this.http.post<User>(url, indicators, options);
  }

  updateIndicatorConfiguration(indicatorId: string, indicators: Indicator): Observable<User> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}/userindicator`;
    const options = this.utilsService.getOptions();
    return this.http.post<User>(url, indicators, options);
  }

  deleteIndicatorConfiguration(indicatorId: string, userId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}/userindicator/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options);
  }
}
