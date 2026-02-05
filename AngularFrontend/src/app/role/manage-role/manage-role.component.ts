import { Component, computed, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { RoleService } from '../role.service';
import { Role } from '@core/domain-classes/role';
import { ActivatedRoute, Router } from '@angular/router';

import { ToastrService } from '@core/services/toastr-service';
import { Page } from '@core/domain-classes/page';
import { ManageRolePresentationComponent } from '../manage-role-presentation/manage-role-presentation.component';
import { ActionStore } from '../../page/store/action-store';
import { PageStore } from '../../page/store/page-store';

@Component({
  selector: 'app-manage-role',
  templateUrl: './manage-role.component.html',
  styleUrls: ['./manage-role.component.scss'],
  standalone: true,
  imports: [
    ManageRolePresentationComponent
  ]
})
export class ManageRoleComponent extends BaseComponent implements OnInit {
  role: Role;
  actionStore = inject(ActionStore);
  pageStore = inject(PageStore);

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private roleService: RoleService
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
        if (data.role) {
          this.role = data.role;
        } else {
          this.role = {
            roleClaims: [],
            userRoles: []
          };
        }
      });
  }

  manageRole(role: Role): void {
    if (!role.name) {
      this.toastrService.error(this.translationService.getValue('PLEASE_ENTER_ROLE_NAME'));
      return;
    }

    if (role.roleClaims?.length == 0) {
      this.toastrService.error(this.translationService.getValue('PLEASE_SELECT_AT_LEAT_ONE_PERMISSION'));
      return;
    }

    if (!role.id)
      this.sub$.sink = this.roleService.addRole(role).subscribe(() => {
        this.toastrService.success(this.translationService.getValue('ROLE_SAVED_SUCCESSFULLY'));
        this.router.navigate(['/roles']);
      });
    else
      this.sub$.sink = this.roleService.updateRole(role).subscribe(() => {
        this.toastrService.success(this.translationService.getValue('ROLE_UPDATED_SUCCESSFULLY'));
        this.router.navigate(['/roles']);
      });
  }
}
