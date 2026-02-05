import { TitleCasePipe } from '@angular/common';
import { Component, Inject, inject, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { StreamMarkdownService } from '../../stream-markdown-service';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { DialogModule } from '@angular/cdk/dialog';
import { AiDocumentGeneratorService } from '../../ai-document-generator.service';
import { OpenAiMsg } from '../../open-ai-msg';
import { BaseComponent } from '../../../base.component';
import { OpenAiMsgResponse } from '../../open-ai-msg-response';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { AIPromptTemplate } from '../../../template-openai/ai-prompt-template';
import { containsKeywordValidator } from '../../ai-document-generator-form/contains-keyword-validator';
import { AIPromptTemplateService } from '../../../template-openai/ai-prompt-template.service';
import { TextEditorComponent } from '@shared/text-editor/text-editor.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-ai-document-generator-details',
  imports: [
    TextEditorComponent,
    MatSelectModule,
    MatIconModule,
    MatButtonModule,
    DialogModule,
    ReactiveFormsModule,
    TranslateModule,
    MatDialogModule,
    PageHelpTextComponent,
    MatCardModule,
    TitleCasePipe
  ],
  templateUrl: './ai-document-generator-details.component.html',
  styleUrl: './ai-document-generator-details.component.scss'
})
export class AiDocumentGeneratorDetailsComponent
  extends BaseComponent
  implements OnInit {
  aiEditorForm: FormGroup;
  aiPromptTemplates: AIPromptTemplate[] = [];
  selectedAiPromptTemplate: AIPromptTemplate | undefined;
  fb = inject(FormBuilder);
  aIPromptTemplateService = inject(AIPromptTemplateService);
  aiDocumentGeneratorService = inject(AiDocumentGeneratorService);
  dialogRef = inject(MatDialogRef<AiDocumentGeneratorDetailsComponent>);
  streamMarkdownService = inject(StreamMarkdownService);
  dialog = inject(MatDialog);
  constructor(@Inject(MAT_DIALOG_DATA) public openAiMsg: OpenAiMsg) {
    super();
  }

  get keywords(): FormArray {
    return this.aiEditorForm.get('keywords') as FormArray;
  }

  ngOnInit(): void {
    this.createAiEditorForm();
    this.getAiPromtTemplateSettings();
    this.getAiMessage();
  }

  createAiEditorForm(): void {
    this.aiEditorForm = this.fb.group({
      aIPromptTemplateId: [{ value: '', disabled: true }],
      promptInput: ['', [Validators.required, containsKeywordValidator('**')]],
      language: [{ value: 'en-US', disabled: true }],
      maximumLength: [1000],
      creativity: [{ value: '0.25', disabled: true }],
      keywords: this.fb.array([]),
      toneOfVoice: [{ value: 'Professional', disabled: true }],
      selectedModel: [{ value: 'gpt-3.5-turbo', disabled: true }],
      editorData: ['', [Validators.required]],
    });
  }

  getAiPromtTemplateSettings(): void {
    this.aIPromptTemplateService.getAIPromptTemplates().subscribe({
      next: (data: AIPromptTemplate[]) => {
        this.aiPromptTemplates = data;
      },
      error: (error) => { },
    });
  }

  getAiMessage(): void {
    this.aiDocumentGeneratorService
      .getAiDocumentGeneratorResponseById(this.openAiMsg.id ?? '')
      .subscribe((response: OpenAiMsgResponse) => {
        if (response && response.title) {
          const html = this.streamMarkdownService.addChunkResponce(
            response.aiResponse ?? ''
          );
          this.aiEditorForm.patchValue({
            aIPromptTemplateId: response.title,
            editorData: html || '',
            title: response.title,
            promptInput: response.promptInput,
            language: response.language,
            maximumLength: response.maximumLength,
            creativity: response.creativity,
            toneOfVoice: response.toneOfVoice,
            selectedModel: response.selectedModel,
          });
        }
      });
  }

  closeDialog() {
    this.dialogRef.close();
  }

  patchAiPromptTemplateData(data: AIPromptTemplate): void {
    this.aiEditorForm.patchValue({
      aIPromptTemplateId: data?.id,
      promptInput: data?.promptInput,
    });
  }

  onAiPromptTemplateChange(event: any): void {
    if (event.value) {
      this.selectedAiPromptTemplate = this.aiPromptTemplates.find(
        (aiPromptTemplate) => aiPromptTemplate.id === event.value
      );
      if (this.selectedAiPromptTemplate) {
        this.patchAiPromptTemplateData(this.selectedAiPromptTemplate);
      }
    }
  }
}
