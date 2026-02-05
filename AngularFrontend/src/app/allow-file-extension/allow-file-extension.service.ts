import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '../core/error-handler/common-error';
import { CommonHttpErrorService } from '../core/error-handler/common-http-error.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AllowFileExtension } from '../core/domain-classes/allow-file-extension';
import { CommonService } from '../core/services/common.service';
import { ClonerService } from '@core/services/clone.service';

@Injectable({
  providedIn: 'root',
})
export class AllowFileExtensionService {
   private readonly storageKey = 'allowFileExtension';
  private _allowFileExtension$ = new BehaviorSubject<AllowFileExtension[]>([]);
  public readonly allowFileExtension$ = this._allowFileExtension$.asObservable();

  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService,
    private commonService: CommonService,
    private clonerService: ClonerService
  ) {
    const stored = sessionStorage.getItem(this.storageKey);
    if (stored) {
      this._allowFileExtension$.next(JSON.parse(stored));
    }
  }

    get currentExtensions(): AllowFileExtension[] {
    return this._allowFileExtension$.value;
  }

 getAllowFileExtensions(): Observable<AllowFileExtension[] | CommonError> {
    const url = 'AllowFileExtension';
    return this.httpClient.get<AllowFileExtension[]>(url).pipe(
      tap((data) => {
        if (data?.length > 0) {
          this.commonService.setAllowFileExtension(data);
          sessionStorage.setItem(this.storageKey, JSON.stringify(data));
        }
      }),
      catchError(this.commonHttpErrorService.handleError)
    );
  }
  getAllowFileExtension(id: string): Observable<AllowFileExtension | CommonError> {
    const url = `AllowFileExtension/${id}`;
    return this.httpClient
      .get<AllowFileExtension>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

 // ✅ Add new extension
  addAllowFileExtension(setting: AllowFileExtension): Observable<AllowFileExtension | CommonError> {
    return this.httpClient.post<AllowFileExtension>('AllowFileExtension', setting).pipe(
      tap((newSetting) => {
        const updatedList = [...this._allowFileExtension$.value, this.clonerService.deepClone(newSetting)] as AllowFileExtension[];
        this._allowFileExtension$.next(updatedList);
        sessionStorage.setItem(this.storageKey, JSON.stringify(updatedList));
      }),
      catchError(this.commonHttpErrorService.handleError)
    );
  }

  // ✅ Update extension
  updateAllowFileExtension(setting: AllowFileExtension): Observable<AllowFileExtension | CommonError> {
    return this.httpClient.put<AllowFileExtension>(`AllowFileExtension/${setting.id}`, setting).pipe(
      tap((updatedSetting) => {
        const updatedList = this._allowFileExtension$.value.map((item) =>
          item.id === updatedSetting.id ? this.clonerService.deepClone(updatedSetting) : item
        ) as AllowFileExtension[];
        this._allowFileExtension$.next(updatedList);
        sessionStorage.setItem(this.storageKey, JSON.stringify(updatedList));
      }),
      catchError(this.commonHttpErrorService.handleError)
    );
  }

  // ✅ Delete extension
  deleteAllowFileExtension(id: string): Observable<AllowFileExtension | CommonError> {
    return this.httpClient.delete<AllowFileExtension>(`AllowFileExtension/${id}`).pipe(
      tap(() => {
        const updatedList = this._allowFileExtension$.value.filter((item) => item.id !== id);
        this._allowFileExtension$.next(updatedList);
        sessionStorage.setItem(this.storageKey, JSON.stringify(updatedList));
      }),
      catchError(this.commonHttpErrorService.handleError)
    );
  }

  // ✅ Manually set the list (optional)
  setAllowFileExtension(value: AllowFileExtension[]) {
    this._allowFileExtension$.next(this.clonerService.deepClone(value));
    sessionStorage.setItem(this.storageKey, JSON.stringify(value));
  }
}
