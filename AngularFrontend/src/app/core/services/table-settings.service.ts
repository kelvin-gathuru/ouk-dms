import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableSetting } from '../domain-classes/mat-table-setting';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TableSettingsService {
  constructor(private httpClient: HttpClient) { }

  getTableSettings(screenName: string): Observable<MatTableSetting> {
    const url = `tableSettings/${screenName}`;
    return this.httpClient.get<MatTableSetting>(url);
  }
  saveTableSettings(matTableSetting: MatTableSetting): Observable<MatTableSetting> {
    const url = `TableSettings`;
    return this.httpClient.post<MatTableSetting>(url, matTableSetting);
  }

}
