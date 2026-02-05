import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecentActivityService {

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

  getRecentDocuments(resource: DocumentResource): Observable<HttpResponse<DocumentAuditTrail[]>> {
    const url = `recentDocument`;
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy ?? '')
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery ?? '')
      .set('categoryId', resource.categoryId ?? '')
      .set('operation', resource.operation ?? '')
      .set('name', resource.name ?? '')
      .set('documentNumber', resource.documentNumber ?? '')
      .set('id', resource?.id?.toString() ?? '')
      .set('createdBy', resource?.createdBy?.toString() ?? '')

    return this.httpClient.get<DocumentAuditTrail[]>(url, {
      params: customParams,
      observe: 'response'
    });
  }
}
