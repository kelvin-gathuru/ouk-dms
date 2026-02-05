import { Component, inject, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { BaseComponent } from '../../base.component';
import { DocumentMetaTag } from '@core/domain-classes/document-meta-tag';
import { DocumentMetaTagStore } from '../document-meta-tag-store';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { DocumentMetaTagFileTypePipe } from '@shared/pipes/document-meta-tag-file-type.pipe';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-document-meta-tag-list',
  imports: [
    RouterModule,
    MatTableModule,
    TranslateModule,
    DocumentMetaTagFileTypePipe,
    PageHelpTextComponent,
    HasClaimDirective,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './document-meta-tag-list.component.html',
  styleUrls: ['./document-meta-tag-list.component.scss']
})
export class DocumentMetaTagListComponent
  extends BaseComponent
  implements OnInit {
  documentMetaTags: DocumentMetaTag[] = [];
  displayedColumns: string[] = ['action', 'type', 'name', 'isEditable'];

  public documentMetaTagStore = inject(DocumentMetaTagStore);
  private commonDialogService = inject(CommonDialogService);

  ngOnInit(): void {
  }

  deleteDocumentMetaTag(documentMetaTag: DocumentMetaTag) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_DELETE'
        )} ${documentMetaTag.name}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.documentMetaTagStore.deleteDocumentMetaTagById(
            documentMetaTag.id ?? ''
          );
        }
      });
  }
}
