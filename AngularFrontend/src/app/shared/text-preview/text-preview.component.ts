import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DocumentView } from '@core/domain-classes/document-view';
import { CommonService } from '@core/services/common.service';
import { OverlayPanelRef } from '@shared/overlay-panel/overlay-panel-ref';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-text-preview',
  templateUrl: './text-preview.component.html',
  styleUrls: ['./text-preview.component.scss'],
  standalone: true,
  imports: [MatProgressSpinnerModule]
})
export class TextPreviewComponent extends BaseComponent implements OnChanges {
  textLines: string[] = [];
  isLoading = false;
  @Input() document!: MediaPreview | DocumentView;
  constructor(
    private commonService: CommonService,
    private overlayRef: OverlayPanelRef,
    private toastrService: ToastrService
  ) {
    super();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['document']) {
      if (this.document) {
        this.readDocument();
      }
    }
  }

  readDocument() {
    this.isLoading = true;
    this.sub$.sink = this.commonService.readDocument(this.document).subscribe({
      next: (data: { [key: string]: string[] }) => {
        this.isLoading = false;
        this.textLines = data['result'];
      },
      error: (err) => {
        this.toastrService.error(err.error.message);
        this.isLoading = false;
        this.overlayRef.close();
      }
    });
  }

  onCancel() {
    this.overlayRef.close();
  }
}
