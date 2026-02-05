import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { WorkflowInstance } from '@core/domain-classes/workflow-instance';
import { NextTransition } from '@core/domain-classes/next-transition';
import { VisualWorkflowInstance } from '@core/domain-classes/visual-workflow-instance';
import { WorkflowsResource } from '@core/domain-classes/workflows-resource';
import { WorkflowInstanceData } from '@core/domain-classes/workflow-instance-data';
import { Workflow } from '@core/domain-classes/workflow';
import { PerformTransition } from './perform-transition/perform-transition';
import { DocumentShort } from '@core/domain-classes/document-short';

@Injectable({
  providedIn: 'root'
})
export class WorkflowInstanceService {

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) { }

  getCurrentWorkflowInstances(): Observable<WorkflowInstanceData[]> {
    const url = 'WorkflowInstance/CurrentWorkflowInstances';
    return this.httpClient.get<WorkflowInstanceData[]>(url);

  }

  getallWorkflows(): Observable<Workflow[]> {
    const url = 'WorkflowInstance/workflows';
    return this.httpClient.get<Workflow[]>(url);

  }

  getallDocuments(): Observable<DocumentShort[]> {
    const url = 'WorkflowInstance/documents';
    return this.httpClient.get<DocumentShort[]>(url);

  }

  cancelWorkflowInstance(id: string): Observable<boolean> {
    const url = `WorkflowInstance/${id}/Cancel`;
    return this.httpClient.put<boolean>(url, null);

  }

  getAllWorkflowInstances(resource: WorkflowsResource): Observable<HttpResponse<WorkflowInstanceData[]>> {
    const url = 'WorkflowInstance/AllWorkflowInstances';
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy)
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('SearchQuery', resource.searchQuery ?? '')
      .set('documentId', resource.documentId)
      .set('workflowId', resource.workflowId)
      .set('workflowInstanceStatus', resource.workflowInstanceStatus);

    return this.httpClient.get<WorkflowInstanceData[]>(url, {
      params: customParams,
      observe: 'response'
    });

  }

  getWorkflowInstance(id: string): Observable<WorkflowInstance | CommonError> {
    const url = `WorkflowInstance/${id}`;
    return this.httpClient.get<WorkflowInstance>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addWorkflowInstance(workflow: WorkflowInstance): Observable<WorkflowInstance | CommonError> {
    const url = `WorkflowInstance`;
    return this.httpClient.post<WorkflowInstance>(url, workflow)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateWorkflowInstance(workflow: WorkflowInstance): Observable<WorkflowInstance | CommonError> {
    const url = `WorkflowInstance/${workflow.id}`;
    return this.httpClient.put<WorkflowInstance>(url, workflow)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteWorkflowInstance(id: string): Observable<WorkflowInstance | CommonError> {
    const url = `WorkflowInstance/${id}`;
    return this.httpClient.delete<WorkflowInstance>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
  performNextTransition(nextTransition: NextTransition): Observable<boolean> {
    const url = `WorkflowTransition/nextTransition`;
    return this.httpClient
      .post<boolean>(url,
        nextTransition
      );
  }
  performNextTransitionWithDocumentAndSignature(nextTransition: PerformTransition): Observable<boolean> {
    const url = `WorkflowTransition/nextTransitionWithDocumentAndSignature`;
    // const formData = new FormData();
    // formData.append('workflowInstanceId', nextTransition.workflowInstanceId);
    // formData.append('transitionId', nextTransition.transitionId);
    // formData.append('workflowStepInstanceId', nextTransition.workflowStepInstanceId);
    // formData.append('isUploadDocumentVersion', nextTransition.isUploadDocumentVersion.toString());
    // formData.append('isSignatureRequired', nextTransition.isSignatureRequired.toString());
    // formData.append('comment', nextTransition.comment);
    // formData.append('url', nextTransition.url);
    // formData.append('extension', nextTransition.extension);
    // formData.append('signature', nextTransition.signature);
    // if (nextTransition.documentId) {
    //   formData.append('documentId', nextTransition.documentId);
    // }
    // if (nextTransition.documentVersionId) {
    //   formData.append('documentVersionId', nextTransition.documentVersionId);
    // }
    return this.httpClient
      .post<boolean>(url, nextTransition);


  }
  getvisualWorkflowInstance(id: string): Observable<VisualWorkflowInstance> {
    const url = `WorkflowInstance/${id}/visualWorkflowInstance`;
    return this.httpClient.get<VisualWorkflowInstance>(url);

  }

  getWorkflowInstanceByWorkflowId(id: string): Observable<boolean>{
    const url = `WorkflowInstance/${id}/byWorkflowId`;
    return this.httpClient.get<boolean>(url);
  }

}
