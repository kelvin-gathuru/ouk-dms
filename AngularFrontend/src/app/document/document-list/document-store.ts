import { computed, inject } from "@angular/core";
import { tapResponse } from "@ngrx/operators";
import { patchState, signalStore, withComputed, withHooks, withMethods, withState } from "@ngrx/signals";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { ToastrService } from '@core/services/toastr-service';
import { concatMap, debounceTime, distinctUntilChanged, pipe, switchMap, tap, EMPTY } from "rxjs";
import { HttpResponse } from "@angular/common/http";
import { toObservable } from "@angular/core/rxjs-interop";
import { DocumentInfo } from "@core/domain-classes/document-info";
import { DocumentResource } from '@core/domain-classes/document-resource';
import { DocumentService } from "../document.service";
import { CommonError } from '@core/error-handler/common-error';
import { TranslationService } from '@core/services/translation.service';
import { ResponseHeader } from "@core/domain-classes/document-header";
import { SecurityService } from "@core/security/security.service";
import { MatTableSetting } from "@core/domain-classes/mat-table-setting";
import { TableSettingsService } from "@core/services/table-settings.service";
import { DocumentsTableSettings } from '../../core/services/table-setting-value';
import { DocumentAuditTrail } from "@core/domain-classes/document-audit-trail";
import { CommonService } from "@core/services/common.service";

type DocumentState = {
  documents: DocumentInfo[];
  isLoading: boolean;
  loadList: boolean;
  documentResourceParameter: DocumentResource;
  commonError: CommonError | null;
  isAddUpdate: boolean;
  matTableSetting: MatTableSetting | null;
  isTableSettingAdded: boolean;
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
    orderBy: 'CreatedDate desc',
    searchQuery: '',
    pageSize: 20,
    skip: 0,
    name: '',
    totalCount: 0,
    metaTags: '',
    metaTagsTypeId: '',
    documentNumber: ''
  },
  commonError: null,
  matTableSetting: null,
  isTableSettingAdded: false
};

export const DocumentStore = signalStore(
  { providedIn: 'root' },
  withState(initialDocumentState),
  withComputed(({ matTableSetting }) => ({
    tableSettingsVisible: computed(() => {
      const mts = matTableSetting();
      if (mts && mts.settings && Array.isArray(mts.settings) && mts.settings.length > 0) {
        return mts.settings.filter(c => c.isVisible);
      }
      return [...DocumentsTableSettings];
    }),
    visibleTableKeys: computed(() => {
      const mts = matTableSetting();
      if (mts && mts.settings && Array.isArray(mts.settings) && mts.settings.length > 0) {
        const settings = mts.settings.filter(c => c.isVisible);
        return [...settings.map((c: any) => c.key)]
      }
      return [...DocumentsTableSettings.map((c: any) => c.key)];
    })
  })),
  withMethods((store, documentService = inject(DocumentService),
    toastrService = inject(ToastrService),
    commonService = inject(CommonService),
    securityService = inject(SecurityService),
    tableSettingsService = inject(TableSettingsService),
    translationService = inject(TranslationService)) => ({
      loadByQuery: rxMethod<DocumentResource>(
        pipe(
          debounceTime(300),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((resourceParam: DocumentResource) => {
            if (!securityService.hasClaim('ALL_VIEW_DOCUMENTS')) {
              patchState(store,
                {
                  isLoading: false,
                  documents: []
                });
              // Return an empty observable to satisfy the type requirement
              return import('rxjs').then(rxjs => rxjs.EMPTY);
            }
            return documentService.getDocuments(resourceParam).pipe(
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
                    documents: [...(documentsWithHeader.body ?? [])],
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
      loadTableSettingsByQuery: rxMethod<string>(
        pipe(
          debounceTime(300),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((screenName: string) => {
            return tableSettingsService.getTableSettings(screenName).pipe(
              tapResponse({
                next: (tableSettings: MatTableSetting) => {
                  if (tableSettings.screenName) {
                    const settings = tableSettings.settings.sort((a, b) => a.orderNumber - b.orderNumber)
                    patchState(store, {
                      matTableSetting: { ...tableSettings, settings: [...settings] },
                      isLoading: false,
                      commonError: null
                    })
                  } else {
                    const newTableSetting: MatTableSetting = {
                      id: 0,
                      screenName: screenName,
                      settings: [...DocumentsTableSettings]
                    };
                    patchState(store, {
                      matTableSetting: { ...newTableSetting },
                      isLoading: false,
                      commonError: null
                    })
                  }
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
      addDocumentAudit: rxMethod<DocumentAuditTrail>(
        pipe(
          concatMap((documentAuditTrail: DocumentAuditTrail) => {
            return commonService.addDocumentAuditTrail(documentAuditTrail).pipe(
              tapResponse({
                next: (documentAudit: any) => {
                },
                error: (err: CommonError) => {
                  // patchState(store, { commonError: err, isLoading: false });
                  console.error(err);
                },
              })
            );
          })
        )
      ),
      loadDocuments() {
        this.loadByQuery(store.documentResourceParameter());
      },
      loadTableSettingByScreenName(screenName: string) {
        this.loadTableSettingsByQuery(screenName);
      },
      updateTableSettingAdded() {
        patchState(store, { isTableSettingAdded: false });
      },
      setLoadingFlag(flag: boolean) {
        patchState(store, { isLoading: flag });
      },
      saveTableSettings: rxMethod<MatTableSetting>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((tableSetting: MatTableSetting) => {
            return tableSettingsService.saveTableSettings(tableSetting).pipe(
              tapResponse({
                next: (tableSettings: MatTableSetting) => {
                  toastrService.success(translationService.getValue('TABLESETTINGS_SAVE_SUCCESSFULLY'));
                  const settings = tableSettings.settings.sort((a, b) => a.orderNumber - b.orderNumber)
                  patchState(store, {
                    matTableSetting: { ...tableSettings, settings: [...settings] },
                    isLoading: false,
                    commonError: null,
                    isTableSettingAdded: true
                  })
                },
                error: (err: CommonError) => {
                  patchState(store, { commonError: err, isLoading: false });
                },
              })
            );
          })
        )
      ),
    })),
  withHooks({
    onInit(store, securityService = inject(SecurityService)) {
      toObservable(store.loadList).subscribe((flag) => {
        if (flag) {
          store.loadByQuery(store.documentResourceParameter());
        }
      });
      if (securityService.isUserAuthenticate() && securityService.hasClaim('ALL_VIEW_DOCUMENTS')) {
        store.loadByQuery(store.documentResourceParameter());
      }
    },
  }),
);
