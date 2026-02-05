import { KeyValuePipe } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { BaseComponent } from '../../base.component';
import * as XLSX from 'xlsx';
import { NgxPaginationModule } from 'ngx-pagination';

@Component({
  selector: 'app-excel-preview',
  templateUrl: './excel-preview.component.html',
  styleUrls: ['./excel-preview.component.scss'],
  standalone: true,
  imports: [FormsModule, NgxPaginationModule, KeyValuePipe]
})
export class ExcelPreviewComponent extends BaseComponent implements OnChanges {
  @Input() document: DocumentInfo;
  @Input() documentBlob: Blob | null = null;
  isLoading: boolean = false;

  excelData: any[] = [];
  searchTerm: string = '';
  page: number = 1;
  order: string = ''; // Column to sort
  reverse: boolean = false;
  token = '';
  ngOnChanges(changes: SimpleChanges): void {
    if (this.documentBlob) {
      this.isLoading = true;
      this.onFileChange(this.documentBlob);
    }
  }

  onFileChange(blob: Blob) {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      const data = new Uint8Array(e.target.result);
      const workbook = XLSX.read(data, { type: 'array' });
      const sheetName = workbook.SheetNames[0];
      const sheet = workbook.Sheets[sheetName];
      this.excelData = XLSX.utils.sheet_to_json(sheet, { header: 1 });
      this.formatData();
    };
    reader.readAsArrayBuffer(blob);
  }

  formatData() {
    const headers = this.excelData[0];
    this.excelData = this.excelData.slice(1).map((row: any[]) => {
      const obj: any = {};
      headers.forEach((header: string, index: number) => {
        obj[header] = row[index] || '';
      });
      return obj;
    });
  }

  filteredData() {
    return this.excelData.filter(row =>
      Object.values(row).some(val =>
        val?.toString().toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    );
  }

  setOrder(column: any | undefined) {
    if (!column) return;
    this.reverse = this.order === column ? !this.reverse : false;
    this.order = column;
  }
}




