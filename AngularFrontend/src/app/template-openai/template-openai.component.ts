import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { ToastrService } from '@core/services/toastr-service';
import { AIPromptTemplate } from './ai-prompt-template';
import { AIPromptTemplateService } from './ai-prompt-template.service';
import { TranslationService } from '@core/services/translation.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-template-openai',
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    RouterModule,
    PageHelpTextComponent,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './template-openai.component.html',
  styleUrl: './template-openai.component.scss'
})
export class TemplateOpenaiComponent {
  aiPromtTemplateForm: FormGroup;
  aiPromptTemplate: AIPromptTemplate;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private aIPromptTemplateService: AIPromptTemplateService,
    private translationService: TranslationService
  ) {
  }

  ngOnInit(): void {
    this.createAiPromptTemplateForm();
    this.getAIPromptResolverData();
    //  this.getEmailResolverData();
  }

  getAIPromptResolverData(): void {
    this.route.data.subscribe((data: any) => {
      if (data.aIPromptTemplate) {
        this.aiPromptTemplate = data.aIPromptTemplate;
        this.patchAiPromptTemplateData();
      }
    });
  }

  addUpdateAiPromptTemplate() {
    if (this.aiPromtTemplateForm.valid) {
      if (this.aiPromptTemplate) {
        this.aIPromptTemplateService
          .updateAIPromptTemplate(this.createBuildObject())
          .subscribe((c) => {
            this.toastrService.success(
              this.translationService.getValue(
                'AI_PROMPT_TEMPLATE_SAVE_SUCCESSFULLY'
              )
            );
            this.router.navigate(['/aiprompttemplate']);
          });
      } else {
        this.aIPromptTemplateService
          .addAIPromptTemplate(this.createBuildObject())
          .subscribe((c) => {
            this.toastrService.success(
              this.translationService.getValue(
                'AI_PROMPT_TEMPLATE_SAVE_SUCCESSFULLY'
              )
            );
            this.router.navigate(['/aiprompttemplate']);
          });
      }
    } else {

    }
  }

  createBuildObject(): AIPromptTemplate {
    const emailTemplate: AIPromptTemplate = {
      id: this.aiPromptTemplate ? this.aiPromptTemplate.id : undefined,
      name: this.aiPromtTemplateForm.get('name')?.value,
      description: this.aiPromtTemplateForm.get('description')?.value,
      promptInput: this.aiPromtTemplateForm.get('promptInput')?.value,
    };
    return emailTemplate;
  }

  createAiPromptTemplateForm() {
    this.aiPromtTemplateForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      promptInput: ['', [Validators.required]]
    });
  }

  patchAiPromptTemplateData() {
    this.aiPromtTemplateForm.patchValue({
      name: this.aiPromptTemplate.name,
      description: this.aiPromptTemplate.description,
      promptInput: this.aiPromptTemplate.promptInput
    });
  }
}
