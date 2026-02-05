import { Component, inject, OnInit } from '@angular/core';
import { AIPromptTemplate } from './ai-prompt-template';
import { AIPromptTemplateService } from './ai-prompt-template.service';
import { MatDialog } from '@angular/material/dialog';

import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-template-openai-list',
  imports: [
    RouterModule,
    TranslateModule,
    MatTableModule,
    MatIconModule,
    PageHelpTextComponent,
    MatPaginatorModule,
    MatSortModule,
    HasClaimDirective,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './template-openai-list.component.html',
  styleUrl: './template-openai-list.component.scss'
})
export class TemplateOpenaiListComponent implements OnInit {
  aiPromptTemplates: AIPromptTemplate[] = [];
  displayedColumns: string[] = ['action', 'name', 'description', 'promptInput'];
  isLoadingResults = false;
  private aIPromptTemplateService = inject(AIPromptTemplateService);
  private dialog = inject(MatDialog);
  private toastrService = inject(ToastrService);
  private translationService = inject(TranslationService);
  private commonDialogService = inject(CommonDialogService);
  private router = inject(Router);

  ngOnInit(): void {
    this.getAiPromtTemplateSettings();
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

  onAiPromptTemplates(): void {
    this.router.navigate([`/aiprompttemplate/new`]);
  }

  editAiPromptTemplates(aiPromptTemplates: AIPromptTemplate) {
    this.router.navigate([`/aiprompttemplate/${aiPromptTemplates.id}`]);
  }
  deleteAiPromptTemplates(aiPromptTemplates: AIPromptTemplate) {
    this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_DELETE'
        )} ${aiPromptTemplates.name}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.aIPromptTemplateService.deleteAIPromptTemplate(aiPromptTemplates.id ?? '').subscribe({
            next:
              (data: boolean) => {
                if (data) {
                  this.toastrService.success(
                    this.translationService.getValue(
                      'AI_PROMPT_TEMPLATE_DELETE_SUCCESSFULLY'
                    )
                  );
                  this.getAiPromtTemplateSettings();
                }
              },
            error: (error) => {
            }
          });
        }
      });
  }
}

