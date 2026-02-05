import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WorkflowLog } from '@core/domain-classes/workflow-log';
import { WorkflowLogResource } from '@core/domain-classes/workflow-log-resource';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WorkflowLogService {

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

  getWorkflowLogs(resource: WorkflowLogResource): Observable<HttpResponse<WorkflowLog[]>> {
    const url = 'WorkflowTransitionLog';
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy)
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery ?? '')
      .set('documentId', resource.documentId)
      .set('workflowId', resource.workflowId)
      .set('workflowInstanceStatus', resource.workflowInstanceStatus);

    return this.httpClient.get<WorkflowLog[]>(url, {
      params: customParams,
      observe: 'response'
    });
  }
}
