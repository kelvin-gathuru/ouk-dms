import { Component, OnInit, inject, OnDestroy, Input } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { SubSink } from 'subsink';

import { ToastrService } from '@core/services/toastr-service';
import { DocumentStatusService } from '../document-status.service';
import { ManageDocumentStatusComponent } from '../manage-document-status/manage-document-status.component';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { TranslationService } from '@core/services/translation.service';
import { DocumentStatus } from '../document-status';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { NgStyle } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-document-status-list',
  imports: [
    RouterModule,
    TranslateModule,
    MatTableModule,
    MatDialogModule,
    PageHelpTextComponent,
    MatCardModule,
    MatIconModule,
    NgStyle,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './document-status-list.component.html',
  styleUrls: ['./document-status-list.component.scss']
})
export class DocumentStatusListComponent implements OnInit, OnDestroy {
  @Input() documentStatus: DocumentStatus;
  documentStatuses: DocumentStatus[] = [];
  displayedColumns: string[] = ['action', 'name', 'description', 'colorCode'];
  isLoadingResults = false;

  private subs = new SubSink();
  private documentStatusService = inject(DocumentStatusService);
  private dialog = inject(MatDialog);
  private toastrService = inject(ToastrService);
  private translationService = inject(TranslationService);
  private commonDialogService = inject(CommonDialogService);

  constructor() { }

  ngOnInit(): void {
    this.getDocumentStatus();
  }

  getDocumentStatus(): void {
    this.isLoadingResults = true;
    this.documentStatusService.getDocumentStatuss().subscribe({
      next: (data: DocumentStatus[]) => {
        this.documentStatuses = data;
        this.isLoadingResults = false;
      },
      error: (error) => {
        this.isLoadingResults = false;
      },
    });
  }

  onCreateDocumentStatus(): void {
    const dialogRef = this.dialog.open(ManageDocumentStatusComponent, {
      width: '500px',
    });
    this.subs.sink = dialogRef.afterClosed().subscribe((result: DocumentStatus) => {
      if (result) {
        this.documentStatuses = [result, ...this.documentStatuses];
      }
    });
  }

  onEditDocumentStatus(documentStatus: DocumentStatus): void {
    const dialogRef = this.dialog.open(ManageDocumentStatusComponent, {
      width: '500px',
      data: { ...documentStatus }
    });

    this.subs.sink = dialogRef.afterClosed().subscribe((result: DocumentStatus) => {
      if (result) {
        this.documentStatuses = this.documentStatuses.map(item =>
          item.id === result.id ? { ...item, ...result } : item
        );
      }
    });
  }

  deleteDocumentStatus(id: string): void {
    this.subs.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')
      )
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.isLoadingResults = true;
          this.documentStatusService.deleteDocumentStatus(id).subscribe({
            next: () => {
              this.isLoadingResults = false;
              this.getDocumentStatus();
              this.toastrService.success(
                this.translationService.getValue(
                  'DOCUMENT_STATUS_DELETED_SUCCESSFULLY'
                )
              );
            },
            error: (error) => {
              this.isLoadingResults = false;
            }
          });
        }
      });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }
}
