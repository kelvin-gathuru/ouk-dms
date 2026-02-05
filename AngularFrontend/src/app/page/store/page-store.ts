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
import { CommonError } from '@core/error-handler/common-error';
import { TranslationService } from '@core/services/translation.service';
import { Page } from '@core/domain-classes/page';
import { PageService } from '@core/services/page.service';

type PageState = {
    pages: Page[];
    isLoading: boolean;
    loadList: boolean;
    isAddUpdate: boolean;
};

export const initialPageState: PageState = {
    pages: [],
    isLoading: false,
    loadList: true,
    isAddUpdate: false,
};

export const PageStore = signalStore(
    { providedIn: 'root' },
    withState(initialPageState),
    withComputed(({ }) => ({})),
    withMethods(
        (
            store,
            pageService = inject(PageService),
            toastrService = inject(ToastrService),
            translationService = inject(TranslationService),
        ) => ({
            loadPages: rxMethod<void>(
                pipe(
                    debounceTime(300),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap(() =>
                        pageService.getAllPages().pipe(
                            tapResponse({
                                next: (pages: Page[] | CommonError) => {
                                    const pagesNew = pages as Page[];
                                    patchState(store, {
                                        pages: [...pagesNew],
                                        isLoading: false,
                                        loadList: false
                                    });
                                },
                                error: (err: CommonError) => {
                                    patchState(store, { isLoading: false, loadList: false });
                                    console.error(err);
                                },
                            })
                        )
                    )
                )
            ),
            deletePageById: rxMethod<string>(
                pipe(
                    distinctUntilChanged(),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap((pageId: string) =>
                        pageService.deletePage(pageId).pipe(
                            tapResponse({
                                next: () => {
                                    toastrService.success(
                                        translationService.getValue('PAGE_DELETED_SUCCESSFULLY')
                                    );
                                    patchState(store, {
                                        pages: store.pages().filter((w) => w.id !== pageId),
                                        isLoading: false,
                                    });
                                },
                                error: (err: CommonError) => {
                                    patchState(store, { isLoading: false });
                                    console.error(err);
                                },
                            })
                        )
                    )
                )
            ),
            addUpdatePage: rxMethod<Page>(
                pipe(
                    distinctUntilChanged(),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap((page: Page) => {
                        if (page.id) {
                            return pageService.updatePage(page).pipe(
                                tapResponse({
                                    next: () => {
                                        toastrService.success(
                                            translationService.getValue('PAGE_UPDATED_SUCCESSFULLY')
                                        );
                                        patchState(store, {
                                            isLoading: false,
                                            isAddUpdate: true,
                                            pages: store.pages().map((w) =>
                                                w.id === page.id ? page : w
                                            ),
                                        });
                                    },
                                    error: (err: CommonError) => {
                                        patchState(store, { isLoading: false });
                                    },
                                })
                            );
                        } else {
                            return pageService.addPage(page).pipe(
                                tapResponse({
                                    next: () => {
                                        toastrService.success(
                                            translationService.getValue('PAGE_ADDED_SUCCESSFULLY')
                                        );
                                        patchState(store, {
                                            isLoading: false,
                                            loadList: true,
                                            isAddUpdate: true,
                                        });
                                    },
                                    error: (err: CommonError) => {
                                        patchState(store, { isLoading: false });
                                    },
                                })
                            );
                        }
                    })
                )
            ),
            resetflag: () => {
                patchState(store, { isAddUpdate: false });
            }
        })
    ),
    withHooks({
        onInit(store) {
            toObservable(store.loadList).subscribe((flag) => {
                if (flag) {
                    store.loadPages();
                }
            });
        },
    })
);
