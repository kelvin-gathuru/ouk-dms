import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Role } from '@core/domain-classes/role';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { Action } from '@core/domain-classes/action';
import { Page } from '@core/domain-classes/page';
import { BaseComponent } from '../../base.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-manage-role-presentation',
  templateUrl: './manage-role-presentation.component.html',
  styleUrls: ['./manage-role-presentation.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    FormsModule,
    MatCheckboxModule,
    RouterLink,
    TranslateModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule
  ]
})
export class ManageRolePresentationComponent extends BaseComponent implements OnInit {
  @Input() pages: Page[];
  @Input() loading: boolean;
  @Input() role: Role;
  @Output() onManageRoleAction: EventEmitter<Role> =
    new EventEmitter<Role>();

  constructor() {
    super();
  }

  ngOnInit(): void { }

  onPageSelect(event: MatCheckboxChange, page: Page) {
    if (event.checked) {
      if (!this.role.roleClaims) {
        this.role.roleClaims = [];
      }
      page.pageActions.forEach((action) => {
        if (!this.checkPermission(action.id)) {
          this.role.roleClaims?.push({
            roleId: this.role.id,
            claimType: action.code,
            claimValue: '',
            pageActionId: action.id,
          });
        }
      });
    } else {
      var actions = page.pageActions?.map((c) => c.id);
      this.role.roleClaims = Array.isArray(this.role.roleClaims) ? this.role.roleClaims.filter(
        (c) => actions.indexOf(c.pageActionId) < 0
      ) : [];
    }
  }

  selecetAll(event: MatCheckboxChange) {
    if (event.checked) {
      if (!this.role.roleClaims) {
        this.role.roleClaims = [];
      }
      this.pages.forEach((page) => {
        page.pageActions.forEach((action) => {
          if (!this.checkPermission(action.id)) {
            this.role.roleClaims?.push({
              roleId: this.role.id,
              claimType: action.code,
              claimValue: '',
              pageActionId: action.id,
            });
          }
        });
      });
    } else {
      this.role.roleClaims = [];
    }
  }

  checkPermission(actionId: string): boolean {
    const pageAction = this.role.roleClaims?.find(
      (c) => c.pageActionId === actionId
    );
    if (pageAction) {
      return true;
    } else {
      return false;
    }
  }

  onPermissionChange(flag: any, page: Page, action: Action) {
    if (flag.checked) {
      if (!this.role.roleClaims) {
        this.role.roleClaims = [];
      }
      this.role.roleClaims?.push({
        roleId: this.role.id,
        claimType: action.code,
        claimValue: '',
        pageActionId: action.id,
      });
    } else {
      const roleClaimToRemove = this.role.roleClaims?.find(
        (c) => c.pageActionId === action.id
      );
      if (roleClaimToRemove) {
        const index = this.role.roleClaims?.indexOf(roleClaimToRemove, 0);
        if (index !== undefined && index > -1) {
          this.role.roleClaims?.splice(index, 1);
        }
      }
    }
  }

  saveRole(): void {
    this.onManageRoleAction.emit(this.role);
  }
}
