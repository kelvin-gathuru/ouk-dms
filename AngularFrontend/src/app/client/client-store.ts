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
import { debounceTime, distinctUntilChanged, pipe, switchMap, tap } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { CommonError } from '@core/error-handler/common-error';
import { TranslationService } from '@core/services/translation.service';
import { SecurityService } from '@core/security/security.service';
import { Client } from '@core/domain-classes/client';
import { ClientService } from './client.service';
import { Router } from '@angular/router';

import { ToastrService } from '@core/services/toastr-service';


type ClientState = {
  clients: Client[];
  client: Client | null;
  isLoading: boolean;
  loadList: boolean;
  isAddUpdate: boolean;
  commonError: CommonError | null;
};

export const initialClientState: ClientState = {
  clients: [],
  client: null,
  isLoading: false,
  loadList: false,
  isAddUpdate: false,
  commonError: null,
};

export const ClientStore = signalStore(
  { providedIn: 'root' },
  withState(initialClientState),
  withComputed(({ }) => ({})),
  withMethods(
    (
      store,
      clientService = inject(ClientService),
      toastrService = inject(ToastrService),
      translationService = inject(TranslationService),
      router = inject(Router)
    ) => ({
      loadClients: rxMethod<void>(
        pipe(
          debounceTime(300),
          tap(() => patchState(store, { isLoading: true })),
          switchMap(() =>
            clientService.getClients().pipe(
              tapResponse({
                next: (clients: Client[]) => {
                  patchState(store, {
                    clients: [...clients],
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
      deleteClientById: rxMethod<string>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((clientId: string) =>
            clientService.deleteClient(clientId).pipe(
              tapResponse({
                next: () => {
                  toastrService.success(
                    translationService.getValue('CLIENT_DELETED_SUCCESSFULLY')
                  );
                  patchState(store, {
                    clients: store.clients().filter((w) => w.id !== clientId),
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
      addUpdateClient: rxMethod<Client>(
        pipe(
          distinctUntilChanged(),
          tap(() => patchState(store, { isLoading: true })),
          switchMap((client: Client) => {
            if (client.id) {
              return clientService.updateClient(client).pipe(
                tapResponse({
                  next: (res) => {
                    const updatedClients = res as Client;
                    toastrService.success(
                      translationService.getValue('CLIENT_UPDATED_SUCCESSFULLY')
                    );
                    patchState(store, {
                      isLoading: false,
                      loadList: true,
                      isAddUpdate: true,
                      client: updatedClients
                    });
                  },
                  error: (err: CommonError) => {
                    patchState(store, { commonError: err, isLoading: false });
                  },
                })
              );
            } else {
              return clientService.addClient(client).pipe(
                tapResponse({
                  next: (res) => {
                    const createdClient = res as Client;
                    toastrService.success(
                      translationService.getValue('CLIENT_CREATED_SUCCESSFULLY')
                    );
                    patchState(store, {
                      isLoading: false,
                      loadList: true,
                      isAddUpdate: true,
                      client: createdClient
                    });
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
      getClientById: rxMethod<string>(
        pipe(
          tap(() => patchState(store, { isLoading: true })),
          switchMap((clientId: string) =>
            clientService.getClient(clientId).pipe(
              tapResponse({
                next: (client: Client) => {
                  patchState(store, {
                    client: client,
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
      resetflag() {
        patchState(store, { isAddUpdate: false });
      }
    })
  ),
  withHooks({
    onInit(store, securityService = inject(SecurityService)) {
      toObservable(store.loadList).subscribe((flag) => {
        if (flag) {
          store.loadClients();
        }
      });
      if (
        securityService.isUserAuthenticate() &&
        securityService.hasClaim('ALL_VIEW_DOCUMENTS')
      ) {
        store.loadClients();
      }
    },
  })
);
