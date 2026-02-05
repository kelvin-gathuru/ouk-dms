export interface DocumentView {
  documentId?: string;
  extension?: string;
  name?: string;
  isVersion: boolean;
  mediaType?: number;
  isFromPublicPreview: boolean;
  isPreviewDownloadEnabled: boolean;
  linkPassword?: string;
  id?: string;
  url?: string;
  isSignatureExists?: boolean;
  isFileRequestDocument: boolean;
  comment?: string;
  isChunk?: boolean;
  documentVersionId?: string;
  documentNumber?: string;
  documentString64?: string;
  totalChunk?: number;
  moduleNo?: number; // QmsModuleEnum
  isAllowDownload?: boolean;
}
