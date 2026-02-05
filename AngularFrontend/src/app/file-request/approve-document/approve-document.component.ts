import { NgStyle } from '@angular/common';
import { ChangeDetectorRef, Component, EventEmitter, inject, Inject, OnInit, Output, Signal } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { Category } from '@core/domain-classes/category';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { FileRequestDocument } from '@core/domain-classes/file-request-document';
import { CategoryService } from '@core/services/category.service';
import { ClonerService } from '@core/services/clone.service';
import { CommonService } from '@core/services/common.service';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { DocumentStatus } from '../../document-status/document-status';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { StorageSetting } from '../../storage-setting/storage-setting';
import { StorageSettingService } from '../../storage-setting/storage-setting.service';
import { FileRequestDocumentService } from '../file-request-document.service';
import { FileRequestDocumentApprove } from '@core/domain-classes/file-request-document-apporve';
import { ClientStore } from '../../client/client-store';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { DocumentMetaTag } from '@core/domain-classes/document-meta-tag';
import { DocumentMetaData } from '@core/domain-classes/document-meta-data';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { StorageTypePipe } from '../../storage-setting/storage-type.pipe';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-approve-document',
  imports: [
    PageHelpTextComponent,
    MatIconModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatDatepickerModule,
    MatDialogModule,
    TranslateModule,
    MatTooltipModule,
    StorageTypePipe,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    NgStyle
  ],
  templateUrl: './approve-document.component.html',
  styleUrl: './approve-document.component.scss'
})
export class ApproveDocumentComponent extends BaseComponent implements OnInit {
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

  get documentMetaTagsArray(): FormArray {
    return <FormArray>this.documentForm.get('documentMetaTags');
  }
  public clientStore = inject(ClientStore);
  public documentMetaTagStore = inject(DocumentMetaTagStore);
  metaTags: Signal<DocumentMetaTag[]> = this.documentMetaTagStore.documentMetaTags;
  constructor(
    private fb: UntypedFormBuilder,
    private cd: ChangeDetectorRef,
    @Inject(MAT_DIALOG_DATA) public data: FileRequestDocument,
    private categoryService: CategoryService,
    private dialogRef: MatDialogRef<ApproveDocumentComponent>,
    private commonService: CommonService,
    private toastrService: ToastrService,
    private documentStatusService: DocumentStatusService,
    private clonerService: ClonerService,
    private fileRequestDocumentService: FileRequestDocumentService,
    private StorageSettingService: StorageSettingService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.extension = this.data.url.split('.')[1];
    this.getCategories();
    this.getDocumentStatus();
    this.getStorageSetting();
    this.createDocumentForm();
    this.documentMetaTagsArray.push(this.buildDocumentMetaTag());
  }

  getCategories(): void {
    this.categoryService.getAllCategories().subscribe((c) => {
      this.categories = [...c];
      const categories = this.categories.filter((c) => c.parentId == null);
      categories.forEach((category: Category, index: number) => {
        category.deafLevel = 0;
        category.index =
          index * Math.pow(0.1, category.deafLevel);
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
        c.index =
          (parent?.index ?? 0) + index * Math.pow(0.1, c.deafLevel);
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

  createDocumentForm() {
    this.documentForm = this.fb.group({
      name: [this.data.name, [Validators.required]],
      description: [''],
      categoryId: ['', [Validators.required]],
      documentStatusId: [''],
      storageSettingId: [''],
      clientId: [''],
      url: [this.data.url, [Validators.required]],
      extension: [this.extension, [Validators.required]],
      documentMetaTags: this.fb.array([]),
    });
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

  SaveDocument() {
    if (this.documentForm.valid) {
      const document = this.buildDocumentObject();
      this.sub$.sink = this.fileRequestDocumentService
        .addApproveDocument(document)
        .subscribe((documentInfo: FileRequestDocumentApprove) => {
          this.toastrService.success(
            this.translationService.getValue('FILE_APPROVE_SUCCESSFULLY')
          );
          this.dialogRef.close(document.categoryId);
        });
    } else {
      this.markFormGroupTouched(this.documentForm);
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

    this.sub$.sink = this.commonService
      .addDocumentAuditTrail(objDocumentAuditTrail)
      .subscribe((c) => {
        this.dialogRef.close(true);
      });
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

  buildDocumentObject(): FileRequestDocumentApprove {
    const document: FileRequestDocumentApprove = {
      categoryId: this.documentForm.get('categoryId')?.value ?? '',
      documentStatusId: this.documentForm.get('documentStatusId')?.value ?? '',
      clientId: this.documentForm.get('clientId')?.value ?? '',
      storageSettingId: this.documentForm.get('storageSettingId')?.value ?? '',
      description: this.documentForm.get('description')?.value,
      name: this.documentForm.get('name')?.value,
      fileRequestId: this.data.fileRequestId,
      fileRequestDocumentId: this.data.id,
      url: this.data.url,
      documentMetaDatas: [...this.buildMetaTagObject()],
      extension: this.data.url.split('.')[1],
    };
    return document;
  }

  onMetaTagKeyChange(event: MatSelectChange<string>, index: number) {
    const selectedTagId = event.value;
    const metaTagsArray = this.documentMetaTagsArray.value.map(
      (tag: { documentMetaTagId: string }) => tag.documentMetaTagId
    );

    // Check for duplicate meta tags
    if (metaTagsArray.filter((id: string) => id === selectedTagId).length > 1) {
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
}
