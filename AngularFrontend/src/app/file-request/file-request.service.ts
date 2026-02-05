import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '../core/error-handler/common-error';
import { CommonHttpErrorService } from '../core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { CommonService } from '../core/services/common.service';
import { FileRequest } from '../core/domain-classes/file-request';
import { FileRequestInfo } from '@core/domain-classes/file-request-info';

@Injectable({
  providedIn: 'root',
})
export class FileRequestService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService,
    private commonService: CommonService
  ) { }

  getFileRequests(): Observable<FileRequestInfo[]> {
    const url = 'FileRequest';
    return this.httpClient.get<FileRequestInfo[]>(url);
  }
  verifyPassword(id: string, password: string): Observable<boolean> {
    const url = `FileRequest/${id}/verify-password`;
    return this.httpClient
      .get<boolean>(url, { params: { password } });

  }

  getFileRequestData(id: string): Observable<FileRequestInfo> {
    const url = `FileRequest/${id}/data`;
    return this.httpClient
      .get<FileRequestInfo>(url);

  }

  getFileRequest(id: string): Observable<FileRequestInfo | CommonError> {
    const url = `FileRequest/${id}`;
    return this.httpClient
      .get<FileRequestInfo>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addFileRequest(
    setting: FileRequest
  ): Observable<FileRequest | CommonError> {
    const url = `FileRequest`;
    return this.httpClient
      .post<FileRequest>(url, setting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateFileRequest(
    setting: FileRequest
  ): Observable<FileRequestInfo | CommonError> {
    const url = `FileRequest/${setting.id}`;
    return this.httpClient
      .put<FileRequestInfo>(url, setting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteFileRequest(
    id: string
  ): Observable<FileRequest | CommonError> {
    const url = `FileRequest/${id}`;
    return this.httpClient
      .delete<FileRequest>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}
