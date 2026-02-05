import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DocumentView } from '@core/domain-classes/document-view';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { CommonService } from '@core/services/common.service';
import { environment } from '@environments/environment';
import { DocPreviewComponent } from '@shared/doc-preview/doc-preview.component';
import { ExcelPreviewComponent } from '@shared/excel-preview/excel-preview.component';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-office-viewer',
  templateUrl: './office-viewer.component.html',
  styleUrls: ['./office-viewer.component.scss'],
  standalone: true,
  imports: [ExcelPreviewComponent, DocPreviewComponent, MatProgressSpinnerModule]
})
export class OfficeViewerComponent
  implements OnInit, AfterViewInit, OnChanges {
  @ViewChild('iframe') iframe: ElementRef<HTMLIFrameElement>;
  isLive = window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1" ? false : true;
  isLoading: boolean = false;
  token = '';
  @Input() document: Partial<MediaPreview> | Partial<DocumentView>;
  @Input() documentBlob: Blob | null = null;
  data: any[] = [];

  constructor(
    private commonService: CommonService,
    private cdRef: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    if (this.isLive) {
      this.getDocumentToken();
      this.isLoading = false;
    }
  }

  ngOnChanges(): void {
    if (this.documentBlob && this.document) {
      if (this.isLive) {
        this.isLoading = true;
        const url = URL.createObjectURL(this.documentBlob);
        this.iframe.nativeElement.src = url;
        this.isLoading = false;
      } else {
        this.isLoading = true;
        // this.readExcelFile(this.documentBlob);
      }
    }
  }

  getDocumentToken() {
    console.log("document", this.document);
    this.isLoading = true;
    this.commonService
      .getDocumentToken(this.document)
      .subscribe({
        next: (token: { [key: string]: string }) => {
          this.token = token['result'];
          const host = location.host;
          const protocal = location.protocol;
          const password = this.document.linkPassword
            ? encodeURIComponent(this.document.linkPassword)
            : '';
          const url =
            environment.apiUrl === '/'
              ? `${protocal}//${host}/`
              : environment.apiUrl;
          let srcUrl =
            'https://view.officeapps.live.com/op/embed.aspx?src=' +
            encodeURIComponent(
              `${url}api/document/${this.document.documentId}/officeviewer?token=${this.token}&isVersion=${this.document.isVersion}
              &&isPublic=${this.document.isFromPublicPreview}&&isFileRequest=${this.document.isFileRequestDocument}&&password=${password}`
            );
          const apiUrl = `${url}api/document/${this.document.documentId}/officeviewer?token=${this.token}&isVersion=${this.document.isVersion}
              &&isPublic=${this.document.isFromPublicPreview}&&isFileRequest=${this.document.isFileRequestDocument}&&password=${password}`;
          if (this.document.documentVersionId) {
            srcUrl =
              srcUrl + `&&documentVersionId=${this.document.documentVersionId}`;
          }
          this.iframe.nativeElement.src = srcUrl;
          this.isLoading = false;
          this.cdRef.detectChanges()
        },
        error: (err) => {
          this.isLoading = false;
          this.cdRef.detectChanges()
        },
      });
  }

  readExcelFile(blob: Blob) {
    const reader = new FileReader();
    reader.onload = (e) => {
      const data = new Uint8Array(reader.result as ArrayBuffer);
      const workbook = XLSX.read(data, { type: 'array' });
      const sheetName = workbook.SheetNames[0];
      const sheet = workbook.Sheets[sheetName];
      this.data = XLSX.utils.sheet_to_json(sheet, { header: 1 });
      this.isLoading = false;
    };
    reader.readAsArrayBuffer(blob);
  }

  ngOnDestroy() {
    if (this.isLive) {
      this.commonService
        .deleteDocumentToken(this.token)
        .subscribe(() => {
        });
    }
  }
}
