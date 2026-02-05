import { Component, OnInit } from '@angular/core';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { EmailSMTPSetting } from '@core/domain-classes/email-smtp-setting';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { EmailSmtpSettingService } from '../email-smtp-setting.service';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-email-smtp-setting-list',
  templateUrl: './email-smtp-setting-list.component.html',
  styleUrls: ['./email-smtp-setting-list.component.scss'],
  standalone: true,
  imports: [
    MatTableModule,
    MatDialogModule,
    TranslateModule,
    PageHelpTextComponent,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    RouterLink
  ]
})
export class EmailSmtpSettingListComponent extends BaseComponent implements OnInit {
  emailSMTPSettings: EmailSMTPSetting[] = [];
  displayedColumns: string[] = ['action', 'userName', 'host', 'port', 'isDefault'];

  constructor(private emailSmtpSettingService: EmailSmtpSettingService,
    private commonDialogService: CommonDialogService,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getEmailSMTPSettings();
  }

  getEmailSMTPSettings() {
    this.sub$.sink = this.emailSmtpSettingService.getEmailSMTPSettings()
      .subscribe((settings: EmailSMTPSetting[]) => {
        this.emailSMTPSettings = settings;
      })
  }

  deleteEmailSMTPSetting(setting: EmailSMTPSetting) {
    // Assuming `emailSmtpSettings` holds the list of settings
    if (this.emailSMTPSettings.length <= 1) {
      this.toastrService.error(this.translationService.getValue('CANNOT_DELETE_THE_LAST_SMTP_SETTING'));
      return;
    }

    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${setting.host}`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.emailSmtpSettingService.deleteEmailSMTPSetting(setting.id ?? '').subscribe(() => {
            this.toastrService.success(this.translationService.getValue('EMAIL_SMTP_SETUP_DELETED_SUCCESSFULLY'));
            this.getEmailSMTPSettings();
          });
        }
      });
  }

}
