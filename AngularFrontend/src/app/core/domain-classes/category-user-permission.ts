import { User } from "./user";

export interface CategoryUserPermission {
  id?: string;
  categoryId: string;
  userId: string;
  startDate: Date;
  endDate: Date;
  user?: User
}
