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
import { ActionService } from '@core/services/action.service';
import { Action } from '@core/domain-classes/action';

type ActionState = {
    actions: Action[];
    selectedPageAction: Action[];
    isLoading: boolean;
    loadList: boolean;
    isAddUpdate: boolean;
};

export const initialActionState: ActionState = {
    actions: [],
    selectedPageAction: [],
    isLoading: false,
    loadList: true,
    isAddUpdate: false,
};

export const ActionStore = signalStore(
    { providedIn: 'root' },
    withState(initialActionState),
    withComputed(({ }) => ({})),
    withMethods(
        (
            store,
            actionService = inject(ActionService),
            toastrService = inject(ToastrService),
            translationService = inject(TranslationService),
        ) => ({
            loadActions: rxMethod<void>(
                pipe(
                    debounceTime(300),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap(() =>
                        actionService.getAllActions().pipe(
                            tapResponse({
                                next: (actions: Action[] | CommonError) => {
                                    const actionsNew = actions as Action[];
                                    patchState(store, {
                                        actions: actionsNew,
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
            deleteActionById: rxMethod<string>(
                pipe(
                    distinctUntilChanged(),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap((actionId: string) =>
                        actionService.deleteAction(actionId).pipe(
                            tapResponse({
                                next: () => {
                                    toastrService.success(
                                        translationService.getValue('ACTION_DELETED_SUCCESSFULLY')
                                    );
                                    patchState(store, {
                                        isLoading: false,
                                        selectedPageAction: store.selectedPageAction().filter((w) => w.id !== actionId),
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
            addUpdateAction: rxMethod<Action>(
                pipe(
                    distinctUntilChanged(),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap((action: Action) => {
                        if (action.id) {
                            return actionService.updateAction(action).pipe(
                                tapResponse({
                                    next: () => {
                                        toastrService.success(
                                            translationService.getValue('ACTION_UPDATED_SUCCESSFULLY')
                                        );
                                        patchState(store, {
                                            isLoading: false,
                                            isAddUpdate: true,
                                            selectedPageAction: store.selectedPageAction().map((w) =>
                                                w.id === action.id ? action : w
                                            ),
                                        });
                                    },
                                    error: (err: CommonError) => {
                                        patchState(store, { isLoading: false });
                                    },
                                })
                            );
                        } else {
                            return actionService.addAction(action).pipe(
                                tapResponse({
                                    next: (res) => {
                                        const resNew = res as Action;
                                        if (resNew) {
                                            toastrService.success(
                                                translationService.getValue('ACTION_CREATED_SUCCESSFULLY')
                                            );

                                            patchState(store, {
                                                isLoading: false,
                                                selectedPageAction: [...store.selectedPageAction(), resNew],
                                                isAddUpdate: true,
                                            });
                                        }
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
            getActionByPageId: rxMethod<string>(
                pipe(
                    distinctUntilChanged(),
                    tap(() => patchState(store, { isLoading: true })),
                    switchMap((actionId: string) =>
                        actionService.getActionsByPageId(actionId).pipe(
                            tapResponse({
                                next: (res) => {
                                    const resNew = res as Action[];
                                    if (resNew.length > 0) {
                                        toastrService.success(
                                            translationService.getValue('ACTION_DELETED_SUCCESSFULLY')
                                        );
                                        patchState(store, {
                                            selectedPageAction: resNew,
                                            isLoading: false,
                                        });
                                    }
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
            resetflag: () => {
                patchState(store, { isAddUpdate: false });
            }
        })
    ),
    withHooks({
        onInit(store) {
            toObservable(store.loadList).subscribe((flag) => {
                if (flag) {
                    store.loadActions();
                }
            });
        },
    })
);
