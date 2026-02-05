import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { DocumentView } from '@core/domain-classes/document-view';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-json-preview',
  templateUrl: './json-preview.component.html',
  styleUrl: './json-preview.component.scss',
  standalone: true,
  imports: [NgxJsonViewerModule, MatProgressSpinnerModule]
})
export class JsonPreviewComponent extends BaseComponent implements OnChanges {
  @Input() document: MediaPreview | DocumentView;
  @Input() documentBlob: Blob | null = null;

  jsonData: any = null;
  isLoading = false;


  ngOnChanges(changes: SimpleChanges) {
    if (changes['documentBlob'] && this.documentBlob) {
      this.isLoading = true;
      this.processBlob(this.documentBlob);
    }
  }

  async processBlob(blob: Blob) {
    try {
      const text = await blob.text();
      this.jsonData = JSON.parse(text);
    } catch (error) {
      this.jsonData = { error: 'Invalid JSON file' };
    } finally {
      this.isLoading = false;
    }
  }
}
