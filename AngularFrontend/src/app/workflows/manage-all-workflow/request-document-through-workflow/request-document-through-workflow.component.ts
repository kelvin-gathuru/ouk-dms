import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { Workflow } from '@core/domain-classes/workflow';
import { WorkflowService } from '../../workflow.service';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Category } from '@core/domain-classes/category';
import { CategoryService } from '@core/services/category.service';
import { ClonerService } from '@core/services/clone.service';
import { RequestDocWorkflow } from './request-doc-workflow';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { StorageSettingService } from '../../../storage-setting/storage-setting.service';
import { RouterModule } from '@angular/router';
import { StorageTypePipe } from '../../../storage-setting/storage-type.pipe';
import { ToastrService } from '@core/services/toastr-service';
import { TranslateModule } from '@ngx-translate/core';
import { TranslationService } from '@core/services/translation.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { StorageSetting } from '../../../storage-setting/storage-setting';
import { MatCardModule } from '@angular/material/card';
import { NgStyle } from '@angular/common';
@Component({
  selector: 'app-request-document-through-workflow',
  imports: [
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatTooltipModule,
    RouterModule,
    MatSelectModule,
    StorageTypePipe,
    TranslateModule,
    PageHelpTextComponent,
    MatCardModule,
    NgStyle,
  ],
  templateUrl: './request-document-through-workflow.component.html',
  styleUrl: './request-document-through-workflow.component.scss',
})
export class RequestDocumentThroughWorkflowComponent implements OnInit {
  reqDocumentWorkflowForm: FormGroup;
  workflows: Workflow[] = [];
  allCategories: Category[] = [];
  storageSettings: StorageSetting<any>[] = [];
  categories: Category[] = [];
  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private storageSettingService: StorageSettingService,
    private categoryService: CategoryService,
    private clonerService: ClonerService,
    private toastrService: ToastrService,
    private translationService: TranslationService
  ) {}

  ngOnInit() {
    this.getReqDocumentWorkflows();
    this.getCategories();
    this.getStorageSetting();
    this.createReqDocumentWorkflowForm();
  }

  getReqDocumentWorkflows() {
    this.workflowService
      .getReqDocumentWorkflows()
      .subscribe((res: Workflow[]) => {
        this.workflows = res;
      });
  }

  createReqDocumentWorkflowForm() {
    this.reqDocumentWorkflowForm = this.fb.group({
      workflowId: [null, [Validators.required]],
      categoryId: [null, [Validators.required]],
      storageSettingId: [''],
      comment: [''],
      name: ['', [Validators.required]],
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

  getStorageSetting() {
    this.storageSettingService
      .getStorageSettings()
      .subscribe((c: StorageSetting<void>[]) => {
        if (c && c.length > 0) {
          this.storageSettings = c;
          const isDefaultItem = this.storageSettings.find((c) => c.isDefault);
          if (isDefaultItem) {
            this.reqDocumentWorkflowForm.patchValue({
              storageSettingId: isDefaultItem.id,
            });
          }
        }
      });
  }

  createReqDocumentWorkflow() {
    if (!this.reqDocumentWorkflowForm.valid) {
      this.reqDocumentWorkflowForm.markAllAsTouched();
      return;
    }

    const requestDocWorkflow: RequestDocWorkflow = {
      workflowId: this.reqDocumentWorkflowForm.get('workflowId')!.value,
      categoryId: this.reqDocumentWorkflowForm.get('categoryId')!.value,
      storageSettingId:
        this.reqDocumentWorkflowForm.get('storageSettingId')!.value,
      comment: this.reqDocumentWorkflowForm.get('comment')!.value,
      name: this.reqDocumentWorkflowForm.get('name')!.value,
    };
    this.workflowService
      .createReqDocumentWorkflow(requestDocWorkflow)
      .subscribe((res: boolean) => {
        if (res) {
          this.toastrService.success(
            `${this.translationService.getValue(
              'DOCUMENT_REQUEST_IS_SUCCESSFULLY_CREATED'
            )}`
          );
        }
      });
  }
}
