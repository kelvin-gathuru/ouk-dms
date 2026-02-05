import { Category } from './category';
import { Client } from './client';
import { DocumentInfo } from './document-info';
import { DocumentStatus } from '../../document-status/document-status';

export interface DocumentCategoryStatus {
  document: DocumentInfo;
  categories: Category[];
  documentStatuses: DocumentStatus[];
  clients: Client[];
  isCategoryReadonly?: boolean;
}
