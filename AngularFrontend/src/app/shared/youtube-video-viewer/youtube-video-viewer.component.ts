import { Component, HostListener, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MediaPreview } from '@core/domain-classes/media-previw-data';
import { BaseComponent } from '../../base.component';
import { DocumentView } from '@core/domain-classes/document-view';

@Component({
  selector: 'app-youtube-video-viewer',
  imports: [],
  templateUrl: './youtube-video-viewer.component.html',
  styleUrl: './youtube-video-viewer.component.scss'
})
export class YoutubeVideoViewerComponent extends BaseComponent implements OnChanges {
  @Input() document?: MediaPreview | DocumentView;
  url: SafeResourceUrl = '';
  thumbnailUrl = '';
  showPlayer = false;

  constructor(private sanitizer: DomSanitizer) {
    super();
  }

  @HostListener('document:contextmenu', ['$event'])
  disableRightClick(event: MouseEvent): void {
    event.preventDefault();
  }


  ngOnChanges(changes: SimpleChanges): void {
    if (changes['document']) {
      if (this.document && this.document.url) {
        const videoId = this.document.url.split('v=')[1]?.split('&')[0];
        this.url = this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${videoId}?autoplay=1`);
      }
    }
  }
}
