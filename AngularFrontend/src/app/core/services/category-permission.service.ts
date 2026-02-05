import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { catchError, Observable } from 'rxjs';
import { CategoryPermission } from '../domain-classes/category-permission';
import { CommonError } from '@core/error-handler/common-error';
import { CategoryUserPermission } from '../domain-classes/category-user-permission';
import { CategoryRolePermission } from '../domain-classes/category-role-permission';
import { CategoryPermissionUserRole } from '../domain-classes/category-permission-user-role';

@Injectable({ providedIn: 'root' })
export class CategoryPermissionService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService) {
  }

  getCategoryPermission(id: string): Observable<CategoryPermission[]> {
    const url = `CategoryRolePermission/${id}`;
    return this.httpClient.get<CategoryPermission[]>(url);

  }

  deleteCategoryUserPermission(id: string): Observable<void | CommonError> {
    const url = `CategoryUserPermission/${id}`;
    return this.httpClient.delete<void>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteCategoryRolePermission(id: string): Observable<void | CommonError> {
    const url = `CategoryRolePermission/${id}`;
    return this.httpClient.delete<void>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addCategoryUserPermission(CategoryUserPermissions: CategoryUserPermission[]): Observable<void | CommonError> {
    const url = 'CategoryUserPermission';
    return this.httpClient.post<void>(url, { CategoryUserPermissions })
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addCategoryRolePermission(CategoryRolePermissions: CategoryRolePermission[]): Observable<void | CommonError> {
    const url = 'CategoryRolePermission';
    return this.httpClient.post<void>(url, { CategoryRolePermissions })
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  multipleCategorysToUsersAndRoles(permissionUserRole: CategoryPermissionUserRole): Observable<boolean | CommonError> {
    const url = 'CategoryRolePermission/multiple';
    return this.httpClient.post<boolean>(url, permissionUserRole)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

}
