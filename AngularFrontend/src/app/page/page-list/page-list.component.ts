import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';

import { ToastrService } from '@core/services/toastr-service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { ManageActionComponent } from '../manage-action/manage-action.component';
import { ManagePageComponent } from '../manage-page/manage-page.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { Page } from '@core/domain-classes/page';
import { Action } from '@core/domain-classes/action';
import { ActionService } from '@core/services/action.service';
import { PageService } from '@core/services/page.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { ActionStore } from '../store/action-store';
import { PageStore } from '../store/page-store';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-page-list',
  standalone: true,
  imports: [
    TranslateModule,
    MatTableModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    PageHelpTextComponent,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './page-list.component.html',
  styleUrl: './page-list.component.scss',
})

export class PageListComponent extends BaseComponent implements OnInit {
  displayedColumns: string[] = ['action', 'name', 'order'];
  pages: Page[] = []
  subActions: Action[] = [];
  subActionColumnToDisplay: string[] = ['action', 'name', 'order', 'code'];
  expandedElement!: Page | null;
  dialog = inject(MatDialog);
  commonDialogService = inject(CommonDialogService);
  actionService = inject(ActionService);
  pageService = inject(PageService);
  toastrService = inject(ToastrService);
  actionStore = inject(ActionStore);
  pageStore = inject(PageStore);

  ngOnInit(): void { }

  toggleRow(element: Page) {
    if (element) {
      this.expandedElement = this.expandedElement === element ? null : element;
      if (!this.expandedElement) {
        return;
      }
      this.actionStore.getActionByPageId(this.expandedElement.id);
    }
  }

  managePage(page?: Page) {
    this.dialog.open(ManagePageComponent, {
      width: '350px',
      data: Object.assign({}, page || null)
    });
  }

  deletePage(selectpage: Page): void {
    this.commonDialogService.deleteConfirmWithCommentDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${selectpage.name}`)
      .subscribe({
        next: (result: boolean) => {
          if (result) {
            this.pageStore.deletePageById(selectpage.id);
          }
        }
      })
  };

  refresh() {
    this.pageStore.loadPages();
  }


  manageAction(action?: Action): void {
    this.dialog.open(ManageActionComponent, {
      width: '350px',
      data: Object.assign({}, {
        pageId: this.expandedElement?.id,
        pageName: this.expandedElement?.name,
        action: action || null,
      })
    });
  }

  deleteAction(action: Action): void {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmWithCommentDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${action.name}?`)
      .subscribe(isTrue => {
        if (isTrue) {
          this.actionStore.deleteActionById(action.id);
        }
      });
  }
}
