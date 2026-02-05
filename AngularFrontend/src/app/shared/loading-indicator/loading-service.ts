import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private _pendingRequests = signal(0);
  private _loading = signal(false);

  get pendingRequests() {
    return this._pendingRequests.asReadonly();
  }
  get loading() {
    return this._loading.asReadonly();
  }

  setLoadingFlag(flag: boolean) {
    if (flag)
      this._loading.set(flag);
    else {
      setTimeout(() => this._loading.set(flag), 100);
    }
  }

  startRequest() {
    this._pendingRequests.update(count => count + 1);
  }

  endRequest() {
    this._pendingRequests.update(count => Math.max(count - 1, 0));
  }

}
