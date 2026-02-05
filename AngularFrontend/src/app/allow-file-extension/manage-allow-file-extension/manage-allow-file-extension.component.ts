import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastrService } from '@core/services/toastr-service';
import { AllowFileExtensionService } from '../allow-file-extension.service';
import { BaseComponent } from '../../base.component';
import { AllowFileExtension } from '@core/domain-classes/allow-file-extension';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { FileType } from '@core/domain-classes/file-type.enum';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';

@Component({
  selector: 'app-manage-allow-file-extension',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatOptionModule,
    MatIconModule,
    MatCardModule,
    PageHelpTextComponent
  ],
  templateUrl: './manage-allow-file-extension.component.html',
  styleUrl: './manage-allow-file-extension.component.scss'
})
export class ManageAllowFileExtensionComponent
  extends BaseComponent
  implements OnInit {
  isEditMode: boolean = false;
  allowFileExtensionForm: FormGroup;
  fileTypes: { key: string; value: number }[] = [];
  get extensions(): FormArray {
    return this.allowFileExtensionForm.get('extensions') as FormArray;
  }

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activeRoute: ActivatedRoute,
    private allowFileExtensionService: AllowFileExtensionService,
    private toastrService: ToastrService
  ) {
    super();
  }

  createAllowFileExtensionForm() {
    this.allowFileExtensionForm = this.fb.group({
      id: [],
      fileType: [{ value: null, disabled: false }, [Validators.required]],
      extensions: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.fileTypes = this.getEnumValues(FileType);
    this.createAllowFileExtensionForm();

    this.sub$.sink = this.activeRoute.data.subscribe(
      (data) => {
        const allowFileExtension = (data as { allowFileExtension: AllowFileExtension }).allowFileExtension;
        if (allowFileExtension) {
          this.isEditMode = true;
          this.allowFileExtensionForm.patchValue({
            id: allowFileExtension.id,
            fileType: allowFileExtension.fileType
          });
          if (allowFileExtension.extensions && allowFileExtension.extensions.length > 0) {
            allowFileExtension.extensions.forEach((ext: string) => {
              this.extensions.push(this.buildExtension(ext));
            });
          }
          this.allowFileExtensionForm.get('fileType')?.disable();
        } else {
          this.extensions.push(this.buildExtension());
        }
      }
    );
  }
  addExtensionField(): void {
    this.extensions.push(this.buildExtension());
  }

  buildExtension(extension?: string): FormGroup {
    return this.fb.group({
      extension: [extension ? extension : '', [Validators.required, Validators.pattern(/^[a-zA-Z0-9,]*$/)]],
    });
  }

  trackByIndex(index: number, control: any): number {
  return index;
}

  getEnumValues(enumObj: any): { key: string; value: number }[] {
    return Object.keys(enumObj)
      .filter((key) => isNaN(Number(key)))
      .map((key) => ({
        key: key,
        value: enumObj[key],
      }));
  }

  removeExtensionField(index: number): void {
    if (this.extensions.length <= 1) {
      this.toastrService.error(
        this.translationService.getValue('AT_LEAST_ONE_EXTENSION_REQUIRED')
      );
      return;
    }
    this.extensions.removeAt(index);
  }

  saveAllowFileExtension() {
    if (this.allowFileExtensionForm.valid) {
      const allowFileExtension = this.createBuildObject();
      if (this.isEditMode) {
        this.sub$.sink = this.allowFileExtensionService
          .updateAllowFileExtension(allowFileExtension)
          .subscribe(() => {
            this.toastrService.success(
              this.translationService.getValue(
                'FILE_TYPE_EXTENSION_UPDATED_SUCCESSFULLY'
              )
            );
            this.router.navigate(['/allow-file-extension']);
          });
      } else {
        this.sub$.sink = this.allowFileExtensionService
          .addAllowFileExtension(allowFileExtension)
          .subscribe(() => {
            this.toastrService.success(
              this.translationService.getValue(
                'FILE_TYPE_EXTENSION_CREATED_SUCCESSFULLY'
              )
            );
            this.router.navigate(['/allow-file-extension']);
          });
      }
    } else {
      this.allowFileExtensionForm.markAllAsTouched();
    }
  }

  createBuildObject(): AllowFileExtension {
    const id = this.allowFileExtensionForm.get('id')?.value;
    const extensions = this.extensions.controls.map((x) => x.get('extension')?.value);
    const data: AllowFileExtension = {
      id: id,
      fileType: this.allowFileExtensionForm.get('fileType')?.value,
      extension: extensions.join(','),
      extensions: extensions,
    };
    return data;
  }
}
