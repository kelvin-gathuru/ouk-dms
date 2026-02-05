import { Injectable } from '@angular/core';
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { AIPromptTemplate } from './ai-prompt-template';
import { AIPromptTemplateService } from './ai-prompt-template.service';

@Injectable({ providedIn: 'root' })
export class TemplateOpenAiResolverService implements Resolve<AIPromptTemplate | null> {
  constructor(private aIPromptTemplateService: AIPromptTemplateService) { }
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<AIPromptTemplate | null> {
    const id = route.paramMap.get('id');
    if (id == 'new') {
      return of(null);
    }
    return this.aIPromptTemplateService.getAIPromptTemplate(id as string) as Observable<AIPromptTemplate>;
  }
}
