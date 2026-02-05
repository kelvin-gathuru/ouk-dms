import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { WorkflowTransition } from '@core/domain-classes/workflow-transition';
import { ServiceResponse } from '@core/domain-classes/service-response';

@Injectable({
  providedIn: 'root',
})
export class WorkflowTransitionService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService
  ) { }

  getWorkflowTransitions(): Observable<WorkflowTransition[] | CommonError> {
    const url = 'WorkflowTransition';
    return this.httpClient
      .get<WorkflowTransition[]>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getWorkflowTransition(id: string): Observable<WorkflowTransition | CommonError> {
    const url = `WorkflowTransition/${id}`;
    return this.httpClient
      .get<WorkflowTransition>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addWorkflowTransition(
    transitions: WorkflowTransition[]
  ): Observable<WorkflowTransition[]> {
    const url = `WorkflowTransition`;
    return this.httpClient
      .post<ServiceResponse<WorkflowTransition[]>>(url, {
        workflowTransitions: transitions,
      }).pipe(map((response) => response.data));

  }

  updateWorkflowTransition(
    transitions: WorkflowTransition[]
  ): Observable<WorkflowTransition[]> {
    const url = `WorkflowTransition`;
    return this.httpClient
      .put<ServiceResponse<WorkflowTransition[]>>(url, { workflowTransitions: transitions })
      .pipe(map((response) => response.data));

  }

  deleteWorkflowTransition(id: string): Observable<WorkflowTransition | CommonError> {
    const url = `WorkflowTransition/${id}`;
    return this.httpClient
      .delete<WorkflowTransition>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}
