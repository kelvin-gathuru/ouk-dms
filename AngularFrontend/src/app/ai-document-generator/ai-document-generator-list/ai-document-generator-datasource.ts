import { DataSource } from '@angular/cdk/table';
import { HttpResponse } from '@angular/common/http';
import { OpenAiMsg } from '../open-ai-msg';
import { ResponseHeader } from '@core/domain-classes/response-header';
import { AiDocumentGeneratorService } from '../ai-document-generator.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AiDocumentGeneratorResource } from '../../core/domain-classes/ai-document-generator-resource';

export class AiDocumentGeneratorDataSource implements DataSource<OpenAiMsg> {

  private aiDocumentGeneratorSubject = new BehaviorSubject<OpenAiMsg[]>([]);
  private responseHeaderSubject = new BehaviorSubject<ResponseHeader>({} as ResponseHeader);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();
  private _count: number = 0;


  public get count(): number {
    return this._count;
  }

  public responseHeaderSubject$ = this.responseHeaderSubject.asObservable();

  constructor(private aiDocumentGeneratorService: AiDocumentGeneratorService) { }

  connect(): Observable<OpenAiMsg[]> {
    return this.aiDocumentGeneratorSubject.asObservable();
  }

  disconnect(): void {
    this.aiDocumentGeneratorSubject.complete();
    this.loadingSubject.complete();
  }

  loadAiDocumentGenerators(aiDocumentGeneratorResource: AiDocumentGeneratorResource) {
    this.loadingSubject.next(true);
    this.aiDocumentGeneratorService.getAiMsgs(aiDocumentGeneratorResource).pipe(
      finalize(() => this.loadingSubject.next(false))
    )
      .subscribe(
        (resp: HttpResponse<OpenAiMsg[]>) => {
          const paginationParam = JSON.parse(
            resp.headers.get('X-Pagination') ?? '{}'
          ) as ResponseHeader;
          this.responseHeaderSubject.next(paginationParam);
          const aiDocumentGenerators = [...resp.body ?? []];
          this._count = aiDocumentGenerators.length;
          this.aiDocumentGeneratorSubject.next(aiDocumentGenerators);
        }
      );
  }
}
