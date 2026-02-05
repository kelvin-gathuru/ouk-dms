import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import {
  AbstractControl,
  ReactiveFormsModule,
  UntypedFormArray,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';

import { ReminderFrequency } from '@core/domain-classes/reminder-frequency';
import { User } from '@core/domain-classes/user';
import { CommonService } from '@core/services/common.service';
import { BaseComponent } from '../../base.component';
import { Reminder } from '@core/domain-classes/reminder';
import { Frequency } from '@core/domain-classes/frequency.enum';
import { DayOfWeek } from '@core/domain-classes/day-of-week.enum';
import { Quarter } from '@core/domain-classes/quarter.enum';
import {
  MatCheckboxChange,
  MatCheckboxModule,
} from '@angular/material/checkbox';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import {
  dayOfWeekArray,
  monthsArray,
} from '@core/domain-classes/reminder-constant';
import { IdNameNumber } from '@core/domain-classes/id-name';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-reminder-detail',
  templateUrl: './reminder-detail.component.html',
  styleUrls: ['./reminder-detail.component.scss'],
  imports: [
    MatDatepickerModule,
    TranslateModule,
    MatCheckboxModule,
    MatRadioModule,
    ReactiveFormsModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  providers: [DatePipe]
})
export class ReminderDetailComponent extends BaseComponent implements OnInit {
  reminderFrequencies: ReminderFrequency[] = [];
  reminderForm: UntypedFormGroup;
  minDate = new Date();
  selectedUsers: User[] = [];
  reminder: Reminder;
  isLoading = false;

  dayOfWeek = dayOfWeekArray;
  months = monthsArray;
  days: number[] = [];

  get dailyRemindersArray(): UntypedFormArray {
    return <UntypedFormArray>this.reminderForm.get('dailyReminders');
  }

  get quarterlyRemindersArray(): UntypedFormArray {
    return <UntypedFormArray>this.reminderForm.get('quarterlyReminders');
  }

  get halfYearlyRemindersArray(): UntypedFormArray {
    return <UntypedFormArray>this.reminderForm.get('halfYearlyReminders');
  }

  constructor(
    private fb: UntypedFormBuilder,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: string,
    private dialogRef: MatDialogRef<ReminderDetailComponent>,
    private datePipe: DatePipe
  ) {
    super();
  }

  ngOnInit(): void {
    for (let i = 1; i <= 31; i++) {
      this.days.push(i);
    }
    this.createReminderForm();
    this.getReminderFrequency();
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  getReminder() {
    this.sub$.sink = this.commonService
      .getReminder(this.data)
      .subscribe((c: Reminder) => {
        this.reminder = { ...c };
        if (this.reminder.dailyReminders) {
          this.reminder.dailyReminders = this.reminder.dailyReminders.sort(
            (c1, c2) => c1.dayOfWeek - c2.dayOfWeek
          );
        }

        if (this.reminder.quarterlyReminders) {
          this.reminder.quarterlyReminders =
            this.reminder.quarterlyReminders.sort(
              (c1, c2) => c1.quarter - c2.quarter
            );
        }

        if (this.reminder.halfYearlyReminders) {
          this.reminder.halfYearlyReminders =
            this.reminder.halfYearlyReminders.sort(
              (c1, c2) => c1.quarter - c2.quarter
            );
        }
        this.reminderForm.patchValue({
          frequency: this.reminder.frequency,
        });
        this.onFrequencyChange();
        this.reminderForm.patchValue(this.reminder);
        this.reminderForm.patchValue({
          startDate: this.datePipe.transform(
            this.reminder.startDate,
            'dd/MM/yyyy hh:mm:ss a'
          ),
          endDate: this.datePipe.transform(
            this.reminder.endDate,
            'dd/MM/yyyy hh:mm:ss a'
          ),
        });
      });
  }

  getReminderFrequency() {
    this.sub$.sink = this.commonService
      .getReminderFrequency()
      .subscribe((f) => {
        this.reminderFrequencies = [...f]
        this.getReminder();
      });
  }

  createReminderForm() {
    const currentDate = new Date();
    this.reminderForm = this.fb.group({
      id: [''],
      subject: [{ value: '', disabled: true }, [Validators.required]],
      message: [{ value: '', disabled: true }, [Validators.required]],
      frequency: [{ value: '', disabled: true }],
      isRepeated: [{ value: false, disabled: true }],
      isEmailNotification: [{ value: false, disabled: true }],
      startDate: [
        { value: currentDate, disabled: true },
        [Validators.required],
      ],
      endDate: [{ value: null, disabled: true }],
      dayOfWeek: [{ value: null, disabled: true }],
      documentId: [{ value: null, disabled: true }],
    });
  }

  checkData(event: MatCheckboxChange) {
    if (event.checked) {
      this.reminderForm.get('frequency')?.setValidators([Validators.required]);
    } else {
      this.reminderForm.get('frequency')?.setValidators([]);
    }
    this.reminderForm.updateValueAndValidity();
  }

  onFrequencyChange() {
    let frequency = this.reminderForm.get('frequency')?.value;
    frequency = frequency == 0 ? '0' : frequency;
    if (frequency == Frequency.Daily.toString()) {
      this.removeQuarterlyReminders();
      this.removeHalfYearlyReminders();
      this.addDailReminders();
      this.reminderForm.get('dayOfWeek')?.setValue('');
    } else if (frequency == Frequency.Weekly.toString()) {
      this.removeDailReminders();
      this.removeQuarterlyReminders();
      this.removeHalfYearlyReminders();
      this.reminderForm.get('dayOfWeek')?.setValue(2);
    } else if (frequency == Frequency.Quarterly.toString()) {
      this.removeDailReminders();
      this.removeHalfYearlyReminders();
      this.addQuarterlyReminders();
      this.reminderForm.get('dayOfWeek')?.setValue('');
    } else if (frequency == Frequency.HalfYearly.toString()) {
      this.removeDailReminders();
      this.removeQuarterlyReminders();
      this.addHalfYearlyReminders();
      this.reminderForm.get('dayOfWeek')?.setValue('');
    } else {
      this.removeDailReminders();
      this.removeQuarterlyReminders();
      this.removeHalfYearlyReminders();
      this.reminderForm.get('dayOfWeek')?.setValue('');
    }
  }

  addDailReminders() {
    if (!this.reminderForm.contains('dailyReminders')) {
      const formArray = this.fb.array([]);
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Sunday));
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Monday));
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Tuesday));
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Wednesday));
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Thursday));
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Friday));
      formArray.push(this.createDailyReminderFormGroup(DayOfWeek.Saturday));
      this.reminderForm.addControl('dailyReminders', formArray);
    }
  }

  addQuarterlyReminders() {
    if (!this.reminderForm.contains('quarterlyReminders')) {
      const formArray = this.fb.array([]);
      const firstQuaterMonths = this.months.filter(
        (c) => [1, 2, 3].indexOf(c.id) >= 0
      );
      const secondQuaterMonths = this.months.filter(
        (c) => [4, 5, 6].indexOf(c.id) >= 0
      );
      const thirdQuaterMonths = this.months.filter(
        (c) => [7, 8, 9].indexOf(c.id) >= 0
      );
      const forthQuaterMonths = this.months.filter(
        (c) => [10, 11, 12].indexOf(c.id) >= 0
      );
      formArray.push(
        this.createQuarterlyReminderFormGroup(
          Quarter.Quarter1,
          'JAN_MAR',
          firstQuaterMonths
        )
      );
      formArray.push(
        this.createQuarterlyReminderFormGroup(
          Quarter.Quarter2,
          'APR_JUN',
          secondQuaterMonths
        )
      );
      formArray.push(
        this.createQuarterlyReminderFormGroup(
          Quarter.Quarter3,
          'JUL_SEPT',
          thirdQuaterMonths
        )
      );
      formArray.push(
        this.createQuarterlyReminderFormGroup(
          Quarter.Quarter4,
          'OCT_DEC',
          forthQuaterMonths
        )
      );
      this.reminderForm.addControl('quarterlyReminders', formArray);
    }
  }

  addHalfYearlyReminders() {
    if (!this.reminderForm.contains('halfYearlyReminders')) {
      const formArray = this.fb.array([]);
      const firstQuaterMonths = this.months.filter(
        (c) => [1, 2, 3, 4, 5, 6].indexOf(c.id) >= 0
      );
      const secondQuaterMonths = this.months.filter(
        (c) => [7, 8, 9, 10, 11, 12].indexOf(c.id) >= 0
      );
      formArray.push(
        this.createHalfYearlyReminderFormGroup(
          Quarter.Quarter1,
          'JAN_JUN',
          firstQuaterMonths
        )
      );
      formArray.push(
        this.createHalfYearlyReminderFormGroup(
          Quarter.Quarter2,
          'JUL_DEC',
          secondQuaterMonths
        )
      );
      this.reminderForm.addControl('halfYearlyReminders', formArray);
    }
  }

  removeDailReminders() {
    if (this.reminderForm.contains('dailyReminders')) {
      this.reminderForm.removeControl('dailyReminders');
    }
  }

  removeQuarterlyReminders() {
    if (this.reminderForm.contains('quarterlyReminders')) {
      this.reminderForm.removeControl('quarterlyReminders');
    }
  }

  removeHalfYearlyReminders() {
    if (this.reminderForm.contains('halfYearlyReminders')) {
      this.reminderForm.removeControl('halfYearlyReminders');
    }
  }

  createDailyReminderFormGroup(dayOfWeek: DayOfWeek) {
    return this.fb.group({
      id: [''],
      reminderId: [''],
      dayOfWeek: [dayOfWeek],
      isActive: [{ value: true, disabled: true }],
      name: [DayOfWeek[dayOfWeek]],
    });
  }

  createQuarterlyReminderFormGroup(
    quater: Quarter,
    name: string,
    monthValues: IdNameNumber[]
  ) {
    return this.fb.group({
      id: [''],
      reminderId: [''],
      quarter: [quater],
      day: [{ value: this.getCurrentDay(), disabled: true }],
      month: [{ value: monthValues[0], disabled: true }],
      name: [name],
      monthValues: [monthValues],
    });
  }

  createHalfYearlyReminderFormGroup(
    quater: Quarter,
    name: string,
    monthValues: IdNameNumber[]
  ) {
    return this.fb.group({
      id: [''],
      reminderId: [''],
      quarter: [quater],
      day: [{ value: this.getCurrentDay(), disabled: true }],
      month: [{ value: monthValues[0], disabled: true }],
      name: [name],
      monthValues: [monthValues],
    });
  }

  getCurrentDay(): number {
    return new Date().getDate();
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  onDateChange(formGroup: AbstractControl<any, any>) {
    const day = formGroup.get('day')?.value;
    const month = formGroup.get('month')?.value;
    const daysInMonth = new Date(
      new Date().getFullYear(),
      Number.parseInt(month),
      0
    ).getDate();
    if (day > daysInMonth) {
      formGroup.setErrors({
        invalidDate: 'Invalid Date',
      });
      formGroup.markAllAsTouched();
    }
  }
}
