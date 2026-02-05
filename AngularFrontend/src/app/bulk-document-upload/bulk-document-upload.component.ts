import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  inject,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  FormArray,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule } from '@angular/router';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { Role } from '@core/domain-classes/role';
import { User } from '@core/domain-classes/user';
import { CategoryService } from '@core/services/category.service';
import { CommonService } from '@core/services/common.service';
import { TranslateModule } from '@ngx-translate/core';
import { DocumentStatusService } from '../document-status/document-status.service';
import { ClonerService } from '@core/services/clone.service';
import { StorageSettingService } from '../storage-setting/storage-setting.service';
import { Category } from '@core/domain-classes/category';
import { StorageSetting } from '../storage-setting/storage-setting';
import { DocumentStatus } from '../document-status/document-status';
import { BaseComponent } from '../base.component';
import { MatOptionModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { NgStyle } from '@angular/common';
import { StorageTypePipe } from '../storage-setting/storage-type.pipe';
import { DocumentInfo } from '@core/domain-classes/document-info';
import {
  catchError,
  concatMap,
  from,
  mergeMap,
  Observable,
  of,
  toArray,
} from 'rxjs';
import { DocumentService } from '../document/document.service';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DocumentStore } from '../document/document-list/document-store';
import { ClientStore } from '../client/client-store';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { CommonError } from '@core/error-handler/common-error';
import { environment } from '@environments/environment';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DocumentChunkStatus } from '@core/domain-classes/document-chunk-status';
import { validateFile } from '@core/domain-classes/extension-types';

import { ToastrService } from '@core/services/toastr-service';
import { UserStore } from '../user/store/user.store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { retentionValidator } from '@shared/retention-validator';
import { DocumentResource } from '@core/domain-classes/document-resource';

@Component({
  selector: 'app-bulk-document-upload',
  imports: [
    TranslateModule,
    MatDialogModule,
    MatSelectModule,
    ReactiveFormsModule,
    RouterModule,
    MatOptionModule,
    MatButtonModule,
    StorageTypePipe,
    MatCheckboxModule,
    MatDatepickerModule,
    FormsModule,
    MatProgressSpinnerModule,
    TranslateModule,
    PageHelpTextComponent,
    MatTooltipModule,
    HasClaimDirective,
    MatCardModule,
    NgStyle,
    MatIconModule,
  ],
  templateUrl: './bulk-document-upload.component.html',
  styleUrl: './bulk-document-upload.component.scss',
})
export class BulkDocumentUploadComponent
  extends BaseComponent
  implements OnInit
{
  documentForm: UntypedFormGroup;
  extension = '';
  categories: Category[] = [];
  allCategories: Category[] = [];
  storageSettings: StorageSetting<any>[] = [];
  documentStatues: DocumentStatus[] = [];
  roles: Role[];
  selectedRoles: Role[] = [];
  selectedUsers: User[] = [];
  allowFileExtension: AllowFileExtension[] = [];
  loading: boolean = false;
  resultArray: any = [];
  minDate = new Date();
  maxDate = new Date();
  @ViewChild('file') fileInput: any;
  @Output() onSaveDocument: EventEmitter<DocumentInfo> =
    new EventEmitter<DocumentInfo>();
  progress: number = 0;
  isLoading: boolean = false;
  mode = 'indeterminate';

  public clientStore = inject(ClientStore);
  private fb = inject(UntypedFormBuilder);
  private cd = inject(ChangeDetectorRef);
  private categoryService = inject(CategoryService);
  private commonService = inject(CommonService);
  private documentStatusService = inject(DocumentStatusService);
  private clonerService = inject(ClonerService);
  private storageSettingService = inject(StorageSettingService);
  private documentService = inject(DocumentService);
  private toastrService = inject(ToastrService);
  counter: number;
  private documentStore = inject(DocumentStore);
  documentResource: DocumentResource =
    this.documentStore.documentResourceParameter();
  userStore = inject(UserStore);
  chunkSize = environment.chunkSize;
  chunkUploads: Observable<any>[] = [];
  /**
   *
   */
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.createDocumentForm();
    this.getCategories();
    this.getDocumentStatus();
    this.getStorageSetting();
    this.getRoles();
    this.getAllAllowFileExtension();
  }
  get fileInputs(): FormArray {
    return (<FormArray>this.documentForm.get('files')) as FormArray;
  }

  get rolePermissionFormGroup() {
    return this.documentForm.get('rolePermissionForm') as FormGroup;
  }

  get userPermissionFormGroup() {
    return this.documentForm.get('userPermissionForm') as FormGroup;
  }

  createDocumentForm() {
    this.documentForm = this.fb.group(
      {
        name: [''],
        description: [''],
        categoryId: ['', [Validators.required]],
        url: [''],
        extension: [''],
        documentMetaTags: this.fb.array([]),
        documentStatusId: [''],
        storageSettingId: [''],
        clientId: [''],
        selectedRoles: [],
        selectedUsers: [],
        files: this.fb.array([]),
        rolePermissionForm: this.fb.group({
          isTimeBound: [false],
          startDate: [''],
          endDate: [''],
          isAllowDownload: [false],
        }),
        userPermissionForm: this.fb.group({
          isTimeBound: [false],
          startDate: [''],
          endDate: [''],
          isAllowDownload: [false],
        }),
        retentionPeriodInDays: ['0'],
        onExpiryAction: ['0'],
      },
      { validators: retentionValidator }
    );
  }

  getRoles() {
    this.sub$.sink = this.commonService
      .getRoles()
      .subscribe((roles: Role[]) => (this.roles = roles));
  }

  getDocumentStatus() {
    this.documentStatusService
      .getDocumentStatuss()
      .subscribe((c: DocumentStatus[]) => {
        if (c && c.length > 0) {
          this.documentStatues = c;
        } else {
          this.documentStatues = [];
        }
      });
  }

  getStorageSetting() {
    this.storageSettingService
      .getStorageSettings()
      .subscribe((c: StorageSetting<void>[]) => {
        if (c && c.length > 0) {
          this.storageSettings = c;
          const isDefaultItem = this.storageSettings.find((c) => c.isDefault);
          if (isDefaultItem) {
            this.documentForm.patchValue({
              storageSettingId: isDefaultItem.id,
            });
          }
        }
      });
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

  getCategories(): void {
    this.categoryService.getAllCategories().subscribe((c) => {
      this.categories = [...c];
      const categories = this.categories.filter((c) => c.parentId == null);
      categories.forEach((category: Category, index: number) => {
        category.deafLevel = 0;
        category.index = index * Math.pow(0.1, category.deafLevel);
        category.displayName = category.name;
        this.allCategories.push(category);
        this.setDeafLevel(category);
      });
      this.allCategories = this.clonerService.deepClone(this.allCategories);
    });
  }

  setDeafLevel(parent?: Category) {
    if (parent?.children && parent.children.length > 0) {
      parent.children.map((c, index) => {
        c.displayName = parent.displayName + ' > ' + c.name;
        c.deafLevel = parent ? (parent.deafLevel ?? 0) + 1 : 0;
        c.index = parent
          ? (parent.index ?? 0) + index * Math.pow(0.1, c.deafLevel)
          : 0;
        this.allCategories.push(c);
        this.setDeafLevel(c);
      });
    }
    return parent;
  }

  getCategoryNameById(id: string): string {
    const category = this.allCategories?.find((category) => category.id === id);
    return category?.name || '';
  }

  async onFileSelect(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.resultArray = [];
      for (let i = 0; i < input.files.length; i++) {
        if (!(await validateFile(input.files[i]))) {
          this.toastrService.error(
            this.translationService.getValue(
              'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
            )
          );
          this.cd.markForCheck();
          continue;
        }
        const file = input.files[i];
        this.extension = file.name.split('.').pop() ?? '';
        if (this.fileExtesionValidation(this.extension)) {
          this.fileInputs.push(
            this.fb.group({
              fileName: [file.name],
              file: [file],
              name: [file.name, Validators.required],
              extension: [this.extension],
              message: [''],
              isSuccess: [false],
              isLoading: [false],
              documentId: [''],
            })
          );
        }
      }
      this.cd.markForCheck();
    }
  }

  // Remove a file from the selected list
  removeFile(index: number): void {
    this.fileInputs.removeAt(index);
  }

  fileExtesionValidation(extension: string): boolean {
    const allowTypeExtenstion = this.allowFileExtension.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extension.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }

  private markFormGroupTouched(formGroup: UntypedFormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control: any) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  saveFilesAndDocument() {
    if (this.documentForm.valid) {
      this.loading = true;
      this.counter = 0;
      const concatObservable$: any[] = [];
      this.fileInputs.controls.map((control) => {
        if (!control.get('isSuccess')?.value) {
          const documentObj = this.buildDocumentObject();
          documentObj.url = control.get('fileName')?.value;
          documentObj.name = control.get('name')?.value;
          documentObj.extension = control.get('extension')?.value;
          documentObj.file = control.get('file')?.value;
          if (control.get('file')?.value.size > this.chunkSize) {
            concatObservable$.push(this.saveDocumentChunk({ ...documentObj }));
          } else {
            concatObservable$.push(
              this.documentService.addDocument({ ...documentObj })
            );
          }
        }
      });
      if (concatObservable$.length === 0) {
        return;
      }
      this.resultArray = [];
      this.chunkUploads = [];
      this.sub$.sink = from(concatObservable$)
        .pipe(
          concatMap((obs, index) => {
            return obs.pipe(
              catchError((err) => {
                return of(`${err.error[0]}`);
              })
            );
          })
        )
        .subscribe({
          next: (document: any | string) => {
            this.counter++;
            if (typeof document !== 'string') {
              this.addDocumentTrail(document.id ?? '');
              this.fileInputs.at(this.counter - 1).patchValue({
                isLoading: false,
                documentId: document.id,
              });

              if (document.isChunk) {
                this.loading = true;
                const fileData = this.fileInputs
                  .at(this.counter - 1)
                  .get('file')?.value;
                this.chunkUploads.push(
                  this.uploadFile(
                    document.documentVersionId ?? '',
                    document.id ?? '',
                    fileData
                  )
                );
              } else {
                this.resultArray.push({
                  isSuccess: true,
                  documentId: document.id,
                  name: this.fileInputs.at(this.counter - 1).get('name')?.value,
                  message: this.translationService.getValue(
                    'DOCUMENT_SAVE_SUCCESSFULLY'
                  ),
                });
                this.fileInputs.at(this.counter - 1).patchValue({
                  isLoading: false,
                });
              }
            }
            if (typeof document === 'string') {
              this.resultArray.push({
                isSuccess: false,
                documentId: '',
                message: document,
                name: this.fileInputs.at(this.counter - 1).get('name')?.value,
              });
            }
            this.documentStore.loadByQuery(this.documentResource);
          },
          error: (error) => {
            this.fileInputs.at(this.counter).patchValue({
              isLoading: false,
              documentId: '',
            });
            this.loading = false;
          },
          complete: () => {
            this.uploadedChunksAllFile();
          },
        });
    } else {
      this.markFormGroupTouched(this.documentForm);
      return;
    }
  }

  uploadedChunksAllFile() {
    this.sub$.sink = from(this.chunkUploads)
      .pipe(
        concatMap((obs, index) => {
          return obs.pipe(
            catchError((err) => {
              return of(`${err.error[0]}`);
            })
          );
        })
      )
      .subscribe({
        next: (data: DocumentChunkStatus) => {
          this.loading = false;
          if (data.status) {
            const arrayValue = this.fileInputs.getRawValue();
            const document = arrayValue.find(
              (c) => c.documentId === data.documentId
            );
            this.resultArray.push({
              isSuccess: true,
              documentId: document.id,
              name: document.name,
              message: this.translationService.getValue(
                'DOCUMENT_SAVE_SUCCESSFULLY'
              ),
            });
          } else {
            this.documentChunkRemove(data.documentId);
          }
        },
        error: (error) => {
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
          this.isLoading = false;
          this.fileInputs.clear();
        },
      });
  }

  private uploadFile(
    documentVersionId: string,
    documentId: string,
    file: File
  ): Observable<any> {
    const totalChunks = Math.ceil(file.size / this.chunkSize);
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const chunkUploads: Observable<any>[] = [];
    this.progress = 0;
    this.isLoading = true;
    for (let i = 0; i < totalChunks; i++) {
      const start = i * this.chunkSize;
      const end = Math.min(start + this.chunkSize, file.size);
      const chunk = file.slice(start, end);
      const formData = new FormData();
      formData.append('file', chunk);
      formData.append('chunkIndex', i.toString());
      formData.append('size', this.chunkSize.toString());
      formData.append('totalChunks', totalChunks.toString());
      formData.append('extension', this.extension);
      formData.append('documentVersionId', documentVersionId);
      chunkUploads.push(this.uploadChunk(formData));
    }
    return from(chunkUploads).pipe(
      mergeMap((upload) => upload, parallelCalls), // Uploads max 5 chunks at a time
      toArray(),
      concatMap(() => {
        return this.completeUpload(documentId);
      }),
      catchError((err) => {
        const documentChunkStatus: DocumentChunkStatus = {
          documentId: documentId,
          status: false,
        };
        return of(documentChunkStatus);
      })
    );
  }

  documentChunkRemove(documentId: string) {
    this.loading = true;
    this.isLoading = true;
    this.commonService.markChunkAsUploaded(documentId, false).subscribe({
      next: (data) => {
        this.loading = false;
        this.isLoading = false;
      },
      error: (error) => {
        this.loading = false;
        this.isLoading = false;
      },
    });
  }
  private completeUpload(documentId: string): Observable<any> {
    return this.commonService.markChunkAsUploaded(documentId, true);
  }

  uploadChunk(formData: FormData): Observable<DocumentChunk | CommonError> {
    return this.documentService.uploadChunkDocument(formData);
  }

  saveDocumentChunk(documentObj: any): Observable<DocumentInfo> {
    return this.documentService.addChunkDocument(documentObj);
  }

  addDocumentTrail(id: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: DocumentOperation.Created.toString(),
    };
    this.loading = false;
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
  }

  buildDocumentObject(): Partial<DocumentInfo> {
    const files = this.fileInputs.getRawValue();
    const document: Partial<DocumentInfo> = {
      categoryId: this.documentForm.get('categoryId')?.value ?? '',
      documentStatusId: this.documentForm.get('documentStatusId')?.value ?? '',
      storageSettingId: this.documentForm.get('storageSettingId')?.value ?? '',
      clientId: this.documentForm.get('clientId')?.value ?? '',
      description: this.documentForm.get('description')?.value ?? '',
      documentMetaDatas: [],
      isAssignToMe: false,
      retentionPeriodInDays:
        this.documentForm.get('retentionPeriodInDays')?.value ?? null,
      onExpiryAction: this.documentForm.get('onExpiryAction')?.value ?? null,
    };
    if (this.selectedRoles.length > 0) {
      document.documentRolePermissions = this.documentForm
        .get('selectedRoles')
        ?.value?.map((role: any) => {
          return Object.assign(
            {},
            {
              id: '',
              documentId: '',
              roleId: role.id,
            },
            this.rolePermissionFormGroup.value
          );
        });
    }

    if (this.selectedUsers.length > 0) {
      document.documentUserPermissions = this.documentForm
        .get('selectedUsers')
        ?.value?.map((user: any) => {
          return Object.assign(
            {},
            {
              id: '',
              documentId: '',
              userId: user.id,
            },
            this.userPermissionFormGroup.value
          );
        });
    }
    return document;
  }

  roleTimeBoundChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.rolePermissionFormGroup
        .get('startDate')
        ?.setValidators([Validators.required]);
      this.rolePermissionFormGroup
        .get('endDate')
        ?.setValidators([Validators.required]);
    } else {
      this.rolePermissionFormGroup.get('startDate')?.clearValidators();
      this.rolePermissionFormGroup.get('startDate')?.updateValueAndValidity();
      this.rolePermissionFormGroup.get('endDate')?.clearValidators();
      this.rolePermissionFormGroup.get('endDate')?.updateValueAndValidity();
    }
  }

  userTimeBoundChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.userPermissionFormGroup
        .get('startDate')
        ?.setValidators([Validators.required]);
      this.userPermissionFormGroup
        .get('endDate')
        ?.setValidators([Validators.required]);
    } else {
      this.userPermissionFormGroup.get('startDate')?.clearValidators();
      this.userPermissionFormGroup.get('startDate')?.updateValueAndValidity();
      this.userPermissionFormGroup.get('endDate')?.clearValidators();
      this.userPermissionFormGroup.get('endDate')?.updateValueAndValidity();
    }
  }
}
