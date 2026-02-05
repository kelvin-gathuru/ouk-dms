
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslateModule } from '@ngx-translate/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { CommonService } from '@core/services/common.service'; // Adjust the import path as necessary
import { TranslationService } from '../core/services/translation.service';
import { ToastrService } from '@core/services/toastr-service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { ArchiveRetentionPeriod } from '@core/domain-classes/archive-retention-period';


@Component({
  selector: 'app-archieve-retention-period',
  imports: [
    ReactiveFormsModule,
    TranslateModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatSelectModule,
    PageHelpTextComponent,
    MatCardModule
  ],
  templateUrl: './archieve-retention-period.component.html',
  styleUrl: './archieve-retention-period.component.scss'
})
export class ArchieveRetentionPeriodComponent implements OnInit {
  formBuilder = inject(FormBuilder);
  archieveRetentionPeriodForm: FormGroup;
  commonService = inject(CommonService); // Assuming DocumentService is provided in the module
  toastrService = inject(ToastrService);
  translationService = inject(TranslationService);

  ngOnInit(): void {
    this.createForm();
    this.getArchiveRetentionPeriod();
  }

  createForm() {
    this.archieveRetentionPeriodForm = this.formBuilder.group({
      retentionPeriodInDays: ['0'],
      isEnabled: [false]
    });
  }

  getArchiveRetentionPeriod() {
    this.commonService.getarchiveRetentionPeriod().subscribe({
      next: (response) => {
        if (response) {
          this.archieveRetentionPeriodForm.patchValue({
            retentionPeriodInDays: response.retentionPeriodInDays > 0 ? response.retentionPeriodInDays.toString() : null,
            isEnabled: response.isEnabled
          });
        } else {
          //this.toastrService.error(this.translationService.getValue('FAILED_TO_LOAD_ARCHIVE_RETENTION_PERIOD'));
        }
      },
      error: (error: any) => {
        this.toastrService.error(this.translationService.getValue('FAILED_TO_LOAD_ARCHIVE_RETENTION_PERIOD'));
      }
    });
  }

  onSubmit() {
    if (!this.archieveRetentionPeriodForm.valid) {
      this.archieveRetentionPeriodForm.markAllAsTouched();
      return;
    }
    const archiveRetentionPeriod: ArchiveRetentionPeriod = {
      retentionPeriodInDays: this.archieveRetentionPeriodForm.value.retentionPeriodInDays ? Number(this.archieveRetentionPeriodForm.value.retentionPeriodInDays) : 0,
      isEnabled: this.archieveRetentionPeriodForm.value.isEnabled ? this.archieveRetentionPeriodForm.value.isEnabled : false
    };
    this.commonService.archiveRetentionPeriod(archiveRetentionPeriod).subscribe({
      next: (response: boolean) => {
        if (response) {
          this.toastrService.success(this.translationService.getValue('ARCHIVE_RETENTION_PERIOD_UPDATED_SUCCESSFULLY'));
        } else {
          this.toastrService.error(this.translationService.getValue('FAILED_TO_UPDATE_ARCHIVE_RETENTION_PERIOD'));
        }
      },
      error: (error: any) => {
        this.toastrService.error(this.translationService.getValue('FAILED_TO_UPDATE_ARCHIVE_RETENTION_PERIOD'));
      }
    });
  }
}
