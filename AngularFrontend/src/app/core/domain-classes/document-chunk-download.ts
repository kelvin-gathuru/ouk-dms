export interface DocumentChunkDownload {
  data: any;
  contentType: string;
  fileName: string;
  chunkIndex: number;
  blobChunk?: Blob;
}
