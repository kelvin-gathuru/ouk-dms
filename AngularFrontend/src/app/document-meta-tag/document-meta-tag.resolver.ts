import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonError } from '../core/error-handler/common-error';
import { DocumentMetaTagService } from './document-meta-tag.service';
import { DocumentMetaTag } from '../core/domain-classes/document-meta-tag';

@Injectable({
    providedIn: 'root'
  })
export class DocumentMetaTagResolver  {
    constructor(private documentMetaTagService: DocumentMetaTagService) { }
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<DocumentMetaTag | CommonError> {
        const id = route.paramMap.get('id') ?? '';
        return this.documentMetaTagService.getDocumentMetaTag(id) as Observable<DocumentMetaTag | CommonError>;
    }
}