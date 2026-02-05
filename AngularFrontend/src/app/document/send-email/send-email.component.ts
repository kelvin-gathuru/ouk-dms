import { Component, Inject, OnInit } from '@angular/core';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { SendEmail } from '@core/domain-classes/send-email';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { EmailSendService } from './email-send.service';
import { Subscription } from 'rxjs';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslateModule } from '@ngx-translate/core';
import { TextEditorComponent } from '@shared/text-editor/text-editor.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrls: ['./send-email.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatTooltipModule,
    TranslateModule,
    TextEditorComponent,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
})
export class SendEmailComponent extends BaseComponent implements OnInit {
  emailForm: UntypedFormGroup;
  subscriptions = new Subscription();
  isLoading = false;
  smtpConfigured: boolean;
  constructor(
    private fb: UntypedFormBuilder,
    private toastrService: ToastrService,
    private emailSendService: EmailSendService,
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo,
    private dialogRef: MatDialogRef<SendEmailComponent>,
    private commonService: CommonService
  ) {
    super();
  }

  ngOnInit(): void {
    this.createEmailForm();
    this.subscriptions.add(
      this.commonService.IsSmtpConfigured.subscribe((isConfigured) => {
        if (isConfigured == null) {
          this.checkEmailSMTPSettings();
          return;
        }
        this.smtpConfigured = isConfigured;
      })
    );
  }
  checkEmailSMTPSettings() {
    this.sub$.sink = this.commonService.checkEmailSMTPSetting().subscribe({
      next: (response: ServiceResponse<boolean>) => {
        if (response && response?.data) {
        }
      },
      error: (error) => {
        this.toastrService.error(
          this.translationService.getValue('SMTP_SETTINGS_ERROR')
        );
      },
    });
  }

  closeDialog() {
    this.dialogRef.close();
  }

  createEmailForm() {
    this.emailForm = this.fb.group({
      id: [''],
      toAddress: ['', [Validators.required, Validators.email]],
      subject: ['', [Validators.required]],
      body: ['', [Validators.required]],
      documentId: [this.data.id, [Validators.required]],
    });
  }

  sendEmail() {
    if (!this.emailForm.valid) {
      this.emailForm.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    this.sub$.sink = this.emailSendService
      .sendEmail(this.buildObject())
      .subscribe({
        next: () => {
          this.addDocumentTrail();
          this.toastrService.success(
            this.translationService.getValue('EMAIL_SENT_SUCCESSFULLY')
          );
          this.dialogRef.close();
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        },
      });
  }

  buildObject() {
    const sendEmail: SendEmail = {
      documentId: this.emailForm.get('documentId')?.value,
      email: this.emailForm.get('toAddress')?.value,
      subject: this.emailForm.get('subject')?.value,
      message: this.emailForm.get('body')?.value,
    };
    return sendEmail;
  }

  addDocumentTrail() {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: this.data.id,
      operationName: DocumentOperation.Send_Email.toString(),
    };

    this.sub$.sink = this.commonService
      .addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe((c) => { });
  }
}
