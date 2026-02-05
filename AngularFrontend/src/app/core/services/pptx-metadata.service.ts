import { Injectable } from '@angular/core';
import JSZip from 'jszip';
import * as pdfjsLib from 'pdfjs-dist';
import { GlobalWorkerOptions } from 'pdfjs-dist';

@Injectable()
export class PptxMetadataService {
  /**
   *
   */
  constructor() {
  }

  private async loadPdfWorker() {
    // Dynamically import the worker script path
    if (!GlobalWorkerOptions.workerSrc) {
      GlobalWorkerOptions.workerSrc = `/js/pdf.worker.min.mjs`;
    }
  }

  async getPDFMetadata(file: File) {
    await this.loadPdfWorker(); // Ensure worker is assigned
    const arrayBuffer = await file.arrayBuffer();
    const pdf = await pdfjsLib.getDocument({ data: new Uint8Array(arrayBuffer) }).promise;
    const metadata = await pdf.getMetadata();
    const info = metadata?.info as { [key: string]: any };
    return {
      title: '',
      author: info?.['Author'] ?? '',
      created: this.parsePdfDate(info?.['CreationDate']) ?? null,
      modified: this.parsePdfDate(info?.['ModDate']) ?? null
    };

  }
  parsePdfDate(pdfDate: any) {
    if (!pdfDate) {
      return null;
    }

    if (pdfDate?.startsWith("D:")) {
      pdfDate = pdfDate.substring(2); // Remove "D:"
    }

    // Extract YYYYMMDDHHMMSS part
    let datePart = pdfDate.substring(0, 14);

    // Convert to JavaScript Date format
    let year = datePart.substring(0, 4);
    let month = datePart.substring(4, 6) - 1; // JS months are 0-based
    let day = datePart.substring(6, 8);
    let hours = datePart.substring(8, 10);
    let minutes = datePart.substring(10, 12);
    let seconds = datePart.substring(12, 14);

    return new Date(year, month, day, hours, minutes, seconds);
  }

  async extractMetadata(file: File): Promise<any> {
    try {
      const zip = new JSZip();
      const zipFile = await zip.loadAsync(await file.arrayBuffer());

      // Extract metadata from core.xml
      const coreXml = await zipFile.file('docProps/core.xml')?.async('text');
      if (!coreXml) {
        return null;
      }

      // Parse XML to extract metadata
      const parser = new DOMParser();
      const xmlDoc = parser.parseFromString(coreXml, 'text/xml');

      return {
        title: this.getXmlValue(xmlDoc, 'dc:title') ?? '',
        author: this.getXmlValue(xmlDoc, 'dc:creator') ?? '',
        created: this.getXmlValue(xmlDoc, 'dcterms:created') ?? null,
        modified: this.getXmlValue(xmlDoc, 'dcterms:modified') ?? null,
      };
    } catch (error) {
      return null;
    }
  }

  private getXmlValue(xmlDoc: Document, tag: string): string {
    const element = xmlDoc.getElementsByTagName(tag)[0];
    return element ? element.textContent || 'N/A' : 'N/A';
  }
}
