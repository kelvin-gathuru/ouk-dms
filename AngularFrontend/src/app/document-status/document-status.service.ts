import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { DocumentStatus } from './document-status';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';

@Injectable({ providedIn: 'root' })
export class DocumentStatusService {
  private commonHttpErrorService = inject(CommonHttpErrorService);
  private httpClient = inject(HttpClient);

  getDocumentStatuss(): Observable<DocumentStatus[]> {
    const url = 'documentstatus';
    return this.httpClient.get<DocumentStatus[]>(url);

  }
  getDocumentStatusById(id: string): Observable<DocumentStatus | CommonError> {
    const url = `documentstatus/${id}`;
    return this.httpClient.get<DocumentStatus>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addDocumentStatus(documentStatus: DocumentStatus): Observable<DocumentStatus | CommonError> {
    const url = `documentstatus`;
    return this.httpClient.post<DocumentStatus>(url, documentStatus)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateDocumentStatus(documentStatus: DocumentStatus): Observable<DocumentStatus | CommonError> {
    const url = `documentstatus/${documentStatus.id}`;
    return this.httpClient.post<DocumentStatus>(url, documentStatus)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteDocumentStatus(id: string): Observable<void | CommonError> {
    const url = `documentstatus/${id}`;
    return this.httpClient.delete<void>(url).pipe(
      catchError(this.commonHttpErrorService.handleError)
    );
  }

}
