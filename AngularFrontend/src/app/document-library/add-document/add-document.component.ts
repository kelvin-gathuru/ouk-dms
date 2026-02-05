import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Inject,
  inject,
  OnInit,
  Output,
  Signal,
} from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormArray,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { Category } from '@core/domain-classes/category';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentMetaData } from '@core/domain-classes/document-meta-data';
import { CategoryService } from '@core/services/category.service';
import { ClonerService } from '@core/services/clone.service';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { ClientStore } from '../../client/client-store';
import { DocumentStatus } from '../../document-status/document-status';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { StorageSetting } from '../../storage-setting/storage-setting';
import { StorageSettingService } from '../../storage-setting/storage-setting.service';
import { DocumentService } from '../../document/document.service';
import { bufferCount, concatMap, from, mergeMap, Observable, tap } from 'rxjs';
import { DocumentChunk } from '@core/domain-classes/document-chunk';
import { CommonError } from '@core/error-handler/common-error';
import {
  MatProgressSpinnerModule,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';
import { DocumentStore } from '../../document/document-list/document-store';
import { environment } from '@environments/environment';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { validateFile } from '@core/domain-classes/extension-types';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { PptxMetadataService } from '@core/services/pptx-metadata.service';
import { DocumentMetaTag } from '../../core/domain-classes/document-meta-tag';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { TranslateModule } from '@ngx-translate/core';
import { StorageTypePipe } from '../../storage-setting/storage-type.pipe';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { NgStyle } from '@angular/common';
import { ManageAssignCategoryComponent } from '../folders-view/manage-assign-category/manage-assign-category.component';
import { ManageClientComponent } from '../../client/manage-client/manage-client.component';
import { ManageDocumentStatusComponent } from '../../document-status/manage-document-status/manage-document-status.component';
import { Client } from '@core/domain-classes/client';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { retentionValidator } from '@shared/retention-validator';
import { MatDividerModule } from "@angular/material/divider";

@Component({
  selector: 'app-add-document',
  templateUrl: './add-document.component.html',
  styleUrls: ['./add-document.component.scss'],
  standalone: true,
  providers: [PptxMetadataService],
  imports: [
    PageHelpTextComponent,
    ReactiveFormsModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatSelectModule,
    TranslateModule,
    StorageTypePipe,
    MatButtonModule,
    MatTooltipModule,
    MatIconModule,
    MatCardModule,
    NgStyle,
    HasClaimDirective

  ],
})
export class AddDocumentComponent extends BaseComponent implements OnInit {
  isLoadingResults = false;
  document: DocumentInfo;
  documentForm: UntypedFormGroup;
  extension: string = '';
  categories: Category[] = [];
  allCategories: Category[] = [];
  allStorageSetting: StorageSetting<any>[] = [];
  allDocumentStatus: DocumentStatus[] = [];
  allowFileExtension: AllowFileExtension[] = [];
  @Output() onSaveDocument: EventEmitter<DocumentInfo> =
    new EventEmitter<DocumentInfo>();
  fileData: any;
  documentId: string = '';
  isLoading: boolean = false;
  mode: ProgressSpinnerMode = 'determinate';
  progress: number = 0;
  documentStore = inject(DocumentStore);
  chunkSize = environment.chunkSize;
  foldersViewStore = inject(FoldersViewStore);
  dialog = inject(MatDialog);

  get documentMetaTagsArray(): FormArray {
    return <FormArray>this.documentForm.get('documentMetaTags');
  }
  public clientStore = inject(ClientStore);
  public documentMetaTagStore = inject(DocumentMetaTagStore);
  metaTags: Signal<DocumentMetaTag[]> =
    this.documentMetaTagStore.documentMetaTags;
  selectedCategory: Category | undefined;

  constructor(
    private fb: UntypedFormBuilder,
    private cd: ChangeDetectorRef,
    private categoryService: CategoryService,
    private dialogRef: MatDialogRef<AddDocumentComponent>,
    private commonService: CommonService,
    private toastrService: ToastrService,
    private documentStatusService: DocumentStatusService,
    private clonerService: ClonerService,
    private StorageSettingService: StorageSettingService,
    private pptxMetadataService: PptxMetadataService,
    private documentService: DocumentService,
    @Inject(MAT_DIALOG_DATA) public categoryId: any
  ) {
    super();
  }

  ngOnInit(): void {
    this.getCategories();
    this.getDocumentStatus();
    this.getStorageSetting();
    this.createDocumentForm();
    this.getAllAllowFileExtension();
    this.documentMetaTagsArray.push(this.buildDocumentMetaTag());
  }

  getCategories(): void {
    this.categoryService
      .getAllAssignToMeCategoriesForDropDown()
      .subscribe((c) => {
        this.categories = [...c];
        // const categories = this.categories.filter((c) => c.parentId == null);
        this.categories.forEach((category: Category, index: number) => {
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
            this.onCategoryChange({ value: category.id });
          }
        }
      });
  }
  onCategoryChange(filtervalue: any) {
    this.selectedCategory = this.allCategories.find(c => c.id === filtervalue.value);
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

  getDocumentStatus() {
    this.documentStatusService.getDocumentStatuss().subscribe((c) => {
      if (Array.isArray(c)) {
        this.allDocumentStatus = c;
      }
    });
  }

  getStorageSetting() {
    this.StorageSettingService.getStorageSettings().subscribe((c) => {
      if (Array.isArray(c)) {
        this.allStorageSetting = c;
        const isDefaultItem = this.allStorageSetting.find((c) => c.isDefault);
        if (isDefaultItem) {
          this.documentForm.patchValue({ storageSettingId: isDefaultItem.id });
        }
      }
    });
  }

  getAllAllowFileExtension() {
    this.commonService
      .getAllowFileExtensions()
      .subscribe((allowFileExtension: AllowFileExtension[]) => {
        this.allowFileExtension = allowFileExtension;
      });
  }

  fileUploadValidation(fileName: string) {
    this.documentForm.patchValue({
      url: fileName,
    });
    this.documentForm?.get('url')!.markAsTouched();
    this.documentForm.updateValueAndValidity();
  }

  fileUploadExtensionValidation(extension: string) {
    this.documentForm.patchValue({
      extension: extension,
    });
    this.documentForm?.get('extension')!.markAsTouched();
    this.documentForm.updateValueAndValidity();
  }

  fileExtesionValidation(extesion: string): boolean {
    const allowTypeExtenstion = this.allowFileExtension.find((c) =>
      c.extensions?.find((ext) => ext.toLowerCase() === extesion.toLowerCase())
    );
    return allowTypeExtenstion ? true : false;
  }

  createDocumentForm() {
    this.documentForm = this.fb.group(
      {
        name: ['', [Validators.required]],
        description: [''],
        categoryId: [this.categoryId, [Validators.required]],
        documentStatusId: [''],
        storageSettingId: [''],
        clientId: [''],
        url: ['', [Validators.required]],
        extension: ['', [Validators.required]],
        documentMetaTags: this.fb.array([]),
        retentionPeriodInDays: ['0'],
        onExpiryAction: ['0'],
      },
      { validators: retentionValidator }
    );
  }
  buildDocumentMetaTag(): FormGroup {
    return this.fb.group({
      id: [''],
      documentId: [''],
      documentMetaTagId: [''],
      metatag: [''],
      metaTagDate: [null],
      metaTagType: [0],
    });
  }

  onMetaTagKeyChange(event: any, index: number) {
    const selectedTagId = event.value;
    const metaTagsArray = this.documentMetaTagsArray.value.map(
      (tag: { documentMetaTagId: string }) => tag.documentMetaTagId
    );

    // Check for duplicate meta tags
    if (metaTagsArray.filter((id: string) => id === selectedTagId).length > 1) {
      this.toastrService.error(
        this.translationService.getValue('DUPLICATE_META_TAG_NOT_ALLOWED')
      );
      this.documentMetaTagsArray
        .at(index)
        .get('documentMetaTagId')
        ?.setValue(null);
      return;
    }

    const selectedMetaTag = this.metaTags().find(
      (tag) => tag.id === selectedTagId
    );
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
      metaTagType: [documentMetatag.metaTagType],
    });
  }

  saveDocument() {
    if (this.documentForm.valid) {
      this.isLoading = true; // ✅ Ensure spinner ON
      this.cd.detectChanges();
      const speedMbps = this.commonService.getInternetSpeed();
      if (this.fileData.size > this.chunkSize) {
        this.saveDocumentChunk();
      } else {
        this.saveIndividualDocument();
      }
    } else {
      this.markFormGroupTouched(this.documentForm);
    }
  }

  saveDocumentChunk() {
    this.documentService
      .addChunkDocument(this.buildDocumentObject())
      .subscribe((c: DocumentInfo) => {
        this.documentId = c.id ?? '';
        this.document = c;
        this.uploadFileInChunks(c.documentVersionId ?? '');
      });
  }

  uploadFileInChunks(documentVersionId: string) {
    if (!this.fileData) return;
    const { chunkSize1, parallelCalls } = this.commonService.getNetworkSpeed();
    const totalChunks = Math.ceil(this.fileData.size / this.chunkSize);
    const chunkUploads: FormData[] = [];
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
    this.isLoading = true;
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
          this.progress = Math.min(
            this.progress + 100 / chunkUploads.length,
            100
          );
        },
        complete: () => {
          this.progress = 100;
          this.isLoading = false;
          this.markChunkAsUploaded();
          this.addDocumentTrail(this.documentId);
          this.documentStore.loadDocuments();
          if (
            this.foldersViewStore.selectedCategoryId() ==
            this.document.categoryId
          ) {
            this.foldersViewStore.setDocumentsEmpty();
            this.foldersViewStore.loadDocumentsByCategory(
              this.foldersViewStore.selectedCategoryId()
            );
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.markChunkAsUploaded(false);
        },
      });
  }

  markChunkAsUploaded(flag: boolean = true) {
    this.isLoading = true;
    this.commonService.markChunkAsUploaded(this.documentId, flag).subscribe({
      next: (c) => {
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
      },
    });
  }

  uploadChunk(formData: FormData): Observable<DocumentChunk | CommonError> {
    return this.documentService.uploadChunkDocument(formData);
  }

  saveIndividualDocument() {
    this.isLoading = true;
    this.cd.detectChanges();

    if (this.documentForm.valid) {
      const document = this.buildDocumentObject();

      this.sub$.sink = this.commonService
        .addDocumentWithAssign(document)
        .subscribe({
          next: (documentInfo: DocumentInfo) => {
            this.addDocumentTrail(documentInfo.id ?? '');
            this.toastrService.success(
              this.translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY')
            );
            this.isLoading = false;
          },
          error: (err) => {
            this.isLoading = false;
          }
        });
    } else {
      this.markFormGroupTouched(this.documentForm);
      this.isLoading = false; // also reset loading here
    }
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

  closeDialog() {
    this.dialogRef.close();
  }

  addDocumentTrail(id: string) {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: id,
      operationName: DocumentOperation.Created.toString(),
    };
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
    this.dialogRef.close(true);
  }

  private markFormGroupTouched(formGroup: UntypedFormGroup) {
    (<any>Object).values(formGroup.controls).forEach((control: any) => {
      control.markAsTouched();

      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  buildMetaTagObject(): DocumentMetaData[] {
    const documentMetaTags =
      this.documentMetaTagsArray.getRawValue().length > 0
        ? this.documentMetaTagsArray.getRawValue()
        : [];
    const metaTagObjects: DocumentMetaData[] = documentMetaTags.map((tag) => {
      return {
        id: tag.id,
        documentId: tag.documentId,
        documentMetaTagId: tag.documentMetaTagId,
        metatag: tag.metatag,
        metaTagDate:
          tag.metaTagDate?.toString() === 'Invalid Date'
            ? undefined
            : tag.metaTagDate
              ? new Date(tag.metaTagDate).toISOString()
              : undefined,
        metaTagType: tag.metaTagType,
      };
    });
    return metaTagObjects;
  }

  buildDocumentObject(): Partial<DocumentInfo> {
    const document: Partial<DocumentInfo> = {
      categoryId: this.documentForm?.get('categoryId')?.value ?? '',
      documentStatusId: this.documentForm?.get('documentStatusId')?.value ?? '',
      storageSettingId: this.documentForm?.get('storageSettingId')?.value ?? '',
      clientId: this.documentForm?.get('clientId')?.value ?? '',
      description: this.documentForm?.get('description')?.value,
      name: this.documentForm?.get('name')?.value,
      url: this.fileData.fileName,
      documentMetaDatas: [...this.buildMetaTagObject()],
      file: this.fileData,
      extension: this.extension,
      isAssignToMe: true,
      retentionPeriodInDays:
        this.documentForm?.get('retentionPeriodInDays')?.value ?? null,
      onExpiryAction: this.documentForm?.get('onExpiryAction')?.value ?? null,
    };
    return document;
  }

  async upload(files: FileList | undefined | null): Promise<void> {
    if (!files || files.length === 0) return;

    if (!(await validateFile(files[0]))) {
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
      this.cd.markForCheck();
    }

    this.fileData = files[0];
    if (this.extension === 'pdf') {
      await this.getPDFMetadata(files[0]);
    } else if (this.extension === 'docx' || this.extension === 'doc') {
      const metadata = await this.readWordMetadata(files[0]);
    }
    // else if (allowTypeExtenstion.type === 'image') {
    //   this.readImageMetadata(files[0]);
    // }
    else if (this.extension === 'ppt' || this.extension === 'pptx') {
      this.readPPTXMetadata(files[0]);
    } else if (this.extension === 'xls' || this.extension === 'xlsx') {
      this.readXlsMetadata(files[0]);
    }
    this.documentForm?.get('url')!.setValue(files[0].name);
    this.documentForm?.get('name')!.setValue(files[0].name);
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
      const matchingTag = this.metaTags().find(
        (tag) => tag?.name.toLocaleLowerCase() === key
      );
      const value =
        key.toLocaleLowerCase() === 'title' &&
          (!metadata[key] || metadata[key].toLocaleLowerCase() === 'n/a')
          ? this.fileData.name.split('.')[0]
          : metadata[key];
      if (matchingTag && value) {
        this.documentMetaTagsArray.push(
          this.fb.group({
            id: [''],
            documentId: [''],
            documentMetaTagId: [matchingTag.id],
            metatag: [matchingTag.type == 0 ? value : ''],
            metaTagDate: [
              matchingTag.type == 1 ? (value ? new Date(value) : null) : null,
            ],
            metaTagType: [matchingTag.type],
          })
        );
      }
    });
    this.documentForm.updateValueAndValidity();
    this.cd.detectChanges();
  }

  async getPDFMetadata(file: File) {
    const metadata = await this.pptxMetadataService.getPDFMetadata(file);
    if (metadata) {
      this.populateMetadataTags(metadata);
    }
  }

  addFolder(): void {
    const dialogRef = this.dialog.open(ManageAssignCategoryComponent, {
      width: '400px',
      data: Object.assign({}, null),
    });
    dialogRef.afterClosed().subscribe((result) => {
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
      data: true,
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
        this.allDocumentStatus = [...this.allDocumentStatus, result];
        this.documentForm.get('documentStatusId')?.setValue(result.id);
      }
    });
  }
}
