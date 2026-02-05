export interface DocumentSignature {
  id?: string;
  documentId: string;
  signatureUserId?: string;
  signatureUrl: string;
  signatureDate?: Date;
  data?: string;
  signatureUser?: string;
}
