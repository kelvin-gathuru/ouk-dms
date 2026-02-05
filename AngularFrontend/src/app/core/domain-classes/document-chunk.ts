export interface DocumentChunk {
  file?: File;
  chunkIndex: number;
  size: number;
  totalChunks: number;
  extension: string;
  documentVersionId: string;
  documentId: string;
}
