import {
  Component,
  ElementRef,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CompanyProfileService } from './company-profile.service';
import { CompanyProfile } from './company-profile';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

import { ToastrService } from '@core/services/toastr-service';
import { TranslationService } from '@core/services/translation.service';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { SecurityService } from '../core/security/security.service';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';

@Component({
  selector: 'app-company-profile',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    PageHelpTextComponent
  ],
  templateUrl: './company-profile.component.html',
  styleUrls: ['./company-profile.component.scss']
})
export class CompanyProfileComponent implements OnInit {
  @ViewChild('logoUpload', { static: false }) logoUploadInput!: ElementRef;
  @ViewChild('logoIconUpload', { static: false }) logoIconUploadInput!: ElementRef;
  @ViewChild('bannerUpload', { static: false }) bannerUploadInput!: ElementRef;
  companyProfileForm: FormGroup;

  private companyProfileService = inject(CompanyProfileService);
  private toastrService = inject(ToastrService);
  private translationService = inject(TranslationService);
  private securityService = inject(SecurityService);

  companyId: string = '';
  logoUrl: string | ArrayBuffer | null = null;
  logoIconUrl: string | ArrayBuffer | null = null;
  bannerUrl: string | ArrayBuffer | null = null;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createFormGroup();
    this.getCompanyProfile();
  }

  createFormGroup() {
    this.companyProfileForm = this.fb.group({
      companyTitle: ['', [Validators.required, Validators.maxLength(100)]],
      logo: [this.logoUrl],
      logoIcon: [this.logoIconUrl],
      banner: [this.bannerUrl]
    });
  }

  triggerLogoUpload(fileInput: HTMLInputElement) {
    fileInput.click();
  }

  triggerLogoIconUpload(fileInput: HTMLInputElement) {
    fileInput.click();
  }

  triggerBannerUpload(fileInput: HTMLInputElement) {
    fileInput.click();
  }

  onLogoUpload(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.companyProfileForm.patchValue({ logo: e.target?.result });
        this.logoUrl = e.target?.result ?? '';
      };
      reader.readAsDataURL(file);
    }
  }

  onLogoIconUpload(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();

      reader.onload = () => {
        const dataUrl = reader.result as string;
        this.logoIconUrl = dataUrl;
      };
      reader.readAsDataURL(file);
    }
  }


  onBannerUpload(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.companyProfileForm.patchValue({ banner: e.target?.result });
        this.bannerUrl = e.target?.result ?? '';
      };
      reader.readAsDataURL(file);
    }
  }

  onSave() {
    if (this.companyProfileForm.invalid) {
      return;
    }

    const companyProfile: CompanyProfile = {
      id: this.companyId,
      name: this.companyProfileForm.get('companyTitle')?.value
    };

    const logoInput = this.logoUploadInput?.nativeElement as HTMLInputElement;
    const logoIconInput = this.logoIconUploadInput?.nativeElement as HTMLInputElement;
    const bannerInput = this.bannerUploadInput
      ?.nativeElement as HTMLInputElement;

    const logoFile = logoInput.files?.item(0) || null;
    const bannerFile = bannerInput.files?.item(0) || null;
    const logoIconFile = logoIconInput.files?.item(0) || null;

    this.companyProfileService
      .updateCompanyProfile(companyProfile, logoFile ?? undefined, bannerFile ?? undefined, logoIconFile ?? undefined)
      .subscribe({
        next: (c: CompanyProfile) => {
          this.securityService.setCompany(c);
          this.toastrService.success(
            this.translationService.getValue(
              'COMPANY_PROFILE_UPDATED_SUCCESSFULLY'
            )
          );
        },
        error: () => {
          this.toastrService.error(
            this.translationService.getValue('FAILED_TO_SAVE_COMPANY_PROFILE')
          );
        },
      });
  }

  getCompanyProfile(): void {
    this.securityService.companyProfile.subscribe((c) => {
      if (c) {
        this.companyProfileForm.patchValue({
          companyTitle: c.name,
          logoUrl: c.logoUrl,
          logoIconUrl: c.logoIconUrl,
          bannerUrl: c.bannerUrl,
          companyId: c.id
        });
        this.logoUrl = c.logoUrl ?? '';
        this.logoIconUrl = c.logoIconUrl ?? '';
        this.bannerUrl = c.bannerUrl ?? '';
        this.companyId = c.id ?? '';
      }
    });
  }
}
