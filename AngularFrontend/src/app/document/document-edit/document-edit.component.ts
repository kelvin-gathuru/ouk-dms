import { ChangeDetectorRef, Component, inject, Inject, OnInit, Signal } from '@angular/core';
import {
  FormArray,
  FormGroup,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { Category } from '@core/domain-classes/category';
import { DocumentAuditTrail } from '@core/domain-classes/document-audit-trail';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { DocumentOperation } from '@core/domain-classes/document-operation';
import { DocumentMetaData } from '@core/domain-classes/document-meta-data';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { DocumentService } from '../document.service';
import { DocumentStatus } from '../../document-status/document-status';
import { DocumentCategoryStatus } from '@core/domain-classes/document-category';
import { CategoryService } from '@core/services/category.service';
import { DocumentStatusService } from '../../document-status/document-status.service';
import { ClonerService } from '@core/services/clone.service';
import { DocumentStore } from '../document-list/document-store';
import { ClientStore } from '../../client/client-store';
import { FoldersViewStore } from '../folders-view/folders-view-store';
import { DocumentMetaTagStore } from '../../document-meta-tag/document-meta-tag-store';
import { DocumentMetaTag } from '@core/domain-classes/document-meta-tag';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { NgStyle } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { TranslateModule } from '@ngx-translate/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { retentionValidator } from '@shared/retention-validator';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-document-edit',
  templateUrl: './document-edit.component.html',
  styleUrls: ['./document-edit.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    PageHelpTextComponent,
    MatSelectModule,
    HasClaimDirective,
    TranslateModule,
    MatDatepickerModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    NgStyle,
    MatTooltipModule

  ]
})
export class DocumentEditComponent extends BaseComponent implements OnInit {
  document: DocumentInfo;
  documentForm: UntypedFormGroup;
  extension: string = '';
  documentSource: string;
  allCategories: Category[] = [];
  categories: Category[] = [];
  documentStatuses: DocumentStatus[] = [];
  documentStore = inject(DocumentStore);
  foldersViewStore = inject(FoldersViewStore);
  get documentMetaTagsArray(): FormArray {
    return <FormArray>this.documentForm.get('documentMetaTags');
  }

  public clientStore = inject(ClientStore);
  public documentMetaTagStore = inject(DocumentMetaTagStore);
  metaTags: Signal<DocumentMetaTag[]> = this.documentMetaTagStore.documentMetaTags;
  selectedCategory: Category | undefined;
  constructor(
    private fb: UntypedFormBuilder,
    public dialogRef: MatDialogRef<DocumentEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DocumentCategoryStatus,
    private toastrService: ToastrService,
    private documentService: DocumentService,
    private cd: ChangeDetectorRef,
    private categoryService: CategoryService,
    private documentStatusService: DocumentStatusService,
    private clonerService: ClonerService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.document = this.data.document;
    this.createDocumentForm();
    this.pushValuesDocumentMetatagArray();
    this.patchDocumentForm();
    this.getCategories();
    this.populateDocumentStatus();

    ;
    if (!!this.data.isCategoryReadonly) {
      this.documentForm.get('categoryId')?.disable();
    }
  }

  populateDocumentStatus() {
    if (
      this.data &&
      this.data.documentStatuses &&
      this.data.documentStatuses.length > 0
    ) {
      this.documentStatuses = [...this.data.documentStatuses];
    } else {
      this.documentStatusService
        .getDocumentStatuss()
        .subscribe((c: DocumentStatus[]) => {
          if (c && c.length > 0) {
            this.documentStatuses = [...c];
          }
        });
    }
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
      this.onCategoryChange({ value: this.data.document.categoryId });
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

  patchDocumentForm() {
    this.documentForm.patchValue({
      name: this.data.document.name,
      description: this.data.document.description,
      categoryId: this.data.document.categoryId,
      documentStatusId: this.data.document.documentStatusId,
      clientId: this.data.document.clientId,
      retentionPeriodInDays: this.data.document.retentionPeriodInDays ? this.data.document.retentionPeriodInDays.toString() : null,
      onExpiryAction: this.data.document.onExpiryAction,
    });
  }

  createDocumentForm() {
    this.documentForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      categoryId: [{ value: '', disabled: this.data?.isCategoryReadonly ?? false }, [Validators.required]],
      documentStatusId: [''],
      clientId: [''],
      documentMetaTags: this.fb.array([]),
      retentionPeriodInDays: ['0'],
      onExpiryAction: ['0'],
    }, { validators: retentionValidator });
  }

  SaveDocument() {
    if (this.documentForm.valid) {
      this.sub$.sink = this.documentService
        .updateDocument(this.buildDocumentObject())
        .subscribe((c) => {
          this.toastrService.success(
            this.translationService.getValue('DOCUMENT_UPDATE_SUCCESSFULLY')
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
      documentId: this.data.document.id,
      operationName: DocumentOperation.Modified.toString(),
    };
    this.documentStore.addDocumentAudit(objDocumentAuditTrail);
    this.dialogRef.close('loaded');
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

  buildDocumentObject(): Partial<DocumentInfo> {
    const document: Partial<DocumentInfo> = {
      id: this.data.document.id,
      categoryId: this.documentForm.get('categoryId')?.value,
      documentStatusId: this.documentForm.get('documentStatusId')?.value,
      clientId: this.documentForm.get('clientId')?.value,
      description: this.documentForm.get('description')?.value,
      name: this.documentForm.get('name')?.value,
      documentMetaDatas: [...this.buildMetaTagObject()],
      retentionPeriodInDays: this.documentForm.get('retentionPeriodInDays')?.value,
      onExpiryAction: this.documentForm.get('onExpiryAction')?.value,
    };
    return document;
  }
  onDocumentCancel() {
    this.dialogRef.close('canceled');
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
      metaTagType: [0]
    });
  }

  pushValuesDocumentMetatagArray() {
    this.sub$.sink = this.documentService
      .getdocumentMetadataById(this.data.document.id ?? '')
      .subscribe((result: DocumentMetaData[]) => {
        if (result.length > 0) {
          result.map((documentMetatag) => {
            this.documentMetaTagsArray.push(
              this.editDocmentMetaData(documentMetatag)
            );
          });
        } else {
          this.documentMetaTagsArray.push(this.buildDocumentMetaTag());
        }
      });
  }

  onMetaTagKeyChange(event: any, index: number) {
    const selectedTagId = event.value;
    const metaTagControl = this.documentMetaTagsArray.at(index);
    const metaTagValueControl = metaTagControl.get('metatag');
    const metaTagDateControl = metaTagControl.get('metaTagDate');

    if (!selectedTagId) {
      // Clear both key and value if no tag is selected
      metaTagControl.get('documentMetaTagId')?.setValue(null);
      metaTagValueControl?.setValue(null);
      metaTagDateControl?.setValue(null);
      metaTagValueControl?.clearValidators();
      metaTagDateControl?.clearValidators();
      metaTagValueControl?.updateValueAndValidity();
      metaTagDateControl?.updateValueAndValidity();
      return;
    }

    const metaTagsArray = this.documentMetaTagsArray.value.map(
      (tag: { documentMetaTagId: string }) => tag.documentMetaTagId
    );

    // Check for duplicate meta tags
    if (metaTagsArray.filter((id: string) => id === selectedTagId).length > 1) {
      this.toastrService.error(
        this.translationService.getValue('DUPLICATE_META_TAG_NOT_ALLOWED')
      );
      metaTagControl.get('documentMetaTagId')?.setValue(null);
      return;
    }
    const selectedMetaTag = this.metaTags().find((tag) => tag.id === selectedTagId);
    if (!selectedMetaTag) return;

    // Update metaTagType dynamically
    metaTagControl.get('metaTagType')?.setValue(selectedMetaTag.type);

    if (selectedMetaTag.type === 0) {
      // Text field case
      metaTagValueControl?.setValidators([Validators.required]);
      metaTagValueControl?.setValue('');
      metaTagValueControl?.updateValueAndValidity();

      metaTagDateControl?.clearValidators();
      metaTagDateControl?.setValue(null);
      metaTagDateControl?.updateValueAndValidity();
    } else if (selectedMetaTag.type === 1) {
      // Date picker case
      metaTagDateControl?.setValidators([Validators.required]);
      metaTagDateControl?.updateValueAndValidity();

      metaTagValueControl?.clearValidators();
      metaTagValueControl?.setValue('');
      metaTagValueControl?.updateValueAndValidity();
    }
  }

  editDocmentMetaData(documentMetatag: DocumentMetaData): FormGroup {
    const formDocumentMetaForm = this.fb.group({
      id: [documentMetatag.id],
      documentId: [documentMetatag.documentId],
      documentMetaTagId: [documentMetatag.documentMetaTagId],
      metatag: [documentMetatag.metatag],
      metaTagDate: [documentMetatag.metaTagDate],
      metaTagType: [documentMetatag.metaTagType]
    });
    if (documentMetatag.metaTagType === 0) {
      formDocumentMetaForm.get('metatag')?.setValidators([Validators.required]);
      formDocumentMetaForm.get('metatag')?.updateValueAndValidity();
    } else if (documentMetatag.metaTagType === 1) {
      formDocumentMetaForm.get('metaTagDate')?.setValidators([Validators.required]);
      formDocumentMetaForm.get('metaTagDate')?.updateValueAndValidity();
    }
    return formDocumentMetaForm;
  }
}
