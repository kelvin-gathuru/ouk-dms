import { User } from './user';
import { Role } from './role';

export interface CategoryPermission {
  id?: string;
  categoryId: string;
  userId?: string;
  roleId?: string;
  startDate: Date;
  endDate: Date;
  user?: User;
  role?: Role;
  type: string;
}
