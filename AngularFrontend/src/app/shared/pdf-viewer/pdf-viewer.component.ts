import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  HostListener,
  inject,
  input,
  linkedSignal,
  OnDestroy,
  Output,
  ViewChild,
} from '@angular/core';
import { MatProgressSpinnerModule, ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { SafeUrl } from '@angular/platform-browser';
import { DocumentView } from '@core/domain-classes/document-view';
import { NgxExtendedPdfViewerComponent, NgxExtendedPdfViewerModule, PageRenderedEvent, PdfLoadedEvent, PDFPageView, PDFScriptLoaderService } from 'ngx-extended-pdf-viewer';
import { BaseComponent } from '../../base.component';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddSignature } from '@shared/add-signature/add-signature';
import { DocumentSignature } from '@core/domain-classes/document-signature';
import { PDFDocument, rgb } from 'pdf-lib';
import { UTCToLocalTime, UTCToLocalTimeFormat } from '@shared/pipes/utc-to-localtime.pipe';
import { CommonService } from '@core/services/common.service';
import { environment } from '@environments/environment';
import { DocumentVersion } from '@core/domain-classes/documentVersion';
import { DocumentService } from '../../document/document.service';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { ToastrService } from '@core/services/toastr-service';
import { bufferCount, concatMap, from, mergeMap, Observable, tap } from 'rxjs';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { CommonError } from '@core/error-handler/common-error';
import { DocumentSignaturePosition } from '@core/domain-classes/document-signature-position';
import { PDFDocumentProxy } from 'pdfjs-dist';
import { SecurityService } from '@core/security/security.service';
import { UserAuth } from '@core/domain-classes/user-auth';
import { DatePipe } from '@angular/common';
import * as pdfjsLib from 'pdfjs-dist';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentStore } from '../../document/document-list/document-store';
import { GlobalWorkerOptions } from 'pdfjs-dist';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { CdkDragEnd, DragDropModule } from '@angular/cdk/drag-drop';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { DocumentResource } from '@core/domain-classes/document-resource';
import { Router } from '@angular/router';


@Component({
  selector: 'app-pdf-viewer',
  templateUrl: './pdf-viewer.component.html',
  styleUrls: ['./pdf-viewer.component.scss'],
  standalone: true,
  imports: [
    NgxExtendedPdfViewerModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    TranslateModule,
    MatDialogModule,
    DatePipe,
    HasClaimDirective,
    DragDropModule
  ],
  providers: [UTCToLocalTime, DatePipe]
})
export class PdfDataViewerComponent extends BaseComponent implements AfterViewInit, OnDestroy {
  imageUrl?: SafeUrl;
  isLoading = false;
  isFileRequest: boolean = false;
  document = input.required<MediaPreview | DocumentView>();
  documentBlob = input.required<Blob>();
  documentNewBlob = linkedSignal(() => this.documentBlob());
  latestBlob: Blob;
  @ViewChild('pdfViewer') pdfViewer: NgxExtendedPdfViewerComponent;
  public pageViews: Map<number, any> = new Map();
  documentStore = inject(DocumentStore);
  documentResource: DocumentResource =
    this.documentStore.documentResourceParameter();
  @HostListener('document:contextmenu', ['$event'])
  disableRightClick(event: MouseEvent): void {
    event.preventDefault();
  }
  signatureUrl: string | null = null;
  isPlacingSignature = false;
  isSignatureAdded = false;
  mouseX = 0;
  mouseY = 0;
  today: Date = new Date();

  mousePageX = 100;
  mousePageY = 100;

  pdfJsViewer: any; // store PDF.js viewer
  currentUserName: string = '';
  pageNumber: number = 1;
  imgBytes!: any;
  mineType: string = '';
  chunkSize = environment.chunkSize;
  progress = 0;
  mode: ProgressSpinnerMode = 'determinate';
  signatureBase64: string = '';
  public viewports: { [page: number]: any } = {};
  private pdfDoc!: PDFDocumentProxy;
  currentZoom: number = 100;
  appUserAuth: UserAuth | null = null;
  pageType: string = '';
  viewportWidth: number = 0;
  viewportHeight: number = 0;
  isPdfImage: boolean = false;
  password: string = '';
  isDraggingSignature: boolean = false;
  pageInfo: Map<number, { div: HTMLElement; width: number; height: number; rect?: any }> = new Map();
  BASE_OFFSET_X = 10; // original offset at 100% zoom
  BASE_OFFSET_Y = 60;
  isMobile: boolean = false;

  @Output() loadPdf = new EventEmitter<any>();
  constructor(
    private dialog: MatDialog,
    private commonService: CommonService,
    private documentService: DocumentService,
    private toastrService: ToastrService,
    private cd: ChangeDetectorRef,
    private securityService: SecurityService,
    private datePipe: DatePipe,
    private breakpointObserver: BreakpointObserver,
    private router: Router,
    private pDFScriptLoaderService: PDFScriptLoaderService
  ) {
    super();
    // Dynamically import the worker script path
    if (!GlobalWorkerOptions.workerSrc) {
      GlobalWorkerOptions.workerSrc = `/js/pdf.worker.min.mjs`;
    }
  }

  ngOnInit(): void {
    this.isFileRequest = this.router.url.includes('/file-request');
    this.setTopLogAndName();
  }

  ngAfterViewInit(): void {
  }

  attachHoverHighlights() {
    const textSpans = document.querySelectorAll('.textLayer span');
    textSpans.forEach(span => {
      span.addEventListener('mouseover', () => {
        (span as HTMLElement).style.backgroundColor = 'yellow';
      });
      span.addEventListener('mouseout', () => {
        (span as HTMLElement).style.backgroundColor = '';
      });
    });
  }

  startDrag() {
    this.isDraggingSignature = true;
  }

  /** Called when drag ends */
  async onDragEnded(event: CdkDragEnd) {
    // Anchor the signature by top-left corner
    const floatDiv = event.source.element.nativeElement as HTMLElement;
    if (this.isMobile) {
      const coords = await this.getPdfCoordinatesFromDragMobile(event);
      if (!coords) return;
      this.mouseX = coords.x + 10;
      this.mouseY = coords.y + 15;
    }
    else {
      const coords = await this.getPdfCoordinatesFromDragFixed(event);
      if (!coords) return;
      this.mouseX = (coords.x - floatDiv.offsetWidth / 2) + this.BASE_OFFSET_X;
      this.mouseY = (coords.y - floatDiv.offsetHeight / 2) + this.BASE_OFFSET_Y;
    }

  }



  onPdfLoaded(pdf: PdfLoadedEvent) {
    console.log('PDF loaded', pdf.pagesCount);
  }

  onZoomChange(newZoom: number | string | undefined) {
    console.log('Zoom changed to:', newZoom);
    if (typeof newZoom === 'number') {
      this.currentZoom = newZoom;
      // Optional: force recalculation after zoom
      // setTimeout(() => this.updateAllPageViewports(), 500);
    }
    // update signature positions or UI if needed
  }


  async onPageRendered(event: PageRenderedEvent) {
    const page = event.source.div as HTMLElement;
    const pageRect = page.getBoundingClientRect();

    // event.source.viewport is available during page rendering
    const viewport = event.source.viewport;
    const pdfWidth = viewport?.width || 595;
    const pdfHeight = viewport?.height || 842;

    this.pageInfo.set(event.pageNumber, {
      div: page,
      width: pdfWidth,
      height: pdfHeight,
      rect: pageRect,
    });
  }

  setTopLogAndName() {
    this.sub$.sink = this.securityService.SecurityObject.subscribe((c) => {
      if (c) {
        this.appUserAuth = c;
        this.currentUserName = c.firstName + ' ' + c.lastName;
      }
    });
  }

  getPageView(pageNumber: number) {
    const pdfViewer = (this.pdfViewer as any).pdfViewer; // internal property
    return pdfViewer.getPageView(pageNumber - 1)
    // const pdfViewer = this.pdfViewer.pdfViewer; // internal PDF.js viewer
    // if (!pdfViewer) return null;
    // return pdfViewer.getPageView(pageNumber - 1);
  }

  async checkPdfBlob() {
    const arrayBuffer = await this.documentNewBlob().arrayBuffer();
    const typedArray = new Uint8Array(arrayBuffer);
    let flagValue = true;
    try {
      const pdfDoc = await PDFDocument.load(typedArray, { ignoreEncryption: false })
        .catch(() => {
          alert("PDF cannot be modified or is encrypted");
          flagValue = false;
        });
      const loadingTask = pdfjsLib.getDocument({ data: typedArray });
      // Handle password-protected PDFs
      loadingTask.onPassword = (callback: Function, reason: number) => {
        if (reason === pdfjsLib.PasswordResponses.NEED_PASSWORD) {
          const password = prompt("PDF is password-protected. Enter password:");
          this.password = password ?? '';
          flagValue = true;
          callback(password);
        } else if (reason === pdfjsLib.PasswordResponses.INCORRECT_PASSWORD) {
          const password = prompt("Incorrect password. Try again:");
          callback(password);
        }
      };
      const pdf = await loadingTask.promise;
      // Check PDF permissions
      if ((pdf as any).encrypted) {
        alert("PDF is encrypted / password-protected");
        return false;
      }
      if (!flagValue) {
        return flagValue;
      }
      return true;
    } catch (err: any) {
      alert("Cannot open PDF. It may be corrupted or password-protected.");
      return false;
    }
  }

  async checkIfImagePDF() {
    const arrayBuffer = await this.documentNewBlob().arrayBuffer();
    const typedArray = new Uint8Array(arrayBuffer);
    let pdf;
    if (this.password) {
      pdf = await pdfjsLib.getDocument({ data: typedArray, password: this.password }).promise;
    }
    else {
      pdf = await pdfjsLib.getDocument({ data: typedArray }).promise;
    }

    const page = await pdf.getPage(1); // check first page
    const textContent = await page.getTextContent();

    return textContent.items.length === 0; // true => image PDF
  }

  getPageElement(pageNumber: number): HTMLElement | null {
    return document.querySelector(`.page[data-page-number="${pageNumber}"]`) as HTMLElement;
  }
  getPageCanvas(pageNumber: number): HTMLCanvasElement | null {
    const pageElement = this.getPageElement(pageNumber);
    return pageElement?.querySelector('canvas') as HTMLCanvasElement;
  }

  async getPdfCoordinatesFromDrag(event: CdkDragEnd) {
    const floatDiv = event.source.element.nativeElement as HTMLElement;
    const floatRect = floatDiv.getBoundingClientRect();

    const pdfViewer = this.pDFScriptLoaderService.PDFViewerApplication.pdfViewer;
    const pageView = pdfViewer._pages[this.pageNumber - 1];
    const viewport = pageView.viewport;
    const canvas = pageView.canvas;
    if (!canvas) return null;

    const canvasRect = canvas.getBoundingClientRect();

    // Scaling for HiDPI displays
    const scaleX = canvas.width / canvasRect.width;
    const scaleY = canvas.height / canvasRect.height;

    // Position center relative to canvas
    const sigCenterX = (floatRect.left - canvasRect.left + floatRect.width / 2) * scaleX;
    const sigCenterY = (floatRect.top - canvasRect.top + floatRect.height / 2) * scaleY;

    // Flip Y-axis (DOM → PDF coordinates)
    // const flippedY = canvas.height - sigCenterY;

    const [pdfX, pdfY] = viewport.convertToPdfPoint(sigCenterX, sigCenterY) as [number, number];
    return { x: pdfX, y: pdfY };
  }

  async getPdfCoordinatesFromDragFixed(event: CdkDragEnd) {
    const floatDiv = event.source.element.nativeElement as HTMLElement;
    const floatRect = floatDiv.getBoundingClientRect();

    const pdfViewer = this.pDFScriptLoaderService.PDFViewerApplication.pdfViewer;
    const pageView = pdfViewer._pages[this.pageNumber - 1];
    if (!pageView) return null;

    const viewport = pageView.viewport;
    const canvas = pageView.canvas;
    if (!canvas) return null;

    const canvasRect = canvas.getBoundingClientRect();

    // Scale from DOM (CSS) → canvas pixels (handles retina / HiDPI)
    const scaleX = canvas.width / canvasRect.width;
    const scaleY = canvas.height / canvasRect.height;

    // Center of dragged element relative to canvas
    const canvasX = (floatRect.left - canvasRect.left + floatRect.width / 2) * scaleX;
    const canvasY = (floatRect.top - canvasRect.top + floatRect.height / 2) * scaleY;

    // Convert to PDF coordinates (viewport handles zoom, rotation, top/bottom)
    const [pdfX, pdfY] = viewport.convertToPdfPoint(canvasX, canvasY) as [number, number];

    return { x: pdfX, y: pdfY };
  }

  async getPdfCoordinatesFromDragMobile(event: CdkDragEnd) {
    const floatDiv = event.source.element.nativeElement as HTMLElement;
    const floatRect = floatDiv.getBoundingClientRect();

    const pdfViewer = this.pDFScriptLoaderService.PDFViewerApplication.pdfViewer;
    const pageView = pdfViewer._pages[this.pageNumber - 1];
    if (!pageView) return null;

    const viewport = pageView.viewport;
    const canvas = pageView.canvas!;
    const canvasRect = canvas.getBoundingClientRect();
    const pageDivRect = pageView.div.getBoundingClientRect();

    // 1️⃣ Offsets relative to page
    const offsetX = floatRect.left - pageDivRect.left + floatDiv.offsetWidth / 2;
    const offsetY = floatRect.top - pageDivRect.top + floatDiv.offsetHeight / 2;

    // 2️⃣ Adjust for device pixel ratio and zoom
    const devicePixelRatio = window.devicePixelRatio || 1;
    const canvasScaleX = (canvas.width / canvasRect.width) / devicePixelRatio;
    const canvasScaleY = (canvas.height / canvasRect.height) / devicePixelRatio;

    // 3️⃣ Convert DOM -> PDF pixel coordinates
    const canvasX = offsetX * canvasScaleX;
    const canvasY = offsetY * canvasScaleY;

    // 4️⃣ Convert to PDF coordinate space (accounts for rotation & scale)
    let [pdfX, pdfY] = viewport.convertToPdfPoint(canvasX, canvasY) as [number, number];

    // 5️⃣ Adjust anchor point to top-left
    pdfX -= (floatDiv.offsetWidth / viewport.scale) / 2;
    pdfY -= (floatDiv.offsetHeight / viewport.scale) / 2;

    // 6️⃣ Clamp to valid page area
    pdfX = Math.max(0, Math.min(pdfX, viewport.width - (floatDiv.offsetWidth / viewport.scale)));
    pdfY = Math.max(0, Math.min(pdfY, viewport.height - (floatDiv.offsetHeight / viewport.scale)));

    return { x: pdfX, y: pdfY };
  }

  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
  }

  async addSignature() {
    const flag = await this.checkPdfBlob();
    if (flag) {
      let dialogWidth = '50vw';
      let dialogHeight = 'auto';

      if (this.breakpointObserver.isMatched(Breakpoints.Handset)) {
        dialogWidth = '95vw';
        dialogHeight = '90vh';
        this.isMobile = true;
        // this.BASE_OFFSET_Y = 0;
      }
      // Initialize drag & drop signature
      const dialogRef = this.dialog.open(AddSignature, {
        width: dialogWidth, // 95% width on mobile
        maxWidth: '100vw',
        data: Object.assign({}, document),
      });

      this.sub$.sink = dialogRef.afterClosed()
        .subscribe((result: DocumentSignature) => {
          if (result && result.signatureUrl) {
            this.signatureBase64 = result.signatureUrl;
            this.isSignatureAdded = true;
            const blob = this.base64ToBlob(result.signatureUrl, 'image/png');
            this.signatureUrl = URL.createObjectURL(blob);
            this.imgBytes = this.base64ToBytes(result.signatureUrl);
            this.isPlacingSignature = true;
          }
        });
    }

  }

  base64ToBlob(base64Data: string, defaultType = 'image/png'): Blob {
    // Extract mime type from base64 header if present
    const matches = base64Data.match(/^data:(.*);base64,/);
    this.mineType = matches ? matches[1] : defaultType;
    // Remove the prefix (data:image/...;base64,)
    const base64 = base64Data.replace(/^data:.*;base64,/, '');
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: this.mineType });
  }
  // pdfPassword event receives an object with { reason, callback }
  onPdfPassword(event: any) {
    if (event.reason === 1) {
      const pw = prompt('Enter PDF password:') || '';
      event.callback(pw);
      this.password = pw;
    } else if (event.reason === 2) {
      const pw = prompt('Incorrect password. Try again:') || '';
      event.callback(pw);
    }
  }

  base64ToBytes(base64Data: string): Uint8Array {
    // Remove the prefix (data:image/png;base64, or data:image/jpeg;base64,)
    const cleanedBase64 = base64Data.replace(/^data:.*;base64,/, '');

    // Decode base64 to binary string
    const binaryString = atob(cleanedBase64);

    // Convert binary string to Uint8Array
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
      bytes[i] = binaryString.charCodeAt(i);
    }

    return bytes;
  }

  async downloadSignedPDFLatest() {
    if (!this.documentNewBlob()) return;
    // Convert Blob to Uint8Array
    const arrayBuffer = await this.documentNewBlob().arrayBuffer();
    const pdfDoc = await PDFDocument.load(arrayBuffer, { ignoreEncryption: true });

    // Embed the signature image
    let embeddedImage;
    if (this.mineType.startsWith('image/png')) {
      embeddedImage = await pdfDoc.embedPng(this.imgBytes);
    } else if (this.mineType.startsWith('image/jpeg') || this.mineType.startsWith('image/jpg')) {
      embeddedImage = await pdfDoc.embedJpg(this.imgBytes);
    }

    if (!embeddedImage) return;
    // Get target page
    const pages = pdfDoc.getPages();
    const targetPage = pages[this.pageNumber - 1];
    const pdfX = this.mouseX;
    const pdfY = this.mouseY;
    // Signature image size
    const imgWidth = 70;
    const imgHeight = 30;
    // Draw image at correct position (bottom-left origin)
    targetPage.drawImage(embeddedImage, {
      x: pdfX,
      y: pdfY - imgHeight, // adjust so image bottom aligns with click
      width: imgWidth,
      height: imgHeight,
    });
    // Draw username and date below signature
    const userName = 'Signed by: ' + this.currentUserName;
    const today = this.datePipe.transform(new Date(), 'yyyy-MM-dd hh:mm:ss a') || '';

    targetPage.drawText(userName, {
      x: pdfX + 10,
      y: pdfY - imgHeight - 10,
      size: 6,
      color: rgb(1, 0, 0),
    });

    targetPage.drawText(today, {
      x: pdfX + 10,
      y: pdfY - imgHeight - 20,
      size: 5,
      color: rgb(1, 0, 0),
    });

    // Save PDF
    const pdfBytes = await pdfDoc.save({ useObjectStreams: false });
    const safeArray = new Uint8Array(pdfBytes);
    this.documentNewBlob.set(new Blob([safeArray], { type: 'application/pdf' }));

  }

  async downloadSignedPDFSameImage() {
    if (!this.documentNewBlob()) return;

    // Convert Blob to Uint8Array
    const arrayBuffer = await this.documentNewBlob().arrayBuffer();
    const pdfDoc = await PDFDocument.load(arrayBuffer, { ignoreEncryption: true });

    // Embed the signature image
    let embeddedImage;
    if (this.mineType.startsWith('image/png')) {
      embeddedImage = await pdfDoc.embedPng(this.imgBytes);
    } else if (this.mineType.startsWith('image/jpeg') || this.mineType.startsWith('image/jpg')) {
      embeddedImage = await pdfDoc.embedJpg(this.imgBytes);
    }
    if (!embeddedImage) return;

    // Get target page
    const pages = pdfDoc.getPages();
    const targetPage = pages[this.pageNumber - 1];

    // --- 🔑 Extract CSS sizes from DOM ---
    const floatDiv = document.querySelector('.floating-signature') as HTMLElement;
    if (!floatDiv) return;

    const imgEl = floatDiv.querySelector('img') as HTMLImageElement;
    const userEl = floatDiv.querySelector('.sig-user') as HTMLElement;
    const dateEl = floatDiv.querySelector('.sig-date') as HTMLElement;

    // CSS pixel sizes
    const cssImgWidth = imgEl.clientWidth;     // e.g. 80px
    const cssImgHeight = imgEl.clientHeight;   // e.g. 40px
    const cssUserFont = parseFloat(getComputedStyle(userEl).fontSize); // e.g. 9px
    const cssDateFont = parseFloat(getComputedStyle(dateEl).fontSize); // e.g. 8px

    // Convert CSS px → PDF points (96px → 72pt)
    const cssToPdfScale = 72 / 96;
    const imgWidth = cssImgWidth * cssToPdfScale;
    const imgHeight = cssImgHeight * cssToPdfScale;
    const sigUserFontSize = cssUserFont * cssToPdfScale;
    const sigDateFontSize = cssDateFont * cssToPdfScale;

    // --- 📌 Use saved PDF coordinates ---
    const pdfX = this.mouseX;
    const pdfY = this.mouseY;

    // --- 🖼️ Draw signature image ---
    targetPage.drawImage(embeddedImage, {
      x: pdfX,
      y: pdfY - imgHeight, // align bottom
      width: imgWidth,
      height: imgHeight,
    });

    // --- 📝 Add username & date ---
    const userName = '   Signed by: ' + this.currentUserName;
    const today = this.datePipe.transform(new Date(), 'yyyy-MM-dd hh:mm:ss a') || '';

    targetPage.drawText(userName, {
      x: pdfX,
      y: pdfY - imgHeight - sigUserFontSize - 2,
      size: sigUserFontSize,
      color: rgb(1, 0, 0),
    });

    targetPage.drawText('   ' + today, {
      x: pdfX,
      y: pdfY - imgHeight - sigUserFontSize - sigDateFontSize - 4,
      size: sigDateFontSize,
      color: rgb(1, 0, 0),
    });

    // Save PDF
    const pdfBytes = await pdfDoc.save({ useObjectStreams: false });
    const safeArray = new Uint8Array(pdfBytes);
    this.documentNewBlob.set(new Blob([safeArray], { type: 'application/pdf' }));
  }

  blobToBase64(blob: any): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onloadend = () => resolve(reader.result as string);
      reader.onerror = reject;
      reader.readAsDataURL(blob);
    });
  }

  resetSignature() {
    this.isSignatureAdded = false;
    this.signatureUrl = '';
    this.mousePageX = 100;
    this.mousePageY = 100;
  }

  saveSignature() {
    this.isLoading = true;
    const password = this.pdfViewer.password ? this.pdfViewer.password : this.password
    if (this.isPdfImage || password) {
      this.saveImagePdfSignature();
    } else {
      this.uploadSignature();
    }
  }

  saveImagePdfSignature() {
    this.isLoading = true;
    const documentSignaturePosition: DocumentSignaturePosition = {
      documentId: this.document().documentId ?? '',
      pageNumber: this.pageNumber == 0 ? 1 : this.pageNumber,
      signatureUrl: this.signatureBase64,
      xAxis: this.mouseX,
      yAxis: this.mouseY,
      viewportWidth: this.viewportWidth,
      viewportHeight: this.viewportHeight,
      userName: this.currentUserName,
      password: this.pdfViewer.password ? this.pdfViewer.password : this.password
    };
    this.documentService.saveDocumentSignaturePostion(documentSignaturePosition)
      .subscribe(
        {
          next: (c: DocumentSignature) => {
            this.isLoading = false;
            this.toastrService.success(
              this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
            );
            //  this.isPlacingSignature = false;
            this.signatureUrl = '';
            this.loadPdf.emit(c);
            this.isPdfImage = false;
            this.resetSignature();
          },
          error: () => {
            this.isLoading = false;
          }
        });
  }

  uploadSignature() {
    this.isLoading = true;
    const documentSignature: DocumentSignature = {
      documentId: this.document().documentId ?? '',
      signatureUrl: this.signatureBase64,
    };

    this.documentService.saveDocumentSignature(documentSignature).subscribe({
      next: async (savedSignature: DocumentSignature) => {
        this.addDocumentTrail(savedSignature.documentId);
        await this.downloadSignedPDFSameImage();
        this.isLoading = false;
        this.saveDocument();
      },
      error: () => {
        this.isLoading = false;
        this.toastrService.error(this.translationService.getValue('SIGNATURE_UPLOAD_FAILED'));
      },
    });
  }

  addDocumentTrail(id: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: DocumentOperation.Added_Signature.toString(),
    };
    this.isLoading = false;
    this.toastrService.success(
      this.translationService.getValue('SIGNATURE_SAVED_SUCCESSFULLY')
    );
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
  }


  saveDocument() {
    const speedMbps = this.commonService.getInternetSpeed();
    if (this.documentNewBlob().size > this.chunkSize) {
      this.saveDocumentChunk();
    } else {
      const documentversion: DocumentVersion = {
        documentId: this.document().documentId,
        url: this.document().url,
        file: this.documentNewBlob(),
        extension: this.document().extension,
        comment: '',
        isSignatureExists: true
      };
      this.sub$.sink = this.documentService
        .saveNewVersionDocument(documentversion)
        .subscribe(
          {
            next: (documentInfo: DocumentInfo) => {
              this.isLoading = false;
              this.toastrService.success(
                this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
              );
              this.resetSignature();
              this.documentStore.loadByQuery(this.documentResource);
            },
            error: () => {
              this.isLoading = false;
              this.isSignatureAdded = false;
            }
          });
    }

  }

  saveDocumentChunk() {
    const document: DocumentVersion = {
      documentId: this.document().documentId,
      url: this.document().url,
      extension: this.document().extension,
      comment: '',
    }
    this.documentService.saveNewVersionDocumentChunk(document)
      .subscribe((c: DocumentInfo) => {
        this.uploadFileInChunks(c.id ?? '');
      });
  }

  uploadFileInChunks(documentVersionId: string) {
    if (!this.documentNewBlob()) return;
    this.isLoading = true;
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const totalChunks = Math.ceil(this.documentNewBlob().size / this.chunkSize);
    const chunkUploads = [];
    this.progress = 0;
    for (let i = 0; i < totalChunks; i++) {
      const start = i * this.chunkSize;
      const end = Math.min(start + this.chunkSize, this.documentNewBlob().size);
      const chunk = this.documentNewBlob().slice(start, end);
      const formData = new FormData();
      formData.append('file', chunk);
      formData.append('chunkIndex', i.toString());
      formData.append('size', this.chunkSize.toString());
      formData.append('totalChunks', totalChunks.toString());
      formData.append('extension', this.document().extension ?? 'pdf');
      formData.append('documentVersionId', documentVersionId);
      chunkUploads.push(formData);
    }

    this.sub$.sink = from(chunkUploads).pipe(
      bufferCount(parallelCalls), // Group chunks in batches based on parallelCalls
      concatMap((batch) => // Change concatMap to mergeMap
        from(batch).pipe(
          tap(() => console.log("Processing batch:", batch)),
          mergeMap((formData) => this.uploadChunk(formData), parallelCalls) // Execute uploads in parallel
        )
      )
    )
      .subscribe({
        next: (data: any) => {
          this.progress = Math.min(this.progress + 100 / totalChunks, 100);
          this.cd.markForCheck();
        },
        complete: () => {
          this.progress = 100;
          this.isLoading = false;
          this.markChunkAsUploaded();
          this.toastrService.success(
            this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
          );
          this.cd.markForCheck();
        },
        error: (err) => {
          this.markChunkAsUploaded(false);
          this.isLoading = false;
          this.cd.markForCheck();
        }
      });
  }

  uploadChunk(formData: FormData): Observable<DocumentChunk | CommonError> {
    return this.documentService.uploadChunkDocument(formData);
  }

  markChunkAsUploaded(flag: boolean = true) {
    this.isLoading = true;
    this.commonService.markChunkAsUploaded(this.document().documentId ?? '', flag)
      .subscribe({
        next: (data) => {
          this.isLoading = false;
          this.isSignatureAdded = false;
          this.resetSignature();
        },
        error: (error) => {
          this.isLoading = false;
          this.isSignatureAdded = false;
          this.resetSignature();
        }
      });
  }


}

