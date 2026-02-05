import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DocumentPermission } from '@core/domain-classes/document-permission';
import { DocumentRolePermission } from '@core/domain-classes/document-role-permission';
import { DocumentUserPermission } from '@core/domain-classes/document-user-permission';
import { PermissionUserRole } from '@core/domain-classes/permission-user-role';
import { SharePermission } from '@core/domain-classes/share-permission';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DocumentPermissionService {

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) {
  }

  getDoucmentPermission(id: string): Observable<DocumentPermission[]> {
    const url = `DocumentRolePermission/${id}`;
    return this.httpClient.get<DocumentPermission[]>(url);

  }

  deleteDocumentUserPermission(id: string): Observable<void | CommonError> {
    const url = `DocumentUserPermission/${id}`;
    return this.httpClient.delete<void>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteDocumentRolePermission(id: string): Observable<void | CommonError> {
    const url = `DocumentRolePermission/${id}`;
    return this.httpClient.delete<void>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addDocumentUserPermission(documentUserPermissions: DocumentUserPermission[]): Observable<void | CommonError> {
    const url = 'DocumentUserPermission';
    return this.httpClient.post<void>(url, { documentUserPermissions })
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addDocumentRolePermission(documentRolePermissions: DocumentRolePermission[]): Observable<void | CommonError> {
    const url = 'DocumentRolePermission';
    return this.httpClient.post<void>(url, { documentRolePermissions })
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  multipleDocumentsToUsersAndRoles(permissionUserRole: PermissionUserRole): Observable<boolean | CommonError> {
    const url = 'DocumentRolePermission/multiple';
    return this.httpClient.post<boolean>(url, permissionUserRole)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getSharePermission(documentId: string, categoryId: string): Observable<SharePermission> {
    const url = `DocumentRolePermission/${documentId}/sharepermission/${categoryId}`;
    return this.httpClient.get<SharePermission>(url);

  }

}
