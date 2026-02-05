import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { DocumentView } from '@core/domain-classes/document-view';
import { MediaPreview } from '@core/domain-classes/media-previw-data';

@Component({
  selector: 'app-vimeo-video-viewer',
  imports: [],
  templateUrl: './vimeo-video-viewer.component.html',
  styleUrl: './vimeo-video-viewer.component.scss'
})
export class VimeoVideoViewerComponent implements OnChanges {
  @Input() document?: MediaPreview | DocumentView;
  sanitizedUrl?: SafeResourceUrl;

  constructor(private sanitizer: DomSanitizer) {

  }

  extractVimeoId(url: string): string | null {
    const match = url.match(/vimeo\.com\/(?:video\/)?(\d+)/);
    return match ? match[1] : null;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['document']) {
      if (this.document && this.document.url) {
        const videoId = this.extractVimeoId(this.document.url);
        const embedUrl = `https://player.vimeo.com/video/${videoId}`;
        this.sanitizedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
      }
    }
  }
}
