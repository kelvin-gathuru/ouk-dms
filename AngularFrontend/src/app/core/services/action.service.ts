import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Action } from '@core/domain-classes/action';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { catchError, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ActionService {
  private httpClient = inject(HttpClient);
  private commonHttpErrorService = inject(CommonHttpErrorService);

  getAllActions(): Observable<Action[] | CommonError> {
    const url = 'action';
    return this.httpClient
      .get<Action[]>(url)
      .pipe(
        catchError(this.commonHttpErrorService.handleError)
      );
  }

  addAction(action: Action): Observable<Action | CommonError> {
    const url = 'Action';
    return this.httpClient
      .post<Action>(url, action)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateAction(action: Action): Observable<Action | CommonError> {
    const url = `Action/${action.id}`;
    return this.httpClient
      .put<Action>(url, action)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteAction(id: string): Observable<void | CommonError> {
    const url = `Action/${id}`;
    return this.httpClient
      .delete<void>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getActionsByPageId(pageId: string): Observable<Action[] | CommonError> {
    const url = `Action/${pageId}`;
    return this.httpClient
      .get<Action[]>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));

  }

}
