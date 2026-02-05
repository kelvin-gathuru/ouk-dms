import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { StorageSetting } from './storage-setting';
import { CommonError } from '@core/error-handler/common-error';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';

@Injectable({ providedIn: 'root' })
export class StorageSettingService {
  private commonHttpErrorService = inject(CommonHttpErrorService);
  private httpClient = inject(HttpClient);

  getStorageSettings(): Observable<StorageSetting<void>[]> {
    const url = 'storagesetting';
    return this.httpClient.get<StorageSetting<void>[]>(url);

  }
  getStorageSetting<T>(id: string): Observable<StorageSetting<T> | CommonError> {
    const url = `storagesetting/${id}`;
    return this.httpClient.get<StorageSetting<T>>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  addStorageSetting(storageSetting: StorageSetting<any>): Observable<StorageSetting<any> | CommonError> {
    const url = `storagesetting`;
    return this.httpClient.post<StorageSetting<any>>(url, storageSetting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateStorageSetting<T>(storageSetting: StorageSetting<T>): Observable<StorageSetting<T> | CommonError> {
    const url = `storagesetting/${storageSetting.id}`;
    return this.httpClient.post<StorageSetting<T>>(url, storageSetting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}


