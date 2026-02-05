import { Component, OnInit, ViewChild, Inject, inject } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { BaseComponent } from '../../../base.component';
import { CategoryPermission } from '../../../core/domain-classes/category-permission';
import { Category } from '../../../core/domain-classes/category';
import { Role } from '../../../core/domain-classes/role';
import { MatPaginator } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { CommonDialogService } from '../../../core/common-dialog/common-dialog.service';

import { ToastrService } from '@core/services/toastr-service';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CategoryService } from '../../../core/services/category.service';
import { CommonService } from '../../../core/services/common.service';
import { CategoryPermissionService } from '../../../core/services/category-permission.service';
import { CategoryUserPermission } from '../../../core/domain-classes/category-user-permission';
import { CategoryRolePermission } from '../../../core/domain-classes/category-role-permission';
import { ManageUserCategoryPermissionComponent } from '../manage-user-category-permission/manage-user-category-permission.component';
import { ManageRoleCategoryPermissionComponent } from '../manage-role-category-permission/manage-role-category-permission.component';
import { UserStore } from '../../../user/store/user.store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-category-permission-list',
  imports: [
    MatDialogModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    PageHelpTextComponent,
    HasClaimDirective,
    MatPaginator,
    TranslateModule,
    UTCToLocalTime,
    MatCardModule
  ],
  templateUrl: './category-permission-list.component.html',
  styleUrl: './category-permission-list.component.scss'
})
export class CategoryPermissionListComponent extends BaseComponent implements OnInit {
  categoryPermissions: CategoryPermission[] = [];
  category: Category;
  categoryPermissionsChanges = false;
  footerToDisplayed = ['footer'];
  userStore = inject(UserStore);
  roles: Role[] = [];
  categoryPermissionsColumns = ['action', 'type', 'isAllowDownload', 'name', 'email', 'startDate', 'endDate'];
  permissionsDataSource: MatTableDataSource<CategoryPermission>;
  @ViewChild('categoryPermissionsPaginator') categoryPermissionsPaginator: MatPaginator;

  constructor(private categoryService: CategoryService,
    private categoryPermissionService: CategoryPermissionService,
    private route: ActivatedRoute,
    private commonDialogService: CommonDialogService,
    private toastrService: ToastrService,
    private dialog: MatDialog,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: Category,
    private dialogRef: MatDialogRef<CategoryPermissionListComponent>
  ) {
    super();
    this.category = data;
  }

  ngOnInit(): void {
    this.sub$.sink = this.route.params.subscribe(params => {
      this.getCategoryPrmission();
      this.getCategories();
      this.getRoles();
    });
  }

  getCategories(): void {
    this.categoryService.getAllCategories(true);
  }

  getCategoryPrmission() {
    this.sub$.sink = this.categoryPermissionService.getCategoryPermission(this.category.id ?? '')
      .subscribe((permission: CategoryPermission[]) => {
        this.categoryPermissions = permission;
        this.permissionsDataSource = new MatTableDataSource(this.categoryPermissions);
        this.permissionsDataSource.paginator = this.categoryPermissionsPaginator;
      });
  }

  getRoles() {
    this.sub$.sink = this.commonService.getRoles()
      .subscribe((roles: Role[]) => this.roles = roles);
  }

  deleteCategoryUserPermission(permission: CategoryUserPermission) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')}?`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.categoryPermissionService.deleteCategoryUserPermission(permission.id ?? '')
            .subscribe(() => {
              this.toastrService.success(this.translationService.getValue('PERMISSION_DELETED_SUCCESSFULLY'));
              this.getCategoryPrmission();
              this.categoryPermissionsChanges = true;
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
              this.getCategoryPrmission();
              this.categoryPermissionsChanges = true;
            });
        }
      });
  }

  addCategoryUserPermission(): void {
    const dialogRef = this.dialog.open(ManageUserCategoryPermissionComponent, {
      width: '600px',
      data: Object.assign({ users: [...this.userStore?.users()], categoryId: this.category.id })
    });
    this.sub$.sink = dialogRef.afterClosed()
      .subscribe((result: Screen) => {
        if (result) {
          this.getCategoryPrmission();
          this.getCategories();
          this.categoryPermissionsChanges = true;
        }
      });
  }

  addCategoryRolePermission(): void {
    const dialogRef = this.dialog.open(ManageRoleCategoryPermissionComponent, {
      width: '600px',
      data: Object.assign({ roles: this.roles, categoryId: this.category.id })
    });

    this.sub$.sink = dialogRef.afterClosed()
      .subscribe((result: Screen) => {
        if (result) {
          this.getCategoryPrmission();
          this.getCategories();
          this.categoryPermissionsChanges = true;
        }
      });
  }

  applyPermissionFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement)?.value.trim().toLocaleLowerCase();
    var userPermissions = this.categoryPermissions.filter(d => (d.type == 'User' && (d.user?.firstName?.toLocaleLowerCase().includes(filterValue)
      || d?.user?.lastName?.toLocaleLowerCase().includes(filterValue) || d?.user?.email.toLocaleLowerCase().includes(filterValue)))
      || (d.type == 'Role' && d.role?.name?.toLocaleLowerCase().includes(filterValue)))
    this.permissionsDataSource = new MatTableDataSource(userPermissions);
    this.permissionsDataSource.paginator = this.categoryPermissionsPaginator;
  }

  closeDialog() {
    this.dialogRef.close(this.categoryPermissionsChanges);
  }
}
