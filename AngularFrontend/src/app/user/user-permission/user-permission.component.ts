import { Component, computed, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { ActivatedRoute, Router } from '@angular/router';

import { ToastrService } from '@core/services/toastr-service';
import { User } from '@core/domain-classes/user';
import { UserService } from '../user.service';
import { UserPermissionPresentationComponent } from '../user-permission-presentation/user-permission-presentation.component';
import { PageStore } from '../../page/store/page-store';
import { ActionStore } from '../../page/store/action-store';

@Component({
  selector: 'app-user-permission',
  templateUrl: './user-permission.component.html',
  styleUrls: ['./user-permission.component.scss'],
  standalone: true,
  imports: [
    UserPermissionPresentationComponent
  ]
})
export class UserPermissionComponent extends BaseComponent implements OnInit {
  user: User;
  actionStore = inject(ActionStore);
  pageStore = inject(PageStore);

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private userService: UserService
  ) {
    super();
  }

  pagesWithActions = computed(() => {
    const pages = this.pageStore.pages();
    const actions = this.actionStore.actions();

    return pages.map(page => ({
      ...page,
      pageActions: Array.isArray(actions) ? actions.filter(action => action.pageId === page.id) : []
    }));
  });


  ngOnInit(): void {
    this.sub$.sink = this.activeRoute.data.subscribe(
      (data: any) => {
        this.user = data.user;
      });
  }

  manageUserClaimAction(user: User): void {
    const userClaims = user.userClaims?.length ? user.userClaims : [];
    this.sub$.sink = this.userService.updateUserClaim(userClaims, user.id ?? '').subscribe(() => {
      this.toastrService.success(this.translationService.getValue('USER_PERMISSION_UPDATED_SUCCESSFULLY'));
      this.router.navigate(['/users']);
    })
  }
}
