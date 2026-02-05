import { Component, inject, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentPermission } from '@core/domain-classes/document-permission';
import { DocumentRolePermission } from '@core/domain-classes/document-role-permission';
import { DocumentUserPermission } from '@core/domain-classes/document-user-permission';
import { Role } from '@core/domain-classes/role';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../../base.component';
import { DocumentPermissionService } from '../document-permission.service';
import { ManageRolePermissionComponent } from '../manage-role-permission/manage-role-permission.component';
import { ManageUserPermissionComponent } from '../manage-user-permission/manage-user-permission.component';
import { UserStore } from '../../../user/store/user.store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-document-permission-list',
  templateUrl: './document-permission-list.component.html',
  styleUrls: ['./document-permission-list.component.scss'],
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    PageHelpTextComponent,
    TranslateModule,
    HasClaimDirective,
    UTCToLocalTime,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ]
})

export class DocumentPermissionListComponent extends BaseComponent implements OnInit {
  documentPermissions: DocumentPermission[] = [];
  document: DocumentInfo;
  isDocumentUpdate: boolean = false;
  footerToDisplayed = ['footer'];
  userStore = inject(UserStore);
  roles: Role[] = [];
  documentPermissionsColumns = ['action', 'type', 'isAllowDownload', 'name', 'email', 'startDate', 'endDate'];
  permissionsDataSource: MatTableDataSource<DocumentPermission>;
  @ViewChild('userPermissionsPaginator') userPermissionsPaginator: MatPaginator;

  constructor(
    private documentPermissionService: DocumentPermissionService,
    private route: ActivatedRoute,
    private commonDialogService: CommonDialogService,
    private toastrService: ToastrService,
    private dialog: MatDialog,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo,
    private dialogRef: MatDialogRef<DocumentPermissionListComponent>
  ) {
    super();
    this.document = data;
  }

  ngOnInit(): void {
    this.sub$.sink = this.route.params.subscribe(params => {
      this.getDocumentPrmission();
      this.getRoles();
    });
  }

  getDocumentPrmission() {
    this.sub$.sink = this.documentPermissionService.getDoucmentPermission(this.document.id ?? '')
      .subscribe((permission: DocumentPermission[]) => {
        this.documentPermissions = permission;
        this.permissionsDataSource = new MatTableDataSource(this.documentPermissions);
        this.permissionsDataSource.paginator = this.userPermissionsPaginator;
      });
  }

  getRoles() {
    this.sub$.sink = this.commonService.getRoles()
      .subscribe((roles: Role[]) => this.roles = roles);
  }

  deleteDocumentUserPermission(permission: DocumentUserPermission) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')}?`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.documentPermissionService.deleteDocumentUserPermission(permission.id ?? '')
            .subscribe(() => {
              this.toastrService.success(this.translationService.getValue('PERMISSION_DELETED_SUCCESSFULLY'));
              this.isDocumentUpdate = true;
              this.getDocumentPrmission();
            });
        }
      });
  }

  deleteDocumentRolePermission(permission: DocumentRolePermission) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')}?`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.documentPermissionService.deleteDocumentRolePermission(permission.id ?? '')
            .subscribe(() => {
              this.toastrService.success(this.translationService.getValue('PERMISSION_DELETED_SUCCESSFULLY'));
              this.isDocumentUpdate = true;
              this.getDocumentPrmission();
            });
        }
      });
  }

  addDocumentUserPermission(): void {
    const dialogRef = this.dialog.open(ManageUserPermissionComponent, {
      width: '600px',
      data: Object.assign({ users: this.userStore.users(), documentId: this.document.id })
    });
    this.sub$.sink = dialogRef.afterClosed()
      .subscribe((result: Screen) => {
        if (result) {
          this.isDocumentUpdate = true;
          this.getDocumentPrmission();
        }
      });
  }

  addDocumentRolePermission(): void {
    const dialogRef = this.dialog.open(ManageRolePermissionComponent, {
      width: '600px',
      data: Object.assign({ roles: this.roles, documentId: this.document.id })
    });

    this.sub$.sink = dialogRef.afterClosed()
      .subscribe((result: Screen) => {
        if (result) {
          this.isDocumentUpdate = true;
          this.getDocumentPrmission();
        }
      });
  }

  applyPermissionFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toLocaleLowerCase();
    var userPermissions = this.documentPermissions.filter(d =>
      (d.type == 'User' &&
        (
          d.user?.firstName?.toLocaleLowerCase().includes(filterValue) ||
          d.user?.lastName?.toLocaleLowerCase().includes(filterValue) ||
          d.user?.email?.toLocaleLowerCase().includes(filterValue)
        )
      ) ||
      (d.type == 'Role' && d.role?.name?.toLocaleLowerCase().includes(filterValue))
    );
    this.permissionsDataSource = new MatTableDataSource(userPermissions);
    this.permissionsDataSource.paginator = this.userPermissionsPaginator;
  }

  closeDialog() {
    if (this.isDocumentUpdate) {
      this.dialogRef.close('loaded');
      return;
    }
    this.dialogRef.close();
  }
}
