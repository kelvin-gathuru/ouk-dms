import { TableSetting } from "../domain-classes/table-setting";

export const DocumentsTableSettings: TableSetting[] = [{
  key: 'select',
  header: 'select',
  width: 50,
  type: 'text',
  isVisible: true,
  orderNumber: 1,
  allowSort: false
}, {
  key: 'action',
  header: 'ACTION',
  width: 50,
  type: 'text',
  isVisible: true,
  orderNumber: 2,
  allowSort: false
},
{
  key: 'documentNumber',
  header: 'DOCUMENT_NO',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 3,
  allowSort: true
},
{
  key: 'currentWorkflow',
  header: 'WORKFLOW',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 4,
  allowSort: false
}
  , {
  key: 'name',
  header: 'NAME',
  width: 200,
  type: 'text',
  isVisible: true,
  orderNumber: 5,
  allowSort: true
}, {
  key: 'categoryName',
  header: 'CATEGORY',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 6,
  allowSort: true
}
  , {
  key: 'createdDate',
  header: 'CREATED_DATE',
  width: 150,
  type: 'datetime',
  isVisible: true,
  orderNumber: 7,
  allowSort: true
}, {
  key: 'createdBy',
  header: 'CREATED_BY',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 8,
  allowSort: true
}, {
  key: 'documentStatus',
  header: 'STATUS',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 9,
  allowSort: true
}, {
  key: 'isSigned',
  header: 'ISSIGN',
  width: 150,
  type: 'bool',
  isVisible: true,
  orderNumber: 10,
  allowSort: false
}, {
  key: 'signByDate',
  header: 'SIGNDATE',
  width: 150,
  type: 'datetime',
  isVisible: true,
  orderNumber: 11,
  allowSort: false
}, {
  key: 'storageType',
  header: 'STORAGE',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 12,
  allowSort: true
},
{
  key: 'client',
  header: 'CLIENT',
  width: 150,
  type: 'text',
  isVisible: true,
  orderNumber: 13,
  allowSort: false
}];
