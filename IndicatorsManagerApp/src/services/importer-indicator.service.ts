import { Injectable } from '@angular/core';
import { UtilsService } from '.';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ImporterIndicatorService {

  constructor(
    private utilsService: UtilsService,
    private http: HttpClient) {}

  getImporterInfo(): Observable<any> {
    const url = `${environment.apiEndpoint}/indicatorImports/info`;
    const options = this.utilsService.getOptions();
    return this.http.get<any>(url, options)
      .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

  importData(importerData: any): Observable<any> {
      const url = `${environment.apiEndpoint}/indicatorImports`;
      const options = this.utilsService.getOptions();
      return this.http.post<any>(url, importerData, options)
        .pipe(catchError((error: HttpErrorResponse) => throwError(error.error || 'Server Error')));
  }

}
