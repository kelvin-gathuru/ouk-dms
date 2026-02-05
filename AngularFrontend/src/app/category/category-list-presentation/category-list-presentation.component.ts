import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { Category } from '@core/domain-classes/category';
import { BaseComponent } from '../../base.component';
import { ManageCategoryComponent } from '../manage-category/manage-category.component';
import { SecurityService } from '@core/security/security.service';
import { CategoryPermissionListComponent } from '../../document/category-permission/category-permission-list/category-permission-list.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTreeModule } from '@angular/material/tree';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';


@Component({
  selector: 'app-category-list-presentation',
  templateUrl: './category-list-presentation.component.html',
  styleUrls: ['./category-list-presentation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    PageHelpTextComponent,
    HasClaimDirective,
    MatIconModule,
    MatButtonModule,
    MatTreeModule,
    TranslateModule,
    MatCardModule,
    MatTooltipModule
  ]
})
export class CategoryListPresentationComponent extends BaseComponent {
  @Input() categories: Category[] | null = [];
  @Output() addEditCategoryHandler: EventEmitter<Category> =
    new EventEmitter<Category>();
  @Output() deleteCategoryHandler: EventEmitter<Category> =
    new EventEmitter<Category>();
  @Output() refershCategoriesHandler: EventEmitter<void> =
    new EventEmitter<void>();

  constructor(
    private dialog: MatDialog,
    private commonDialogService: CommonDialogService,
    private securityService: SecurityService,
  ) {
    super();
  }
  childrenAccessor = (node: Category) => node.children ?? [];
  hasChild = (_: number, node: Category) =>
    !!node.children && node.children.length > 0;

  deleteCategory(category: Category): void {
    this.commonDialogService
      .deleteConfirmtionDialog(
        this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_ARCHIVE')
      )
      .subscribe((isTrue) => {
        if (isTrue) {
          this.deleteCategoryHandler.emit(category);
        }
      });
  }

  manageCategory(category: Category | null): void {
    if (!this.hasPermission(['CREATE_CATEGORY'])) {
      return;
    }
    const dialogRef = this.dialog.open(ManageCategoryComponent, {
      width: '400px',
      data: Object.assign({}, category),
    });

    dialogRef.afterClosed().subscribe((result: Category) => {
      if (result) {
        this.addEditCategoryHandler.emit(result);
      }
    });
  }

  manageCategoryPermission(category: Category) {
    const dialogRef = this.dialog.open(CategoryPermissionListComponent, {
      data: category,
      width: '100%',
      maxWidth: '50vw'
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.refershCategoriesHandler.emit();
      }
    });
  }

  addSubCategory(category: Category) {
    this.manageCategory({
      id: '',
      description: '',
      name: '',
      parentId: category.id,
      createdDate: new Date(),
    });
  }

  hasPermission(permission: string[]): boolean {
    return this.securityService.hasClaim(permission);
  }
}
