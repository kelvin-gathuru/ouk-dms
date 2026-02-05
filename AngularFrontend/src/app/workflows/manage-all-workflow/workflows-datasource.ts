import { DataSource } from '@angular/cdk/table';
import { HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { WorkflowInstanceData } from '../../core/domain-classes/workflow-instance-data';
import { ResponseHeader } from '../../core/domain-classes/response-header';
import { WorkflowInstanceService } from '../workflow-instance.service';
import { WorkflowsResource } from '@core/domain-classes/workflows-resource';

export class WorkflowsDataSource implements DataSource<WorkflowInstanceData> {

  private workflowsSubject = new BehaviorSubject<WorkflowInstanceData[]>([]);
  private responseHeaderSubject = new BehaviorSubject<ResponseHeader>({} as ResponseHeader);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();
  private _count: number = 0;


  public get count(): number {
    return this._count;
  }

  public responseHeaderSubject$ = this.responseHeaderSubject.asObservable();

  constructor(private workflowInstanceService: WorkflowInstanceService) { }

  connect(): Observable<WorkflowInstanceData[]> {
    return this.workflowsSubject.asObservable();
  }

  disconnect(): void {
    this.workflowsSubject.complete();
    this.loadingSubject.complete();
  }

  loadWorkflows(workflowsResource: WorkflowsResource) {
    this.loadingSubject.next(true);
    this.workflowInstanceService.getAllWorkflowInstances(workflowsResource).pipe(
      finalize(() => this.loadingSubject.next(false))
    )
      .subscribe(
        (resp: HttpResponse<WorkflowInstanceData[]>) => {
          const paginationParam = JSON.parse(
            resp.headers.get('X-Pagination') || '{}'
          ) as ResponseHeader;
          this.responseHeaderSubject.next(paginationParam);
          const workflows = resp.body ? [...resp.body] : [];
          this._count = workflows.length;
          this.workflowsSubject.next(workflows);
        }
      );
  }
}
