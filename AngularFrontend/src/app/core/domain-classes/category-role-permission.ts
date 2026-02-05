import { Role } from "./role";

export interface CategoryRolePermission {
  id?: string;
  categoryId: string;
  roleId: string;
  startDate: Date;
  endDate: Date;
  role?: Role
}
