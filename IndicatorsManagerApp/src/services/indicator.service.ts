import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Indicator, IndicatorItem } from '../models';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';
import { UtilsService } from './utils.service';

@Injectable({
  providedIn: 'root'
})
export class IndicatorService {
  constructor(
    private utilsService: UtilsService,
    private http: HttpClient) {}

  getIndicator(indicatorId: string): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<Indicator>(url, options);
  }

  updateIndicatorName(indicatorId: string, indicator: Indicator): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.put<Indicator>(url, indicator, options);
  }

  deleteIndicator(indicatorId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options);
  }

  addIndicatorItem(indicatorId: string, indicatorItem: IndicatorItem): Observable<Indicator> {
    const url = `${environment.apiEndpoint}/indicators/${indicatorId}/items`;
    const options = this.utilsService.getOptions();
    return this.http.post<Indicator>(url, indicatorItem, options);
  }

  updateIndicatorItem(indicatorId: string, indicatorItem: IndicatorItem): Observable<IndicatorItem> {
    const url = `${environment.apiEndpoint}/items/${indicatorId}`;
    const options = this.utilsService.getOptions();
    return this.http.put<IndicatorItem>(url, indicatorItem, options);
  }

  getIndicatorItem(indicatorItemId: string): Observable<IndicatorItem> {
    const url = `${environment.apiEndpoint}/items/${indicatorItemId}`;
    const options = this.utilsService.getOptions();
    return this.http.get<IndicatorItem>(url, options);
  }

  deleteIndicatorItem(indicatorItemId: string): Observable<any> {
    const url = `${environment.apiEndpoint}/items/${indicatorItemId}`;
    const options = this.utilsService.getOptions();
    return this.http.delete(url, options);
  }

}
