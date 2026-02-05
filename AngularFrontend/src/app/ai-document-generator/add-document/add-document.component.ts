import {
  ChangeDetectorRef,
  Component,
  Inject,
  inject,
  OnInit,
  Signal,
} from '@angular/core';
import {
  FormGroup,
  ReactiveFormsModule,
  FormArray,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { DialogModule } from '@angular/cdk/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { DocumentMetaTag } from '@core/domain-classes/document-meta-tag';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { Category } from '@core/domain-classes/category';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentStatus } from '../../document-status/document-status';
import { DocumentStore } from '../../document/document-list/document-store';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { ClientStore } from '../../client/client-store';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { TranslationService } from '../../core/services/translation.service';
import { CategoryService } from '../../core/services/category.service';
import { ClonerService } from '../../core/services/clone.service';
import { DocumentService } from '../../document/document.service';
import { DocumentMetaData } from '@core/domain-classes/document-meta-data';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { StorageSettingService } from '../../storage-setting/storage-setting.service';
import { StorageSetting } from '../../storage-setting/storage-setting';
import { MatButtonModule } from '@angular/material/button';
import { StorageTypePipe } from '../../storage-setting/storage-type.pipe';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgStyle } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ToastrService } from '@core/services/toastr-service';
import { retentionValidator } from '@shared/retention-validator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BaseComponent } from '../../base.component';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  imports: [
    DialogModule,
    ReactiveFormsModule,
    TranslateModule,
    MatSelectModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    StorageTypePipe,
    PageHelpTextComponent,
    MatDatepickerModule,
    NgStyle,
    MatCardModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  templateUrl: './add-document.component.html',
  styleUrl: './add-document.component.scss',
})
export class AddDocumentComponent extends BaseComponent implements OnInit {
  document: DocumentInfo;
  documentForm: FormGroup;
  extension: string = '';
  documentSource: string;
  allCategories: Category[] = [];
  categories: Category[] = [];
  documentStatuses: DocumentStatus[] = [];
  documentStore = inject(DocumentStore);
  foldersViewStore = inject(FoldersViewStore);
  storageSettingService = inject(StorageSettingService);
  isCategoryReadonly: boolean;
  allStorageSetting: StorageSetting<any>[] = [];
  get documentMetaTagsArray(): FormArray {
    return <FormArray>this.documentForm.get('documentMetaTags');
  }

  public clientStore = inject(ClientStore);
  public documentMetaTagStore = inject(DocumentMetaTagStore);
  metaTags: Signal<DocumentMetaTag[]> =
    this.documentMetaTagStore.documentMetaTags;
  loading: boolean = false;
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AddDocumentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo,
    private toastrService: ToastrService,
    private documentService: DocumentService,
    private cd: ChangeDetectorRef,
    private categoryService: CategoryService,
    private documentStatusService: DocumentStatusService,
    private clonerService: ClonerService
  ) {
    super();
  }

  ngOnInit(): void {
    this.isCategoryReadonly = false;
    this.getStorageSetting();
    this.createDocumentForm();
    this.getCategories();
    this.getDocumentStatuses();
  }

  getDocumentStatuses() {
    this.documentStatusService
      .getDocumentStatuss()
      .subscribe((c: DocumentStatus[]) => {
        if (c && c.length > 0) {
          this.documentStatuses = [...c];
        }
      });
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

  createDocumentForm() {
    this.documentForm = this.fb.group(
      {
        name: ['', [Validators.required]],
        description: [''],
        categoryId: [{ value: '', disabled: false }, [Validators.required]],
        documentStatusId: [''],
        clientId: [''],
        storageSettingId: [''],
        documentMetaTags: this.fb.array([]),
        documentContent: [this.data?.documentContent || ''],
        retentionPeriodInDays: ['0'],
        onExpiryAction: ['0'],
      },
      { validators: retentionValidator }
    );
  }

  saveDocument() {
    if (this.documentForm.valid) {
      this.loading = true;
      this.documentService
        .addAIDocumentCreated(this.buildDocumentObject())
        .subscribe((c: DocumentInfo) => {
          this.loading = false;
          this.document = c;
          this.toastrService.success(
            this.translationService.getValue('DOCUMENT_ADDED_SUCCESSFULLY')
          );
          this.documentStore.loadDocuments();
          this.foldersViewStore.setDocumentsEmpty();
          this.foldersViewStore.loadDocumentsByCategory(
            this.foldersViewStore.selectedCategoryId()
          );
          this.addDocumentTrail();
        });
    } else {
      this.documentForm.markAllAsTouched();
    }
  }

  addDocumentTrail() {
    const objDocumentAuditTrail: DocumentAuditTrail = {
      documentId: this.document.id,
      operationName: DocumentOperation.Created.toString(),
    };
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
    this.dialogRef.close('loaded');
  }

  private markFormGroupTouched(formGroup: FormGroup) {
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
            ? null
            : tag.metaTagDate
            ? new Date(tag.metaTagDate).toISOString()
            : null,
        metaTagType: tag.metaTagType,
      } as DocumentMetaData;
    });
    return metaTagObjects;
  }

  buildDocumentObject(): Partial<DocumentInfo> {
    const document: Partial<DocumentInfo> = {
      categoryId: this.documentForm.get('categoryId')?.value,
      documentStatusId: this.documentForm.get('documentStatusId')?.value,
      storageSettingId: this.documentForm.get('storageSettingId')?.value,
      clientId: this.documentForm.get('clientId')?.value,
      description: this.documentForm.get('description')?.value,
      name: this.documentForm.get('name')?.value,
      documentMetaDatas: [...this.buildMetaTagObject()],
      documentContent: this.data?.documentContent,
      extension: 'pdf',
      retentionPeriodInDays:
        this.documentForm.get('retentionPeriodInDays')?.value ?? null,
      onExpiryAction: this.documentForm.get('onExpiryAction')?.value ?? null,
    };
    return document;
  }
  onDocumentCancel() {
    this.dialogRef.close('canceled');
  }

  getStorageSetting() {
    this.storageSettingService
      .getStorageSettings()
      .subscribe((c: StorageSetting<any>[]) => {
        if (c && c?.length > 0) {
          this.allStorageSetting = [...c];
          const isDefaultItem = this.allStorageSetting.find((c) => c.isDefault);
          if (isDefaultItem) {
            this.documentForm.patchValue({
              storageSettingId: isDefaultItem.id,
            });
          }
        }
      });
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
}
