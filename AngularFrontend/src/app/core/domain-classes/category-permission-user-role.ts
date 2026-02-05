export interface CategoryPermissionUserRole {
  roles: Array<string>;
  users: Array<string>;
  isTimeBound: false;
  startDate: Date;
  endDate: Date;
  isAllowDownload: boolean;
  categories: string[];
  isAllowEmailNotification: boolean;
}
