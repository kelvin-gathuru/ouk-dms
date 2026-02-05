import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { DocumentSignature } from '@core/domain-classes/document-signature';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable, timer } from 'rxjs';
import { catchError, map, retry } from 'rxjs/operators';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { DocumentMetaData } from '@core/domain-classes/document-meta-data';
import { DocumentSignaturePosition } from '@core/domain-classes/document-signature-position';

@Injectable({
  providedIn: 'root',
})
export class DocumentService {
  maxRetries: number = 2;
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService
  ) { }

  updateDocument(
    document: Partial<DocumentInfo>
  ): Observable<DocumentInfo | CommonError> {
    const url = `document/${document.id}`;
    return this.httpClient
      .put<DocumentInfo>(url, document)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addDocument(document: Partial<DocumentInfo>): Observable<DocumentInfo> {
    const url = `document`;
    const formData = new FormData();
    formData.append('files', document.file ?? '');
    formData.append('name', document.name ?? '');
    formData.append('categoryId', document.categoryId ?? '');
    formData.append('documentStatusId', document.documentStatusId ?? '');
    formData.append('clientId', document.clientId ?? '');
    formData.append('storageSettingId', document.storageSettingId ?? '');
    formData.append('categoryName', document.categoryName ?? '');
    formData.append('description', document.description ?? '');
    formData.append('extension', document.extension ?? '');
    formData.append('retentionPeriodInDays', document.retentionPeriodInDays ? document.retentionPeriodInDays?.toString() : '0');
    formData.append('onExpiryAction', document.onExpiryAction ? document.onExpiryAction?.toString() : '0');
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
      .post<DocumentInfo>(url, formData);

  }

  addChunkDocument(document: Partial<DocumentInfo>): Observable<DocumentInfo> {
    document.documentMetaDatas = document.documentMetaDatas?.filter(
      (c) => c.metatag
    );
    const url = `document/chunk`;
    const documentObj: any = {
      name: document.name,
      categoryId: document.categoryId,
      documentStatusId: document.documentStatusId,
      clientId: document.clientId,
      storageSettingId: document.storageSettingId,
      categoryName: document.categoryName ?? '',
      description: document.description,
      extension: document.extension,
      documentMetaDataString: JSON.stringify(document.documentMetaDatas),
      documentRolePermissionString: JSON.stringify(document.documentRolePermissions ?? []),
      documentUserPermissionString: JSON.stringify(document.documentUserPermissions ?? []),
      isAssignToMe: document.isAssignToMe
    }
    return this.httpClient
      .post<DocumentInfo>(url, documentObj);

  }

  uploadChunkDocument(documentChunkForm: FormData): Observable<DocumentChunk | CommonError> {
    const url = `document/chunk/upload`;
    return this.httpClient
      .post<DocumentChunk>(url, documentChunkForm)
      .pipe(
        retry({
          count: this.maxRetries, // Retry up to 2 times
          delay: (error, retryCount) => {
            console.warn(`Retrying chunk... Attempt ${retryCount}`);
            return timer(Math.pow(2, retryCount) * 1000); // Exponential backoff: 1s, 2s, 4s...
          }
        })
      );
  }


  deleteDocument(id: string): Observable<void | CommonError> {
    const url = `document/${id}`;
    return this.httpClient
      .delete<void>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
  archiveDocument(id: string): Observable<void> {
    const url = `document/${id}/archive`;
    return this.httpClient
      .post<void>(url, null);

  }

  restoreDocument(id: string): Observable<void | CommonError> {
    const url = `document/${id}/restore`;
    return this.httpClient
      .post<void>(url, null)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getDocumentSharedUserRoles(id: string): Observable<DocumentInfo> {
    const url = `document/${id}/sharedUsersRoles`;
    return this.httpClient
      .get<DocumentInfo>(url);
  }

  getDocument(id: string): Observable<DocumentInfo> {
    const url = `document/${id}`;
    return this.httpClient
      .get<DocumentInfo>(url);

  }
  getDocumentDetail(id: string): Observable<DocumentInfo> {
    const url = `document/${id}/Detail`;
    return this.httpClient
      .get<DocumentInfo>(url);

  }

  getDocuments(
    resource: DocumentResource
  ): Observable<HttpResponse<DocumentInfo[]>> {
    const url = `documents`;
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy)
      .set(
        'createDate',
        resource.createDate ? resource.createDate.toISOString() : ''
      )
      .set('StartDate', resource.startDate ? resource.startDate.toString() : '')
      .set('EndDate', resource.endDate ? resource.endDate.toString() : '')
      .set('MetaTagsTypeId', resource.metaTagsTypeId ?? '')
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery ?? '')
      .set('categoryId', resource.categoryId ?? '')
      .set('documentStatusId', resource.documentStatusId ?? '')
      .set('clientId', resource.clientId ?? '')
      .set('storageSettingId', resource.storageSettingId ?? '')
      .set('name', resource.name ?? '')
      .set('documentNumber', resource.documentNumber ?? '')
      .set('metaTags', resource.metaTags ?? '')
      .set('id', resource.id?.toString() ?? '');

    return this.httpClient
      .get<DocumentInfo[]>(url, {
        params: customParams,
        observe: 'response',
      });

  }

  getArchiveDocuments(
    resource: DocumentResource
  ): Observable<HttpResponse<DocumentInfo[]>> {
    const url = `documents/archive`;
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy)
      .set(
        'createDate',
        resource.createDate ? resource.createDate.toISOString() : ''
      )
      .set('StartDate', resource.startDate ? resource.startDate.toString() : '')
      .set('EndDate', resource.endDate ? resource.endDate.toString() : '')
      .set('MetaTagsTypeId', resource.metaTagsTypeId ?? '')
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery ?? '')
      .set('categoryId', resource.categoryId ?? '')
      .set('documentStatusId', resource.documentStatusId ?? '')
      .set('clientId', resource.clientId ?? '')
      .set('storageSettingId', resource.storageSettingId ?? '')
      .set('documentNumber', resource.documentNumber ?? '')
      .set('name', resource.name ?? '')
      .set('metaTags', resource.metaTags ?? '')
      .set('id', resource?.id?.toString() ?? '');

    return this.httpClient
      .get<DocumentInfo[]>(url, {
        params: customParams,
        observe: 'response',
      });

  }

  getDocumentsByDeepSearch(
    searchQuery: string
  ): Observable<DocumentInfo[]> {
    const url = `document/deepSearch?searchQuery=${searchQuery}`;
    return this.httpClient.get<DocumentInfo[]>(url);

  }

  removePageIndexing(id: string): Observable<boolean> {
    const url = `document/${id}/remove/pageindexing`;
    return this.httpClient.post<ServiceResponse<boolean>>(url, null)
      .pipe(
        map((response) => response.data)
      );
  }
  addPageIndexing(id: string): Observable<boolean> {
    const url = `document/${id}/add/pageindexing`;
    return this.httpClient.post<ServiceResponse<boolean>>(url, null)
      .pipe(
        map((response) => response.data)
      );
  }


  saveNewVersionDocument(document: any): Observable<DocumentInfo> {
    const url = `documentVersion`;
    const formData = new FormData();
    formData.append('file', document.file, document.url);
    formData.append('url', document.url);
    formData.append('extension', document.extension);
    formData.append('documentId', document.documentId);
    formData.append('comment', document.comment);
    formData.append('isSignatureExists', document?.isSignatureExists ? document.isSignatureExists.toString() : 'false');
    formData.append('signBy', document.signBy ? document.signBy : '');
    formData.append('signByDate', document.signByDate ? document.signByDate : '');
    return this.httpClient
      .post<DocumentInfo>(url, formData);

  }

  saveNewVersionDocumentChunk(document: any): Observable<DocumentInfo> {
    const url = `documentVersion/chunk`;
    const documentChunk = {
      documentId: document.documentId,
      url: document.url,
      extension: document.extension,
      isSignatureExists: document?.isSignatureExists ? document.isSignatureExists : false,
    };
    return this.httpClient
      .post<DocumentInfo>(url, documentChunk);

  }

  getDocumentVersion(id: string) {
    const url = `documentversion/${id}`;
    return this.httpClient
      .get<DocumentVersion[]>(url);
  }

  restoreDocumentVersion(id: string, versionId: string) {
    const url = `documentversion/${id}/restore/${versionId}`;
    return this.httpClient
      .post<boolean>(url, {})
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getdocumentMetadataById(id: string) {
    const url = `document/${id}/getMetatag`;
    return this.httpClient
      .get<DocumentMetaData[]>(url);

  }

  getDocumentShareableLink(
    id: string
  ): Observable<DocumentShareableLink> {
    const url = `DocumentShareableLink/${id}`;
    return this.httpClient
      .get<DocumentShareableLink>(url);

  }

  createDocumentShareableLink(
    link: DocumentShareableLink
  ): Observable<DocumentShareableLink> {
    const url = `DocumentShareableLink`;
    return this.httpClient
      .post<DocumentShareableLink>(url, link);

  }

  deleteDocumentShareableLInk(id: string): Observable<boolean | CommonError> {
    const url = `DocumentShareableLink/${id}`;
    return this.httpClient
      .delete<boolean>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getLinkInfoByCode(
    code: string
  ): Observable<DocumentShareableLink> {
    const url = `DocumentShareableLink/${code}/info`;
    return this.httpClient
      .get<DocumentShareableLink>(url);
  }

  getDocumentByCode(code: string): Observable<DocumentInfo> {
    const url = `DocumentShareableLink/${code}/document`;
    return this.httpClient
      .get<DocumentInfo>(url);
  }

  saveDocumentSignature(documentSignature: DocumentSignature): Observable<DocumentSignature> {
    const url = `DocumentSignature`;
    return this.httpClient
      .post<DocumentSignature>(url, documentSignature);

  }
  saveDocumentSignaturePostion(documentSignature: DocumentSignaturePosition): Observable<DocumentSignature> {
    const url = `DocumentSignature/position`;
    return this.httpClient
      .post<DocumentSignature>(url, documentSignature);
  }

  getDocumentSignature(documentId: string): Observable<DocumentSignature[]> {
    const url = `DocumentSignature/${documentId}`;
    return this.httpClient
      .get<DocumentSignature[]>(url);

  }
  addAIDocumentCreated(document: Partial<DocumentInfo>): Observable<DocumentInfo> {
    const url = `document/aiDocumentCreated`;
    const documentInfo = {
      name: document.name,
      categoryId: document.categoryId,
      documentStatusId: document.documentStatusId,
      clientId: document.clientId,
      storageSettingId: document.storageSettingId,
      categoryName: document.categoryName ?? '',
      description: document.description,
      extension: document.extension,
      documentMetaDataString: JSON.stringify(document.documentMetaDatas),
      documentRolePermissionString: JSON.stringify(document.documentRolePermissions ?? []),
      documentUserPermissionString: JSON.stringify(document.documentUserPermissions ?? []),
      isAssignToMe: document.isAssignToMe,
      documentContent: document.documentContent,
      retentionPeriodInDays: document.retentionPeriodInDays,
      onExpiryAction: document.onExpiryAction
    }
    return this.httpClient
      .post<DocumentInfo>(url, documentInfo);
  }

}
