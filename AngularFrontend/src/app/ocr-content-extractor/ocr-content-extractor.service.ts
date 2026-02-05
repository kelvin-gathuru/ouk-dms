import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonHttpErrorService } from '../core/error-handler/common-http-error.service';
import { catchError, Observable } from 'rxjs';
import { CommonError } from '../core/error-handler/common-error';

@Injectable({ providedIn: 'root' })
export class OCRContentExtractorService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService
  ) { }

  getDocumentContentByOcr(file: File, extension: string): Observable<string | CommonError> {
    const url = `documents/ocr_content_extractor`;
    const formData = new FormData();
    formData.append('file', file);
    formData.append('extension', extension);

    return this.httpClient
      .post<string>(url, formData)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }


}
