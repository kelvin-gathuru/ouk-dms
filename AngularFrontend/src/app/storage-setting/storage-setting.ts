import { StorageType } from "./storage-type-enum";

export interface StorageSetting<T> {
  id?: string;
  storageType: StorageType;
  name: string;
  isDefault: boolean;
  enableEncryption: boolean;
  jsonValue?: T;
  isEditing?: boolean;
}

export interface AWSS3storage {
  accessKey: string;
  secretKey: string;
  bucketName: string;
  region: string;
}

export interface CloudflareStorage {
  accessKey: string;
  accountID: string;
  secretKey: string;
  bucketName: string;
  region: string;
}
