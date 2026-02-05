import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { AIPromptTemplate } from '../../template-openai/ai-prompt-template';
import { TitleCasePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { AIPromptTemplateService } from '../../template-openai/ai-prompt-template.service';
import { MatIconModule } from '@angular/material/icon';
import { containsKeywordValidator } from './contains-keyword-validator';
import { OpenAiMsg } from '../open-ai-msg';
import { AiDocumentGeneratorService } from '../ai-document-generator.service';
import { ToastrService } from '@core/services/toastr-service'
import { SignalrService } from '@core/services/signalr.service';
import { SecurityService } from '@core/security/security.service';
import { OnlineUser } from '@core/domain-classes/online-user';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BaseComponent } from '../../base.component';


@Component({
  selector: 'app-ai-document-generator-form',
  imports: [
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
    MatIconModule,
    RouterModule,
    TranslateModule,
    MatIconModule,
    MatCardModule,
    TitleCasePipe,
    MatProgressSpinnerModule
  ],
  templateUrl: './ai-document-generator-form.component.html',
  styleUrl: './ai-document-generator-form.component.scss'
})
export class AiDocumentGeneratorFormComponent extends BaseComponent implements OnInit {
  aiPromptTemplates: AIPromptTemplate[] = [];
  aIPromptTemplateService = inject(AIPromptTemplateService);
  aiDocumentGeneratorService = inject(AiDocumentGeneratorService);
  toastrService = inject(ToastrService);
  signalrService = inject(SignalrService);
  securityService = inject(SecurityService);
  aiPromtForm: FormGroup;
  fb = inject(FormBuilder);
  selectedAiPromptTemplate: AIPromptTemplate | undefined;
  isLoadingResults: boolean = false;

  get keywords(): FormArray {
    return this.aiPromtForm.get('keywords') as FormArray;
  }

  ngOnInit(): void {
    this.getAiPromtTemplateSettings();
    this.createAiPromptTemplateForm();
  }

  createAiPromptTemplateForm(): void {
    this.aiPromtForm = this.fb.group({
      aIPromptTemplateId: [],
      promptInput: ['', [Validators.required, containsKeywordValidator("**")]],
      language: ['en-US'],
      maximumLength: [1000],
      creativity: ["0.25"],
      keywords: this.fb.array([]),
      toneOfVoice: ['Professional'],
      selectedModel: ['gpt-3.5-turbo']
    });
  }
  getAiPromtTemplateSettings(): void {
    this.aIPromptTemplateService.getAIPromptTemplates().subscribe({
      next:
        (data: AIPromptTemplate[]) => {
          this.aiPromptTemplates = data;
        },
      error: (error) => {
      }
    });
  }
  onAiPromptTemplateChange(event: any): void {
    if (event.value) {
      this.selectedAiPromptTemplate = this.aiPromptTemplates.find((aiPromptTemplate) => aiPromptTemplate.id === event.value);
      if (this.selectedAiPromptTemplate) {
        this.patchAiPromptTemplateData(this.selectedAiPromptTemplate);
        this.keywords.clear(); // Clear existing keywords
        const specialKeywords = this.findSpecialKeywords(this.selectedAiPromptTemplate.promptInput);
        if (specialKeywords.length > 0) {
          this.addKeyword(specialKeywords); // Add new keywords
        }
      }
    }
  }

  findSpecialKeywords(str: string): string[] {
    const regex = /\*\*(.*?)\*\*/g;
    const matches: string[] = [];
    let match;
    while ((match = regex.exec(str)) !== null) {
      matches.push(match[1]);
    }
    return matches;
  }

  addKeyword(keywords: string[]): void {
    for (const keyword of keywords) {
      const group = this.fb.group({
        keyword: [keyword],
        keywordValue: [''],
      });
      this.keywords.push(group);
    }
  }

  onInputChange(index: number, event: any): void {
    this.replaceArrayKeywordValue();
  }
  replaceArrayKeywordValue(): void {
    let promptInput = this.selectedAiPromptTemplate?.promptInput ?? '';
    for (let i = 0; i < this.keywords.length; i++) {
      const keywordValue = this.keywords.at(i).get('keywordValue')?.value;
      if (keywordValue) {
        const keyword = this.keywords.at(i).get('keyword')?.value;
        promptInput = promptInput?.replace(`**${keyword}**`, keywordValue);
      }
    }
    if (promptInput) {
      this.aiPromtForm.patchValue({
        promptInput: promptInput,
      });
    }
  }

  patchAiPromptTemplateData(data: AIPromptTemplate): void {
    this.aiPromtForm.patchValue({
      aIPromptTemplateId: data?.id,
      promptInput: data?.promptInput,
    });
  }
  buildUserAIPromptTemplate(): OpenAiMsg {
    const openAiMsg: OpenAiMsg = {
      title: this.getTitle(),
      promptInput: this.aiPromtForm.get('promptInput')?.value,
      language: this.aiPromtForm.get('language')?.value,
      maximumLength: this.aiPromtForm.get('maximumLength')?.value,
      creativity: this.aiPromtForm.get('creativity')?.value,
      toneOfVoice: this.aiPromtForm.get('toneOfVoice')?.value,
      selectedModel: this.aiPromtForm.get('selectedModel')?.value
    }
    return openAiMsg;
  }
  getTitle(): string {
    const id = this.aiPromtForm.get('aIPromptTemplateId')?.value;
    if (id) {
      const selectedAiPromptTemplate = this.aiPromptTemplates.find((aiPromptTemplate) => aiPromptTemplate.id === id);
      return selectedAiPromptTemplate?.name || '';
    }
    return '';
  }
  generate() {
    if (this.aiPromtForm.valid) {
      this.isLoadingResults = true;
      const openAiMsg = this.buildUserAIPromptTemplate();
      this.aiDocumentGeneratorService.saveUserOpenaiMsg(openAiMsg).subscribe({
        next: (data: OpenAiMsg) => {
          //this.isLoadingResults = false;
          if (data.id) {
            const userId = this.securityService.securityObject.id;
            const email = this.securityService.securityObject.email;
            const onlineUser: OnlineUser = {
              id: userId ?? '',
              email: email
            };
            this.signalrService.addUser(onlineUser);
            setTimeout(() => {
              this.getAiResponse(data.id ?? '');
            }, 100);
          }
        },
        error: (error) => {
          this.isLoadingResults = false;
          // Handle error
        }
      });
    } else {
      this.aiPromtForm.markAllAsTouched(); // Mark all fields as touched to show validation errors
    }
  }

  getAiResponse(id: string): void {
    this.aiDocumentGeneratorService.getAiMsgById(id).subscribe({
      next: (data: boolean) => {
        if (data) {
          this.isLoadingResults = false;
          this.toastrService.success(
            this.translationService.getValue('AI_DOCUMENT_GENERATED_SUCCESSFULLY')
          );
        }
      },
      error: (error) => {
        // Handle error
        this.isLoadingResults = false;
      }
    });
  }

}
