import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Area, Indicator } from '../models';
import { environment } from '../environments/environment';
import { Observable, throwError } from 'rxjs';
import { UtilsService } from './utils.service';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AreaService {
  constructor(
    private utilsService: UtilsService,
    private http: HttpClient) {}

  getAllAreas(): Observable<Array<Area>> {
    const url = `${environment.apiEndpoint}/areas`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Area>>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getArea(areaId: string): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${areaId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<Area>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  addArea(area: Area): Observable<Area> {
      const url = `${environment.apiEndpoint}/areas`;
      const options = this.utilsService.getOptions();
      return this.http.post<Area>(url, area, options)
        .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  updateArea(area: Area): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${area.id}`;
    const body = JSON.stringify(area);
    const options = this.utilsService.getOptions();
    return this.http.put<Area>(url, body, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  deleteArea(areaId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/areas/${areaId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  addManagerToArea(areaId: string): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/userarea`;
    const body = JSON.stringify(areaId);
    const options = this.utilsService.getOptions();
    return this.http.post<Area>(url, body, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  deleteManagerFromArea(areaId: string, userId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/userarea/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  addIndicators(areaId: string, indicator: Indicator): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/indicators`;
    const body = JSON.stringify(indicator);
    const options = this.utilsService.getOptions();
    return this.http.post<Indicator>(url, body, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  getIndicators(areaId: string): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/indicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }
}
