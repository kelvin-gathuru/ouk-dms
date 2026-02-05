import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { CommonDialogService } from '../../core/common-dialog/common-dialog.service';
import { TranslationService } from '../../core/services/translation.service';
import { AllowFileExtensionService } from '../allow-file-extension.service';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { BaseComponent } from '../../base.component';

import { ToastrService } from '@core/services/toastr-service';
import { FileTypePipe } from '../../shared/pipes/file-type.pipe';
import { FileType } from '@core/domain-classes/file-type.enum';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { CommonError } from '@core/error-handler/common-error';

@Component({
  selector: 'app-allow-file-extension-list',
  imports: [FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    FileTypePipe,
    MatTableModule,
    MatInputModule,
    PageHelpTextComponent,
    HasClaimDirective
  ],
  templateUrl: './allow-file-extension-list.component.html',
  styleUrl: './allow-file-extension-list.component.scss'
})

export class AllowFileExtensionListComponent extends BaseComponent implements OnInit {

  allowFileExtensions: AllowFileExtension[] = [];
  displayedColumns: string[] = ['action', 'type', 'extension'];

  private allowFileExtensionService = inject(AllowFileExtensionService);
  private commonDialogService = inject(CommonDialogService);
  private toastrService = inject(ToastrService);

  ngOnInit(): void {
    this.getAllowFileExtensions();
  }

  getAllowFileExtensions() {
    this.sub$.sink = this.allowFileExtensionService.getAllowFileExtensions()
      .subscribe((result: AllowFileExtension[] | CommonError) => {
        if (Array.isArray(result)) {
          this.allowFileExtensions = result;
          for (const extension of this.allowFileExtensions) {
            extension.extension = extension.extension.split(',').join(', ');
          }
        } else {
          // handle error if needed, e.g., show a toast
          this.toastrService.error(this.translationService.getValue('ERROR_LOADING_FILE_EXTENSIONS'));
        }
      })
  }

  deleteAllowFileExtension(setting: AllowFileExtension) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${FileType[setting.fileType]}`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.allowFileExtensionService.deleteAllowFileExtension(setting.id ?? '').subscribe(() => {
            this.toastrService.success(this.translationService.getValue('ALLOW_FILE_EXTENSION_DELETED_SUCCESSFULLY'));
            this.getAllowFileExtensions();
          });
        }
      });
  }
}
