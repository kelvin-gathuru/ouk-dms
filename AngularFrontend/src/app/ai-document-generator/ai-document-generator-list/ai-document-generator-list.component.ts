import { AsyncPipe } from '@angular/common';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { AiDocumentGeneratorResource } from '@core/domain-classes/ai-document-generator-resource';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { BreakpointsService } from '@core/services/breakpoints.service';
import { debounceTime, distinctUntilChanged, fromEvent, merge, Observable, tap } from 'rxjs';
import { BaseComponent } from '../../base.component';
import { AIPromptTemplate } from '../../template-openai/ai-prompt-template';
import { AIPromptTemplateService } from '../../template-openai/ai-prompt-template.service';
import { AiDocumentGeneratorDataSource } from './ai-document-generator-datasource';
import { AiDocumentGeneratorService } from '../ai-document-generator.service';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { ToastrService } from '@core/services/toastr-service';
import { OpenAiMsg } from '../open-ai-msg';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AiDocumentGeneratorDetailsComponent } from './ai-document-generator-details/ai-document-generator-details.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-ai-document-generator-list',
  imports: [
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    PageHelpTextComponent,
    TranslateModule,
    UTCToLocalTime,
    HasClaimDirective,
    MatCardModule,
    MatIconModule,
    AsyncPipe,
    MatProgressSpinnerModule,
    MatButtonModule
  ],
  templateUrl: './ai-document-generator-list.component.html',
  styleUrl: './ai-document-generator-list.component.scss'
})
export class AiDocumentGeneratorListComponent extends BaseComponent
  implements OnInit, AfterViewInit {
  dataSource: AiDocumentGeneratorDataSource;
  displayedColumns: string[] = ['action', 'createdDate', 'title', 'promptInput', 'selectedModel'];
  isLoadingResults = true;
  aiDocumentGeneratorResource: AiDocumentGeneratorResource;
  aiPromptTemplates: AIPromptTemplate[] = [];
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  footerToDisplayed = ['footer'];
  constructor(
    private aiDocumentGeneratorService: AiDocumentGeneratorService,
    private aIPromptTemplateService: AIPromptTemplateService,
    private commonDialogService: CommonDialogService,
    private toastrService: ToastrService,
    private dialog: MatDialog,
    private breakpointsService: BreakpointsService
  ) {
    super();
    this.aiDocumentGeneratorResource = new AiDocumentGeneratorResource();
    this.aiDocumentGeneratorResource.pageSize = 10;
    this.aiDocumentGeneratorResource.orderBy = 'createdDate desc';
  }

  ngOnInit(): void {
    this.dataSource = new AiDocumentGeneratorDataSource(this.aiDocumentGeneratorService);
    this.dataSource.loadAiDocumentGenerators(this.aiDocumentGeneratorResource);
    this.getResourceParameter();
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

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap((c: any) => {
          this.aiDocumentGeneratorResource.skip =
            this.paginator.pageIndex * this.paginator.pageSize;
          this.aiDocumentGeneratorResource.pageSize = this.paginator.pageSize;
          this.aiDocumentGeneratorResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadAiDocumentGenerators(this.aiDocumentGeneratorResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.aiDocumentGeneratorResource.title = this.input.nativeElement.value;
          this.dataSource.loadAiDocumentGenerators(this.aiDocumentGeneratorResource);
        })
      )
      .subscribe();
  }


  onAiPromptTemplateChange(filterValue: MatSelectChange) {
    const aiPromptTemplate: AIPromptTemplate = filterValue.value;
    if (aiPromptTemplate) {
      this.aiDocumentGeneratorResource.title = aiPromptTemplate.name;
    } else {
      this.aiDocumentGeneratorResource.title = '';
    }
    this.aiDocumentGeneratorResource.skip = 0;
    this.paginator.pageIndex = 0;
    this.dataSource.loadAiDocumentGenerators(this.aiDocumentGeneratorResource);
  }

  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.aiDocumentGeneratorResource.pageSize = c.pageSize;
          this.aiDocumentGeneratorResource.skip = c.skip;
          this.aiDocumentGeneratorResource.totalCount = c.totalCount;
        }
      }
    );
  }

  onAiDocumentGeneratorDetails(setting: OpenAiMsg) {
    const screenWidth = window.innerWidth;
    const dialogWidth = screenWidth < 768 ? '90vw' : '60vw';
    const dialogRef = this.dialog.open(AiDocumentGeneratorDetailsComponent, {
      //maxWidth: dialogWidth,
      //maxHeight: '80vh',
      data: setting,
    });
    dialogRef.afterClosed().subscribe((result: string) => {
      if (result === 'loaded') {
        // this.aiEditorForm.patchValue({
        //   editorData: '',
        // });
      }
    });
  }

  deleteAiDocumentGenerator(setting: OpenAiMsg) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${setting.title}`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.aiDocumentGeneratorService.deleteAiDocumentGenerator(setting.id ?? '').subscribe(() => {
            this.toastrService.success(this.translationService.getValue('AI_DOCUMENT_GENERATOR_DELETED_SUCCESSFULLY'));
            this.dataSource.loadAiDocumentGenerators(this.aiDocumentGeneratorResource);
          });
        }
      });
  }
}
