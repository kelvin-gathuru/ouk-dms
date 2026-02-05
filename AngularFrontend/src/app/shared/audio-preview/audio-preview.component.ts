import {
  Component,
  ElementRef,
  HostListener,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { DocumentView } from '@core/domain-classes/document-view';
import { CommonService } from '@core/services/common.service';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-audio-preview',
  templateUrl: './audio-preview.component.html',
  styleUrls: ['./audio-preview.component.scss'],
  standalone: true,
  imports: [MatProgressSpinnerModule],
})
export class AudioPreviewComponent extends BaseComponent implements OnChanges {
  @ViewChild('playerEl', { static: true }) playerEl: ElementRef;
  isLoading = false;
  @Input() document: MediaPreview | DocumentView;
  htmlSource: HTMLSourceElement;
  @Input() documentBlob: Blob;

  @HostListener('document:contextmenu', ['$event'])
  disableRightClick(event: MouseEvent): void {
    event.preventDefault();
  }

  constructor(
    public commonService: CommonService,
    public toastrService: ToastrService
  ) {
    super();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['documentBlob']) {
      this.getDocument();
    }
  }

  getDocument() {
    if (this.documentBlob) {
      this.htmlSource = document.createElement('source');
      this.htmlSource.src = URL.createObjectURL(this.documentBlob);
      this.htmlSource.type = this.documentBlob?.type;
      this.player().pause();
      this.player().load();
      this.player().appendChild(this.htmlSource);
      setTimeout(() => this.player().play(), 100);
    }
  }

  player() {
    return this.playerEl.nativeElement as HTMLVideoElement | HTMLAudioElement;
  }
}
