import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Indicator, IndicatorItem } from '../models';
import { environment } from '../environments/environment';
import { Observable, throwError } from 'rxjs';
import { UtilsService } from './utils.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class IndicatorService {
  constructor(
    private utilsService: UtilsService,
    private http: HttpClient) {}

  getIndicator(indicatorId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<any>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  updateIndicatorName(indicatorId: string, indicator: Indicator): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.put<Indicator>(url, indicator, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  deleteIndicator(indicatorId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  addIndicatorItem(indicatorId: string, indicatorItem: IndicatorItem): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}/items`;
    const options = this.utilsService.getOptions();
    return this.http.post<Indicator>(url, indicatorItem, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  updateIndicatorItem(indicatorId: string, indicatorItem: IndicatorItem): Observable<IndicatorItem> {
    const url = `${environment.apiEndpoint}/items/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.put<IndicatorItem>(url, indicatorItem, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getIndicatorItem(indicatorItemId: string): Observable<IndicatorItem> {
    const url = `${environment.apiEndpoint}/items/${indicatorItemId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<IndicatorItem>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  deleteIndicatorItem(indicatorItemId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/items/${indicatorItemId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

}
