import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { DocumentMetaTag } from '@core/domain-classes/document-meta-tag';
import { BaseComponent } from '../../base.component';
import { DocumentMetaTagStore } from '../document-meta-tag-store';
import { TranslateModule } from '@ngx-translate/core';
import { MatSelectModule } from '@angular/material/select';
import { MetaTagType } from '@core/domain-classes/meta-tag-type.enum';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-manage-document-meta-tag',
  imports: [
    TranslateModule,
    RouterModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatSlideToggleModule,
    PageHelpTextComponent,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './manage-document-meta-tag.component.html',
  styleUrl: './manage-document-meta-tag.component.scss'
})
export class ManageDocumentMetaTagComponent extends BaseComponent implements OnInit {
  documentMetaTag: DocumentMetaTag;
  documentMetaTagForm: UntypedFormGroup;
  isEditMode = false;
  fileTypes: { key: string; value: number }[] = [];

  private fb = inject(UntypedFormBuilder);
  private activeRoute = inject(ActivatedRoute);
  public documentMetaTagStore = inject(DocumentMetaTagStore);


  ngOnInit(): void {
    this.fileTypes = this.getEnumValues(MetaTagType);
    this.createDocumentMetaTagForm();
    this.sub$.sink = this.activeRoute.data.subscribe((data: any) => {
      if (data && data.documentMetaTag) {
        this.isEditMode = true;
        this.documentMetaTagForm.patchValue(data.documentMetaTag);
        this.documentMetaTag = data.documentMetaTag;
      }
    });
  }

  createDocumentMetaTagForm() {
    this.documentMetaTagForm = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      type: ['', Validators.required],
      isEditable: [false]
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

  getEnumValues(enumObj: any): { key: string; value: number }[] {
    return Object.keys(enumObj)
      .filter((key) => isNaN(Number(key)))
      .map((key) => ({
        key: key,
        value: enumObj[key],
      }));
  }

  saveDocumentMetaTag() {
    if (this.documentMetaTagForm.valid) {
      const documentMetaTag = this.createBuildObject();
      if (this.isEditMode) {
        this.documentMetaTagStore.addUpdateDocumentMetaTag(documentMetaTag);
      } else {
        this.documentMetaTagStore.addUpdateDocumentMetaTag(documentMetaTag);
      }
    } else {
      this.markFormGroupTouched(this.documentMetaTagForm);
    }
  }

  createBuildObject(): DocumentMetaTag {
    const documentMetaTag: DocumentMetaTag = {
      id: this.documentMetaTagForm.get('id')?.value,
      name: this.documentMetaTagForm.get('name')?.value,
      type: this.documentMetaTagForm.get('type')?.value,
      isEditable: this.documentMetaTagForm.get('isEditable')?.value
    }
    return documentMetaTag;
  }
}
