import { Component, OnInit } from '@angular/core';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { Role } from '@core/domain-classes/role';
import { CommonError } from '@core/error-handler/common-error';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { RoleService } from '../role.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    MatTableModule,
    RouterLink,
    HasClaimDirective,
    TranslateModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ]
})
export class RoleListComponent extends BaseComponent implements OnInit {

  roles: Role[] = [];
  displayedColumns: string[] = ['action', 'name'];
  isLoadingResults = true;

  constructor(
    private roleService: RoleService,
    private toastrService: ToastrService,
    private commonDialogService: CommonDialogService,
    private commonService: CommonService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getRoles();
  }

  deleteRole(role: Role) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${role.name}`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.sub$.sink = this.roleService.deleteRole(role.id ?? '').subscribe(() => {
            this.toastrService.success(this.translationService.getValue('ROLE_DELETED_SUCCESSFULLY'));
            this.getRoles();
          });
        }
      });
  }

  getRoles(): void {
    this.isLoadingResults = true;
    this.sub$.sink = this.commonService.getRoles()
      .subscribe((data: Role[]) => {
        this.isLoadingResults = false;
        this.roles = data;
      }, (err: CommonError) => {
        err.messages.forEach(msg => {
          this.toastrService.error(msg)
        });
      });
  }

}
