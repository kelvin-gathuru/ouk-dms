export interface Category {
  id?: string;
  name: string;
  description: string;
  parentId?: string;
  deafLevel?: number;
  index?: number;
  children?: Category[];
  displayName?: string;
  createdUserName?: string;
  createdDate: Date;
  isShared?: boolean;
  level?: number;
  isArchive?: boolean;
}
