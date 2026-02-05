import {
  ChangeDetectorRef,
  Component,
  HostListener,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { DocumentView } from '@core/domain-classes/document-view';
import { CommonService } from '@core/services/common.service';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-image-preview',
  templateUrl: './image-preview.component.html',
  styleUrls: ['./image-preview.component.scss'],
  standalone: true,
  imports: [MatProgressSpinnerModule]
})
export class ImagePreviewComponent
  extends BaseComponent
  implements OnInit, OnChanges {
  imageUrl: SafeUrl;
  isLoading = false;
  @Input() document: MediaPreview | DocumentView;
  @Input() documentBlob: Blob;

  @HostListener('document:contextmenu', ['$event'])
  disableRightClick(event: MouseEvent): void {
    event.preventDefault();
  }

  constructor(
    private sanitizer: DomSanitizer,
    private ref: ChangeDetectorRef,
    private commonService: CommonService,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnInit(): void { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['documentBlob'] && this.documentBlob) {
      this.getImage();
    }
  }

  getImage() {
    this.isLoading = false;
    this.imageUrl = this.sanitizer.bypassSecurityTrustUrl(
      URL.createObjectURL(this.documentBlob)
    );
    this.ref.markForCheck();
  }
}
