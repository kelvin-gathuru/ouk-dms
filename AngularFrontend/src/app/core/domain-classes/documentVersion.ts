export interface DocumentVersion {
  id?: string;
  documentId?: string;
  url?: string;
  CreatedByUser?: string;
  isCurrentVersion?: boolean;
  file?: string | Blob;
  isFileChange?: boolean;
  isSignatureExists?: boolean;
  signBy?: string;
  signDate?: Date;
  extension?: string;
  comment?: string;
  versionNumber?: number;
  isChunk?: boolean;
  modifiedDate?: Date;
  modifiedBy?: string;
  createdByUser?: string;
  createdDate?: Date;

}
