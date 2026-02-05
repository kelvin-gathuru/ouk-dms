import { Component, inject, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { DocumentShareableLink } from '@core/domain-classes/document-shareable-link';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';
import { DocumentLinkPreviewPasswordComponent } from './document-link-preview-password/document-link-preview-password.component';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentService } from '../../document/document.service';
import { DocumentView } from '@core/domain-classes/document-view';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-document-link-preview',
  imports: [
    RouterModule, // Required for ActivatedRoute
    MatDialogModule,
    MatProgressSpinnerModule
  ],
  standalone: true,
  templateUrl: './document-link-preview.component.html',
  styleUrls: ['./document-link-preview.component.scss']
})
export class DocumentLinkPreviewComponent implements OnInit {
  isLoadingResults = false;
  @Input() documents: DocumentInfo[] = [];
  isLinkExpired = false;
  code: string;
  private route = inject(ActivatedRoute);
  private overlay = inject(OverlayPanel);
  private dialog = inject(MatDialog);
  private documentService = inject(DocumentService);

  ngOnInit(): void {
    this.code = this.route.snapshot.params['code'];
    this.getLinkInfo();
  }

  getLinkInfo() {
    this.documentService.getLinkInfoByCode(this.code).subscribe({
      next: (info: DocumentShareableLink) => {
        if (info.isLinkExpired) {
          this.isLinkExpired = true;
        } else {
          this.getDocument(info);
        }
      },
      error: () => {
        this.isLinkExpired = true;
      },
    });
  }

  getDocument(info: DocumentShareableLink) {
    this.documentService.getDocumentByCode(this.code).subscribe(
      {
        next: async (document: DocumentInfo) => {
          this.isLoadingResults = true;
          try {
            const urls = document.url?.split('.') ?? [];
            const extension = urls[1];
            const documentView: DocumentView = {
              documentId: this.code,
              name: document.name,
              extension: extension,
              isVersion: false,
              isFromPublicPreview: true,
              isPreviewDownloadEnabled: document.isAllowDownload ?? false,
              isFileRequestDocument: false,
              isSignatureExists: document.isSignatureExists,
              documentNumber: document.documentNumber,
            };
            if (info.hasPassword) {
              this.openForgotPasswordDialog(info, documentView);
            } else {
              const { BasePreviewComponent } = await import(
                '../../shared/base-preview/base-preview.component'
              );
              this.overlay.open(BasePreviewComponent, {
                position: 'center',
                origin: 'global',
                panelClass: ['file-preview-overlay-container', 'white-background'],
                data: documentView,
                closeOnBackdropClick: false,
              });
            }
          }
          finally {
            this.isLoadingResults = false;
          }
        },
        error: () => {
          this.isLinkExpired = true;
        }
      }
    );
  }

  openForgotPasswordDialog(
    info: DocumentShareableLink,
    documentView: DocumentView
  ) {
    const dialogRef = this.dialog.open(DocumentLinkPreviewPasswordComponent, {
      data: { linkInfo: info, docView: documentView },
      disableClose: true,
      backdropClass: 'black-background',
      width: '500px',
    });
    dialogRef.afterClosed().subscribe(async (password: string) => {
      this.isLoadingResults = true;
      try {
        documentView.linkPassword = password;
        const { BasePreviewComponent } = await import(
          '../../shared/base-preview/base-preview.component'
        );
        const overlayRef = this.overlay.open(BasePreviewComponent, {
          position: 'center',
          origin: 'global',
          panelClass: ['file-preview-overlay-container', 'white-background'],
          data: documentView,
          closeOnBackdropClick: false,
        });

        overlayRef.afterClosed().subscribe(() => {
          this.openForgotPasswordDialog(info, documentView);
        });
      }
      finally {
        this.isLoadingResults = false;
      }
    });
  }
}
