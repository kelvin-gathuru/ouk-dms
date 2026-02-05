import { OverlayModule } from '@angular/cdk/overlay';
import {
  ChangeDetectorRef,
  Component,
  Inject,
  Input,
  OnInit,
  Renderer2,
  DOCUMENT,
} from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FileRequestService } from '../../file-request.service';
import { FileRequestInfo } from '@core/domain-classes/file-request-info';
import { TranslateModule } from '@ngx-translate/core';
import { FileRequestDocumentService } from '../../file-request-document.service';
import { FileRequestDocumentInfo } from '../../../core/domain-classes/file-request-document-info';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { FileRequestDocument } from '../../../core/domain-classes/file-request-document';
import { FileType } from '@core/domain-classes/file-type.enum';
import { FileRequestDocumentStatusPipe } from '../../file-request-document-status.pipe';
import { SecurityService } from '@core/security/security.service';
import { StatusColorDirective } from '../../status-color.directive';
import { validateFile } from '@core/domain-classes/extension-types';
import { BaseComponent } from '../../../base.component';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-file-request-link-preview',
  imports: [
    ReactiveFormsModule,
    RouterModule,
    FileRequestDocumentStatusPipe,
    TranslateModule,
    UTCToLocalTime,
    StatusColorDirective,
    OverlayModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './file-request-link-preview.component.html',
  styleUrls: ['./file-request-link-preview.component.scss'],
})
export class FileRequestLinkPreviewComponent
  extends BaseComponent
  implements OnInit {
  @Input() documents: any[] = [];
  logoUrl?: string;
  isLinkExpired = false;
  fileRequestInfo: FileRequestInfo;
  fileRequestDocuments: FileRequestDocument[] = [];
  uploadForm: FormGroup;
  requiresPassword = false;
  isPasswordVerified = false;
  subject: string;
  requestedBy: string;
  extension: string;
  allowFileExtension: AllowFileExtension[] = [];
  uploadedFiles: (File | undefined)[] = [];
  uploadedFileNames: (string | undefined)[] = [];
  isLoading = false;

  get fileInputs(): FormArray {
    return this.uploadForm.get('files') as FormArray;
  }

  addFileInput() {
    this.fileInputs.push(this.createFileGroup());
  }

  removeFileInput(index: number) {
    if (index > 0) {
      this.fileInputs.removeAt(index);
      this.uploadedFiles.splice(index, 1);
      this.uploadedFileNames.splice(index, 1);
    }
  }

  private code: string;

  documentLinkForm: FormGroup;
  isPasswordInvalid = false;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    private fileRequestService: FileRequestService,
    private fileRequestDocumentService: FileRequestDocumentService,
    private fb: FormBuilder,
    private commonService: CommonService,
    private toastrService: ToastrService,
    private securityService: SecurityService,
    private cd: ChangeDetectorRef
  ) {
    super();
  }

  ngOnInit(): void {
    this.code = this.route.snapshot.params['code'];
    this.createFileRequestLinkForm();
    this.uploadForm = this.fb.group({
      files: this.fb.array([this.createFileGroup()]),
    });
    this.getLinkInfo();
    this.getAllAllowFileExtension();
    this.getCompanyProfile();
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

  createFileRequestLinkForm() {
    this.documentLinkForm = this.fb.group({
      password: ['', [Validators.required]],
    });
  }

  checkPassword() {
    if (this.documentLinkForm.valid) {
      const password = this.documentLinkForm.get('password')?.value;
      this.fileRequestService.verifyPassword(this.code, password).subscribe({
        next: (result: boolean) => {
          this.isPasswordVerified = result;
          if (result) {
            this.documentLinkForm.reset();
          } else {
            this.toastrService.error(
              this.translationService.getValue('INVALID_PASSWORD')
            );
            this.documentLinkForm.get('password')?.setErrors({ invalid: true });
          }
        },
        error: () => {
          this.documentLinkForm.get('password')?.setErrors({ invalid: true });
        },
      });
    } else {
      this.documentLinkForm.markAllAsTouched();
    }
  }

  createFileGroup() {
    return this.fb.group({
      name: ['', [Validators.required]],
      file: ['', [Validators.required]],
    });
  }

  createBuildObject(): FileRequestDocumentInfo {
    const id = this.fileRequestInfo.id;
    const files = this.fileInputs.controls?.map((x) => x.get('file')?.value);
    const names = this.fileInputs.controls?.map((x) => x.get('name')?.value);
    const data: FileRequestDocumentInfo = {
      fileRequestId: id ?? '',
      files: files,
      names: names,
    };
    return data;
  }

  getLinkInfo() {
    this.fileRequestService.getFileRequestData(this.code).subscribe({
      next: (fileRequestInfo: FileRequestInfo) => {
        this.isLinkExpired = fileRequestInfo.isLinkExpired ? true : false;
        if (!this.isLinkExpired) {
          this.fileRequestInfo = fileRequestInfo;
          this.fileRequestDocuments =
            fileRequestInfo.fileRequestDocuments || [];
          this.requiresPassword = fileRequestInfo.hasPassword ? true : false;
        }
      },
      error: () => {
        this.isLinkExpired = true;
      },
    });
  }

  onFileUpload() {
    const filesToUpload = this.uploadedFiles.filter(
      (file): file is File => file !== undefined
    );

    if (!filesToUpload.length) {
      this.toastrService.error('Please select at least one valid file.');
      return;
    }

    if (this.uploadForm.valid) {
      this.isLoading = true;
      const fileRequestDocument: FileRequestDocumentInfo = {
        fileRequestId: this.code,
        files: filesToUpload,
        names: this.uploadedFileNames.filter(
          (name): name is string => name !== undefined
        ),
      };

      this.fileRequestDocumentService
        .addFileRequestDocument(fileRequestDocument)
        .subscribe({
          next: () => {
            this.isLoading = false;
            this.toastrService.success(
              this.translationService.getValue('FILE_UPLOADED_SUCCESSFULLY')
            );
            this.uploadedFiles = [];
            this.uploadedFileNames = [];
            this.fileInputs.clear();
            this.getLinkInfo();
          },
          error: () => {
            this.isLoading = false;
          },
        });
    } else {
      this.uploadForm.markAllAsTouched();
    }
  }

  async onFileSelected(event: Event, index: number) {
    const inputElement = event.target as HTMLInputElement;
    const file = inputElement?.files?.[0];
    if (!file || !(await validateFile(file))) {
      this.toastrService.error(
        this.translationService.getValue(
          'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
        )
      );
      this.cd.markForCheck();
      return;
    }
    if (file) {
      const fileSizeInMb = file.size / (1024 * 1024);
      if (fileSizeInMb > this.fileRequestInfo.sizeInMb) {
        this.uploadedFiles[index] = undefined;
        this.uploadedFileNames[index] = undefined;
        this.fileInputs
          .at(index)
          ?.get('file')
          ?.setErrors({ sizeExceeded: true });
        this.uploadForm.updateValueAndValidity();
        return;
      }

      if (this.validateFileExtension(file)) {
        this.uploadedFiles[index] = file;
        this.uploadedFileNames[index] = file.name;
        this.fileInputs.at(index)?.get('file')?.setErrors(null);
        this.fileInputs.at(index)?.get('name')?.setErrors(null);
      } else {
        this.uploadedFiles[index] = undefined;
        this.uploadedFileNames[index] = undefined;
        this.fileInputs
          .at(index)
          ?.get('file')
          ?.setErrors({ invalidExtension: true });
        this.uploadForm.updateValueAndValidity();
        return;
      }
    }
  }

  validateFileExtension(file: File): boolean {
    const extension = file.name.split('.').pop()?.toLowerCase();
    if (!extension) return false;
    const allowedFileTypes = this.fileRequestInfo.allowExtension
      ? this.fileRequestInfo.allowExtension.split(',')
      : [];
    const allowedFileTypeEnumValues = allowedFileTypes.map((type) => {
      const enumKey = type as keyof typeof FileType;
      return FileType[enumKey];
    });

    const data = this.allowFileExtension.some(
      (fileType) =>
        allowedFileTypeEnumValues.includes(fileType.fileType) &&
        fileType.extensions?.includes(extension)
    );
    return data;
  }

  fileExtesionValidation(extension: string): boolean {
    const allowTypeExtenstion = this.allowFileExtension.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extension.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }

  mobileMenuSidebarOpen(event: Event, className: string) {
    const hasClass = (event.target as HTMLInputElement).classList.contains(
      className
    );
    if (hasClass) {
      this.renderer.removeClass(this.document.body, className);
    } else {
      this.renderer.addClass(this.document.body, className);
    }
  }

  getCompanyProfile(): void {
    this.securityService.getCompanyProfile().subscribe((c) => {
      if (c) {
        this.logoUrl = c.logoUrl;
      }
    });
  }
}
