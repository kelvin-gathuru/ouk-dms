import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '../core/error-handler/common-error';
import { CommonHttpErrorService } from '../core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { FileRequestDocument } from '../core/domain-classes/file-request-document';
import { FileRequestDocumentInfo } from '@core/domain-classes/file-request-document-info';
import { FileRequestDocumentApprove } from '@core/domain-classes/file-request-document-apporve';

@Injectable({
  providedIn: 'root',
})
export class FileRequestDocumentService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService,
  ) { }

  getFileRequestDocuments(): Observable<FileRequestDocument[] | CommonError> {
    const url = 'FileRequestDocument';
    return this.httpClient.get<FileRequestDocument[]>(url).pipe(
      catchError(this.commonHttpErrorService.handleError)
    );
  }
  getFileRequestDocument(id: string): Observable<FileRequestDocument[] | CommonError> {
    const url = `FileRequestDocument/${id}`;
    return this.httpClient
      .get<FileRequestDocument[]>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError
      ));
  }

  addFileRequestDocument(
    setting: FileRequestDocumentInfo
  ): Observable<FileRequestDocumentInfo | CommonError> {
    const url = `FileRequestDocument`;
    const formData = new FormData();
    if (setting?.files && setting.files.length > 0) {
      Array.from(setting?.files).forEach((file) => {
        formData.append('files', file, file.name);
      });
    }
    Array.from(setting?.names).forEach((file) => {
      formData.append('names', file);
    });
    formData.append('fileRequestId', setting?.fileRequestId);
    return this.httpClient
      .post<FileRequestDocumentInfo>(url, formData)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateFileRequestDocument(
    setting: FileRequestDocument
  ): Observable<FileRequestDocument> {
    const url = `FileRequestDocument/${setting.id}`;
    return this.httpClient
      .put<FileRequestDocument>(url, setting);
  }

  addApproveDocument(document: FileRequestDocumentApprove): Observable<FileRequestDocumentApprove> {
    const url = `FileRequestDocument/document`;
    const formData = new FormData();
    formData.append('fileRequestId', document.fileRequestId ?? '');
    formData.append('fileRequestDocumentId', document.fileRequestDocumentId ?? '');
    formData.append('name', document.name ?? '');
    formData.append('categoryId', document.categoryId ?? '');
    formData.append('documentStatusId', document.documentStatusId ?? '');
    formData.append('clientId', document.clientId ?? '');
    formData.append('storageSettingId', document.storageSettingId ?? '');
    formData.append('categoryName', document.categoryName ?? '');
    formData.append('description', document.description ?? '');
    formData.append('extension', document.extension ?? '');
    formData.append(
      'documentMetaDataString',
      JSON.stringify(document.documentMetaDatas)
    );
    formData.append(
      'documentRolePermissionString',
      JSON.stringify(document.documentRolePermissions ?? [])
    );

    formData.append(
      'documentUserPermissionString',
      JSON.stringify(document.documentUserPermissions ?? [])
    );
    return this.httpClient
      .post<FileRequestDocumentApprove>(url, formData);

  }

}
