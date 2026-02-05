import { HttpClient, HttpEvent, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DocumentLibraryService {

  constructor(private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

  getDocuments(resource: DocumentResource): Observable<HttpResponse<DocumentInfo[]>> {
    const url = `DocumentLibraries`;
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy)
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery ?? '')
      .set('categoryId', resource.categoryId ?? '')
      .set('documentStatusId', resource.documentStatusId ?? '')
      .set('StartDate', resource.startDate ? resource.startDate.toString() : '')
      .set('EndDate', resource.endDate ? resource.endDate.toString() : '')
      .set('MetaTagsTypeId', resource.metaTagsTypeId ?? '')
      .set('clientId', resource.clientId ?? '')
      .set('storageSettingId', resource.storageSettingId ?? '')
      .set('name', resource.name ?? '')
      .set('documentNumber', resource.documentNumber ?? '')
      .set('metaTags', resource.metaTags ?? '')
      .set('id', resource?.id?.toString() ?? '')

    return this.httpClient.get<DocumentInfo[]>(url, {
      params: customParams,
      observe: 'response'
    });
  }

  getDocumentLibrary(id: string): Observable<DocumentInfo> {
    return this.httpClient.get<DocumentInfo>('document/' + id);
  }
  getDocumentViewLibrary(id: string): Observable<DocumentInfo> {
    return this.httpClient.get<DocumentInfo>('document/view/' + id);
  }


}
