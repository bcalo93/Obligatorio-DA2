import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Area } from '../models';
import { environment } from '../environments/environment';
import { map, tap, catchError} from 'rxjs/operators';
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
    return this.http.get<Array<Area>>(url);
  }

  getArea(id: number): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${id}`;
    return this.http.get<Area>(url);
  }

  addArea(area: Area): Observable<Area> {
      const options = this.utilsService.getOptions();
      return this.http.post<Area>(`${environment.apiEndpoint}/area`, area, options);
  }

  updateArea(area: Area): Observable<Area> {
    const url = `${environment.apiEndpoint}/areas/${area.id}`;
    const body = JSON.stringify(area);
    const options = this.utilsService.getOptions();

    return this.http.put<Area>(url, body, options);
  }

  deleteArea(id: number): Observable<any> {
    const url = `${environment.apiEndpoint}/area/${id}`;
    const options = this.utilsService.getOptions();

    return this.http.delete(url, options);
  }
}
