import { Injectable } from '@angular/core';
import { UtilsService } from '.';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { User, Indicator } from '../models';

@Injectable({
  providedIn: 'root'
})
export class ReportsService {
    constructor(
      private http: HttpClient,
      private utilsService: UtilsService) {}

  getTopFrequentUsers(): Observable<Array<User>> {
    const url = `${environment.apiEndpoint}/reports/topusers`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<User>>(url, options);
  }

  getTopHiddenReports(): Observable<Array<Indicator>> {
    const url = `${environment.apiEndpoint}/reports/tophiddenindicators`;
    const options = this.utilsService.getOptions();
    return this.http.get<Array<Indicator>>(url, options);
  }
}
