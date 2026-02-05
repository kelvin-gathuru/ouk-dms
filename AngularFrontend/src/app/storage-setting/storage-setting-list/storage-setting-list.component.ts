import { Component, inject, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { StorageSettingService } from '../storage-setting.service';
import { StorageSetting } from '../storage-setting';
import { MatDialog } from '@angular/material/dialog';
import { ManageStorageSettingComponent } from '../manage-storage-setting/manage-storage-setting.component';

import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FormBuilder, FormsModule } from '@angular/forms';
import { StorageTypePipe } from '../storage-type.pipe';
import { StorageType } from '../storage-type-enum';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { NgStyle } from '@angular/common';


@Component({
  selector: 'app-storage-setting-list',
  imports: [
    RouterModule,
    TranslateModule,
    MatTableModule,
    MatSlideToggleModule,
    FormsModule,
    StorageTypePipe,
    MatTooltipModule,
    MatIconModule,
    PageHelpTextComponent,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    NgStyle
  ],
  templateUrl: './storage-setting-list.component.html',
  styleUrls: ['./storage-setting-list.component.scss']
})
export class StorageSettingListComponent implements OnInit {
  storageSettings: StorageSetting<any>[] = [];
  storageType = StorageType;

  displayedColumns: string[] = ['action', 'storageType', 'name', 'jsonValue', 'isDefault', 'enableEncryption'];
  isLoadingResults = false;
  fb = inject(FormBuilder);
  smtpSettingForm = this.fb.group({
    isDefault: [false]
  });

  private storageSettingService = inject(StorageSettingService);
  private dialog = inject(MatDialog);
  private toastrService = inject(ToastrService);
  private translationService = inject(TranslationService);

  ngOnInit(): void {
    this.getStorageSetting();
  }

  getStorageSetting(): void {
    this.storageSettingService.getStorageSettings().subscribe({
      next:
        (data: StorageSetting<any>[]) => {
          this.storageSettings = data;
        },
      error: (error) => {
      }
    });
  }

  onCreateStorageSetting(): void {
    const dialogRef = this.dialog.open(ManageStorageSettingComponent, {
      width: '600px'
    });
    dialogRef.afterClosed()
      .subscribe((result: string) => {
        this.getStorageSetting();
      });
  }

  editStorageSetting(storageSetting: any) {
    storageSetting.isEditing = true;
    this.updateDisplayedColumns();
  }

  isAnyEditing(): boolean {
    return this.storageSettings.some(setting => setting.isEditing);
  }

  updateDisplayedColumns() {
    this.displayedColumns = ['action', 'storageType', 'name', 'jsonValue', 'isDefault', 'enableEncryption'];
    if (this.isAnyEditing()) {
      this.displayedColumns.push('update');
    }
  }

  saveStorageSetting(storageSetting: any) {
    storageSetting.isEditing = false;
    this.storageSettingService.updateStorageSetting(storageSetting).subscribe({
      next: () => {
        this.toastrService.success(
          this.translationService.getValue('STORAGE_SETTING_UPDATED_SUCCESSFULLY')
        );
        this.getStorageSetting();
      },
      error: (error) => {
        this.toastrService.error(
          this.translationService.getValue('ERROR_UPDATING_STORAGE_SETTING')
        );
      }
    });
    this.updateDisplayedColumns();
  }

  cancelEditing(storageSetting: any) {
    storageSetting.isEditing = false;
    this.updateDisplayedColumns();
  }
}
