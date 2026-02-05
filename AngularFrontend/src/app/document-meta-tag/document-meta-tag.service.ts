import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '../core/error-handler/common-error';
import { CommonHttpErrorService } from '../core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { DocumentMetaTag } from '../core/domain-classes/document-meta-tag';

@Injectable({
  providedIn: 'root',
})
export class DocumentMetaTagService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService,
  ) {}


  getDocumentMetaTags(): Observable<DocumentMetaTag[] | CommonError> {
    const url = 'DocumentMetaTag';
    return this.httpClient.get<DocumentMetaTag[]>(url).pipe(
      catchError(this.commonHttpErrorService.handleError)
    );
  }

  getDocumentMetaTag(id: string): Observable<DocumentMetaTag | CommonError> {
    const url = `DocumentMetaTag/${id}`;
    return this.httpClient
      .get<DocumentMetaTag>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addDocumentMetaTag(
    setting: DocumentMetaTag
  ): Observable<DocumentMetaTag | CommonError> {
    const url = `DocumentMetaTag`;
    return this.httpClient
      .post<DocumentMetaTag>(url, setting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateDocumentMetaTag(
    setting: DocumentMetaTag
  ): Observable<DocumentMetaTag | CommonError> {
    const url = `DocumentMetaTag/${setting.id}`;
    return this.httpClient
      .put<DocumentMetaTag>(url, setting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteDocumentMetaTag(
    id: string
  ): Observable<DocumentMetaTag | CommonError> {
    const url = `DocumentMetaTag/${id}`;
    return this.httpClient
      .delete<DocumentMetaTag>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}
