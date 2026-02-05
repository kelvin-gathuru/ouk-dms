import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { WorkflowStep } from '@core/domain-classes/workflow-step';
import { NextTransition } from '@core/domain-classes/next-transition';
import { ServiceResponse } from '@core/domain-classes/service-response';

@Injectable({
  providedIn: 'root',
})
export class WorkflowStepService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService
  ) { }

  getWorkflowSteps(): Observable<WorkflowStep[] | CommonError> {
    const url = 'WorkflowStep';
    return this.httpClient
      .get<WorkflowStep[]>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getWorkflowStep(id: string): Observable<WorkflowStep | CommonError> {
    const url = `WorkflowStep/${id}`;
    return this.httpClient
      .get<WorkflowStep>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }



  addWorkflowStep(
    steps: WorkflowStep[]
  ): Observable<WorkflowStep[]> {
    const url = `WorkflowStep`;
    return this.httpClient
      .post<ServiceResponse<WorkflowStep[]>>(url, {
        workflowSteps: steps,
      }).pipe(map((response) => response.data));
  }

  updateWorkflowStep(
    steps: WorkflowStep[]
  ): Observable<WorkflowStep[]> {
    const url = `WorkflowStep`;
    return this.httpClient
      .put<ServiceResponse<WorkflowStep[]>>(url, { workflowSteps: steps })
      .pipe(map((response) => response.data));

  }

  deleteWorkflowStep(id: string): Observable<WorkflowStep> {
    const url = `WorkflowStep/${id}`;
    return this.httpClient
      .delete<WorkflowStep>(url);

  }
}
