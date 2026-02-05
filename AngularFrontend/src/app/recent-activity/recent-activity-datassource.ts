import { CollectionViewer } from '@angular/cdk/collections';
import { DataSource } from '@angular/cdk/table';
import { HttpResponse } from '@angular/common/http';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { RecentActivityService } from './recent-activity.service';

export class RecentActivityDataSource implements DataSource<DocumentAuditTrail> {

  private recentActivitySubject = new BehaviorSubject<DocumentAuditTrail[]>([]);
  private responseHeaderSubject = new BehaviorSubject<ResponseHeader | null>(null);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();
  private _count: number = 0;


  public get count(): number {
    return this._count;
  }

  public responseHeaderSubject$ = this.responseHeaderSubject.asObservable();

  constructor(private recentActivityService: RecentActivityService) { }

  connect(collectionViewer: CollectionViewer): Observable<DocumentAuditTrail[]> {
    return this.recentActivitySubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.recentActivitySubject.complete();
    this.loadingSubject.complete();
  }

  loadRecentActivityDocuments(documentResource: DocumentResource) {
    this.loadingSubject.next(true);
    this.recentActivityService.getRecentDocuments(documentResource).pipe(
      finalize(() => this.loadingSubject.next(false))
    )
      .subscribe(
        (resp: HttpResponse<DocumentAuditTrail[]>) => {
          const paginationHeader = resp.headers.get('X-Pagination');
          const paginationParam = paginationHeader ? JSON.parse(paginationHeader) : null;
          this.responseHeaderSubject.next(paginationParam);
          const recentActivityDocuments = resp.body ? [...resp.body] : [];
          this._count = recentActivityDocuments.length;
          this.recentActivitySubject.next(recentActivityDocuments);
        }
      );
  }
}
