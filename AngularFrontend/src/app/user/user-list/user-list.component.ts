import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router, RouterModule } from '@angular/router';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { User } from '@core/domain-classes/user';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { ResetPasswordComponent } from '../reset-password/reset-password.component';
import { UserService } from '../user.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { LoginAuditResource } from '@core/domain-classes/login-audit-resource';
import {
  Observable,
  merge,
  tap,
  fromEvent,
  debounceTime,
  distinctUntilChanged,
} from 'rxjs';
import { UserDataSource } from './user-datasource';
import { ResourceParameter } from '@core/domain-classes/resource-parameter';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatTableModule } from '@angular/material/table';
import { TranslateModule } from '@ngx-translate/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { AsyncPipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
  standalone: true,
  imports: [
    MatPaginator,
    MatSort,
    RouterModule,
    TranslateModule,
    PageHelpTextComponent,
    HasClaimDirective,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    AsyncPipe,
    MatProgressSpinnerModule
  ],
})
export class UserListComponent extends BaseComponent implements OnInit {
  dataSource: UserDataSource;
  displayedColumns: string[] = [
    'action',
    'email',
    'firstName',
    'lastName',
    'phoneNumber',
    'isDeleted',
  ];
  isLoadingResults = true;
  userResource: ResourceParameter;
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  footerToDisplayed = ['footer'];

  constructor(
    private userService: UserService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private commonDialogService: CommonDialogService,
    private dialog: MatDialog,
    private router: Router

  ) {
    super();
    this.userResource = new LoginAuditResource();
    this.userResource.pageSize = 10;
    this.userResource.orderBy = 'email asc';
  }

  ngOnInit(): void {
    this.dataSource = new UserDataSource(this.userService);
    this.dataSource.loadUsers(this.userResource);
    this.getResourceParameter();
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap((c: any) => {
          this.userResource.skip =
            this.paginator.pageIndex * this.paginator.pageSize;
          this.userResource.pageSize = this.paginator.pageSize;
          this.userResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadUsers(this.userResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.userResource.skip = 0;
          this.userResource.searchQuery = this.input.nativeElement.value;
          this.dataSource.loadUsers(this.userResource);
        })
      )
      .subscribe();
  }

  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.userResource.pageSize = c.pageSize;
          this.userResource.skip = c.skip;
          this.userResource.totalCount = c.totalCount;
        }
      }
    );
  }

  deleteUser(user: User) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(
        `${this.translationService.getValue(
          'ARE_YOU_SURE_YOU_WANT_TO_DELETE'
        )} ${user.userName}`
      )
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.userService
            .deleteUser(user.id ?? '')
            .subscribe(() => {
              this.toastrService.success(
                this.translationService.getValue('USER_DELETE_SUCCESSFULLY')
              );
              this.dataSource.loadUsers(this.userResource);
            });
        }
      });
  }

  resetPassword(user: User): void {
    this.dialog.open(ResetPasswordComponent, {
      width: '100%',
      maxWidth: '500px',
      data: Object.assign({}, user),
    });
  }

  editUser(userId: string) {
    this.router.navigate(['/users/manage', userId]);
  }

  userPermission(userId: string) {
    this.router.navigate(['/users/permission', userId]);
  }
}
