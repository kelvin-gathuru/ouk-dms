import { MetaTagType } from "./meta-tag-type.enum";

export interface DocumentMetaData {
  id?: string;
  documentId?: string;
  documentMetaTagId?: string;
  metatag?: string;
  metaTagType?: MetaTagType;
  metaTagDate?: string;
}
