import { Component, inject, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { Page } from '@core/domain-classes/page';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { PageStore } from '../store/page-store';
import { toObservable } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-page',
  imports: [
    TranslateModule,
    MatIconModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './manage-page.component.html',
  styleUrl: './manage-page.component.scss'
})

export class ManagePageComponent extends BaseComponent implements OnInit {
  pageForm!: FormGroup;
  isEdit: boolean = false;
  dialogRef = inject(MatDialogRef<ManagePageComponent>);
  pageStore = inject(PageStore);
  toastrService = inject(ToastrService);
  fb = inject(FormBuilder);
  router = inject(Router);

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Page,
  ) {
    super();
    this.subscribeIsAddorUpdate();
  }

  ngOnInit(): void {
    this.createForm();
    if (this.data && this.data.id) {
      this.pageForm.patchValue(this.data);
      this.isEdit = true;
    }
  }

  createForm() {
    this.pageForm = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      order: ['', [Validators.required]]
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  savePage(): void {
    if (!this.pageForm.valid) {
      this.pageForm.markAllAsTouched();
      return;
    }
    const page: Page = this.pageForm.value;

    this.pageStore.addUpdatePage(page);
  }

  subscribeIsAddorUpdate() {
    toObservable(this.pageStore.isAddUpdate).subscribe((flag) => {
      if (flag) {
        this.dialogRef.close();
      }
      this.pageStore.resetflag();
    });
  }
}
