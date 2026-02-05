import { DocumentInfo } from './document-info';
import { User } from './user';

export interface RecentActivity {
  id?: string;
  createdDate?: Date;
  name?: string;
  isShared?: boolean;
  folderId?: string;
  documentId?: string;
  action: RecentActivityType;
  thumbnailPath?: string;
  document?: DocumentInfo;
  users?: User[];
}
export enum RecentActivityType {
  VIEWED,
  CREATED
}
