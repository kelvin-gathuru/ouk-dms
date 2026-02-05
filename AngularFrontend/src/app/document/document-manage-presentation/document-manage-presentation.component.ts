import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  inject,
  OnInit,
  Output,
  Signal
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormArray,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { Category } from '@core/domain-classes/category';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentMetaData } from '@core/domain-classes/document-meta-data';
import { FileInfo } from '@core/domain-classes/file-info';
import { Role } from '@core/domain-classes/role';
import { User } from '@core/domain-classes/user';
import { CategoryService } from '@core/services/category.service';
import { CommonService } from '@core/services/common.service';
import { BaseComponent } from '../../base.component';
import { DocumentStatus } from '../../document-status/document-status';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { StorageSetting } from '../../storage-setting/storage-setting';
import { StorageSettingService } from '../../storage-setting/storage-setting.service';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { ClonerService } from '@core/services/clone.service';
import { ClientStore } from '../../client/client-store';
import { bufferCount, concatMap, from, mergeMap, Observable, tap } from 'rxjs';
import { DocumentService } from '../document.service';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { CommonError } from '@core/error-handler/common-error';
import { ToastrService } from '@core/services/toastr-service';
import { MatProgressSpinnerModule, ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { environment } from '@environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { FoldersViewStore } from '../folders-view/folders-view-store';
import { validateFile } from '@core/domain-classes/extension-types';
import { PptxMetadataService } from '@core/services/pptx-metadata.service';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { DocumentMetaTag } from '@core/domain-classes/document-meta-tag';
import { UserStore } from '../../user/store/user.store';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { StorageTypePipe } from '../../storage-setting/storage-type.pipe';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { NgStyle } from '@angular/common';
import { ManageAssignCategoryComponent } from '../../document-library/folders-view/manage-assign-category/manage-assign-category.component';
import { MatDialog } from '@angular/material/dialog';
import { ManageDocumentStatusComponent } from '../../document-status/manage-document-status/manage-document-status.component';
import { Client } from '@core/domain-classes/client';
import { ManageClientComponent } from '../../client/manage-client/manage-client.component';
import { retentionValidator } from '@shared/retention-validator';


@Component({
  selector: 'app-document-manage-presentation',
  templateUrl: './document-manage-presentation.component.html',
  styleUrls: ['./document-manage-presentation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  providers: [PptxMetadataService],
  imports: [
    ReactiveFormsModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatDatepickerModule,
    TranslateModule,
    PageHelpTextComponent,
    HasClaimDirective,
    StorageTypePipe,
    MatTooltipModule,
    MatCardModule,
    MatButtonModule,
    NgStyle,
  ]
})
export class DocumentManagePresentationComponent
  extends BaseComponent
  implements OnInit {
  document: DocumentInfo;
  documentForm: FormGroup;
  extension = '';
  categories: Category[] = [];
  allCategories: Category[] = [];
  storageSettings: StorageSetting<any>[] = [];
  documentStatues: DocumentStatus[] = [];
  documentSource: string = '';
  // eslint-disable-next-line @angular-eslint/no-output-on-prefix
  @Output() onSaveDocument: EventEmitter<Partial<DocumentInfo>> =
    new EventEmitter<Partial<DocumentInfo>>();
  @Output() addDocumentTrail: EventEmitter<Partial<DocumentInfo>> =
    new EventEmitter<Partial<DocumentInfo>>();
  progress = 0;
  message = '';
  fileInfo: FileInfo;
  isFileUpload = false;
  fileData: File;
  userStore = inject(UserStore);
  roles: Role[];
  selectedRoles: Role[] = [];
  selectedUsers: User[] = [];
  allowFileExtension: AllowFileExtension[] = [];
  minDate: Date;
  parallelUploads = 5;
  isLoading: boolean = false;
  chunkSize = environment.chunkSize;
  mode: ProgressSpinnerMode = 'determinate';
  view: string | null = '';
  categoryId: string | null = '';
  dialog = inject(MatDialog);
  get documentMetaTagsArray(): FormArray {
    return <FormArray>this.documentForm.get('documentMetaTags');
  }
  public clientStore = inject(ClientStore);
  foldersViewStore = inject(FoldersViewStore);
  documentMetaTagStore = inject(DocumentMetaTagStore);
  metaTags: Signal<DocumentMetaTag[]> = this.documentMetaTagStore.documentMetaTags;
  selectedCategory: Category | undefined;
  constructor(
    private fb: UntypedFormBuilder,
    private cd: ChangeDetectorRef,
    private categoryService: CategoryService,
    private commonService: CommonService,
    private documentStatusService: DocumentStatusService,
    private clonerService: ClonerService,
    private StorageSettingService: StorageSettingService,
    private documentService: DocumentService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private pptxMetadataService: PptxMetadataService
  ) {
    super();
    this.minDate = new Date();
  }

  ngOnInit(): void {
    this.view = this.route.snapshot.queryParamMap.get('view');
    this.categoryId = this.route.snapshot.queryParamMap.get('categoryId')
      ? this.route.snapshot.queryParamMap.get('categoryId')
      : '';

    this.createDocumentForm();
    this.getCategories();
    this.getDocumentStatus();
    this.getStorageSetting();
    this.documentMetaTagsArray.push(this.buildDocumentMetaTag());
    this.getRoles();
    this.getAllAllowFileExtension();
  }

  getRoles() {
    this.commonService
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
  onCategoryChange(filtervalue: any) {
    this.selectedCategory = this.allCategories.find(c => c.id === filtervalue.value);
  }

  getStorageSetting() {
    this.StorageSettingService.getStorageSettings().subscribe(
      (c: StorageSetting<void>[]) => {
        if (c && c.length > 0) {
          this.storageSettings = c;
          const isDefaultItem = this.storageSettings.find((c) => c.isDefault);
          if (isDefaultItem) {
            this.documentForm.patchValue({
              storageSettingId: isDefaultItem.id,
            });
          }
        }
      }
    );
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
      if (this.categoryId) {
        const category = this.allCategories.find((c) => c.id === this.categoryId);
        if (category) {
          this.documentForm.patchValue({
            categoryId: category.id,
          });
          this.onCategoryChange({ value: category.id });
        }
      }
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

  onDocumentChange($event: any) {
    const files = $event.target.files || $event.srcElement.files;
    const file_url = files[0];
    this.fileData = files[0];
    this.extension = file_url.name.split('.').pop();
    if (this.fileExtesionValidation(this.extension)) {
      const reader = new FileReader();
      reader.onload = async (e: any) => {
        this.documentSource = e.target.result;
        this.fileUploadValidation('upload');
      };
      reader.readAsArrayBuffer(file_url);
    } else {
      this.documentSource = '';
      this.fileUploadValidation('');
    }
  }
  async getPDFMetadata(file: File) {
    const metadata = await this.pptxMetadataService.getPDFMetadata(file);
    if (metadata) {
      this.populateMetadataTags(metadata);
    }
  }

  fileUploadValidation(fileName: string) {
    this.documentForm.patchValue({
      url: fileName,
    });
    this.documentForm.get('url')?.markAsTouched();
    this.documentForm.updateValueAndValidity();
  }

  fileUploadExtensionValidation(extension: string) {
    this.documentForm.patchValue({
      extension: extension,
    });
    this.documentForm.get('extension')?.markAsTouched();
    this.documentForm.updateValueAndValidity();
  }

  fileExtesionValidation(extension: string): boolean {
    const allowTypeExtenstion = this.allowFileExtension?.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extension.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }

  createDocumentForm() {
    this.documentForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      categoryId: [this.categoryId, [Validators.required]],
      url: ['', [Validators.required]],
      extension: ['', [Validators.required]],
      documentMetaTags: this.fb.array([]),
      documentStatusId: [''],
      storageSettingId: [''],
      clientId: [''],
      selectedRoles: [],
      selectedUsers: [],
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
    }, { validators: retentionValidator }
    );
  }

  buildDocumentMetaTag(): FormGroup {
    return this.fb.group({
      id: [''],
      documentId: [''],
      documentMetaTagId: [''],
      metatag: [''],
      metaTagDate: [null],
      metaTagType: [0]
    });
  }

  get rolePermissionFormGroup() {
    return this.documentForm.get('rolePermissionForm') as FormGroup;
  }

  get userPermissionFormGroup() {
    return this.documentForm.get('userPermissionForm') as FormGroup;
  }

  onMetatagChange(event: any, index: number) {
    const email = this.documentMetaTagsArray.at(index).get('metatag')?.value;
    if (!email) {
      return;
    }
    const emailControl = this.documentMetaTagsArray.at(index).get('metatag');
    emailControl?.setValidators([Validators.required]);
    emailControl?.updateValueAndValidity();
  }

  onMetaTagKeyChange(event: any, index: number) {
    const selectedTagId = event.value;
    const metaTagsArray = this.documentMetaTagsArray.value.map(
      (tag: { documentMetaTagId: any }) => tag.documentMetaTagId
    );

    // Check for duplicate meta tags
    if (metaTagsArray.filter((id: any) => id === selectedTagId).length > 1) {
      this.toastrService.error(
        this.translationService.getValue('DUPLICATE_META_TAG_NOT_ALLOWED')
      );
      this.documentMetaTagsArray.at(index).get('documentMetaTagId')?.setValue(null);
      return;
    }

    const selectedMetaTag = this.metaTags().find((tag) => tag.id === selectedTagId);
    if (!selectedMetaTag) return;

    const metaTagControl = this.documentMetaTagsArray.at(index);
    const metaTagValueControl = metaTagControl.get('metatag');
    const metaTagDateControl = metaTagControl.get('metaTagDate');

    // Update metaTagType dynamically
    metaTagControl.get('metaTagType')?.setValue(selectedMetaTag.type);

    if (selectedMetaTag.type === 0) {
      // Text field case
      metaTagValueControl?.setValidators([Validators.required]);
      metaTagValueControl?.updateValueAndValidity();

      metaTagDateControl?.clearValidators();
      metaTagDateControl?.setValue(null);
      metaTagDateControl?.updateValueAndValidity();
    } else if (selectedMetaTag.type === 1) {
      // Date picker case
      metaTagDateControl?.setValidators([Validators.required]);
      metaTagDateControl?.updateValueAndValidity();

      metaTagValueControl?.clearValidators();
      metaTagValueControl?.setValue(null);
      metaTagValueControl?.updateValueAndValidity();
    }
  }

  editDocmentMetaData(documentMetatag: DocumentMetaData): FormGroup {
    return this.fb.group({
      id: [documentMetatag.id],
      documentId: [documentMetatag.documentId],
      documentMetaTagId: [documentMetatag.documentMetaTagId],
      metatag: [documentMetatag.metatag],
      metaTagDate: [documentMetatag.metaTagDate],
      metaTagType: [documentMetatag.metaTagType]
    });
  }

  saveDocument() {
    if (this.documentForm.valid) {
      const speedMbps = this.commonService.getInternetSpeed();
      if (this.fileData.size > this.chunkSize) {
        this.saveDocumentChunk();
      } else {
        this.onSaveDocument.emit(this.buildDocumentObject());
      }
    } else {
      this.documentForm.markAllAsTouched();
    }
  }

  saveDocumentChunk() {
    this.documentService
      .addChunkDocument(this.buildDocumentObject())
      .subscribe((c: DocumentInfo) => {
        this.document = c;
        this.uploadFileInChunks(c.documentVersionId ?? '');
      });
  }

  private markFormGroupTouched(formGroup: UntypedFormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control: any) => {
      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  buildMetaTagObject(): DocumentMetaData[] {
    const documentMetaTags = this.documentMetaTagsArray.getRawValue().length > 0 ? this.documentMetaTagsArray.getRawValue() : [];
    const metaTagObjects: DocumentMetaData[] = documentMetaTags.map((tag) => {
      return {
        id: tag.id,
        documentId: tag.documentId,
        documentMetaTagId: tag.documentMetaTagId,
        metatag: tag.metatag,
        metaTagDate: tag.metaTagDate?.toString() === 'Invalid Date'
          ? undefined
          : tag.metaTagDate
            ? new Date(tag.metaTagDate).toISOString()
            : undefined,
        metaTagType: tag.metaTagType
      };
    });
    return metaTagObjects;

  }



  buildDocumentObject(): Partial<DocumentInfo> {
    const document: Partial<DocumentInfo> = {
      categoryId: this.documentForm.get('categoryId')?.value ?? '',
      documentStatusId: this.documentForm.get('documentStatusId')?.value ?? '',
      clientId: this.documentForm.get('clientId')?.value ?? '',
      storageSettingId: this.documentForm.get('storageSettingId')?.value ?? '',
      description: this.documentForm.get('description')?.value ?? '',
      name: this.documentForm.get('name')?.value,
      url: this.fileData.name,
      documentMetaDatas: [...this.buildMetaTagObject()],
      file: this.fileData,
      extension: this.extension,
      isAssignToMe: false,
      retentionPeriodInDays: this.documentForm.get('retentionPeriodInDays')?.value ?? null,
      onExpiryAction: this.documentForm.get('onExpiryAction')?.value ?? null
    };
    if (this.selectedRoles.length > 0) {
      document.documentRolePermissions = this.selectedRoles.map((role) => {
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
      document.documentUserPermissions = this.selectedUsers.map((user) => {
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

  onAddAnotherMetaTag() {
    const existingKeys = this.documentMetaTagsArray.value.map(
      (tag: { documentMetaTagId: string }) => tag.documentMetaTagId
    );
    const filteredKeys = existingKeys.filter((key: string) => key !== '');
    if (new Set(filteredKeys).size !== filteredKeys.length) {
      this.toastrService.error(
        this.translationService.getValue('DUPLICATE_META_TAG_NOT_ALLOWED')
      );
      return;
    }

    this.documentMetaTagsArray.push(this.buildDocumentMetaTag());
    this.documentForm.updateValueAndValidity();
    this.cd.detectChanges();
  }

  onDeleteMetaTag(index: number) {
    this.documentMetaTagsArray.removeAt(index);
    this.documentForm.updateValueAndValidity();
    this.cd.detectChanges();
  }

  async upload(files: FileList | undefined | null): Promise<void> {
    if (!files || files.length === 0) return;
    if (!(await validateFile(files[0]))) {
      this.documentSource = '';
      this.fileUploadValidation('');
      this.toastrService.error(
        this.translationService.getValue(
          'INVALID_EXTENSION_OR_CORRUPT_INVALID_SIGNATURE'
        )
      );
      this.cd.markForCheck();
      return;
    }

    this.extension = files[0].name.split('.').pop() ?? '';
    if (!this.fileExtesionValidation(this.extension)) {
      this.fileUploadExtensionValidation('');
      this.cd.markForCheck();
      return;
    } else {
      this.fileUploadExtensionValidation('valid');
    }
    const allowTypeExtenstion = environment.allowExtesions.find((c) =>
      c.extentions.find(
        (ext) => ext.toLowerCase() === this.extension.toLowerCase()
      )
    );
    this.fileData = files[0];

    if (this.extension === 'pdf') {
      await this.getPDFMetadata(files[0]);
    } else if (this.extension === 'docx' || this.extension === 'doc') {
      const metadata = await this.readWordMetadata(files[0]);
    } else if (this.extension === 'ppt' || this.extension === 'pptx') {
      this.readPPTXMetadata(files[0]);
    } else if (this.extension === 'xls' || this.extension === 'xlsx') {
      this.readXlsMetadata(files[0]);
    }
    this.documentForm.get('url')?.setValue(files[0].name);
    this.documentForm.get('name')?.setValue(files[0].name);
  }

  async readWordMetadata(file: File) {
    const metadata = await this.pptxMetadataService.extractMetadata(file);
    if (metadata) {
      this.populateMetadataTags(metadata);
      console.log('Metadata:', metadata);
    }
  }

  async readPPTXMetadata(file: File) {
    const metadata = await this.pptxMetadataService.extractMetadata(file);
    if (metadata) {
      this.populateMetadataTags(metadata);
      console.log('Metadata:', metadata);
    }
  }

  async readXlsMetadata(file: File) {
    const metadata = await this.pptxMetadataService.extractMetadata(file);
    if (metadata) {
      this.populateMetadataTags(metadata);
      console.log('Metadata:', metadata);
    }
  }


  populateMetadataTags(metadata: any) {
    this.documentMetaTagsArray.clear();
    Object.keys(metadata).forEach((key) => {
      const matchingTag = this.metaTags()
        .find((tag) => tag?.name.toLocaleLowerCase() === key);
      const value =
        key.toLocaleLowerCase() === 'title' &&
          (!metadata[key] || metadata[key].toLocaleLowerCase() === 'n/a')
          ? this.fileData.name.split('.')[0]
          : metadata[key];
      if (matchingTag && value) {
        this.documentMetaTagsArray.push(this.fb.group({
          id: [''],
          documentId: [''],
          documentMetaTagId: [matchingTag.id],
          metatag: [matchingTag.type == 0 ? value : ''],
          metaTagDate: [matchingTag.type == 1 ? value ? new Date(value) : null : null],
          metaTagType: [matchingTag.type]
        }));
      }
    });
    this.documentForm.updateValueAndValidity();
    this.cd.detectChanges();
  }

  uploadFileInChunks(documentVersionId: string) {
    if (!this.fileData) return;
    const { parallelCalls } = this.commonService.getNetworkSpeed();
    const totalChunks = Math.ceil(this.fileData.size / this.chunkSize);
    const chunkUploads: FormData[] = [];
    this.progress = 0;
    this.isLoading = true;
    for (let i = 0; i < totalChunks; i++) {
      const start = i * this.chunkSize;
      const end = Math.min(start + this.chunkSize, this.fileData.size);
      const chunk = this.fileData.slice(start, end);
      const formData = new FormData();
      formData.append('file', chunk);
      formData.append('chunkIndex', i.toString());
      formData.append('size', this.chunkSize.toString());
      formData.append('totalChunks', totalChunks.toString());
      formData.append('extension', this.extension);
      formData.append('documentVersionId', documentVersionId);
      chunkUploads.push(formData);
    }

    this.sub$.sink = from(chunkUploads)
      .pipe(
        bufferCount(parallelCalls), // Group chunks in batches based on parallelCalls
        concatMap(
          (
            batch // Change concatMap to mergeMap
          ) =>
            from(batch).pipe(
              tap(() => console.log('Processing batch:', batch)),
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
          this.isLoading = false;
          this.progress = 100;
          this.markChunkAsUploaded();
          this.cd.markForCheck();
        },
        error: (err) => {
          this.markChunkAsUploaded(false);
          this.isLoading = false;
          this.cd.markForCheck();
        },
      });
  }

  uploadChunk(formData: FormData): Observable<DocumentChunk | CommonError> {
    return this.documentService.uploadChunkDocument(formData);
  }

  markChunkAsUploaded(flag = true) {
    this.isLoading = true;
    this.commonService.markChunkAsUploaded(this.document.id ?? '', flag).subscribe({
      next: (c) => {
        this.isLoading = false;
        if (flag) {
          this.addDocumentTrail.emit(this.document);
        }
      },
      error: (err) => {
        this.isLoading = false;
      },
    });
  }

  roleTimeBoundChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.rolePermissionFormGroup
        .get('startDate')!
        .setValidators([Validators.required]);
      this.rolePermissionFormGroup
        .get('endDate')!
        .setValidators([Validators.required]);
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
        .get('startDate')!
        .setValidators([Validators.required]);
      this.userPermissionFormGroup
        .get('endDate')!
        .setValidators([Validators.required]);
    } else {
      this.userPermissionFormGroup.get('startDate')!.clearValidators();
      this.userPermissionFormGroup.get('startDate')!.updateValueAndValidity();
      this.userPermissionFormGroup.get('endDate')!.clearValidators();
      this.userPermissionFormGroup.get('endDate')!.updateValueAndValidity();
    }
  }

  cancel() {
    if (this.view === 'list') {
      this.router.navigate(['/documents/list-view']);
    } else if (this.view === 'folders') {
      this.router.navigate(['/documents/folder-view']);
    } else {
      this.router.navigate(['/documents/list-view']);
    }
  }

  addFolder() {
    const dialogRef = this.dialog.open(ManageAssignCategoryComponent, {
      width: '400px',
      data: Object.assign({}, null),
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.categories = [...this.categories, result];
        this.categories.forEach((category: Category, index: number) => {
          category.deafLevel = 0;
          category.index = index * Math.pow(0.1, category.deafLevel);
          category.displayName = category.name;
          this.allCategories.push(category);
          this.setDeafLevel(category);
        });
        this.allCategories = this.clonerService.deepClone(this.allCategories);
        this.documentForm.get('categoryId')?.setValue(result.id);
      }
    });
  }

  addClient(): void {
    const dialogRef = this.dialog.open(ManageClientComponent, {
      maxWidth: '50vw',
      width: '100%',
      data: true
    });
    dialogRef.afterClosed().subscribe((result: Client) => {
      if (result) {
        this.documentForm.get('clientId')?.setValue(result.id);
      }
    });
  }

  addDocumentStatus(): void {
    const dialogRef = this.dialog.open(ManageDocumentStatusComponent, {
      width: '500px',
    });
    dialogRef.afterClosed().subscribe((result: DocumentStatus) => {
      if (result) {
        this.documentStatues = [...this.documentStatues, result];
        this.documentForm.get('documentStatusId')?.setValue(result.id);
      }
    });
  }
}
