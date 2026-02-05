import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { User } from '@core/domain-classes/user';
import { Observable } from 'rxjs';
import { CommonError } from '@core/error-handler/common-error';
import { catchError } from 'rxjs/operators';
import { UserClaim } from '@core/domain-classes/user-claim';
import { PageHelper } from '@core/domain-classes/pageHelper';

@Injectable({ providedIn: 'root' })
export class PageHelperService {

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }


  getPageHelpers(): Observable<PageHelper[]> {
    const url = `PageHelper`;
    return this.httpClient.get<PageHelper[]>(url);

  }

  updatePageHelper(pageHelper: PageHelper): Observable<PageHelper | CommonError> {
    const url = `PageHelper/${pageHelper.id}`;
    return this.httpClient.put<User>(url, pageHelper)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addPageHelper(pageHelper: PageHelper): Observable<PageHelper | CommonError> {
    const url = `PageHelper`;
    return this.httpClient.post<User>(url, pageHelper)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getPageHelper(id: string): Observable<PageHelper | CommonError> {
    const url = `PageHelper/${id}`;
    return this.httpClient.get<PageHelper>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}
