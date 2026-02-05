import { Component, inject, Input, OnInit } from '@angular/core';
import { FileRequestInfo } from '../../core/domain-classes/file-request-info';
import { FileRequestDocument } from '../../core/domain-classes/file-request-document';
import { FileRequestDocumentService } from '../file-request-document.service';
import { CommonDialogService } from '../../core/common-dialog/common-dialog.service';
import { TranslationService } from '../../core/services/translation.service';

import { ToastrService } from '@core/services/toastr-service';
import { CommonError } from '../../core/error-handler/common-error';
import { TranslateModule } from '@ngx-translate/core';
import { MatTableModule } from '@angular/material/table';
import { DocumentView } from '@core/domain-classes/document-view';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { FileRequestDocumentStatusPipe } from '../file-request-document-status.pipe';
import { MatDialog } from '@angular/material/dialog';
import { ApproveDocumentComponent } from '../approve-document/approve-document.component';
import { DocumentLibraryDataSource } from '../../document-library/document-library-list/document-library-datasource';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { FileRequestDocumentStatus } from '@core/domain-classes/file-request-document-status.enum';
import { StatusColorDirective } from '../status-color.directive';
import { DocumentStore } from '../../document/document-list/document-store';
import { SecurityService } from '@core/security/security.service';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-file-request-document-list',
  imports:
    [
      PageHelpTextComponent,
      TranslateModule,
      UTCToLocalTime,
      FileRequestDocumentStatusPipe,
      StatusColorDirective,
      MatTableModule,
      HasClaimDirective,
      MatIconModule,
      MatCardModule,
      MatProgressSpinnerModule,
      MatButtonModule
    ],
  templateUrl: './file-request-document-list.component.html',
  styleUrl: './file-request-document-list.component.scss'
})
export class FileRequestDocumentListComponent implements OnInit {
  isLoadingResults = false;
  @Input() fileRequestInfo: FileRequestInfo;
  dataSource: DocumentLibraryDataSource;
  documentResource: DocumentResource;
  fileRequestDocuments: FileRequestDocument[] = [];
  fileRequestDocumentStatus = FileRequestDocumentStatus;
  displayedColumns: string[] = ['action', 'fileUrl', 'status', 'createdDate', 'reason'];
  documentStore = inject(DocumentStore);
  securityService = inject(SecurityService);
  foldersViewStore = inject(FoldersViewStore);

  private fileRequestDocumentService = inject(FileRequestDocumentService);
  private overlay = inject(OverlayPanel);
  private commonDialogService = inject(CommonDialogService);
  private translationService = inject(TranslationService);
  private toastrService = inject(ToastrService);
  private dialog = inject(MatDialog);


  ngOnInit(): void {
    this.getFileRequestDocuments();
  }

  async onDocumentView(fileRequestDocument: FileRequestDocument) {
    this.isLoadingResults = true;
    try {
      const fileUrl = fileRequestDocument?.url.split('.');
      const extension = fileUrl[1];
      const documentView: DocumentView = {
        documentId: fileRequestDocument.id,
        name: fileRequestDocument.name,
        url: fileRequestDocument.url,
        extension: extension,
        isVersion: false,
        isFromPublicPreview: false,
        isPreviewDownloadEnabled: false,
        isFileRequestDocument: true,
        isSignatureExists: false,
        documentNumber: ''
      };
      const { BasePreviewComponent } = await import(
        '../../shared/base-preview/base-preview.component'
      );
      this.overlay.open(BasePreviewComponent, {
        position: 'center',
        origin: 'global',
        panelClass: ['file-preview-overlay-container', 'white-background'],
        data: documentView,
      });
    }
    finally {
      this.isLoadingResults = false;
    }
  }

  getFileRequestDocuments() {
    const fileRequestId = this.fileRequestInfo?.id;
    if (fileRequestId) {
      this.fileRequestDocumentService.getFileRequestDocument(fileRequestId)
        .subscribe({
          next: (response: FileRequestDocument[] | CommonError) => {
            if (Array.isArray(response)) {
              if (response.length > 0) {
                this.fileRequestDocuments = response;
              }
            }
          },
          error: (error: any) => {
          }
        });
    }
  }

  onAddDocument(fileRequestDocument: FileRequestDocument) {
    const dialogRef = this.dialog.open(ApproveDocumentComponent, {
      data: fileRequestDocument,
      maxWidth: '60vw',
      maxHeight: '80vh',
    });
    dialogRef.afterClosed().subscribe((result: string) => {
      if (result) {
        this.documentStore.loadDocuments();
        if (this.foldersViewStore.selectedCategoryId() == result) {
          this.foldersViewStore.setDocumentsEmpty();
          this.foldersViewStore.loadDocumentsByCategory(this.foldersViewStore.selectedCategoryId())
        }
        this.getFileRequestDocuments();
      }
    });
  }

  rejectFileRequestDocument(fileRequestDocument: FileRequestDocument) {
    this.commonDialogService.deleteConfirmWithCommentDialog(
      `${this.translationService.getValue(
        'ARE_YOU_SURE_YOU_WANT_TO_REJECT'
      )} ${fileRequestDocument.name} ?`
    ).subscribe((commentFlag: any) => {
      if (commentFlag.flag) {
        if (commentFlag.comment == '') {
          this.toastrService.error(
            this.translationService.getValue('PLEASE_ENTER_A_COMMENT')
          );
          return;
        }
        fileRequestDocument.reason = commentFlag.comment;
        this.fileRequestDocumentService.updateFileRequestDocument(fileRequestDocument)
          .subscribe({
            next: (response: FileRequestDocument) => {
              if (response) {
                this.toastrService.success(
                  this.translationService.getValue('FILE_REJECTED_SUCCESSFULLY')
                );
                this.getFileRequestDocuments();
              }
            },
            error: (error: any) => {
            }
          });
      }
    });
  }
}
