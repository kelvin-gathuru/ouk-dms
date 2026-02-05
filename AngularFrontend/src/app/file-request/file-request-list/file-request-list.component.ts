import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { TranslationService } from '@core/services/translation.service';
import { TranslateModule } from '@ngx-translate/core';
import { WorkflowStore } from '../../workflows/workflow-store';
import { ToastrService } from '@core/services/toastr-service';
import { FileRequestService } from '../file-request.service';
import { FileRequestInfo } from '../../core/domain-classes/file-request-info';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FileRequestDocumentListComponent } from '../file-request-document-list/file-request-document-list.component';
import { StatusColorDirective } from '../status-color.directive';
import { DocumentStore } from '../../document/document-list/document-store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { FileRequestStatusPipe } from '@shared/pipes/file-request-status.pipe';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatIconModule } from '@angular/material/icon';
import { FileSizeLabelDirective } from '../file-size-label.directive';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { CdkStepLabel } from "@angular/cdk/stepper";

@Component({
  selector: 'app-file-request-list',
  templateUrl: './file-request-list.component.html',
  styleUrls: ['./file-request-list.component.scss'],
  imports: [
    PageHelpTextComponent,
    HasClaimDirective,
    MatTableModule,
    FileRequestDocumentListComponent,
    TranslateModule,
    HasClaimDirective,
    MatTooltipModule,
    RouterModule,
    MatIconModule,
    FileSizeLabelDirective,
    StatusColorDirective,
    FileRequestStatusPipe,
    UTCToLocalTime,
    MatCardModule,
    MatButtonModule,
],
})
export class FileRequestListComponent implements OnInit {
  fileRequestInfos: FileRequestInfo[] = [];
  expandedElement: FileRequestInfo | null;
  displayedColumns: string[] = [
    'action',
    'subject',
    'email',
    'sizeInMb',
    'maxDocument',
    'fileExtension',
    'status',
    'createdBy',
    'createdDate',
    'linkExpiryTime',
  ];
  baseUrl = `${window.location.protocol}//${window.location.host}/file-requests/preview/`;
  documentStore = inject(DocumentStore);

  private fileRequestService = inject(FileRequestService);
  public workflowStore = inject(WorkflowStore);
  private commonDialogService = inject(CommonDialogService);
  private translationService = inject(TranslationService);
  private toastrService = inject(ToastrService);
  private cd = inject(ChangeDetectorRef);
  isExpanded = false;
  ngOnInit(): void {
    this.getFileRequests();
  }

  getFileRequests() {
    this.fileRequestService
      .getFileRequests()
      .subscribe((fileRequests: FileRequestInfo[]) => {
        this.fileRequestInfos = fileRequests;
      });
  }

  copyToClipboard(fileRequestId: string): void {
    const linkToCopy = `${this.baseUrl}${fileRequestId}`;
    navigator.clipboard
      .writeText(linkToCopy)
      .then(() => {
        this.toastrService.success(
          this.translationService.getValue('LINK_COPIED_TO_CLIPBOARD')
        );
      })
      .catch(() => {
        this.toastrService.error(
          this.translationService.getValue('FAILED_TO_COPY_LINK')
        );
      });
  }

  deleteFileRequest(fileRequest: FileRequestInfo) {
    this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_DELETE'
        )} ${[fileRequest.subject]}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.fileRequestService
            .deleteFileRequest(fileRequest.id ?? '')
            .subscribe(() => {
              this.toastrService.success(
                this.translationService.getValue(
                  'FILE_REQUEST_DELETED_SUCCESSFULLY'
                )
              );
              this.getFileRequests();
            });
        }
      });
  }

  toggleRow(element: FileRequestInfo) {
    this.isExpanded = this.expandedElement != element;
    this.expandedElement = this.expandedElement === element ? null : element;
    this.cd.detectChanges();
  }

  isExpandedRow = (index: number, row: FileRequestInfo): boolean => {
    return row === this.expandedElement;
  };
}
