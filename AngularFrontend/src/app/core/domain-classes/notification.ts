import { DocumentInfo } from './document-info';

export class UserNotification {
  id?: string;
  userId?: string;
  message: string;
  createdDate: Date;
  documentName?: string;
  documentId?: string;
  isRead: boolean;
  url?: string;
  document?: DocumentInfo;
  notificationsType?: NotificationType;
  workflowInstanceId?: string;
  documentNumber: string;

}

export enum NotificationType {
  REMINDER = 0,
  SHARE_USER = 1,
  WORKFLOW = 2,
  FILE_REQUEST = 3,
  SHARE_FOLDER = 4,
}
