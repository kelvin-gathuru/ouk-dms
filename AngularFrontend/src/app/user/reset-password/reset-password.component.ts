import { Component, Inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { User } from '@core/domain-classes/user';
import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { UserService } from '../user.service';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    TranslateModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
})
export class ResetPasswordComponent extends BaseComponent implements OnInit {
  resetPasswordForm: FormGroup;
  constructor(
    private userService: UserService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ResetPasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data: User,
    private toastrService: ToastrService

  ) {
    super();
  }

  ngOnInit(): void {
    this.createResetPasswordForm();
    this.resetPasswordForm.get('email')?.setValue(this.data.userName);
  }

  createResetPasswordForm() {
    this.resetPasswordForm = this.fb.group(
      {
        email: [''],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
      },
      {
        validator: this.checkPasswords,
      }
    );
  }

  checkPasswords(group: FormGroup) {
    let pass = group.get('password')?.value;
    let confirmPass = group.get('confirmPassword')?.value;
    return pass === confirmPass ? null : { notSame: true };
  }

  resetPassword() {
    if (this.resetPasswordForm.valid) {
      this.sub$.sink = this.userService
        .resetPassword(this.createBuildObject())
        .subscribe((d) => {
          this.toastrService.success(
            this.translationService.getValue('SUCCESSFULLY_RESET_PASSWORD')
          );
          this.dialogRef.close();
        });
    }
  }

  createBuildObject(): User {
    return {
      email: '',
      password: this.resetPasswordForm.get('password')?.value,
      userName: this.resetPasswordForm.get('email')?.value,
    };
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
