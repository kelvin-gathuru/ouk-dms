import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AIPromptTemplate } from './ai-prompt-template';

@Injectable({ providedIn: 'root' })
export class AIPromptTemplateService {
  constructor(private httpClient: HttpClient) { }

  getAIPromptTemplates() {
    return this.httpClient.get<AIPromptTemplate[]>('AIPromptTemplate');
  }
  getAIPromptTemplate(id: string) {
    return this.httpClient.get<AIPromptTemplate>(`AIPromptTemplate/${id}`);
  }
  addAIPromptTemplate(template: AIPromptTemplate) {
    return this.httpClient.post<AIPromptTemplate>('AIPromptTemplate', template);
  }
  updateAIPromptTemplate(template: AIPromptTemplate) {
    return this.httpClient.put<AIPromptTemplate>(`AIPromptTemplate/${template.id}`, template);
  }
  deleteAIPromptTemplate(id: string) {
    return this.httpClient.delete<boolean>(`AIPromptTemplate/${id}`);
  }

}
