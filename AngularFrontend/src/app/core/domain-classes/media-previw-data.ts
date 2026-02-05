export interface MediaPreview {
  id: string;
  name: string;
  url: string;
  extension?: string;
  isChunked?: boolean;
  isAllChunkUploaded?: boolean;
  mediaType: number;
  isTracking: boolean;
  employeeCourseSessionLink?: string;
  isCourseSessionMedia?: boolean;
  isVersion: boolean;
  isFromPublicPreview: boolean;
  isPreviewDownloadEnabled: boolean;
  linkPassword?: string;
  isSignatureExists?: boolean;
  isFileRequestDocument: boolean;
  comment?: string;
  isChunk?: boolean;
  documentVersionId?: string;
  documentNumber?: string;
  documentString64?: string;
  totalChunk?: number;
  moduleNo?: number; // QmsModuleEnum
  documentId?: string; // For NonConformanceResponseDocument
}
