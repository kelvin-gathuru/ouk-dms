import { inject } from "@angular/core";
import { tapResponse } from "@ngrx/operators";
import { patchState, signalStore, withComputed, withHooks, withMethods, withState } from "@ngrx/signals";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { ToastrService } from '@core/services/toastr-service';
import { debounceTime, distinctUntilChanged, pipe, switchMap, tap } from "rxjs";
import { HttpResponse } from "@angular/common/http";
import { toObservable } from "@angular/core/rxjs-interop";
import { DocumentInfo } from "@core/domain-classes/document-info";
import { DocumentResource } from '@core/domain-classes/document-resource';
import { DocumentService } from "../document/document.service";
import { CommonError } from '@core/error-handler/common-error';
import { TranslationService } from '@core/services/translation.service';
import { ResponseHeader } from "@core/domain-classes/document-header";

type DocumentState = {
  documents: DocumentInfo[];
  isLoading: boolean;
  loadList: boolean;
  documentResourceParameter: DocumentResource;
  commonError: CommonError | null;
  isAddUpdate: boolean;
};

export const initialDocumentState: DocumentState = {
  documents: [],
  isLoading: false,
  loadList: false,
  isAddUpdate: false,
  documentResourceParameter: {
    id: '',
    createdBy: '',
    categoryId: '',
    documentStatusId: '',
    storageSettingId: '',
    clientId: '',
    createDate: undefined,
    fields: '',
    orderBy: 'createdDate asc',
    searchQuery: '',
    pageSize: 10,
    skip: 0,
    name: '',
    totalCount: 0,
    metaTags: '',
    documentNumber: ''
  },
  commonError: null
};

export const ArchiveDocumentsStore = signalStore(
  { providedIn: 'root' },
  withState(initialDocumentState),
  withComputed(({ }) => ({
  })),
  withMethods((store, documentService = inject(DocumentService),
    toastrService = inject(ToastrService),
    translationService = inject(TranslationService)) => ({
      loadByQuery: rxMethod<DocumentResource>(
        pipe(
          debounceTime(300),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((resourceParam: DocumentResource) => {
            return documentService.getArchiveDocuments(resourceParam).pipe(
              tapResponse({
                next: (documentsWithHeader: HttpResponse<DocumentInfo[]>) => {
                  const paginationParam = JSON.parse(
                    documentsWithHeader.headers.get('X-Pagination') ?? '{}'
                  ) as ResponseHeader;

                  const newDocumentResourceParameter = {
                    ...resourceParam,
                    totalCount: paginationParam.totalCount,
                    pageSize: paginationParam.pageSize,
                    skip: paginationParam.skip
                  };
                  patchState(store, {
                    documents: [...documentsWithHeader.body ?? []],
                    isLoading: false,
                    commonError: null,
                    loadList: false,
                    isAddUpdate: false,
                    documentResourceParameter: { ...newDocumentResourceParameter }
                  })
                },
                error: (err: CommonError) => {
                  patchState(store, { commonError: err, isLoading: false });
                  console.error(err);
                },
              })
            );
          })
        )
      ),
      archiveById: rxMethod<string>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((id: string) => {
            return documentService.archiveDocument(id).pipe(
              tapResponse({
                next: () => {
                  toastrService.success(translationService.getValue('DOCUMENT_ARCHIVE_SUCCESSFULLY'));
                  patchState(store, { isLoading: false, loadList: true });
                },
                error: (err: CommonError) => {
                  patchState(store, { commonError: err, isLoading: false });
                },
              })
            );
          })
        )
      ),
      addUpdateDocument: rxMethod<DocumentInfo>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((document: DocumentInfo) => {
            if (document.id) {
              return documentService.updateDocument(document).pipe(
                tapResponse({
                  next: () => {
                    toastrService.success(translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY'));
                    patchState(store, { isLoading: false, loadList: true, isAddUpdate: true });
                  },
                  error: (err: CommonError) => {
                    patchState(store, { commonError: err, isLoading: false });
                  },
                })
              );
            } else {
              return documentService.addDocument(document).pipe(
                tapResponse({
                  next: () => {
                    toastrService.success(translationService.getValue('DOCUMENT_SAVE_SUCCESSFULLY'));
                    patchState(store, { isLoading: false, loadList: true, isAddUpdate: true, });
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
      getArchiveDocuments() {
        this.loadByQuery(store.documentResourceParameter());
      },
      setLoadingFlag(flag: boolean) {
        patchState(store, { isLoading: flag });
      }
    })),
  withHooks({
    onInit(store) {
      toObservable(store.loadList).subscribe((flag) => {
        if (flag) {
          store.loadByQuery(store.documentResourceParameter());
        }
      });
      // store.loadByQuery(store.documentResourceParameter());
    },
  }),
);
