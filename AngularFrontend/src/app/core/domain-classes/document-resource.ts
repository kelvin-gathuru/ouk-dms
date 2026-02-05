import { ResourceParameter } from './resource-parameter';

export class DocumentResource extends ResourceParameter {
  id?: string = '';
  createdBy?: string = '';
  categoryId?: string = '';
  documentStatusId?: string = '';
  storageSettingId?: string = '';
  clientId?: string = '';
  createDate?: Date;
  operation?: string = '';
  documentNumber?: string = '';
  startDate?: Date;
  endDate?: Date;
  metaTagsTypeId?: string = '';
}
