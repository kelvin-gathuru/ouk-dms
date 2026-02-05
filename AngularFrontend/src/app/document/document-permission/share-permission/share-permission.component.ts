import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { TranslateModule } from '@ngx-translate/core';
import { Component, inject, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
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
import { SharePermission } from '@core/domain-classes/share-permission';
import { CategoryPermission } from '@core/domain-classes/category-permission';
import { CategoryUserPermission } from '@core/domain-classes/category-user-permission';
import { CategoryPermissionService } from '@core/services/category-permission.service';
import { CategoryRolePermission } from '@core/domain-classes/category-role-permission';
import { ManageUserCategoryPermissionComponent } from '../../category-permission/manage-user-category-permission/manage-user-category-permission.component';
import { ManageRoleCategoryPermissionComponent } from '../../category-permission/manage-role-category-permission/manage-role-category-permission.component';
import { UserStore } from '../../../user/store/user.store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-share-permission',
  imports: [
    ReactiveFormsModule,
    MatTableModule,
    MatDialogModule,
    MatPaginatorModule,
    TranslateModule,
    MatButtonModule,
    PageHelpTextComponent,
    HasClaimDirective,
    UTCToLocalTime,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './share-permission.component.html',
  styleUrl: './share-permission.component.scss'
})
export class SharePermissionComponent extends BaseComponent implements OnInit {
  sharePermissions: SharePermission;
  document: DocumentInfo;
  isDocumentUpdate: boolean = false;
  userStore = inject(UserStore);
  footerToDisplayed = ['footer'];
  roles: Role[] = [];
  documentPermissionsColumns = ['action', 'type', 'isAllowDownload', 'name', 'email', 'startDate', 'endDate'];
  categoryPermissionsColumns = ['action', 'type', 'isAllowDownload', 'name', 'email', 'startDate', 'endDate'];
  documentpermissionsDataSource: MatTableDataSource<DocumentPermission>;
  categorypermissionsDataSource: MatTableDataSource<CategoryPermission>;
  @ViewChild('documentPermissionsPaginator') documentPermissionsPaginator: MatPaginator;
  @ViewChild('categoryPermissionsPaginator') categoryPermissionsPaginator: MatPaginator;


  constructor(
    private documentPermissionService: DocumentPermissionService,
    private categoryPermissionService: CategoryPermissionService,
    private route: ActivatedRoute,
    private commonDialogService: CommonDialogService,
    private toastrService: ToastrService,
    private dialog: MatDialog,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo,
    private dialogRef: MatDialogRef<SharePermissionComponent>
  ) {
    super();
    this.document = data;
  }

  ngOnInit(): void {
    this.sub$.sink = this.route.params.subscribe(params => {
      this.getSharePrmission();
      this.getRoles();
    });
  }

  getSharePrmission() {
    this.sub$.sink = this.documentPermissionService.getSharePermission(this.document.id ?? '', this.document.categoryId ?? '')
      .subscribe((permission: SharePermission) => {
        this.sharePermissions = permission;
        this.documentpermissionsDataSource = new MatTableDataSource(this.sharePermissions.documentPermissions);
        this.categorypermissionsDataSource = new MatTableDataSource(this.sharePermissions.categoryPermissions);
        this.documentpermissionsDataSource.paginator = this.documentPermissionsPaginator;
        this.categorypermissionsDataSource.paginator = this.categoryPermissionsPaginator;
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
              this.getSharePrmission();
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
              this.getSharePrmission();
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
          this.getSharePrmission();
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
          this.getSharePrmission();
        }
      });
  }

  applyDocumentPermissionFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toLocaleLowerCase();
    var userPermissions = this.sharePermissions?.documentPermissions?.filter(d =>
      (d.type == 'User' && (
        d.user?.firstName?.toLocaleLowerCase().includes(filterValue) ||
        d.user?.lastName?.toLocaleLowerCase().includes(filterValue) ||
        d.user?.email?.toLocaleLowerCase().includes(filterValue)
      )) ||
      (d.type == 'Role' && d.role?.name?.toLocaleLowerCase().includes(filterValue))
    );
    this.documentpermissionsDataSource = new MatTableDataSource(userPermissions);
    this.documentpermissionsDataSource.paginator = this.documentPermissionsPaginator;
  }

  deleteCategoryUserPermission(permission: CategoryUserPermission) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')}?`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.categoryPermissionService.deleteCategoryUserPermission(permission.id ?? '')
            .subscribe(() => {
              this.toastrService.success(this.translationService.getValue('PERMISSION_DELETED_SUCCESSFULLY'));
              this.isDocumentUpdate = true;
              this.getSharePrmission();
              //this.categoryPermissionsChanges = true;
            });
        }
      });
  }

  deleteCategoryRolePermission(permission: CategoryRolePermission) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')}?`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.categoryPermissionService.deleteCategoryRolePermission(permission.id ?? '')
            .subscribe(() => {
              this.toastrService.success(this.translationService.getValue('PERMISSION_DELETED_SUCCESSFULLY'));
              this.isDocumentUpdate = true;
              this.getSharePrmission();
              //this.categoryPermissionsChanges = true;
            });
        }
      });
  }

  addCategoryUserPermission(): void {
    const dialogRef = this.dialog.open(ManageUserCategoryPermissionComponent, {
      width: '600px',
      data: Object.assign({ users: this.userStore.users(), categoryId: this.document.categoryId })
    });
    this.sub$.sink = dialogRef.afterClosed()
      .subscribe((result: Screen) => {
        if (result) {
          this.isDocumentUpdate = true;
          this.getSharePrmission();
          // this.getCategories();
          //this.categoryPermissionsChanges = true;
        }
      });
  }

  addCategoryRolePermission(): void {
    const dialogRef = this.dialog.open(ManageRoleCategoryPermissionComponent, {
      width: '600px',
      data: Object.assign({ roles: this.roles, categoryId: this.document.categoryId })
    });

    this.sub$.sink = dialogRef.afterClosed()
      .subscribe((result: Screen) => {
        if (result) {
          this.isDocumentUpdate = true;
          this.getSharePrmission();
          //this.getCategories();
          //this.categoryPermissionsChanges = true;
        }
      });
  }

  applyCategoryPermissionFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toLocaleLowerCase();
    var userPermissions = this.sharePermissions?.categoryPermissions?.filter(d => (d.type == 'User' && (d.user?.firstName?.toLocaleLowerCase().includes(filterValue)
      || d?.user?.lastName?.toLocaleLowerCase().includes(filterValue) || d?.user?.email.toLocaleLowerCase().includes(filterValue)))
      || (d.type == 'Role' && d.role?.name?.toLocaleLowerCase().includes(filterValue)))
    this.categorypermissionsDataSource = new MatTableDataSource(userPermissions);
    this.categorypermissionsDataSource.paginator = this.categoryPermissionsPaginator;
  }

  closeDialog() {
    if (this.isDocumentUpdate) {
      this.dialogRef.close('loaded');
      return;
    }
    this.dialogRef.close();
  }
}
