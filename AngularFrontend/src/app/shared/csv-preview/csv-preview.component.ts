import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DocumentView } from '@core/domain-classes/document-view';
import { BaseComponent } from '../../base.component';
import * as Papa from 'papaparse';
import { MediaPreview } from '@core/domain-classes/media-previw-data';

@Component({
  selector: 'app-csv-preview',
  templateUrl: './csv-preview.component.html',
  styleUrls: ['./csv-preview.component.scss'],
  standalone: true,
  imports: [
  ]
})
export class CSVPreviewComponent extends BaseComponent implements OnChanges {
  textLines: string[] = [];
  isLoading = false;
  csvData: any[] = [];
  headers: any[] = [];

  @Input() document: MediaPreview | DocumentView | null = null;
  @Input() documentBlob: Blob | null = null;

  async ngOnChanges(changes: SimpleChanges) {
    if (changes['documentBlob'] && this.documentBlob) {
      const content = await this.blobToString(this.documentBlob)
      this.parseCSV(content);
    }
  }

  parseCSV(csvData: string) {
    Papa.parse(csvData, {
      header: true,
      skipEmptyLines: true,
      complete: (result: any) => {
        this.csvData = result.data;
        this.headers = result.meta.fields || [];
      },
    });
  }
  async blobToString(blob: Blob | null): Promise<string> {
    if (!blob) {
      return '';
    }
    return await blob.text();
  }
}
