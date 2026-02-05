import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Page } from '@core/domain-classes/page';
import { CommonHttpErrorService } from '@core/error-handler/common-http-error.service';
import { catchError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PageService {
  private httpClient = inject(HttpClient);
  private commonHttpErrorService = inject(CommonHttpErrorService);

  getAllPages() {
    const url = 'Screen';
    return this.httpClient
      .get<Page[]>(url)
      .pipe(catchError(err => this.commonHttpErrorService.handleError(err)));
  }

  deletePage(id: string) {
    const url = `Screen/${id}`;
    return this.httpClient
      .delete<void>(url)
      .pipe(catchError(err => this.commonHttpErrorService.handleError(err)));
  }

  addPage(page: Page) {
    const url = 'Screen';
    return this.httpClient
      .post<Page>(url, page)
      .pipe(catchError(err => this.commonHttpErrorService.handleError(err)));
  }

  updatePage(page: Page) {
    const url = `Screen/${page.id}`;
    return this.httpClient
      .put<Page>(url, page)
      .pipe(catchError(err => this.commonHttpErrorService.handleError(err)));
  }
}
