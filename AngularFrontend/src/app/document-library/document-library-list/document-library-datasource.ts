import { CollectionViewer } from '@angular/cdk/collections';
import { DataSource } from '@angular/cdk/table';
import { HttpResponse } from '@angular/common/http';
import { ResponseHeader } from '@core/domain-classes/document-header';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { DocumentLibraryService } from '../document-library.service';

export class DocumentLibraryDataSource implements DataSource<DocumentInfo> {

  private documentsSubject = new BehaviorSubject<DocumentInfo[]>([]);
  private responseHeaderSubject = new BehaviorSubject<ResponseHeader>({} as ResponseHeader);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  private _data: DocumentInfo[];
  public get data(): DocumentInfo[] {
    return this._data;
  }
  public set data(v: DocumentInfo[]) {
    this._data = v;
  }

  public loading$ = this.loadingSubject.asObservable();

  public responseHeaderSubject$ = this.responseHeaderSubject.asObservable();

  constructor(private documentService: DocumentLibraryService) { }

  connect(collectionViewer: CollectionViewer): Observable<DocumentInfo[]> {
    return this.documentsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.documentsSubject.complete();
    this.loadingSubject.complete();
  }

  loadDocuments(documentResource: DocumentResource) {

    this.loadingSubject.next(true);

    this.documentService.getDocuments(documentResource).pipe(
      finalize(() => this.loadingSubject.next(false))
    )
      .subscribe(
        (resp: HttpResponse<DocumentInfo[]>) => {
          const paginationParam = JSON.parse(
            resp?.headers?.get('X-Pagination') ?? '{}'
          ) as ResponseHeader;
          this.responseHeaderSubject.next(paginationParam);

          if (resp && resp.body && resp.body.length > 0) {
            resp?.body?.forEach((document) => {
              document.isIndexable = this.checkFileExtension(document.name);
              document.isMoreThan15MinutesFromLocal = this.isMoreThan15MinutesFromLocal(new Date(document.createdDate));
            });
          }

          this._data = resp?.body ? [...resp?.body] : [];
          this.documentsSubject.next(this._data);
        }

      );
  }

  isMoreThan15MinutesFromLocal(utcDate: Date): boolean {
    // Convert the UTC date to local time using the provided timezone and locale
    const currentDate = new Date(); // Get the current local date and time
    // Get the difference in milliseconds
    const differenceInMs = currentDate.getTime() - utcDate.getTime();
    // Convert milliseconds to minutes
    const differenceInMinutes = differenceInMs / (1000 * 60);
    // Check if the difference is greater than 15 minutes
    return differenceInMinutes > 15;
  }

  checkFileExtension(fileName: string): boolean {
    const fileExtension = fileName.split('.').pop()?.toLowerCase(); // Get the file extension
    switch (fileExtension) {
      case 'pdf':
        return true;
      case 'doc':
      case 'docx':
        return true;
      case 'xls':
      case 'xlsx':
        return true;
      case 'txt':
        return true;
      case 'ppt':
      case 'pptx':
        return true;
      case 'tif':
      case 'tiff':
      case 'jpg':
      case 'jpeg':
      case 'bmp':
      case 'pbm':
      case 'pgm':
      case 'ppm':
      case 'png':
        return true;
      default:
        return false;
    }
  }
}
