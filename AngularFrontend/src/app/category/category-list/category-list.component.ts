import { Component, inject, OnInit } from '@angular/core';
import { Category } from '@core/domain-classes/category';
import { CategoryService } from '@core/services/category.service';
import { ToastrService } from '@core/services/toastr-service';
import { Observable } from 'rxjs';
import { ArchiveFoldersViewStore } from '../../archive-folders/archive-folders-view-store';
import { BaseComponent } from '../../base.component';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { CategoryListPresentationComponent } from '../category-list-presentation/category-list-presentation.component';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.scss'],
  standalone: true,
  imports: [CategoryListPresentationComponent, AsyncPipe]
})
export class CategoryListComponent extends BaseComponent implements OnInit {
  categories$: Observable<Category[]>;
  archiveFoldersViewStore = inject(ArchiveFoldersViewStore);
  foldersViewStore = inject(FoldersViewStore);
  // loading$: Observable<boolean>;
  constructor(
    private categoryService: CategoryService,
    private toastrService: ToastrService
  ) {
    super();
  }
  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categories$ = this.categoryService.getAllCategories(true);
  }

  deleteCategory(category: Category): void {
    this.categoryService.archiveFolder(category.id ?? '').subscribe(d => {
      this.toastrService.success(this.translationService.getValue(`CATEGORY_ARCHIVE_SUCCESSFULLY`));
      this.getCategories();
      if (category.parentId == this.foldersViewStore.selectedCategoryId() || (category.id == null || this.foldersViewStore.selectedCategoryId() == '')) {
        this.foldersViewStore.loadCategoriesById(
          this.foldersViewStore.selectedCategoryId()
        );
      }
      if (category.parentId == this.archiveFoldersViewStore.selectedCategoryId() || (category.id == null || this.archiveFoldersViewStore.selectedCategoryId() == '')) {
        this.archiveFoldersViewStore.loadCategoriesById(
          this.archiveFoldersViewStore.selectedCategoryId()
        );
      }
    });

  }

  manageCategory(category: Category): void {
    this.getCategories();
  }

  refershCategories(): void {
    this.getCategories();
  }
}
