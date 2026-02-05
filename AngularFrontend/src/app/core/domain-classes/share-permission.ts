import { DocumentPermission } from "./document-permission";
import { CategoryPermission } from "./category-permission";

export class SharePermission {
   documentPermissions?: DocumentPermission[];
   categoryPermissions?: CategoryPermission[];
}