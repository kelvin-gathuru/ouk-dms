import { Injectable } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { marked } from 'marked';

@Injectable({ providedIn: 'root' })
export class StreamMarkdownService {
  private buffer = '';
  public htmlChunks: SafeHtml[] = [];

  constructor(private sanitizer: DomSanitizer) { }

  addChunk(chunk: string) {
    let html = '';
    if (chunk === '[[DONE]]') {
      const html = marked.parse(this.buffer) as string;
      this.buffer = '';
      return '[[DONE]]';
    } else {
      this.buffer += chunk;
      const parts = this.buffer.split(/\n{2,}/);
      this.buffer = parts.pop() || '';
      if (this.buffer.trim()) {

        html = marked.parse(this.buffer) as string;
      }
      return html
    }
  }

  addChunkResponce(chunk: string) {
    let html = '';
    if (chunk === '[[DONE]]') {
      const html = marked.parse(this.buffer) as string;
      this.buffer = '';
      return '[[DONE]]';
    } else {
      this.buffer = '';
      this.buffer += chunk;
      const parts = this.buffer.split(/\n{2,}/);
      this.buffer = parts.pop() || '';
      if (this.buffer.trim()) {

        html = marked.parse(this.buffer) as string;
      }
      return html
    }
  }

}
