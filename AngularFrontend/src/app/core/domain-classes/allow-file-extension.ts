import { FileType } from "./file-type.enum";

export class AllowFileExtension {
  id?: string;
  fileType: FileType;
  extension: string;
  extensions?: string[];
}
