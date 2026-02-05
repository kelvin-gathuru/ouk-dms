import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, } from '@angular/forms';
import { OCRContentExtractorService } from './ocr-content-extractor.service';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { CommonService } from '@core/services/common.service';
import { validateFile } from '@core/domain-classes/extension-types';
import { ToastrService } from '@core/services/toastr-service';
import { CommonError } from '../core/error-handler/common-error';
import { TranslateModule } from '@ngx-translate/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BaseComponent } from '../base.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-ocr-content-extractor',
  imports: [
    FormsModule,
    TranslateModule,
    MatProgressSpinnerModule,
    PageHelpTextComponent,
    MatIconModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './ocr-content-extractor.component.html',
  styles: ``
})
export class OcrContentExtractorComponent extends BaseComponent implements OnInit {
  documentForm: FormGroup;
  allowFileExtension: AllowFileExtension[] = [];
  extension: string;
  fb = inject(FormBuilder);
  ocrContentExtractorService = inject(OCRContentExtractorService);
  commonService = inject(CommonService)
  toastrService = inject(ToastrService);
  fileData: File | null = null;
  extractedText: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  @ViewChild('file') fileInput!: ElementRef;
  mode: string = 'indeterminate';

  ngOnInit(): void {
    this.getAllAllowFileExtension();
  }



  fileExtesionValidation(extension: string): boolean {
    const allowTypeExtenstion = this.allowFileExtension.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extension.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }
  getAllAllowFileExtension() {
    this.commonService.getAllowFileExtensions().subscribe();
    this.sub$.sink = this.commonService.allowFileExtension$.subscribe(
      (allowFileExtension: AllowFileExtension[]) => {
        if (allowFileExtension) {
          this.allowFileExtension = allowFileExtension;
        }
      }
    );
  }
  async upload(files: FileList | null | undefined) {
    if (!files || files.length === 0) return;
    if (!(await validateFile(files[0]))) {
      this.toastrService.error(
        this.translationService.getValue(
          'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
        )
      );
      this.errorMessage = this.translationService.getValue(
        'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
      );
      return;
    }

    this.extension = files[0].name.split('.').pop() ?? '';
    if (!this.fileExtesionValidation(this.extension)) {
      this.errorMessage = this.translationService.getValue(
        'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
      );
      return;
    }
    this.fileData = files[0];
    this.errorMessage = "";
  }
  extract() {
    if (this.fileData) {
      const maxSize = 10 * 1024 * 1024; // 10MB in bytes
      if (this.fileData.size <= maxSize) {
        this.isLoading = true;
        const ext = "." + this.extension;
        this.ocrContentExtractorService
          .getDocumentContentByOcr(this.fileData, ext)
          .subscribe({
            next: (res: string | CommonError) => {
              this.isLoading = false;
              this.fileInput.nativeElement.value = null;
              this.fileData = null;
              this.errorMessage = '';
              if (res) {
                this.extractedText = res as string;
                this.toastrService.success(
                  this.translationService.getValue('OCR_CONTENT_EXTRACTOR_SUCCESS')
                );
              } else {
                this.extractedText = '';
                this.toastrService.error(
                  this.translationService.getValue('OCR_CONTENT_EXTRACTOR_FAILED')
                );
              }
            },
            error: (error: CommonError) => {
              this.errorMessage = '';
              this.isLoading = false;
            }
          });
      } else {
        this.isLoading = false;
        this.toastrService.error(
          this.translationService.getValue('FILE_SIZE_EXCEEDS_LIMIT')
        );
        this.errorMessage = this.translationService.getValue('FILE_SIZE_EXCEEDS_LIMIT');
      }
    }
  }

}
