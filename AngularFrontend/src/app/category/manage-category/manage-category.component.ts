import {
  Component,
  inject,
  Inject,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { Category } from '@core/domain-classes/category';
import { CategoryService } from '@core/services/category.service';
import { ClonerService } from '@core/services/clone.service';
import { TranslateModule } from '@ngx-translate/core';
import { BaseComponent } from '../../base.component';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { NgStyle } from '@angular/common';

@Component({
  selector: 'app-manage-category',
  templateUrl: './manage-category.component.html',
  styleUrls: ['./manage-category.component.scss'],
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatTooltipModule,
    MatDialogModule,
    PageHelpTextComponent,
    HasClaimDirective,
    MatCardModule,
    MatIconModule,
    NgStyle,
  ],
})
export class ManageCategoryComponent
  extends BaseComponent
  implements OnChanges {
  isEdit = false;
  categories: Category[] = [];
  allCategories: Category[] = [];
  categoryForm: FormGroup;
  foldersViewStore = inject(FoldersViewStore);

  constructor(
    public dialogRef: MatDialogRef<ManageCategoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Category,
    private categoryService: CategoryService,
    private clonerService: ClonerService,
    private fb: FormBuilder
  ) {
    super();
  }

  ngOnInit() {
    if (this.data && this.data.id) {
      this.isEdit = true;
    }
    this.createForm();
    this.sub$.sink = this.categoryService
      .getAllCategories(true)
      .subscribe((c) => {
        this.categories = c;
        const categories = this.categories.filter((c) => c.parentId == null);
        categories.forEach((category: Category, index: number) => {
          if (this.data?.id != category.id) {
            category.deafLevel = 0;
            category.index = index * Math.pow(0.1, category.deafLevel);
            category.displayName = category.name;
            this.allCategories.push(category);
            this.setDeafLevel(category);
          }
        });
        this.allCategories = this.clonerService.deepClone(this.allCategories);
      });
  }

  createForm(): void {
    this.categoryForm = this.fb.group({
      id: [this.data?.id],
      name: [this.data?.name, [Validators.required]],
      description: [this.data?.description],
      parentId: [
        {
          value: this.data?.parentId ? this.data?.parentId : '',
          disabled: this.data?.id ? true : false,
        },
      ],
    });
  }

  getCategoryNameById(id: string): string {
  const category = this.allCategories?.find(category => category.id === id);
  return category?.name || '';
}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['data']) {
      if (this.data.id) {
        this.isEdit = true;
      }
    }
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

  onCancel(): void {
    this.dialogRef.close();
  }

  saveCategory(): void {
    if (this.categoryForm.valid) {
      this.categoryForm.get('parentId')?.enable({ emitEvent: false });
      const formValue = { ...this.categoryForm.value };
      // if you want, disable it back again
      if (this.data?.id) {
        this.categoryForm.get('parentId')?.disable({ emitEvent: false });
      }
      //const formValue = { ...this.categoryForm.value };
      if (formValue.id) {
        this.categoryService.update(formValue).subscribe({
          next: (c) => {
            if (
              formValue.parentId == this.foldersViewStore.selectedCategoryId()
            ) {
              this.foldersViewStore.loadCategoriesBySelectedCategoryId();
            }
            this.dialogRef.close(c);
          },
          error: (err) => {
            console.log(err);
          },
        });
      } else {
        this.categoryService.add(formValue).subscribe({
          next: (c) => {
            if (
              formValue.parentId == this.foldersViewStore.selectedCategoryId()
            ) {
              this.foldersViewStore.loadCategoriesBySelectedCategoryId();
            }
            this.dialogRef.close(c);
          },
          error: (err) => {
            console.log(err);
          },
        });
      }
    } else {
      this.categoryForm.markAllAsTouched();
    }
  }
}
