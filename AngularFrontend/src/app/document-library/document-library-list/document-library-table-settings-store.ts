import { computed, inject } from "@angular/core";
import { tapResponse } from "@ngrx/operators";
import { patchState, signalStore, withComputed, withHooks, withMethods, withState } from "@ngrx/signals";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { ToastrService } from '@core/services/toastr-service';
import { debounceTime, distinctUntilChanged, pipe, switchMap, tap } from "rxjs";
import { CommonError } from '@core/error-handler/common-error';
import { TranslationService } from '@core/services/translation.service';
import { MatTableSetting } from "@core/domain-classes/mat-table-setting";
import { TableSettingsService } from "@core/services/table-settings.service";
import { DocumentsLibraryTableSettings } from "./document-library-table-settings";

type DocumentLibraryTableSettingState = {
  isLoading: boolean;
  commonError: CommonError | null;
  matTableSetting: MatTableSetting | null;
  isTableSettingAdded: boolean;
};

export const initialDocumentLibraryTableSettingsState: DocumentLibraryTableSettingState = {
  isLoading: false,
  commonError: null,
  matTableSetting: null,
  isTableSettingAdded: false
};

export const DocumentLibraryTableSettingsStore = signalStore(
  { providedIn: 'root' },
  withState(initialDocumentLibraryTableSettingsState),
  withComputed(({ matTableSetting }) => ({
    tableSettingsVisible: computed(() => {
      const setting = matTableSetting();
      return setting && setting.settings && setting.settings.length > 0
        ? setting.settings.filter(c => c.isVisible)
        : [...DocumentsLibraryTableSettings];
    }),
  })),
  withMethods((store,
    toastrService = inject(ToastrService),
    tableSettingsService = inject(TableSettingsService),
    translationService = inject(TranslationService)) => ({
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
                      settings: [...DocumentsLibraryTableSettings]
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
      loadTableSettingByScreenName(screenName: string) {
        this.loadTableSettingsByQuery(screenName);
      },
      updateTableSettingAdded() {
        patchState(store, { isTableSettingAdded: false });
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
  }),
);
