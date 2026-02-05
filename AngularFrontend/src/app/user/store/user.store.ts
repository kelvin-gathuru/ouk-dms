import { inject } from '@angular/core';
import { tapResponse } from '@ngrx/operators';
import {
  patchState,
  signalStore,
  withHooks,
  withMethods,
  withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { debounceTime, pipe, switchMap, tap } from 'rxjs';
import { User } from '@core/domain-classes/user';
import { CommonService } from '@core/services/common.service';

type UserState = {
  users: User[];
};

export const initialUserState: UserState = {
  users: [],
};

export const UserStore = signalStore(
  { providedIn: 'root' },
  withState(initialUserState),
  withMethods(
    (
      store,
      userService = inject(CommonService),
    ) => ({
      getUsers: rxMethod<void>(
        pipe(
          debounceTime(300),
          switchMap(() =>
            userService.getUsersForDropdown().pipe(
              tapResponse({
                next: (user: User[]) => {
                  const userNew = user as User[];
                  patchState(store, { users: userNew });
                },
                error: (err) => {
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
    onInit(store) {
      store.getUsers();
    },
  })
);


