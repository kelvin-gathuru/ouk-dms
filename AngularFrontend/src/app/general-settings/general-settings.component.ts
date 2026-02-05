import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { SecurityService } from '../core/security/security.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { GeneralSettingsService } from './general-settings.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-general-settings',
  imports: [
    PageHelpTextComponent,
    FormsModule,
    TranslateModule,
    MatCheckboxModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './general-settings.component.html',
  styleUrl: './general-settings.component.scss'
})
export class GeneralSettingsComponent implements OnInit {
  allowSignatureIntoPdf: boolean = false;
  openAIAPIKey: string = '';
  geminiAPIKey: string = '';
  generalSettingsService = inject(GeneralSettingsService);
  toastrService = inject(ToastrService);
  translationService = inject(TranslationService);
  securityService = inject(SecurityService);
  companyId: string = '';

  ngOnInit(): void {
    this.getCompanyProfile();
  }

  getCompanyProfile(): void {
    this.securityService.companyProfile.subscribe((c) => {
      if (c) {
        this.companyId = c.id ?? '';
        this.allowSignatureIntoPdf = c.allowSignatureIntoPdf ?? false;
      }
    });
  }

  allowSignature() {
    this.generalSettingsService.addAllowSignature(this.companyId, this.allowSignatureIntoPdf).subscribe({
      next: (response) => {
        this.toastrService.success(this.translationService.getValue('ADDED_OR_UPDATED_ALLOW_SIGNATURE_SUCCESSFULLY'));
      },
      error: (error) => {

      }
    });
  }
  onSaveOpenAIAPIKey(company: string) {
    const apiKey = company == 'openai' ? this.openAIAPIKey : this.geminiAPIKey;
    this.generalSettingsService.addAddUpdateOpenAIAPIKey(this.companyId, apiKey, company).subscribe({
      next: (response) => {
        if (company == 'openai') {
          this.toastrService.success(this.translationService.getValue('ADDED_OR_UPDATED_OPENAI_API_KEY_SUCCESSFULLY'));
        } else {
          this.toastrService.success(this.translationService.getValue('ADDED_OR_UPDATED_GEMINI_API_KEY_SUCCESSFULLY'));
        }
      },
      error: (error) => {

      }
    });
  }
}
