import { Component, Inject, inject, OnInit, Optional } from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Client } from '@core/domain-classes/client';
import { TranslateModule } from '@ngx-translate/core';
import { BaseComponent } from '../../base.component';
import { ClientStore } from '../client-store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatCardModule } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgClass } from '@angular/common';
import { toObservable } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-manage-client',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatIconModule,
    PageHelpTextComponent,
    MatCardModule,
    NgClass
  ],
  templateUrl: './manage-client.component.html',
  styleUrl: './manage-client.component.scss'
})
export class ManageClientComponent extends BaseComponent implements OnInit {
  client: Client;
  clientForm: UntypedFormGroup;
  isEditMode = false;
  private fb = inject(UntypedFormBuilder);
  private router = inject(Router);
  private activeRoute = inject(ActivatedRoute);
  public clientStore = inject(ClientStore);
  isDialogOpen: boolean = false;

  constructor(
    @Optional() public dialogRef: MatDialogRef<ManageClientComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: boolean | null,
  ) {
    super();
    this.isDialogOpen = data ? true : false;
    this.subscribeIsAddorUpdate();
  }


  ngOnInit(): void {
    this.createUserForm();
    this.sub$.sink = this.activeRoute.data.subscribe(
      (data) => {
        const clientData = data as { client: Client };
        if (clientData.client) {
          this.isEditMode = true;
          this.clientForm.patchValue(clientData.client);
          this.client = clientData.client;
        }
      });
  }

  createUserForm() {
    this.clientForm = this.fb.group({
      id: [''],
      companyName: ['', [Validators.required]],
      contactPerson: [''],
      lastName: [''],
      email: ['', [Validators.email]],
      phoneNumber: [''],
      address: [''],
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

  saveClient() {
    if (this.clientForm.valid) {

      const client = this.createBuildObject();

      this.clientStore.addUpdateClient(client);
    } else {
      this.markFormGroupTouched(this.clientForm);
    }
  }

  createBuildObject(): Client {
    const client: Client = {
      id: this.clientForm.get('id')?.value,
      contactPerson: this.clientForm.get('contactPerson')?.value,
      email: this.clientForm.get('email')?.value,
      phoneNumber: this.clientForm.get('phoneNumber')?.value,
      address: this.clientForm.get('address')?.value,
      companyName: this.clientForm.get('companyName')?.value,
    }
    return client;
  }

  subscribeIsAddorUpdate() {
    toObservable(this.clientStore.isAddUpdate).subscribe((flag) => {
      if (flag) {
        if (this.isDialogOpen) {
          this.dialogRef.close(this.clientStore.client());
        } else {
          this.router.navigate(['/client']);
        }
      }
      this.clientStore.resetflag();
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
