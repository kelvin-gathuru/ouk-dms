import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class GeneralSettingsService {
  constructor(private httpClient: HttpClient) { }

  addAllowSignature(companyId: string, allowSignatureIntoPdf: boolean) {
    return this.httpClient.post(`api/companyprofile/${companyId}/allow-signature`, { allowSignatureIntoPdf: allowSignatureIntoPdf });
  }
  addAddUpdateOpenAIAPIKey(companyId: string, openAIAPIKey: string, company: string) {
    return this.httpClient.post(`api/companyprofile/openai-api-key`, { openAIAPIKey: openAIAPIKey, company: company });
  }

}
