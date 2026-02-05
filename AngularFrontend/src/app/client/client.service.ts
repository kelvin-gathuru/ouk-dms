import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonError } from '../core/error-handler/common-error';
import { CommonHttpErrorService } from '../core/error-handler/common-http-error.service';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AllowFileExtension } from '../core/domain-classes/allow-file-extension';
import { CommonService } from '../core/services/common.service';
import { Client } from '@core/domain-classes/client';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  constructor(
    private httpClient: HttpClient,
    private commonHttpErrorService: CommonHttpErrorService,
    private commonService: CommonService
  ) { }


  getClients(): Observable<Client[]> {
    const url = 'Client';
    return this.httpClient.get<Client[]>(url);
  }

  getClient(id: string): Observable<Client> {
    const url = `Client/${id}`;
    return this.httpClient
      .get<Client>(url);
  }

  addClient(
    setting: Client
  ): Observable<Client | CommonError> {
    const url = `Client`;
    return this.httpClient
      .post<Client>(url, setting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  updateClient(
    setting: Client
  ): Observable<Client | CommonError> {
    const url = `Client/${setting.id}`;
    return this.httpClient
      .put<Client>(url, setting)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }

  deleteClient(
    id: string
  ): Observable<Client | CommonError> {
    const url = `Client/${id}`;
    return this.httpClient
      .delete<Client>(url)
      .pipe(catchError(this.commonHttpErrorService.handleError));
  }
}
