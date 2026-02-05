import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from '@core/domain-classes/category';
import { catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { ClonerService } from './clone.service';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  categories: Category[] = [];
  public categoryKey: string = 'categoryKey';
  private assignToMeCategoryKey: string = 'assignToMeCategoryKey';

  constructor(
    private httpClient: HttpClient,
    private clonerService: ClonerService,
    private commonHttpErrorService: CommonHttpErrorService
  ) {}

  getAllCategories(isLoad: boolean = false): Observable<Category[]> {
    if (!isLoad) {
      if (this.categories.length > 0) {
        return of(this.categories);
      } else if (sessionStorage.getItem(this.categoryKey) != null) {
        const categories = sessionStorage.getItem(this.categoryKey) as string;
        this.categories = JSON.parse(categories) ?? [];
        return of(this.categories);
      }
    }
    const url = `Category`;
    return this.httpClient.get<Category[]>(url).pipe(
      tap((categories) => {
        this.categories = this.clonerService.deepClone<Category[]>(categories);
        sessionStorage.setItem(
          this.categoryKey,
          JSON.stringify(this.categories)
        );
      })
    );
  }

  delete(id: string): Observable<void> {
    const url = `Category/${id}`;
    return this.httpClient.delete<void>(url);
  }

  update(category: Category): Observable<Category> {
    const url = `Category/${category.id}`;
    return this.httpClient.put<Category>(url, category);
  }

  add(category: Category): Observable<Category> {
    const url = 'Category';
    return this.httpClient.post<Category>(url, category).pipe(
      tap((newCategory) => {
        this.categories.push(this.clonerService.deepClone(newCategory));
        sessionStorage.setItem(
          this.categoryKey,
          JSON.stringify(this.categories)
        );
      }),
      switchMap((newCategory) =>
        this.getAllCategories(true).pipe(map(() => newCategory))
      )
    );
  }
  updateAssign(category: Category): Observable<Category> {
    const url = `Category/AssignToMe/${category.id}`;
    return this.httpClient.put<Category>(url, category);
  }

  addAssign(category: Category): Observable<Category> {
    const url = 'Category/AssignToMe';
    return this.httpClient.post<Category>(url, category).pipe(
      tap((newCategory) => {
        this.categories.push(this.clonerService.deepClone(newCategory));
        sessionStorage.setItem(
          this.categoryKey,
          JSON.stringify(this.categories)
        );
      }),
      switchMap((newCategory) =>
        this.getAllCategories(true).pipe(map(() => newCategory))
      )
    );
  }

  getCategoryById(id: string) {
    const url = `Category/${id}`;
    return this.httpClient.get<Category>(url);
  }

  getSubCategories(id: string) {
    const url = `Category/${id}/subcategories`;
    return this.httpClient.get<Category[]>(url);
  }

  getCategoriesByParentId(id: string) {
    const url = `Category/${id}/children`;
    return this.httpClient.get<Category[]>(url);
  }

  getCategoriesSharedByParentId(id: string) {
    const url = `Category/${id}/shared/children`;
    return this.httpClient.get<Category[]>(url);
  }
  getCategoriesHierarchicalBychildId(id: string) {
    const url = `Category/${id}/hierarchical`;
    return this.httpClient.get<Category[]>(url);
  }

  getAllCategoriesForDropDown() {
    const url = `Category/dropdown`;
    return this.httpClient.get<Category[]>(url);
  }

  getAllAssignToMeCategoriesForDropDown() {
    const url = `Category/AssignToMe/dropdown`;
    return this.httpClient.get<Category[]>(url);
  }

  getAllAssignToMeCategoriesSearchDropdown() {
    const url = `Category/AssignToMe/searchdropdown`;
    return this.httpClient.get<Category[]>(url);
  }

  archiveFolder(id: string): Observable<void> {
    const url = `Category/${id}/archive`;
    return this.httpClient.post<void>(url, null);
  }
  archiveAssignFolder(id: string): Observable<void | CommonError> {
    const url = `Category/${id}/AssignFolders/archive`;
    return this.httpClient
      .post<void>(url, null)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  restoreFolder(id: string): Observable<void | CommonError> {
    const url = `Category/${id}/restore`;
    return this.httpClient
      .post<void>(url, null)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  getArchiveCategoriesByParentId(id: string) {
    const url = `Category/${id}/archive`;
    return this.httpClient.get<Category[]>(url);
  }
}
