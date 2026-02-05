import {
  Component,
  Input,
  OnChanges,
  SimpleChanges
} from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import * as mammoth from 'mammoth';

@Component({
  selector: 'app-doc-preview',
  templateUrl: './doc-preview.component.html',
  styleUrls: ['./doc-preview.component.scss'],
  standalone: true,
  imports: [MatProgressSpinnerModule]
})
export class DocPreviewComponent implements OnChanges {
  @Input() documentBlob: Blob | null = null;
  docData: any = null;
  isLoading: boolean = false;

  ngOnChanges(changes: SimpleChanges) {
    if (changes['documentBlob'] && this.documentBlob) {
      this.isLoading = true;
      this.processBlob(this.documentBlob);
    }
  }


  processBlob(blob: Blob) {
    try {
      const reader = new FileReader();
      reader.onload = async (event) => {
        this.isLoading = false;
        if (event?.target?.result) {
          const data = await mammoth.convertToHtml({ arrayBuffer: event?.target?.result as ArrayBuffer });
          this.docData = data.value;
        }
      };
      reader.readAsArrayBuffer(blob);
    }
    catch (error) {
      this.isLoading = false;
      console.log(error);
    }
    finally {
      this.isLoading = false;
    }
  }
}
