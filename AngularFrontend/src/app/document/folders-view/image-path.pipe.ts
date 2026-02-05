import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';
import { FileType } from '@core/domain-classes/file-type.enum';

@Pipe({
  name: 'imagePath',
  standalone: true
})
export class ImagePathPipe implements PipeTransform {
  FileType = FileType;
  allowExtesions = environment.allowExtesions;
  transform(value: any, ...args: any[]): any {
    if (!value) {
      return "/images/thumbnails/unknow_file.png";
    }
    const extension = value.split('.').pop().toLowerCase();
    if (extension == "pdf") {
      return "/images/thumbnails/pdf.png";
    } else if (extension == "doc" || extension == "docx") {
      return "/images/thumbnails/word.png";
    } else if (extension == "xls" || extension == "xlsx") {
      return "/images/thumbnails/excel.png";
    } else if (extension == "ppt" || extension == "pptx") {
      return "/images/thumbnails/ppt.png";
    } else if (extension == "zip" || extension == "rar" || extension == "7z") {
      return "/images/thumbnails/zip.png";
    } else if (extension == "txt") {
      return "/images/thumbnails/text.png";
    } else if (extension == "csv") {
      return "/images/thumbnails/csv.png";
    } else if (extension == "json") {
      return "/images/thumbnails/json.png";
    } else if (extension == "sql") {
      return "/images/thumbnails/sql.png";
    } else if (extension == "json") {
      return "/images/thumbnails/ms_db.png";
    } else if (extension == "mdf") {
      return "/images/thumbnails/ms_db.png";
    }
    else {
      const allowTypeExtenstion = this.allowExtesions.find((c) =>
        c.extentions.find(
          (ext) => ext?.toLowerCase() === extension
        )
      );
      if (allowTypeExtenstion?.type == "audio") {
        return "/images/thumbnails/audio.png";
      }
      else if (allowTypeExtenstion?.type == "video") {
        return "/images/thumbnails/video.png";
      }
      else if (allowTypeExtenstion?.type == "image") {
        return "/images/thumbnails/image.png";
      }
      return "/images/thumbnails/unknow_file.png";
    }
  }
}
