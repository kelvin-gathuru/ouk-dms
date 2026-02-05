import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  AbstractControlOptions,
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FileRequest } from '@core/domain-classes/file-request';
import { FileType } from '@core/domain-classes/file-type.enum';
import { TranslateModule } from '@ngx-translate/core';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { FileRequestService } from '../file-request.service';
import { FileSizes } from '../../core/domain-classes/file-sizes.enum';
import { FileSizeLabelDirective } from '../file-size-label.directive';
import { FileRequestInfo } from '@core/domain-classes/file-request-info';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-manage-file-request',
  imports: [
    PageHelpTextComponent,
    TranslateModule,
    RouterModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatDatepickerModule,
    NgxMaterialTimepickerModule,
    MatCheckboxModule,
    FileSizeLabelDirective,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './manage-file-request.component.html',
  styleUrl: './manage-file-request.component.scss'
})
export class ManageFileRequestComponent
  extends BaseComponent
  implements OnInit {
  baseUrl = `${window.location.protocol}//${window.location.host}/file-requests/preview/`;
  isEditMode: boolean = false;
  minDate = new Date();
  fileRequestForm: FormGroup;
  fileTypes: { key: string; value: number }[] = [];
  get extensions(): FormArray {
    return this.fileRequestForm.get('extensions') as FormArray;
  }
  passwordFieldType: string = 'password';

  togglePasswordVisibility(): void {
    this.passwordFieldType = this.passwordFieldType === 'password' ? 'text' : 'password';
  }
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activeRoute: ActivatedRoute,
    private fileRequestService: FileRequestService,
    private toastrService: ToastrService
  ) {
    super();
  }

  createFileRequestForm(): void {
    var currentDate = new Date();
    this.fileRequestForm = this.fb.group(
      {
        id: [''],
        isLinkExpiryTime: [false],
        linkExpiryDate: [currentDate],
        linkExpiryTime: ['12:00'],
        hasPassword: [false],
        password: [''],
        subject: ['', Validators.required],
        email: ['', Validators.email],
        maxDocument: [1, Validators.required],
        sizeInMb: [FileSizes.LessThan5MB],
        fileType: [[], Validators.required],
        baseUrl: [this.baseUrl],
      },
      {
        validators: [this.checkData, this.timeValidator],
      } as AbstractControlOptions
    );
  }

  checkData(group: UntypedFormGroup) {
    let isLinkExpiryTime = group.get('isLinkExpiryTime')?.value;
    let linkExpiryDate = group.get('linkExpiryDate')?.value;
    let hasPassword = group.get('hasPassword')?.value;
    let password = group.get('password')?.value;
    const data: Record<string, boolean> = {};
    if (isLinkExpiryTime && !linkExpiryDate) {
      data['linkExpiryDateValidator'] = true;
    }
    if (hasPassword && !password) {
      data['passwordValidator'] = true;
    }
    return data;
  }

  timeValidator(control: AbstractControl) {
    const linkExpiryTime = control.get('linkExpiryTime')?.value;
    const linkExpiryDate = control.get('linkExpiryDate')?.value;

    if (linkExpiryDate && !linkExpiryTime) {
      control.get('linkExpiryTime')?.setErrors({ required: true });
    } else {
      control.get('linkExpiryDate')?.setErrors(null);
    }
    return null;
  }

  ngOnInit(): void {
    this.fileTypes = this.getEnumValues(FileType);
    this.createFileRequestForm();

    this.sub$.sink = this.activeRoute.data.subscribe((data: any) => {
      const fileRequest: FileRequestInfo = data.fileRequest;
      if (fileRequest) {
        this.isEditMode = true;
        const fileExtensions = fileRequest.allowExtension
          ? fileRequest.allowExtension.split(',').map(ext => FileType[ext.trim() as keyof typeof FileType])
          : [];
        const expiryDate = fileRequest.linkExpiryTime ? new Date(fileRequest.linkExpiryTime) : new Date();
        var expiryTime = '12:00';
        if (fileRequest.linkExpiryTime) {
          const hours = expiryDate.getHours().toString().padStart(2, '0');
          const minutes = expiryDate.getMinutes().toString().padStart(2, '0');
          expiryTime = `${hours}:${minutes}`;
        }
        this.fileRequestForm.patchValue({
          id: fileRequest.id,
          subject: fileRequest.subject,
          email: fileRequest.email,
          maxDocument: fileRequest.maxDocument,
          sizeInMb: fileRequest.sizeInMb,
          isLinkExpiryTime: !!fileRequest.linkExpiryTime,
          linkExpiryDate: expiryDate,
          linkExpiryTime: expiryTime,
          hasPassword: fileRequest.hasPassword,
          password: fileRequest.password,
          fileType: fileExtensions,
          baseUrl: this.baseUrl,
        });
      }
    });
  }

  fileSizeOptions = Object.keys(FileSizes)
    .filter((key) => isNaN(Number(key)))
    .map((key) => ({
      label: '',
      value: FileSizes[key as keyof typeof FileSizes]
    }));

  getEnumValues(enumObj: any): { key: string; value: number }[] {
    return Object.keys(enumObj)
      .filter((key) => isNaN(Number(key)))
      .map((key) => ({
        key: key,
        value: enumObj[key],
      }));
  }

  saveFileRequest() {
    if (this.fileRequestForm.valid) {
      const fileRequest = this.createBuildObject();
      if (this.isEditMode) {
        this.sub$.sink = this.fileRequestService
          .updateFileRequest(fileRequest)
          .subscribe(() => {
            this.toastrService.success(
              this.translationService.getValue(
                'FILE_REQUEST_UPDATED_SUCCESSFULLY'
              )
            );
            this.router.navigate(['/file-request']);
          });
      } else {
        this.sub$.sink = this.fileRequestService
          .addFileRequest(fileRequest)
          .subscribe(() => {
            this.toastrService.success(
              this.translationService.getValue(
                'FILE_REQUEST_CREATED_SUCCESSFULLY'
              )
            );
            this.router.navigate(['/file-request']);
          });
      }
    } else {
      this.fileRequestForm.markAllAsTouched();
    }
  }

  createBuildObject(): FileRequest {
    const id = this.fileRequestForm.get('id')?.value;
    let combinedExpiryDate: Date | undefined = undefined;
    if (this.fileRequestForm.get('isLinkExpiryTime')?.value) {
      const expiryDate = new Date(
        this.fileRequestForm.get('linkExpiryDate')?.value
      );
      const expiryTime = this.fileRequestForm.get('isLinkExpiryTime')?.value
        ? this.fileRequestForm.get('linkExpiryTime')?.value
        : '';
      if (expiryTime && typeof expiryTime === 'string') {
        const [hours, minutes] = expiryTime.split(':').map(Number);
        combinedExpiryDate = new Date(
          expiryDate.getFullYear(),
          expiryDate.getMonth(),
          expiryDate.getDate(),
          hours,
          minutes
        );
      }
    }
    const selectedFileTypes = this.fileRequestForm.get('fileType')?.value;
    const data: FileRequest = {
      id: id,
      subject: this.fileRequestForm.get('subject')?.value,
      email: this.fileRequestForm.get('email')?.value,
      maxDocument: this.fileRequestForm.get('maxDocument')?.value,
      sizeInMb: this.fileRequestForm.get('sizeInMb')?.value,
      isLinkExpired: false,
      hasPassword: false,
      password: this.fileRequestForm.get('hasPassword')?.value
        ? this.fileRequestForm.get('password')?.value
        : '',
      linkExpiryTime: combinedExpiryDate,
      fileExtension: selectedFileTypes,
      baseUrl: this.baseUrl,
    };
    return data;
  }

}
