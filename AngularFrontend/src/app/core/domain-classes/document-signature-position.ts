export interface DocumentSignaturePosition {
  documentId: string;
  signatureUrl: string;
  userName: string;
  pageNumber: number;
  yAxis: number;
  xAxis: number;
  viewportWidth: number;
  viewportHeight: number;
  password?: string;
}
