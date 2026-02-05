import { Component, inject, OnInit } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { DocumentService } from '../document.service';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentStore } from '../document-list/document-store';
import { FoldersViewStore } from '../folders-view/folders-view-store';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { DocumentManagePresentationComponent } from '../document-manage-presentation/document-manage-presentation.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-document-manage',
  templateUrl: './document-manage.component.html',
  styleUrls: ['./document-manage.component.scss'],
  standalone: true,
  imports: [DocumentManagePresentationComponent, MatProgressSpinnerModule]
})
export class DocumentManageComponent extends BaseComponent implements OnInit {
  documentForm: UntypedFormGroup;
  loading: boolean = false;
  documentSource: string;
  documentStore = inject(DocumentStore);
  foldersViewStore = inject(FoldersViewStore);
  chunkSize = 1024 * 1024; // 1 MB per chunk
  parallelUploads = 5;
  view = '';
  documentMetaTagStore = inject(DocumentMetaTagStore);

  constructor(
    private toastrService: ToastrService,
    private documentService: DocumentService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    super();
  }

  ngOnInit(): void {
    this.view = this.route.snapshot.queryParams['view'];
  }

  saveDocument(document: Partial<DocumentInfo>) {
    this.loading = true;
    this.sub$.sink = this.documentService.addDocument(document).subscribe({
      next: (documentInfo: DocumentInfo) => {
        this.addDocumentTrail(documentInfo);
      },
      error: (error) => {
        this.loading = false;
      },
    });
  }

  addDocumentTrail(documentInfo: Partial<DocumentInfo>) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: documentInfo.id,
      operationName: DocumentOperation.Created.toString(),
    };
    if (this.foldersViewStore.selectedCategoryId() == documentInfo.categoryId) {
      this.foldersViewStore.setDocumentsEmpty();
      this.foldersViewStore.loadDocumentsByCategory(this.foldersViewStore.selectedCategoryId())
    }
    this.toastrService.success(
      this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
    );
    this.loading = false;
    this.documentStore.loadDocuments();
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
    if (this.view === 'list') {
      this.router.navigate(['/documents/list-view']);
    } else if (this.view === 'folders') {
      this.router.navigate(['/documents/folder-view']);
    } else {
      this.router.navigate(['/documents/list-view']);
    }
  }

}
