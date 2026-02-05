import { Routes } from '@angular/router';
import { LayoutComponent } from '@core/layout/layout.component';
import { AuthGuard } from '@core/security/auth.guard';
import { TemplateOpenAiResolverService } from './template-openai/template-openai-resolver';

export const routes: Routes = [
  {
    path: 'login',
    loadChildren: () =>
      import('./login/login-routes').then((m) => m.LOGIN_ROUTES),
  },
  {
    path: 'preview/:code',
    loadComponent: () =>
      import('./document/document-link-preview/document-link-preview.component').then((m) => m.DocumentLinkPreviewComponent),
  },
  {
    path: 'activate-license',
    redirectTo: '/assign/list-view',
    pathMatch: 'full'
  },
  {
    path: 'file-requests/preview/:code',
    loadComponent: () =>
      import('./file-request/file-request-preview/file-request-link-preview/file-request-link-preview.component').then((m) => m.FileRequestLinkPreviewComponent),
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'assign/list-view',
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./document-library/document-library-routes').then(
            (m) => m.DOCUMENT_LIBRARY_ROUTES
          ),
      },
      {
        path: 'my-profile',
        canActivate: [AuthGuard],
        loadComponent: () => import('./user/my-profile/my-profile.component').then(c => c.MyProfileComponent),
      },
      {
        path: 'dashboard',
        data: { claimType: ['VIEW_DASHBOARD'] },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./dashboard/dashboard-routes').then((m) => m.DASHBOARD_ROUTES),
      },
      {
        path: 'roles',
        data: {
          claimType: [
            'VIEW_ROLES',
            'EDIT_ROLE',
            'CREATE_ROLE',
            'ASSIGN_USER_ROLE',
          ],
        },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./role/role-routes').then((m) => m.ROLE_ROUTES),
      },
      {
        path: 'users',
        data: {
          claimType: [
            'VIEW_USERS',
            'EDIT_USER',
            'CREATE_USER',
            'ASSIGN_PERMISSION',
          ],
        },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./user/user-routes').then((m) => m.USER_ROUTES),
      },
      {
        path: 'categories',
        data: { claimType: ['VIEW_CATEGORIES'] },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./category/category-routes').then((m) => m.CATEGORY_ROUTES),
      },
      {
        path: 'assign/folder-view',
        canMatch: [AuthGuard],
        loadComponent: () =>
          import('./document-library/folders-view/folders-view.component').then(
            (m) => m.FoldersViewComponent
          ),
      },
      {
        path: 'documents',
        canMatch: [AuthGuard],
        data: {
          claimType: [
            'ALL_VIEW_DOCUMENTS',
            'ALL_CREATE_DOCUMENT',
            'ALL_EDIT_DOCUMENT',
            'ALL_SHARE_DOCUMENT',
            'DEEP_SEARCH',
          ],
        },
        loadChildren: () =>
          import('./document/document-routes').then((m) => m.DOCUMENT_ROUTES),
      },
      {
        path: 'archive-documents',
        data: { claimType: [] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import('./archive-documents/archive-documents.component').then(
            (m) => m.ArchiveDocumentsComponent
          ),
      },
      {
        path: 'archive-folders',
        data: { claimType: [] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import('./archive-folders/archive-folders.component').then(
            (m) => m.ArchiveFoldersComponent
          ),
      },
      {
        path: 'document-audit-trails',
        canMatch: [AuthGuard],
        data: { claimType: ['VIEW_DOCUMENT_AUDIT_TRAIL'] },
        loadChildren: () =>
          import('./document-audit-trail/document-audit-trail-routing').then(
            (m) => m.DOCUMENT_AUDIT_TRAIL_ROUTES
          ),
      },
      {
        path: 'login-audit',
        data: { claimType: ['VIEW_LOGIN_AUDIT_LOGS'] },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./login-audit/login-audit-routes').then(
            (m) => m.LOGIN_AUDIT_ROUTES
          ),
      },
      {
        path: 'recent-activity',
        loadComponent: () =>
          import('./recent-activity/recent-activity.component').then(
            (m) => m.RecentActivityComponent
          ),
      },
      {
        path: 'pages',
        loadComponent: () => import('./page/page-list/page-list.component').then(c => c.PageListComponent),
        canMatch: [AuthGuard],
      },
      {
        path: 'logs',
        loadChildren: () =>
          import('./n-log/n-log-routes').then((m) => m.NLOG_ROUTES),
      },
      {
        path: 'notifications',
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./notification/notification-routes').then(
            (m) => m.NOTIFICATION_ROUTES
          ),
      },
      {
        path: 'reminders',
        canMatch: [AuthGuard],
        data: {
          claimType: [
            'VIEW_REMINDERS',
            'CREATE_REMINDER',
            'EDIT_REMINDER',
          ],
        },
        loadChildren: () =>
          import('./reminder/reminder-routes').then((m) => m.REMINDER_ROUTES),
      },
      {
        path: 'email-smtp',
        data: {
          claimType: [
            'VIEW_SMTP_SETTINGS',
            'EDIT_SMTP_SETTING',
            'CREATE_SMTP_SETTING',
          ],
        },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./email-smtp-setting/email-smtp-setting-routes').then(
            (m) => m.EMAIL_SMTP_SETTING_ROUTES
          ),
      },
      {
        path: 'storage-settings',
        canMatch: [AuthGuard],
        data: { claimType: ['MANAGE_STORAGE_SETTINGS'] },
        loadComponent: () =>
          import(
            './storage-setting/storage-setting-list/storage-setting-list.component'
          ).then((m) => m.StorageSettingListComponent),
      },
      {
        path: 'document-status',
        canMatch: [AuthGuard],
        data: { claimType: ['MANAGE_DOCUMENT_STATUS'] },
        loadComponent: () =>
          import(
            './document-status/document-status-list/document-status-list.component'
          ).then((m) => m.DocumentStatusListComponent),
      },
      {
        path: 'company-profile',
        canMatch: [AuthGuard],
        data: { claimType: ['MANAGE_COMPANY_SETTINGS'] },
        loadComponent: () =>
          import('./company-profile/company-profile.component').then(
            (m) => m.CompanyProfileComponent
          ),
      },
      {
        path: 'allow-file-extension',
        canMatch: [AuthGuard],
        data: { claimType: ['VIEW_ALLOW_FILE_EXTENSIONS'] },
        loadComponent: () =>
          import(
            './allow-file-extension/allow-file-extension-list/allow-file-extension-list.component'
          ).then((m) => m.AllowFileExtensionListComponent),
      },
      {
        path: 'allow-file-extension/manage',
        data: {
          claimType: [
            'ADD_ALLOW_FILE_EXTENSIONS',
            'EDIT_ALLOW_FILE_EXTENSIONS',
          ],
        },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./allow-file-extension/allow-file-extension-routes').then(
            (m) => m.ALLOW_FILE_EXTENSION_ROUTES
          ),
      },
      {
        path: 'document-meta-tag',
        canMatch: [AuthGuard],
        data: { claimType: ['VIEW_DOCUMENT_META_TAGS'] },
        loadComponent: () =>
          import(
            './document-meta-tag/document-meta-tag-list/document-meta-tag-list.component'
          ).then((m) => m.DocumentMetaTagListComponent),
      },
      {
        path: 'document-meta-tag/manage',
        canMatch: [AuthGuard],
        data: {
          claimType: [
            'ADD_DOCUMENT_META_TAGS',
            'EDIT_DOCUMENT_META_TAGS',
          ],
        },
        loadChildren: () =>
          import('./document-meta-tag/document-meta-tag-routes').then(
            (m) => m.DOCUMENT_META_TAG_ROUTES
          ),
      },
      {
        path: 'client',
        data: { claimType: ['ADD_CLIENTS', 'EDIT_CLIENTS'] },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./client/client-routes').then((m) => m.CLIENT_ROUTES),
      },
      {
        path: 'page-helper',
        data: { claimType: ['MANAGE_PAGE_HELPER'] },
        loadChildren: () =>
          import('./page-helper/page-helper-routes').then(
            (m) => m.PAGE_HELPER_ROUTES
          ),
      },
      {
        path: 'workflow-settings',
        data: { claimType: ['VIEW_WORKFLOW_SETTINGS'] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import('./workflows/workflow-list/workflow-list.component').then(
            (m) => m.WorkflowListComponent
          ),
      },
      {
        path: 'workflowlogs',
        data: { claimType: ['WORKFLOW_LOGS'] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import(
            './workflows/workflowlogs/workflow-log-list/workflow-log-list.component'
          ).then((m) => m.WorkflowLogListComponent),
      },
      {
        path: 'current-workflow',
        data: { claimType: ['CURRENT_WORKFLOW'] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import(
            './workflows/manage-current-workflow/manage-current-workflow.component'
          ).then((m) => m.ManageCurrentWorkflowComponent),
      },
      {
        path: 'workflows',
        data: { claimType: ['WORKFLOWS'] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import(
            './workflows/manage-all-workflow/manage-all-workflow.component'
          ).then((m) => m.ManageAllWorkflowComponent),
      },
      {
        path: 'workflow-settings/manage',
        data: {
          claimType: [
            'ADD_WORKFLOW_SETTINGS',
            'EDIT_WORKFLOW_SETTINGS',
          ],
        },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./workflows/manage-workflow/workflow-routes').then(
            (m) => m.WORKFLOW_ROUTES
          ),
      },
      {
        path: 'file-request',
        data: { claimType: ['VIEW_FILE_REQUEST'] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import(
            './file-request/file-request-list/file-request-list.component'
          ).then((m) => m.FileRequestListComponent),
      },
      {
        path: 'file-request/manage',
        data: {
          claimType: [
            'ADD_FILE_REQUEST',
            'EDIT_FILE_REQUEST',
          ],
        },
        canMatch: [AuthGuard],
        loadChildren: () =>
          import('./file-request/file-request-routes').then(
            (m) => m.FILE_REQUEST_ROUTES
          ),
      },
      {
        path: 'bulk-document-upload',
        data: { claimType: ['BULK_DOCUMENT_UPLOADS'] },
        canMatch: [AuthGuard],
        loadComponent: () =>
          import('./bulk-document-upload/bulk-document-upload.component').then(
            (m) => m.BulkDocumentUploadComponent
          ),
      },
      {
        path: 'aiprompttemplate',
        canMatch: [AuthGuard],
        data: { claimType: ['VIEW_PROMPT_TEMPLATES'] },
        loadComponent: () =>
          import('./template-openai/template-openai-list.component').then(
            (m) => m.TemplateOpenaiListComponent
          ),
      },
      {
        path: 'aiprompttemplate/:id',
        data: { claimType: ['Edit_Prompt_Templates'] },
        canMatch: [AuthGuard],
        resolve: {
          aIPromptTemplate: TemplateOpenAiResolverService,
        },
        loadComponent: () =>
          import('./template-openai/template-openai.component').then(
            (m) => m.TemplateOpenaiComponent
          ),
      },
      {
        path: 'general-settings',
        canMatch: [AuthGuard],
        data: { claimType: ['GENERAL_SETTINGS'] },
        loadComponent: () =>
          import('./general-settings/general-settings.component').then(
            (m) => m.GeneralSettingsComponent
          ),
      },
      {
        path: 'ai-document-generator',
        canMatch: [AuthGuard],
        data: { claimType: ['AI_DOCUMENT_GENERATOR'] },
        loadComponent: () =>
          import(
            './ai-document-generator/ai-document-generator.component'
          ).then((c) => c.AiDocumentGeneratorComponent),
      },
      {
        path: 'ai-document-generator-list',
        canMatch: [AuthGuard],
        data: { claimType: ['VIEW_AI_DOCUMENT_GENERATOR'] },
        loadComponent: () =>
          import(
            './ai-document-generator/ai-document-generator-list/ai-document-generator-list.component'
          ).then((c) => c.AiDocumentGeneratorListComponent),
      },
      {
        path: 'request_document_through_workflow',
        canMatch: [AuthGuard],
        loadComponent: () =>
          import(
            './workflows/manage-all-workflow/request-document-through-workflow/request-document-through-workflow.component'
          ).then((c) => c.RequestDocumentThroughWorkflowComponent),
      },
      {
        path: 'archive-retention-period',
        canMatch: [AuthGuard],
        loadComponent: () =>
          import('./archieve-retention-period/archieve-retention-period.component').then(
            (c) => c.ArchieveRetentionPeriodComponent
          ),
      },
      {
        path: 'remove-license-key',
        loadComponent: () =>
          import('./remove-license-key/remove-license-key.component').then(
            (m) => m.RemoveLicenseKeyComponent
          ),
        canActivate: [AuthGuard],
      },
      {
        path: '**',
        redirectTo: '/assign/list-view',
      },
    ],
  },
];
