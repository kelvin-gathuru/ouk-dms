import { MetaTagType } from "./meta-tag-type.enum";

export interface DocumentMetaTag {
    id?: string;
    type?: MetaTagType;
    name: string;
    isEditable?: boolean;    
  }
  