import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Role } from '@core/domain-classes/role';
import { Observable } from 'rxjs';
import { RoleService } from './role.service';

@Injectable({
  providedIn: 'root'
})
export class RoleDetailResolverService {
  constructor(private roleService: RoleService) { }
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<Role> {
    const name = route.paramMap.get('id');
    return this.roleService.getRole(name ?? '') as Observable<Role>;
  }
}
