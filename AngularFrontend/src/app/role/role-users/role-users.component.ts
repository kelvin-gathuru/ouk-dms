import { Component, inject, OnInit } from '@angular/core';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Role } from '@core/domain-classes/role';
import { CommonService } from '@core/services/common.service';
import { UserRoles } from '@core/domain-classes/user-roles';
import { RoleService } from '../role.service';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { UserStore } from '../../user/store/user.store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-role-users',
  templateUrl: './role-users.component.html',
  styleUrls: ['./role-users.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    MatSelectModule,
    DragDropModule,
    FormsModule,
    TranslateModule,
    MatCardModule
  ]
})
export class RoleUsersComponent extends BaseComponent implements OnInit {
  roles: Role[];
  userStore = inject(UserStore);
  selectedRole: Role | null = null;
  roleUsers: UserRoles[] = [];
  otherUsers: UserRoles[] = [];
  selectedRoleId: string;

  constructor(
    private commonService: CommonService,
    private roleService: RoleService,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getRoles();
    this.getUsers();
  }

  addUser(event: CdkDragDrop<UserRoles[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      const userRolesToSave: UserRoles[] = [...this.roleUsers];
      userRolesToSave.push(event.previousContainer.data[event.previousIndex])
      this.sub$.sink = this.roleService.updateRoleUsers(this.selectedRole?.id ?? '', userRolesToSave).subscribe(() => {
        transferArrayItem(event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex);
        this.toastrService.success(this.translationService.getValue('USER_ADDED_SUCCESSFULLY_TO_ROLE'));
      }, () => {
        this.roleUsers.splice(event.previousIndex, 1);
        this.toastrService.error(`Error While Adding User to Role ${this.selectedRole?.name}`);
      });
    }
  }

  removeUser(event: CdkDragDrop<UserRoles[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      const userRolesToSave = this.roleUsers.filter(d => event.previousContainer.data[event.previousIndex].userId != d.userId);
      this.sub$.sink = this.roleService.updateRoleUsers(this.selectedRole?.id ?? '', userRolesToSave).subscribe(() => {
        transferArrayItem(event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex);
        this.toastrService.success(this.translationService.getValue('USER_PERMISSION_UPDATED_SUCCESSFULLY'));
      }, () => {
        this.toastrService.error(`Error While Removing User from Role ${this.selectedRole?.name}`);
      });
    }
  }

  addAllUser() {
    const userRolesToSave = this.userStore.users()
      .filter(ds => !!ds.id)
      .map(ds => {
        return {
          userId: ds.id ?? '',
          roleId: this.selectedRole?.id ?? '',
          userName: ds.userName,
          firstName: ds.firstName,
          lastName: ds.lastName
        }
      });
    this.sub$.sink = this.roleService.updateRoleUsers(this.selectedRole?.id ?? '', userRolesToSave).subscribe(() => {
      this.toastrService.success(`All Users Added Successfully to ${this.selectedRole?.name}`);
      this.roleUsers = userRolesToSave;
      this.otherUsers = [];
    });
  }

  removeAllUser() {
    this.sub$.sink = this.roleService.updateRoleUsers(this.selectedRole?.id ?? '', []).subscribe(() => {
      this.toastrService.success(`All Users Removed Successfully from ${this.selectedRole?.name}`);
      this.roleUsers = [];
      this.otherUsers = this.userStore.users()?.map(ds => {
        return {
          userId: ds.id ?? '',
          roleId: this.selectedRole?.id ?? '',
          userName: ds.userName,
          firstName: ds.firstName,
          lastName: ds.lastName
        }
      });
    });
  }

  onRoleChange() {
    this.selectedRole = this.roles.find(c => c.id === this.selectedRoleId) ?? null;
    this.sub$.sink = this.roleService.getRoleUsers(this.selectedRole?.id ?? '').subscribe((users: UserRoles[]) => {
      this.roleUsers = users;
      const selectedUserIds = this.roleUsers.map(m => m.userId);
      this.otherUsers = this.userStore.users().filter(d => selectedUserIds.indexOf(d.id ?? '') < 0)
        .map(ds => {
          return {
            userId: ds.id ?? '',
            roleId: this.selectedRole?.id ?? '',
            userName: ds.userName,
            firstName: ds.firstName,
            lastName: ds.lastName
          }
        });
    });
  }

  getRoles() {
    this.sub$.sink = this.commonService.getRoles()
      .subscribe((roles: Role[]) => {
        this.roles = roles;
        if (this.roles.length > 0) {
          this.selectedRole = this.roles[0];
          this.selectedRoleId = this.roles[0].id ?? '';
          this.onRoleChange();
        }
      });
  }

  getUsers() {
    this.otherUsers = this.userStore.users().map(ds => {
      return {
        userId: ds.id ?? '',
        roleId: '',
        userName: ds.userName,
        firstName: ds.firstName,
        lastName: ds.lastName
      }
    });
  }
}
