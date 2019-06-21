import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../environments/environment';
import { User, Indicator } from '../models';
import { UtilsService } from './utils.service';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ReportsService {
    constructor(
      private http: HttpClient,
      private utilsService: UtilsService) {}

  getTopFrequentUsers(): Observable<Array<User>> {
    const url = `${environment.apiEndpoint}/reports/topusers?limit=10`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<User>>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getTopHiddenReports(): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/reports/tophiddenindicators?limit=10`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));

  }

  getSystemActions(start: Date, end: Date): Observable<Array<any>> {
    const url = `${environment.apiEndpoint}/reports/systemActions?start=${start}&end=${end}`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<any>>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));

  }
}
