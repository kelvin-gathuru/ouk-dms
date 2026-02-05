import { Component, OnInit } from '@angular/core';
import { PageHelper } from '@core/domain-classes/pageHelper';
import { BaseComponent } from '../../base.component';
import {
  Validators,
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
} from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { ToastrService } from '@core/services/toastr-service';
import { PageHelperService } from '../page-helper.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TextEditorComponent } from '@shared/text-editor/text-editor.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { RichTextRequired } from '@shared/validators/rich-text.validator';

@Component({
  selector: 'app-manage-page-helper',
  templateUrl: './manage-page-helper.component.html',
  styleUrls: ['./manage-page-helper.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    TextEditorComponent,
    TranslateModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    ReactiveFormsModule
  ],
})
export class ManagePageHelperComponent extends BaseComponent implements OnInit {
  pageHelper: PageHelper;
  pageHelperForm: FormGroup;
  isEditMode = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private pageHelperService: PageHelperService,
    private toastrService: ToastrService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.createPageHelperForm();
    this.sub$.sink = this.activeRoute.data.subscribe(
      (data: any) => {
        if (data.pageHelper) {
          this.isEditMode = true;
          this.pageHelperForm.patchValue(data.pageHelper);
          this.pageHelper = data.pageHelper;
        }
      }
    );
  }

  createPageHelperForm() {
    this.pageHelperForm = this.fb.group({
      id: [''],
      name: ['', [Validators.required]],
      description: ['', [RichTextRequired]],
    });
  }

  createBuildObject(): PageHelper {
    const pageHelper: PageHelper = {
      id: this.pageHelperForm.get('id')?.value,
      name: this.pageHelperForm.get('name')?.value,
      description: this.pageHelperForm.get('description')?.value,
    };
    return pageHelper;
  }

  saveUser() {
    if (!this.pageHelperForm.valid) {
      this.pageHelperForm.markAllAsTouched();
      return;
    }

    const pageHelper = this.createBuildObject();
    if (this.isEditMode) {
      this.sub$.sink = this.pageHelperService
        .updatePageHelper(pageHelper)
        .subscribe(() => {
          this.toastrService.success(
            this.translationService.getValue(
              'PAGE_HELPER_UPDATED_SUCCESSFULLY'
            )
          );
          this.router.navigate(['/page-helper']);
        });
    } else {
      this.sub$.sink = this.pageHelperService
        .addPageHelper(pageHelper)
        .subscribe(() => {
          this.toastrService.success(
            this.translationService.getValue(
              'PAGE_HELPER_CREATED_SUCCESSFULLY'
            )
          );
          this.router.navigate(['/page-helper']);
        });
    }
  }
}
