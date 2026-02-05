import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentView } from '@core/domain-classes/document-view';
import { FileType } from '@core/domain-classes/file-type.enum';
import { ClonerService } from '@core/services/clone.service';
import { CommonService } from '@core/services/common.service';
import { OverlayPanelRef } from '@shared/overlay-panel/overlay-panel-ref';
import { OverlayPanel } from '@shared/overlay-panel/overlay-panel.service';

import { ToastrService } from '@core/services/toastr-service';
import { bufferCount, concatMap, from, mergeMap } from 'rxjs';
import { BaseComponent } from '../../base.component';
import { DocumentChunkDownload } from '@core/domain-classes/document-chunk-download';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import {
  MatProgressSpinnerModule,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { OVERLAY_PANEL_DATA } from '@shared/overlay-panel/overlay-panel-data';
import { ImagePreviewComponent } from '@shared/image-preview/image-preview.component';
import { CSVPreviewComponent } from '@shared/csv-preview/csv-preview.component';
import { OfficeViewerComponent } from '@shared/office-viewer/office-viewer.component';
import { JsonPreviewComponent } from '@shared/json-preview/json-preview.component';
import { PdfDataViewerComponent } from '@shared/pdf-viewer/pdf-viewer.component';
import { VideoPreviewComponent } from '@shared/video-preview/video-preview.component';
import { TextPreviewComponent } from '@shared/text-preview/text-preview.component';
import { AudioPreviewComponent } from '@shared/audio-preview/audio-preview.component';
import { YoutubeVideoViewerComponent } from '@shared/youtube-video-viewer/youtube-video-viewer.component';
import { VimeoVideoViewerComponent } from '@shared/vimeo-video-viewer/vimeo-video-viewer.component';
import { NoPreviewComponent } from '@shared/no-preview/no-preview.component';
import { MatIconModule } from '@angular/material/icon';
import { LimitToPipe } from '@shared/pipes/limit-to.pipe';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-base-preview',
  templateUrl: './base-preview.component.html',
  styleUrls: ['./base-preview.component.scss'],
  standalone: true,
  imports: [
    ImagePreviewComponent,
    CSVPreviewComponent,
    OfficeViewerComponent,
    JsonPreviewComponent,
    PdfDataViewerComponent,
    VideoPreviewComponent,
    TextPreviewComponent,
    AudioPreviewComponent,
    YoutubeVideoViewerComponent,
    VimeoVideoViewerComponent,
    NoPreviewComponent,
    MatProgressSpinnerModule,
    MatIconModule,
    LimitToPipe,
    MatTooltipModule,
  ],
})
export class BasePreviewComponent extends BaseComponent implements OnInit {
  type = '';
  currentDoc: DocumentView;
  allowFileExtension: AllowFileExtension[] = [];
  isLoading: boolean = false;
  isDownloadFlag: boolean = false;
  isDocumentChange: boolean = false;
  contentType: string = '';
  fileType = FileType;
  mode: ProgressSpinnerMode = 'determinate';
  downloadCountPercentage = 0;

  @HostListener('document:contextmenu', ['$event'])
  disableRightClick(event: MouseEvent): void {
    event.preventDefault();
  }
  documentChunkDownloads: DocumentChunkDownload[] = [];
  documentChunks: DocumentChunk[] = [];
  documentUrl: Blob;
  isChunk: boolean = false;

  constructor(
    public overlay: OverlayPanel,
    private commonService: CommonService,
    @Inject(OVERLAY_PANEL_DATA) public data: DocumentView,
    private clonerService: ClonerService,
    private overlayRef: OverlayPanelRef,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getAllowExtension();
  }

  closeToolbar() {
    this.overlay.setIsClosePanelClose(this.isDocumentChange);
    this.overlayRef.close();
  }

  onDocumentChange(flag: boolean | undefined) {
    if (flag === undefined) {
      flag = true;
    }
    this.isDocumentChange = flag;
    this.overlay.setIsClosePanelClose(this.isDocumentChange);
    this.overlayRef.close();
  }

  getAllowExtension() {
    this.commonService.getAllowFileExtensions().subscribe();
    this.sub$.sink = this.commonService.allowFileExtension$.subscribe(
      (allowFileExtension: AllowFileExtension[]) => {
        this.allowFileExtension = allowFileExtension;
        this.onDocumentView(this.data);
      }
    );
  }

  onDocumentView(document: DocumentView) {
    this.currentDoc = this.clonerService.deepClone<DocumentView>(document);
    if (document.extension !== null) {
      const extension = document.extension?.split('.').pop();
      const allowTypeExtenstion =
        this.allowFileExtension.find((c) =>
          c.extensions?.find(
            (ext) =>
              ext.toLowerCase() === document.extension?.toLowerCase() ||
              ext.toLowerCase() === extension?.toLowerCase()
          )
        ) ?? null;
      this.type =
        allowTypeExtenstion && allowTypeExtenstion.fileType !== undefined
          ? this.fileType[allowTypeExtenstion.fileType]?.toLowerCase()
          : '';
      if (this.data.isFromPublicPreview && this.data.isFileRequestDocument) {
        this.getIsDownloadFlag(document);
      } else if (this.data.isFileRequestDocument) {
        this.isDownloadFlag = true;
      } else if (this.data.isAllowDownload) {
        this.isDownloadFlag = true;
      } else {
        this.isDownloadFlag = this.data.isPreviewDownloadEnabled;
      }
      if (
        !this.data.isFromPublicPreview &&
        !this.data.isFileRequestDocument &&
        isNaN(Number(this.currentDoc.moduleNo))
      ) {
        const documentId: string =
          (document.isVersion ? document.id : document.documentId) ?? '';
        if (documentId) {
          this.addDocumentTrail(documentId, DocumentOperation.Read.toString());
        }
      }
      let isLive = true;
      if (
        window.location.hostname === 'localhost' ||
        window.location.hostname === '127.0.0.1'
      ) {
        isLive = false;
      }

      if (
        !(
          this.type?.toLowerCase() == 'office' ||
          this.type?.toLowerCase() == 'text'
        ) ||
        (!isLive &&
          (this.currentDoc.extension === 'xlsx' ||
            this.currentDoc.extension === 'xls')) ||
        (!isLive &&
          (this.currentDoc.extension === 'doc' ||
            this.currentDoc.extension === 'docx'))
      ) {
        {
          this.checkDocumentChunk();
        }
      }
    }
  }

  addDocumentTrail(documentId: string, operation: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: documentId,
      operationName: operation,
    };

    this.sub$.sink = this.commonService
      .addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe((c) => {});
  }

  checkDocumentChunk() {
    this.sub$.sink = this.commonService
      .checkDocumentStoreAsChunk(this.data)
      .subscribe((c) => {
        this.isChunk = c;
        this.getDocument(false);
      });
  }

  getDocument(isDownload: boolean = false) {
    this.isLoading = true;
    if (this.isChunk) {
      this.getAllDocumentChunks(isDownload);
    } else {
      this.mode = 'indeterminate';
      this.sub$.sink = this.commonService
        .downloadDocument(this.currentDoc)
        .subscribe({
          next: (event: HttpEvent<Blob>) => {
            if (event.type === HttpEventType.Response) {
              this.isLoading = false;
              if (event?.body) {
                this.documentUrl = new Blob([event.body], {
                  type: event.body.type,
                });
                if (isDownload) {
                  this.downloadFile();
                }
              }
            }
          },
          error: async (err) => {
            this.isLoading = false;
            let errorMessage = 'An unexpected error occurred';
            if (err.error instanceof Blob) {
              try {
                const errorText = await err.error.text();
                const errorJson = JSON.parse(errorText);

                if (Array.isArray(errorJson)) {
                  errorMessage = errorJson.join(', ');
                } else if (typeof errorJson === 'object') {
                  const message = errorJson?.message;
                  if (Array.isArray(message)) {
                    errorMessage = message.join(', ');
                  } else if (typeof message === 'string') {
                    errorMessage = message;
                  } else {
                    errorMessage = JSON.stringify(errorJson);
                  }
                }
              } catch (e) {
                console.error('Failed to parse blob error:', e);
              }
            } else if (err.error && Array.isArray(err.error)) {
              errorMessage = err.error.join(', ');
            } else if (err.error && typeof err.error === 'string') {
              errorMessage = err.error;
            }

            this.toastrService.error(errorMessage);
          },
        });
    }
  }

  onLoadPdf(data: any) {
    this.getDocument(false);
  }

  getAllDocumentChunks(isDownload: boolean = false) {
    const documentId = this.currentDoc.documentVersionId
      ? this.currentDoc.documentVersionId
      : this.currentDoc.documentId;
    this.sub$.sink = this.commonService
      .getDocumentChunks(documentId ?? '', this.data)
      .subscribe({
        next: (data) => {
          this.documentChunks = data;
          if (this.documentChunks.length > 0) {
            this.startDownload(isDownload);
          }
        },
        error: (err) => {
          this.toastrService.error(err.error.message);
        },
      });
  }

  startDownload(isDownload: boolean = false) {
    this.isLoading = true;
    this.documentChunkDownloads = []; // Clear previous downloads
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    let completedChunks = 0;
    const chunkRequests = [];
    //  If QmsModule we will download all chunks of the document
    if (
      !isNaN(Number(this.currentDoc.moduleNo)) &&
      this.currentDoc.moduleNo !== 2
    ) {
      for (let i = 0; i < (this.currentDoc.totalChunk ?? 0); i++) {
        chunkRequests.push({
          chunkIndex: i,
          documentVersionId: this.currentDoc.documentId,
        });
      }
    } else {
      for (let i = 0; i < this.documentChunks.length; i++) {
        chunkRequests.push({
          chunkIndex: i,
          documentVersionId: this.documentChunks[i].documentVersionId,
        });
      }
    }
    this.sub$.sink = from(chunkRequests)
      .pipe(
        bufferCount(parallelCalls), // Group requests into batches of 5
        concatMap((batch) =>
          from(batch).pipe(
            mergeMap(
              (chunk) =>
                this.downloadChunk(
                  chunk.chunkIndex,
                  chunk.documentVersionId ?? ''
                ),
              parallelCalls
            ) // Retrieve 5 chunks in parallel
          )
        )
      )
      .subscribe({
        next: (documentChunkDownload: DocumentChunkDownload) => {
          completedChunks++;
          this.downloadCountPercentage = Math.round(
            (completedChunks / chunkRequests.length) * 100
          );
          this.contentType = documentChunkDownload.contentType;
          const chunkBlob = this.base64ToBlob(
            documentChunkDownload.data,
            documentChunkDownload.contentType
          );
          documentChunkDownload.blobChunk = chunkBlob;
          this.documentChunkDownloads.push(documentChunkDownload);
        },
        complete: () => this.mergeChunks(isDownload),
        error: (err) => {
          this.isLoading = false;
        },
      });
  }

  private base64ToBlob(base64: string, contentType: string): Blob {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: contentType });
  }

  downloadChunk(chunkIndex: number, documentVersionId: string) {
    return this.commonService.downloadDocumentChunk(
      documentVersionId,
      chunkIndex,
      this.data
    );
  }

  mergeChunks(isDownload: boolean = false) {
    this.downloadCountPercentage = 100;
    const sortedChunks = this.documentChunkDownloads
      .sort((a, b) => a.chunkIndex - b.chunkIndex)
      .map((entry) => entry.blobChunk)
      .filter((chunk): chunk is Blob => chunk !== undefined);
    const blob = new Blob(sortedChunks, { type: this.contentType });
    this.documentUrl = blob;
    this.isLoading = false;
    if (isDownload) {
      this.downloadFile();
    }
  }

  getIsDownloadFlag(document: DocumentView) {
    this.sub$.sink = this.commonService
      .isDownloadFlag(
        this.data?.isVersion ? document.id ?? '' : document.documentId ?? ''
      )
      .subscribe((c) => {
        this.isDownloadFlag = c;
      });
  }

  downloadDocument(documentView: DocumentView) {
    const docView = this.clonerService.deepClone<DocumentView>(this.currentDoc);
    docView.isVersion = documentView.isVersion;
    if (
      !this.data.isFromPublicPreview &&
      !this.data.isFileRequestDocument &&
      this.data.moduleNo == null
    ) {
      this.addDocumentTrail(
        documentView.isVersion
          ? documentView.id ?? ''
          : documentView.documentId ?? '',
        DocumentOperation.Download.toString()
      );
    }
    if (
      this.type === FileType.Office.toString().toLowerCase() ||
      this.type === FileType.Text.toString().toLowerCase()
    ) {
      this.getDocument(true);
    } else {
      this.downloadFile();
    }
  }

  private downloadFile() {
    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);
    a.download = this.currentDoc.name ?? 'download';
    a.href = URL.createObjectURL(this.documentUrl);
    a.target = '_blank';
    a.click();
    document.body.removeChild(a);
  }
}
