import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpEvent,
  HttpParams,
  HttpResponse,
} from '@angular/common/http';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { BehaviorSubject, Observable, of, throwError, timer } from 'rxjs';
import { User } from '@core/domain-classes/user';
import { catchError, retry, shareReplay, tap } from 'rxjs/operators';
import { Role } from '@core/domain-classes/role';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import {
  reminderFrequencies,
  ReminderFrequency,
} from '@core/domain-classes/reminder-frequency';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { ReminderResourceParameter } from '@core/domain-classes/reminder-resource-parameter';
import { Reminder } from '@core/domain-classes/reminder';
import { DocumentView } from '@core/domain-classes/document-view';
import { ServiceResponse } from '@core/domain-classes/service-response';
import { PageHelper } from '@core/domain-classes/pageHelper';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { ClonerService } from './clone.service';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { DocumentChunkStatus } from '@core/domain-classes/document-chunk-status';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { ArchiveRetentionPeriod } from '@core/domain-classes/archive-retention-period';

@Injectable({ providedIn: 'root' })
export class CommonService {
  maxRetries = 2;
  constructor(
    private httpClient: HttpClient,
    private clonerService: ClonerService
  ) { }
  private _IsSmtpConfigured$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  private _allowFileExtension$: BehaviorSubject<AllowFileExtension[]> =
    new BehaviorSubject<AllowFileExtension[]>([]);
  public get IsSmtpConfigured(): Observable<boolean> {
    return this._IsSmtpConfigured$.asObservable();
  }
  public get allowFileExtension$(): Observable<AllowFileExtension[]> {
    return this._allowFileExtension$.asObservable();
  }
  private _sideMenuStatus$: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(false);
  public get sideMenuStatus$(): Observable<boolean> {
    return this._sideMenuStatus$.asObservable();
  }
  public setSideMenuStatus(flag: boolean) {
    this._sideMenuStatus$.next(flag);
  }

  setIsSmtpConfigured(value: boolean) {
    this._IsSmtpConfigured$.next(value);
  }

  setAllowFileExtension(value: AllowFileExtension[]) {
    if (value) {
      this._allowFileExtension$.next(this.clonerService.deepClone(value));
    } else {
      const allowedExtensions = sessionStorage.getItem('allowFileExtension');
      if (allowedExtensions) {
        this._allowFileExtension$.next(JSON.parse(allowedExtensions));
      }
    }
  }

  getUsersForDropdown(): Observable<User[]> {
    const url = `user/dropdown`;
    return this.httpClient
      .get<User[]>(url);
  }

  getRoles(): Observable<Role[]> {
    const url = `role`;
    return this.httpClient
      .get<Role[]>(url);

  }

  getReminder(id: string): Observable<Reminder> {
    const url = `reminder/${id}`;
    return this.httpClient
      .get<Reminder>(url);
  }

  addDocumentAuditTrail(
    documentAuditTrail: DocumentAuditTrail
  ): Observable<DocumentAuditTrail> {
    return this.httpClient.post<DocumentAuditTrail>(
      'documentAuditTrail',
      documentAuditTrail
    );
  }

  getDocumentChunks(documentId: string, document?: DocumentView): Observable<DocumentChunk[]> {
    let url = '';
    switch (document?.moduleNo) {
      case 2:
        url = `CourseSessionMedia/${document.id}/chunks`;
        break;
      case 3:
        url = `NonConformanceResponseDocument/${documentId}/chunks`;
        break;
      default:
        url = `document/${documentId}/chunks`;
        break;
    }
    return this.httpClient.get<DocumentChunk[]>(url);
  }

  getDocumentSummary(documentId: string): Observable<string> {
    const url = `document/${documentId}/summary`;
    return this.httpClient.get<string>(
      url
    );
  }

  downloadDocumentChunk(documentVersionId: string, chunkIndex: number, document?: DocumentView): Observable<DocumentChunkDownload> {
    let url = '';
    switch (document?.moduleNo) {
      case 1: // Audit
        url = `AuditResponseAttachment/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
      case 2: // Training (Course Session Media)
        url = `CourseSessionMedia/${document.id}/chunk/${chunkIndex}/download`;
        break;
      case 3: // NonConformance
        url = `NonConformanceResponse/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
      case 4: // Capa
        url = `CapaAttachment/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
      case 5: // Risk
        url = `RiskAttachment/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
      case 6: // Complaint
        url = `ComplaintAttachment/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
      case 8: // Supplier
        url = `SupplierCertification/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
      default: // Document or fallback
        url = `document/${documentVersionId}/chunk/${chunkIndex}/download`;
        break;
    }
    return this.httpClient.get<DocumentChunkDownload>(
      url
    ).pipe(
      retry({
        count: this.maxRetries, // Retry up to 2 times
        delay: (error, retryCount) => {
          console.warn(`Retrying chunk... Attempt ${retryCount}`);
          return timer(Math.pow(2, retryCount) * 1000); // Exponential backoff: 1s, 2s, 4s...
        }
      })
    );
  }

  checkDocumentStoreAsChunk(document: DocumentView): Observable<boolean> {
    let url = '';
    switch (document.moduleNo) {
      case 2:
        url = `CourseSessionMedia/${document.id}/isChunked`;
        break;
      default:
        url = `document/${document.documentId}/isChunked`;
        break;
    }
    return this.httpClient.get<boolean>(url);
  }


  downloadDocument(
    documentView: DocumentView
  ): Observable<HttpEvent<Blob>> {
    let url = '';
    if (documentView.isFromPublicPreview) {
      const passowrd = documentView.linkPassword
        ? encodeURIComponent(documentView.linkPassword)
        : '';
      url = `DocumentShareableLink/${documentView.documentId}/download?password=${passowrd}`;
    }
    else if (documentView.isFileRequestDocument) {
      url = `FileRequestDocument/${documentView.documentId}/download`;
    }
    else if (documentView.moduleNo == 2) {
      url = `CourseSessionMedia/${documentView?.id}/download`;
    }
    else if (documentView.moduleNo == 8) {
      url = `SupplierCertification/${documentView.documentId}/download`;
    }
    else {
      if (documentView.documentVersionId) {
        url = `document/${documentView.documentVersionId}/download?isVersion=${documentView.isVersion}`;
      } else {
        url = `document/${documentView.documentId}/download?isVersion=${documentView.isVersion}`;
      }
    }
    return this.httpClient
      .get(url, {
        reportProgress: true,
        observe: 'events',
        responseType: 'blob',
      })
      .pipe(
        retry({
          count: this.maxRetries, // Retry up to 2 times
          delay: (error, retryCount) => {
            console.warn(`Retrying chunk... Attempt ${retryCount}`);
            return timer(Math.pow(2, retryCount) * 1000); // Exponential backoff: 1s, 2s, 4s...
          }
        })
      );
  }

  isDownloadFlag(documentId: string): Observable<boolean> {
    const url = `document/${documentId}/isDownloadFlag`;
    return this.httpClient.get<boolean>(url);
  }

  checkDocumentPermission(documentId: string): Observable<boolean> {
    const url = `document/${documentId}/permission`;
    return this.httpClient.get<boolean>(url);
  }

  //TODO: Need to check if this is used anywhere
  getDocumentToken(
    documentView: Partial<DocumentView> | Partial<MediaPreview>
  ): Observable<{ [key: string]: string }> {
    let url = '';
    switch (documentView.moduleNo) {
      case 1:
        url = `AuditAttachment/${documentView.documentId}/token`;
        break;
      case 3:
        url = `NonConformanceResponseDocument/${documentView.documentId}/token`;
        break;
      case 4:
        url = `CapaAttachment/${documentView.documentId}/token`;
        break;
      case 5:
        url = `RiskAttachment/${documentView.documentId}/token`;
        break;
      case 6:
        url = `ComplaintAttachment/${documentView.documentId}/token`;
        break;
      default:
        if (documentView.isFromPublicPreview) {
          url = `DocumentShareableLink/${documentView.documentId}/token`;
        } else if (documentView.isFileRequestDocument) {
          url = `FileRequestDocument/${documentView.documentId}/token`;
        } else if (documentView.documentVersionId) {
          url = `documentToken/${documentView.documentVersionId}/token`;
        } else {
          url = `documentToken/${documentView.documentId}/token`;
        }
        break;
    }
    return this.httpClient.get<{ [key: string]: string }>(url);
  }

  deleteDocumentToken(token: string): Observable<boolean> {
    const url = `documentToken/${token}`;
    return this.httpClient.delete<boolean>(url);
  }

  readDocument(
    documentView: DocumentView | MediaPreview
  ): Observable<{ [key: string]: string[] }> {
    let url = '';
    switch (documentView?.moduleNo) {
      case 1:
        url = `AuditAttachment/${documentView?.documentId}/readText`;
        break;
      case 2:
        url = `CourseSessionMedia/${documentView?.id}/readText`;
        break;
      case 3:
        url = `NonConformanceResponseDocument/${documentView?.documentId}/readText`;
        break;
      case 4:
        url = `CapaAttachment/${documentView.documentId}/readText`;
        break;
      case 5:
        url = `RiskAttachment/${documentView.documentId}/readText`;
        break;
      case 6:
        url = `ComplaintAttachment/${documentView.documentId}/readText`;
        break;
      default:
        if (documentView.isFromPublicPreview) {
          const passowrd = documentView.linkPassword
            ? encodeURIComponent(documentView.linkPassword)
            : '';
          url = `DocumentShareableLink/${documentView.documentId}/readtext?password=${passowrd}`;
        } else if (documentView.isFileRequestDocument) {
          url = `FileRequestDocument/${documentView.documentId}/readText`;
        } else if (documentView.documentVersionId) {
          url = `document/${documentView.documentVersionId}/readText?isVersion=${documentView.isVersion}`;
        } else {
          url = `document/${documentView.documentId}/readText?isVersion=${documentView.isVersion}`;
        }
        break;
    }
    return this.httpClient.get<{ [key: string]: string[] }>(url);
  }

  getReminderFrequency(): Observable<ReminderFrequency[]> {
    return of(reminderFrequencies);
  }

  addDocumentWithAssign(
    document: Partial<DocumentInfo>
  ): Observable<DocumentInfo> {
    const url = `document/assign`;
    const formData = new FormData();
    formData.append('files', document.file ?? '');
    formData.append('name', document.name ?? '');
    formData.append('categoryId', document.categoryId ?? '');
    formData.append('documentStatusId', document.documentStatusId ?? '');
    formData.append('clientId', document.clientId ?? '');
    formData.append('storageSettingId', document.storageSettingId ?? '');
    formData.append('description', document.description ?? '');
    formData.append('extension', document.extension ?? '');
    formData.append('retentionPeriodInDays', document.retentionPeriodInDays ? document.retentionPeriodInDays?.toString() : '0');
    formData.append('onExpiryAction', document.onExpiryAction ? document.onExpiryAction?.toString() : '0');
    formData.append(
      'documentMetaDataString',
      JSON.stringify(document.documentMetaDatas)
    );
    return this.httpClient
      .post<DocumentInfo>(url, formData);

  }

  markChunkAsUploaded(documentId: string, status: boolean): Observable<DocumentChunkStatus> {
    const url = `document/chunk/uploadStatus`;
    return this.httpClient.post<DocumentChunkStatus>(url, { documentId: documentId, status: status });
  }

  getAllRemindersForCurrentUser(
    resourceParams: ReminderResourceParameter
  ): Observable<HttpResponse<Reminder[]>> {
    const url = 'reminder/all/currentuser';
    const customParams = new HttpParams()
      .set('Fields', resourceParams.fields ? resourceParams.fields : '')
      .set('OrderBy', resourceParams.orderBy ? resourceParams.orderBy : '')
      .set('PageSize', resourceParams.pageSize.toString())
      .set('Skip', resourceParams.skip.toString())
      .set(
        'SearchQuery',
        resourceParams.searchQuery ? resourceParams.searchQuery : ''
      )
      .set('subject', resourceParams.subject ? resourceParams.subject : '')
      .set('message', resourceParams.message ? resourceParams.message : '')
      .set(
        'frequency',
        resourceParams.frequency ? resourceParams.frequency : ''
      );

    return this.httpClient.get<Reminder[]>(url, {
      params: customParams,
      observe: 'response',
    });
  }

  deleteReminderCurrentUser(reminderId: string): Observable<boolean> {
    const url = `reminder/${reminderId}/currentuser`;
    return this.httpClient.delete<boolean>(url);
  }

  // private blobToString(blob) {
  //   const url = URL.createObjectURL(blob);
  //   const xmlRequest = new XMLHttpRequest();
  //   xmlRequest.open('GET', url, false);
  //   xmlRequest.send();
  //   URL.revokeObjectURL(url);
  //   return JSON.parse(xmlRequest.responseText);
  // }

  checkEmailSMTPSetting(): Observable<ServiceResponse<boolean>> {
    const url = 'EmailSMTPSetting/check';
    return this.httpClient.get<ServiceResponse<boolean>>(url).pipe(
      shareReplay(1),
      tap((response) => {
        this._IsSmtpConfigured$.next(response.data);
      })
    );
  }

  getPageHelperText(code: string): Observable<PageHelper> {
    const url = `pagehelper/code/${code}`;
    return this.httpClient.get<PageHelper>(url);
  }
  checkDocumentIsSignedByUser(documentId: string): Observable<boolean> {
    const url = `document/${documentId}/userSignBy`;
    return this.httpClient.get<boolean>(url);
  }

  getAllowFileExtensions(): Observable<AllowFileExtension[]> {
    const allowedExtensions = sessionStorage.getItem('allowFileExtension');
    if (allowedExtensions) {
      this._allowFileExtension$.next(JSON.parse(allowedExtensions));
      return of(JSON.parse(allowedExtensions));
    }
    const url = 'AllowFileExtension';
    return this.httpClient.get<AllowFileExtension[]>(url).pipe(
      tap((allowFileExtension) => {
        this.setAllowFileExtension(allowFileExtension);
        if (allowFileExtension && allowFileExtension.length > 0) {
          sessionStorage.setItem('allowFileExtension', JSON.stringify(allowFileExtension));
        }
      })
    );
  }

  getOptimizedConfig(speedMbps: number): { chunkSize1: number; parallelCalls: number } {
    if (speedMbps < 1) {
      return { chunkSize1: 512 * 1024, parallelCalls: 3 }; // Slow network (512KB chunks, 2 parallel)
    } else if (speedMbps < 5) {
      return { chunkSize1: 1 * 1024 * 1024, parallelCalls: 4 }; // Moderate network (1MB chunks, 3 parallel)
    } else if (speedMbps < 20) {
      return { chunkSize1: 2 * 1024 * 1024, parallelCalls: 6 }; // Fast network (2MB chunks, 5 parallel)
    } else {
      return { chunkSize1: 5 * 1024 * 1024, parallelCalls: 8 }; // Very fast network (5MB chunks, 8 parallel)
    }
  }

  getNetworkSpeed(): { chunkSize1: number; parallelCalls: number } {
    const speedMbps = this.getInternetSpeed();
    console.warn('Speed: ', speedMbps);
    if (speedMbps > 0) {
      return this.getOptimizedConfig(speedMbps);
    }
    // Fallback to default if no network info is available
    return { chunkSize1: 2 * 1024 * 1024, parallelCalls: 3 };
  }

  getInternetSpeed() {
    const connection = (navigator as any).connection || (navigator as any).mozConnection || (navigator as any).webkitConnection;
    if (connection && connection.downlink) {
      const speedMbps = connection.downlink; // Speed in Mbps
      return speedMbps;
    }
    return 0;
  }

  calculateDuration(statDate: Date, endDate: Date) {
    const start = new Date(statDate);
    const end = new Date(endDate);
    const diffMs = end.getTime() - start.getTime();
    const totalSeconds = Math.round(diffMs / 1000);
    const hours = Math.round(totalSeconds / 3600);
    const minutes = Math.round((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;
    return `${hours}h ${minutes}m ${seconds}s`;
  }

  getarchiveRetentionPeriod(): Observable<ArchiveRetentionPeriod> {
    const url = `document/ArchiveRetentionPeriod`;
    return this.httpClient.get<ArchiveRetentionPeriod>(url);
  }
  archiveRetentionPeriod(archiveRetentionPeriod: ArchiveRetentionPeriod): Observable<boolean> {
    const url = `document/ArchiveRetentionPeriod`;
    return this.httpClient.post<boolean>(url, archiveRetentionPeriod);
  }

}
