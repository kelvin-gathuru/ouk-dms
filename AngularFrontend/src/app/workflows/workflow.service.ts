import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Workflow } from '../core/domain-classes/workflow';
import { NextTransition } from '@core/domain-classes/next-transition';
import { RequestDocWorkflow } from './manage-all-workflow/request-document-through-workflow/request-doc-workflow';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

  getWorkflows(): Observable<Workflow[]> {
    const url = 'Workflow';
    return this.httpClient.get<Workflow[]>(url);

  }
  getWorkflowsNotStarted(documentId: string): Observable<Workflow[] | CommonError> {
    const url = 'Workflow/NotStarted/document/' + documentId;
    return this.httpClient.get<Workflow[]>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getWorkflow(id: string): Observable<Workflow> {
    const url = `Workflow/${id}`;
    return this.httpClient.get<Workflow>(url);

  }

  addWorkflow(workflow: Workflow): Observable<Workflow> {
    const url = `Workflow`;
    return this.httpClient.post<Workflow>(url, workflow);

  }

  updateWorkflow(workflow: Workflow): Observable<Workflow> {
    const url = `Workflow/${workflow.id}`;
    return this.httpClient.put<Workflow>(url, workflow);

  }

  deleteWorkflow(id: string): Observable<Workflow | CommonError> {
    const url = `Workflow/${id}`;
    return this.httpClient.delete<Workflow>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
  getReqDocumentWorkflows(): Observable<Workflow[]> {
    const url = 'Workflow/ReqDocument/Workflows';
    return this.httpClient.get<Workflow[]>(url);

  }
  createReqDocumentWorkflow(reqDocWorkflow: RequestDocWorkflow): Observable<boolean> {
    const url = 'Workflow/ReqDocument/Workflow/Create';
    return this.httpClient.post<boolean>(url, reqDocWorkflow);

  }

  getvisualWorkflow(id: string): Observable<VisualWorkflowInstance> {
    const url = `Workflow/${id}/visualWorkflow`;
    return this.httpClient.get<VisualWorkflowInstance>(url);

  }

}
