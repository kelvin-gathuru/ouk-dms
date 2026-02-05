import { Component, Inject, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Action } from '@core/domain-classes/action';
import { TranslationService } from '@core/services/translation.service';
import { TranslateModule } from '@ngx-translate/core';

import { ToastrService } from '@core/services/toastr-service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ActionStore } from '../store/action-store';
import { toObservable } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-manage-action',
  imports: [
    TranslateModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './manage-action.component.html',
  styleUrl: './manage-action.component.scss'
})
export class ManageActionComponent {
  isEdit: boolean = false;
  actionForm!: FormGroup;
  isDisabled = true;
  translationService = inject(TranslationService);
  dialogRef = inject(MatDialogRef<ManageActionComponent>);
  actionStore = inject(ActionStore);
  toastrService = inject(ToastrService);
  fb = inject(FormBuilder);

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { pageId: string, pageName: string, action: Action }
  ) {
    this.subscribeIsAddorUpdate();
  }

  ngOnInit(): void {
    this.createForm();
    if (this.data.action) {
      this.actionForm.patchValue(this.data.action);
      this.isEdit = true
    }
  }

  createForm() {
    this.actionForm = this.fb.group({
      id: [''],
      pagename: [{ value: this.data.pageName, disabled: true }],
      name: ['', Validators.required],
      order: ['', [Validators.required]],
      code: ['', [Validators.required]],
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  saveAction(): void {
    if (!this.actionForm.valid) {
      this.actionForm.markAllAsTouched();
      return;
    }
    let action: Action = this.actionForm.getRawValue();
    action.pageId = this.data.pageId;

    this.actionStore.addUpdateAction(action);
  }

  subscribeIsAddorUpdate() {
    toObservable(this.actionStore.isAddUpdate).subscribe((flag) => {
      if (flag) {
        this.dialogRef.close();
      }
      this.actionStore.resetflag();
    });
  }
}

