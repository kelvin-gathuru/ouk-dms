import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { OpenAiMsg } from './open-ai-msg';
import { OpenAiMsgResponse } from './open-ai-msg-response';
import { AiDocumentGeneratorResource } from '../../app/core/domain-classes/ai-document-generator-resource';
import { catchError, Observable } from 'rxjs';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';

@Injectable({ providedIn: 'root' })
export class AiDocumentGeneratorService {
  constructor(private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService
  ) { }

  saveUserOpenaiMsg(data: OpenAiMsg) {
    return this.httpClient.post<OpenAiMsg>(`UserOpenaiMsg`, data);
  }
  getAiMsgById(id: string) {
    return this.httpClient.get<boolean>(`UserOpenaiMsg/${id}`);
  }

  getAiMsgs(resource: AiDocumentGeneratorResource): Observable<HttpResponse<OpenAiMsg[]>> {
    const url = `UserOpenaiMsg`;
    const customParams = new HttpParams()
      .set('Fields', resource.fields ?? '')
      .set('OrderBy', resource.orderBy)
      .set('PageSize', resource.pageSize.toString())
      .set('Skip', resource.skip.toString())
      .set('PromptInput', resource.promptInput ?? '')
      .set('Title', resource.title ?? '');
    return this.httpClient
      .get<OpenAiMsg[]>(url, {
        params: customParams,
        observe: 'response',
      });

  }

  deleteAiDocumentGenerator(id: string) {
    return this.httpClient.delete<boolean>(`UserOpenaiMsg/${id}`);
  }

  getAiDocumentGeneratorResponseById(id: string) {
    return this.httpClient.get<OpenAiMsgResponse>(`UserOpenaiMsg/${id}/response`);
  }
}
