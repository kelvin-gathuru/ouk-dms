import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmailSmtpSettingListComponent } from './email-smtp-setting-list/email-smtp-setting-list.component';
import { AuthGuard } from '@core/security/auth.guard';
import { ManageEmailSmtpSettingComponent } from './manage-email-smtp-setting/manage-email-smtp-setting.component';
import { EmailSMTPSettingDetailResolver } from './email-settting-detail.resolver';

export const EMAIL_SMTP_SETTING_ROUTES: Routes = [
  {
    path: '',
    component: EmailSmtpSettingListComponent,
    canActivate: [AuthGuard],
    data: { claimType: 'VIEW_SMTP_SETTINGS' },
  }, {
    path: 'manage/:id',
    component: ManageEmailSmtpSettingComponent,
    resolve: { smtpSetting: EmailSMTPSettingDetailResolver },
    canActivate: [AuthGuard],
    data: { claimType: 'EDIT_SMTP_SETTING' },
  }, {
    path: 'manage',
    component: ManageEmailSmtpSettingComponent,
    canActivate: [AuthGuard],
    data: { claimType: 'CREATE_SMTP_SETTING' },
  },
];



