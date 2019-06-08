import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Area, Indicator } from '../models';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { UtilsService } from './utils.service';

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
    return this.http.get<Array<Area>>(url, options);
  }

  getArea(areaId: string): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${areaId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<Area>(url, options);
  }

  addArea(area: Area): Observable<Area> {
      const url = `${environment.apiEndpoint}/areas`;
      const options = this.utilsService.getOptions();
      return this.http.post<Area>(url, area, options);
  }

  updateArea(area: Area): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${area.id}`;
    const body = JSON.stringify(area);
    const options = this.utilsService.getOptions();
    return this.http.put<Area>(url, body, options);
  }

  deleteArea(areaId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/areas/${areaId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options);
  }

  addManagerToArea(areaId: string): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/userarea`;
    const body = JSON.stringify(areaId);
    const options = this.utilsService.getOptions();
    return this.http.post<Area>(url, body, options);
  }

  deleteManagerFromArea(areaId: string, userId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/userarea/${userId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options);
  }

  addIndicators(areaId: string): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/indicators`;
    const body = JSON.stringify(areaId);
    const options = this.utilsService.getOptions();
    return this.http.post<Indicator>(url, body, options);
  }

  getIndicators(areaId: string): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/areas/${areaId}/indicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Indicator>(url, options);
  }
}
