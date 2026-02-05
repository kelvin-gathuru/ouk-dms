import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TableSetting } from '../../../core/domain-classes/table-setting';
import { MatTableSetting } from '../../../core/domain-classes/mat-table-setting';
import { DocumentStore } from '../document-store';
import { TranslationService } from '../../../core/services/translation.service';

import { ToastrService } from '@core/services/toastr-service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { toObservable } from '@angular/core/rxjs-interop';
import { Router, RouterModule } from '@angular/router';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-table-settings',
  imports: [
    ReactiveFormsModule,
    MatCheckboxModule,
    RouterModule,
    PageHelpTextComponent,
    TranslateModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './table-settings.component.html',
  styleUrl: './table-settings.component.scss'
})
export class TableSettingsComponent implements OnInit {
  tableSettingsForm: FormGroup;
  screenName: string = 'documents';
  documentStore = inject(DocumentStore);
  matTableSetting: MatTableSetting = this.documentStore.matTableSetting() ?? { id: 0, screenName: '', settings: [] };
  fb = inject(FormBuilder);
  router = inject(Router);
  translationService = inject(TranslationService);
  toastrService = inject(ToastrService);
  typeOptions = [
    { key: 'text', value: this.translationService.getValue('TEXT') },
    { key: 'datetime', value: this.translationService.getValue('DATETIME') },
    { key: 'bool', value: this.translationService.getValue('BOOL') }
  ];
  /**
   *
   */
  constructor() {
    this.updateTableSettings();
  }

  get settingsArray(): FormArray {
    return <FormArray>this.tableSettingsForm.get('settingsArray');
  }

  ngOnInit(): void {
    this.createTableSettingsForm();
  }

  updateTableSettings() {
    toObservable(this.documentStore.isTableSettingAdded).subscribe((flag) => {
      if (flag) {
        this.documentStore.updateTableSettingAdded();
        this.router.navigate(['/documents/list-view']);
      }
    });
  }

  onSeetingsClose() {
    this.router.navigate(['/documents/list-view']);
  }
  createTableSettingsForm() {
    this.tableSettingsForm = new FormGroup({
      screenName: new FormControl({ value: 'documents', disabled: true }, [Validators.required]),
      settingsArray: new FormArray([])
    });
    this.matTableSetting.settings.forEach((tableSeting: TableSetting) => {
      this.addTableSetting(tableSeting);
    });
  }

  addTableSetting(tableSeting: TableSetting) {
    this.settingsArray.push(this.fb.group({
      key: [tableSeting.key],
      header: [tableSeting.header],
      width: [tableSeting.width, [Validators.required]],
      type: [tableSeting.type],
      isVisible: [tableSeting.isVisible],
      orderNumber: [tableSeting.orderNumber, [Validators.required]]
    }));
  }
  buidlTableSetting() {
    const settings: MatTableSetting = {
      id: this.matTableSetting.id,
      screenName: this.screenName,
      settings: this.settingsArray.value
    }
    return settings;
  }
  saveTableSettings() {
    if (this.tableSettingsForm.valid) {
      this.documentStore.saveTableSettings(this.buidlTableSetting());
    } else {
      this.tableSettingsForm.markAllAsTouched();
    }
  }

}
