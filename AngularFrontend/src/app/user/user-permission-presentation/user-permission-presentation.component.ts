import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from '@core/domain-classes/user';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { Page } from '@core/domain-classes/page';
import { Action } from '@core/domain-classes/action';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-user-permission-presentation',
  templateUrl: './user-permission-presentation.component.html',
  styleUrls: ['./user-permission-presentation.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    HasClaimDirective,
    MatCheckboxModule,
    RouterModule,
    TranslateModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class UserPermissionPresentationComponent implements OnInit {
  @Input() pages: Page[];
  @Input() user: User;
  @Output() manageUserClaimAction: EventEmitter<User> = new EventEmitter<User>();
  step: number = 0;
  constructor() { }

  ngOnInit(): void {
  }

  checkPermission(actionId: string): boolean {
    const pageAction = this.user?.userClaims?.find(c => c.pageActionId === actionId);
    if (pageAction) {
      return true;
    } else {
      return false;
    }
  }

  onPermissionChange(flag: any, page: Page, action: Action) {
    if (flag.checked) {
      if (!this.user.userClaims) {
        this.user.userClaims = [];
      }
      this.user.userClaims?.push({
        userId: this.user.id,
        claimType: action.code,
        claimValue: '',
        pageActionId: action.id,
        pageId: page.id
      })
    } else {
      const roleClaimToRemove = this.user?.userClaims?.find(c => c.pageActionId === action.id);
      if (roleClaimToRemove) {
        const index = this.user?.userClaims?.indexOf(roleClaimToRemove, 0);
        if (typeof index === 'number' && index > -1) {
          this.user?.userClaims?.splice(index, 1);
        }
      }
    }
  }

  onPageSelect(event: MatCheckboxChange, page: Page) {
    if (event.checked) {
      page.pageActions.forEach(action => {
        if (!this.checkPermission(action.id)) {
          this.onPermissionChange({ checked: true }, page, action);
        }
      });
    } else {
      page.pageActions.forEach(action => {
        if (this.checkPermission(action.id)) {
          this.onPermissionChange({ checked: false }, page, action);
        }
      });
    }
  }

  saveUserClaim(): void {
    // Save user claims
    this.manageUserClaimAction.emit(this.user);
  }
}
