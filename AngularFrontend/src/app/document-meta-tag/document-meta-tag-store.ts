import { inject } from '@angular/core';
import { tapResponse } from '@ngrx/operators';
import {
  patchState,
  signalStore,
  withComputed,
  withHooks,
  withMethods,
  withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';

import { ToastrService } from '@core/services/toastr-service';
import { debounceTime, distinctUntilChanged, pipe, switchMap, tap } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { CommonError } from '../core/error-handler/common-error';
import { TranslationService } from '../core/services/translation.service';
import { SecurityService } from '../core/security/security.service';
import { DocumentMetaTagService } from './document-meta-tag.service';
import { DocumentMetaTag } from '../core/domain-classes/document-meta-tag';
import { Router } from '@angular/router';

type DocumentMetaTagState = {
  documentMetaTags: DocumentMetaTag[];
  documentMetaTag: DocumentMetaTag | null;
  isLoading: boolean;
  loadList: boolean;
  isAddUpdate: boolean;
  commonError: CommonError | null;
};

export const initialDocumentMetaTagState: DocumentMetaTagState = {
  documentMetaTags: [],
  documentMetaTag: null,
  isLoading: false,
  loadList: false,
  isAddUpdate: false,
  commonError: null,
};

export const DocumentMetaTagStore = signalStore(
  { providedIn: 'root' },
  withState(initialDocumentMetaTagState),
  withComputed(({ }) => ({})),
  withMethods(
    (
      store,
      documentMetaTagService = inject(DocumentMetaTagService),
      toastrService = inject(ToastrService),
      securityService = inject(SecurityService),
      translationService = inject(TranslationService),
      router = inject(Router)
    ) => ({
      loadDocumentMetaTags: rxMethod<void>(
        pipe(
          debounceTime(300),
          tap(() => patchState(store, { isLoading: true })),
          switchMap(() =>
            documentMetaTagService.getDocumentMetaTags().pipe(
              tapResponse({
                next: (documentMetaTags: DocumentMetaTag[] | CommonError) => {
                  const documentMetaTagsNew = documentMetaTags as DocumentMetaTag[];
                  patchState(store, {
                    documentMetaTags: [...documentMetaTagsNew],
                    isLoading: false,
                    commonError: null,
                    loadList: false
                  });
                },
                error: (err: CommonError) => {
                  patchState(store, { commonError: err, isLoading: false, loadList: false });
                  console.error(err);
                },
              })
            )
          )
        )
      ),
      deleteDocumentMetaTagById: rxMethod<string>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((documentMetaTagId: string) =>
            documentMetaTagService.deleteDocumentMetaTag(documentMetaTagId).pipe(
              tapResponse({
                next: () => {
                  toastrService.success(
                    translationService.getValue('DOCUMENT_META_TAG_DELETED_SUCCESSFULLY')
                  );
                  patchState(store, {
                    documentMetaTags: store.documentMetaTags().filter((w) => w.id !== documentMetaTagId),
                    isLoading: false,
                  });
                },
                error: (err: CommonError) => {
                  patchState(store, { commonError: err, isLoading: false });
                  console.error(err);
                },
              })
            )
          )
        )
      ),
      addUpdateDocumentMetaTag: rxMethod<DocumentMetaTag>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((documentMetaTag: DocumentMetaTag) => {
            if (documentMetaTag.id) {
              return documentMetaTagService.updateDocumentMetaTag(documentMetaTag).pipe(
                tapResponse({
                  next: () => {
                    toastrService.success(
                      translationService.getValue('DOCUMENT_META_TAG_UPDATED_SUCCESSFULLY')
                    );
                    patchState(store, {
                      isLoading: false,
                      loadList: true,
                      isAddUpdate: true,
                    });
                    router.navigate(['/document-meta-tag']);
                  },
                  error: (err: CommonError) => {
                    patchState(store, { commonError: err, isLoading: false });
                  },
                })
              );
            } else {
              return documentMetaTagService.addDocumentMetaTag(documentMetaTag).pipe(
                tapResponse({
                  next: () => {
                    toastrService.success(
                      translationService.getValue('DOCUMENT_META_TAG_CREATED_SUCCESSFULLY')
                    );
                    patchState(store, {
                      isLoading: false,
                      loadList: true,
                      isAddUpdate: true,
                    });
                    router.navigate(['/document-meta-tag']);
                  },
                  error: (err: CommonError) => {
                    patchState(store, { commonError: err, isLoading: false });
                  },
                })
              );
            }
          })
        )
      ),
      getDocumentMetaTagById: rxMethod<string>(
        pipe(
          tap(() => patchState(store, { isLoading: true })),
          switchMap((documentMetaTagId: string) =>
            documentMetaTagService.getDocumentMetaTag(documentMetaTagId).pipe(
              tapResponse({
                next: (documentMetaTag: DocumentMetaTag | CommonError) => {
                  const documentMetaTagNew = documentMetaTag as DocumentMetaTag;
                  patchState(store, {
                    documentMetaTag: documentMetaTagNew,
                    isLoading: false,
                    commonError: null,
                  });
                },
                error: (err: CommonError) => {
                  patchState(store, { commonError: err, isLoading: false });
                  console.error(err);
                },
              })
            )
          )
        )
      ),
    })
  ),
  withHooks({
    onInit(store, securityService = inject(SecurityService)) {
      toObservable(store.loadList).subscribe((flag) => {
        if (flag) {
          store.loadDocumentMetaTags();
        }
      });
      if (
        securityService.isUserAuthenticate()
      ) {
        store.loadDocumentMetaTags();
      }
    },
  })
);
