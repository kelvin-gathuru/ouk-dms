import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmailSmtpSettingService } from '../email-smtp-setting.service';
import { TranslationService } from '@core/services/translation.service';
import { ToastrService } from '@core/services/toastr-service';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { EmailSMTPSetting } from '@core/domain-classes/email-smtp-setting';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-test-smtp-setting',
  standalone: true,
  templateUrl: './test-smtp-setting.component.html',
  styleUrl: './test-smtp-setting.component.scss',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    HasClaimDirective,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
  ]
})
export class TestSmtpSettingComponent {
  smtpForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<TestSmtpSettingComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EmailSMTPSetting,
    private fb: FormBuilder, private emailSmtpSettingService: EmailSmtpSettingService,
    private toastrService: ToastrService, private translationService: TranslationService) {
    this.smtpForm = this.fb.group({
      toEmail: ['', [Validators.required, Validators.email]],
    });
  }

  test() {
    if (this.smtpForm.valid) {
      this.data.toEmail = this.smtpForm.get('toEmail')?.value;
      this.emailSmtpSettingService.testEmailSMTPSetting(this.data).subscribe(() => {
        this.toastrService.success(this.translationService.getValue('EMAIL_SENT_SUCCESSFULLY'));
        this.onCancel();
      });
    } else {
      this.smtpForm.markAllAsTouched();
      return;
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
