import { computed, inject } from "@angular/core";
import { tapResponse } from "@ngrx/operators";
import { patchState, signalStore, withComputed, withHooks, withMethods, withState } from "@ngrx/signals";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { debounceTime, map, pipe, skip, switchMap, tap } from "rxjs";
import { HttpResponse } from "@angular/common/http";
import { DocumentInfo } from "@core/domain-classes/document-info";
import { DocumentResource } from '@core/domain-classes/document-resource';
import { CommonError } from '@core/error-handler/common-error';
import { ResponseHeader } from "@core/domain-classes/document-header";

import { Category } from "../../core/domain-classes/category";
import { CategoryService } from "../../core/services/category.service";
import { SecurityService } from "@core/security/security.service";
import { DocumentLibraryService } from "../document-library.service";

type AssignFoldersViewState = {
  categories: Category[];
  isLoading: boolean;
  documentResourceParameter: DocumentResource;
  commonError: CommonError | null;
  documents: DocumentInfo[];
  selectedCategoryId: string;
  newCategoryPath: Category[];
};

export const initialDocumentState: AssignFoldersViewState = {
  documents: [],
  isLoading: false,
  categories: [],
  selectedCategoryId: '',
  newCategoryPath: [],
  documentResourceParameter: {
    id: '',
    createdBy: '',
    categoryId: '',
    documentStatusId: '',
    storageSettingId: '',
    clientId: '',
    createDate: undefined,
    fields: '',
    orderBy: 'createdDate desc',
    searchQuery: '',
    pageSize: 15,
    skip: 0,
    name: '',
    totalCount: 0,
    metaTags: '',
    documentNumber: '',
  },
  commonError: null
};

export const AssignFoldersViewStore = signalStore(
  { providedIn: 'root' },
  withState(initialDocumentState),
  withComputed(({ documentResourceParameter, categories, documents }) => ({
    sortedCategories: computed(() => {
      if (!categories() || categories().length === 0) {
        return [];
      }
      const orderBy = documentResourceParameter().orderBy;
      const orderByArray = orderBy.split(' ');
      if (orderByArray[0] === 'name') {
        if (orderByArray[1] === 'asc') {
          return [...categories().sort((a, b) => b.name.localeCompare(a.name))];
        } else {
          return [...categories().sort((a, b) => a.name.localeCompare(b.name))];
        }
      } else if (orderByArray[0] === 'createdDate') {
        if (orderByArray[1] === 'asc') {
          return [...categories().sort((a, b) => new Date(a.createdDate).getTime() - new Date(b.createdDate).getTime())];
        } else {
          return [...categories().sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime())];
        }
      } else if (orderByArray[0] === 'createdBy') {
        if (orderByArray[1] === 'asc') {
          return [...categories().sort((a, b) => b.createdUserName?.localeCompare(a.createdUserName ?? '') ?? 0)];
        } else {
          return [...categories().sort((a, b) => a.createdUserName?.localeCompare(b.createdUserName ?? '') ?? 0)];
        }
      }
      return [...categories()];
    }),
    sortedDocuments: computed(() => {
      if (!documents() || documents().length === 0) {
        return [];
      }
      const orderBy = documentResourceParameter().orderBy;
      const orderByArray = orderBy.split(' ');
      if (orderByArray[0] === 'createdDate') {
        if (orderByArray[1] === 'asc') {
          return [...documents().sort((a, b) => new Date(a?.createdDate ?? new Date()).getTime() - new Date(b?.createdDate ?? new Date()).getTime())];
        } else {
          return [...documents().sort((a, b) => new Date(b?.createdDate ?? new Date()).getTime() - new Date(a?.createdDate ?? new Date()).getTime())];
        }
      } else if (orderByArray[0] === 'name') {
        if (orderByArray[1] === 'asc') {
          return [...documents().sort((a, b) => b.name?.localeCompare(a.name ?? '') ?? 0)];
        } else {
          return [...documents().sort((a, b) => a.name?.localeCompare(b.name ?? '') ?? 0)];
        }
      } else if (orderByArray[0] === 'documentNumber') {
        if (orderByArray[1] === 'asc') {
          return [...documents().sort((a, b) => b.documentNumber?.localeCompare(a.documentNumber ?? '') ?? 0)];
        } else {
          return [...documents().sort((a, b) => a.documentNumber?.localeCompare(b.documentNumber ?? '') ?? 0)];
        }
      } else if (orderByArray[0] === 'createdBy') {
        if (orderByArray[1] === 'asc') {
          return [...documents().sort((a, b) => b.createdBy?.localeCompare(a.createdBy ?? '') ?? 0)];
        } else {
          return [...documents().sort((a, b) => a.createdBy?.localeCompare(b.createdBy ?? '') ?? 0)];
        }
      }
      return [...documents()];
    })
  })),
  withMethods((store, documentLibraryService = inject(DocumentLibraryService),
    categoryService = inject(CategoryService),
    securityService = inject(SecurityService)
  ) => ({
    loadCategoriesById: rxMethod<string>(
      pipe(
        debounceTime(300),
        tap(() => patchState(store, { isLoading: true })),
        switchMap((categoryId: string) => {
          if (!categoryId) {
            categoryId = '00000000-0000-0000-0000-000000000000';
          }
          return categoryService.getCategoriesSharedByParentId(categoryId).pipe(
            tapResponse({
              next: (categories: Category[]) => {

                let newCategoryId = categoryId;
                if (categoryId == '00000000-0000-0000-0000-000000000000') {
                  newCategoryId = '';
                }
                patchState(store, {
                  categories: [...categories],
                  isLoading: false,
                  documentResourceParameter: { ...store.documentResourceParameter(), categoryId: newCategoryId },
                  selectedCategoryId: newCategoryId
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
    loadByQuery: rxMethod<DocumentResource>(
      pipe(
        debounceTime(300),
        tap(() => patchState(store, { isLoading: true })),
        map((documentResource: DocumentResource) => {
          if (!documentResource.categoryId) {
            documentResource.categoryId = '00000000-0000-0000-0000-000000000000';
          }
          return documentResource;
        }),
        switchMap((documentResource: DocumentResource) => {
          return documentLibraryService.getDocuments(documentResource).pipe(
            tapResponse({
              next: (documentsWithHeader: HttpResponse<DocumentInfo[]>) => {
                if (documentsWithHeader.headers.get('X-Pagination')) {
                  const headers = documentsWithHeader.headers;
                  const paginationParam = headers.get('X-Pagination') ? JSON.parse(
                    headers.get('X-Pagination') ?? '[]'
                  ) as ResponseHeader : null;
                  const resourceParam = store.documentResourceParameter();
                  const newDocumentResourceParameter = {
                    ...resourceParam,
                    totalCount: paginationParam?.totalCount ?? 0,
                    pageSize: paginationParam?.pageSize ?? 0,
                    skip: paginationParam?.skip ?? 0
                  };
                  const body = documentsWithHeader.body ? documentsWithHeader.body : [];
                  const filteredDocuments = store.documents().filter((doc) => body.findIndex((d) => d.id === doc.id) === -1);
                  const documents = [...filteredDocuments, ...body];
                  patchState(store, {
                    documents: [...documents],
                    isLoading: false,
                    commonError: null,
                    documentResourceParameter: { ...newDocumentResourceParameter }
                  })
                }
              },
              error: (err: CommonError) => {
                patchState(store, { commonError: err, isLoading: false });
              },
            })
          );
        })
      )
    ),
    setLoadingFlag(flag: boolean) {
      patchState(store, { isLoading: flag });
    },
    loadCategoriesBySelectedCategoryId() {
      this.loadCategoriesById(store.selectedCategoryId());
    },
    loadDocumentsByCategory(categoryId: string) {
      const documentResource = {
        ...store.documentResourceParameter(),
        skip: 0,
        categoryId: categoryId
      };
      this.loadByQuery(documentResource);
    },
    setDocumentsEmpty() {
      patchState(store, {
        documents: [],
      });
    },
    addCategoryPath(category: Category) {
      if (!category) {
        patchState(store, {
          newCategoryPath: []
        });
      } else {
        patchState(store, {
          newCategoryPath: [...store.newCategoryPath(), category]
        });
      }
    },
    removeCategoryPath(category: Category | null) {
      if (!category) {
        patchState(store, {
          newCategoryPath: []
        });
      } else {
        const findIndex = store.newCategoryPath().findIndex((c) => c.id === category.id);
        if (findIndex !== -1) {
          const newCategoryPath = store.newCategoryPath().slice(0, findIndex + 1);
          patchState(store, {
            newCategoryPath: newCategoryPath
          });
        }
      }
    },
    setDocumentResource(documentResource: DocumentResource) {
      patchState(store, {
        documentResourceParameter: { ...documentResource },
      });
    },
    loadDocuments() {
      if (store.selectedCategoryId()) {
        this.loadByQuery(store.documentResourceParameter());
      }
    }
  })),
  withHooks({
    onInit(store, securityService = inject(SecurityService)) {
      if (securityService.isUserAuthenticate()) {
        store.loadCategoriesById('');
      }
    },
  }),
);
