import { Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  ReactiveFormsModule,
  UntypedFormArray,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { DayOfWeek } from '@core/domain-classes/day-of-week.enum';
import { DocumentInfo } from '@core/domain-classes/document-info';
import { Frequency } from '@core/domain-classes/frequency.enum';
import { Quarter } from '@core/domain-classes/quarter.enum';
import { Reminder } from '@core/domain-classes/reminder';
import {
  dayOfWeekArray,
  monthsArray,
} from '@core/domain-classes/reminder-constant';
import { ReminderFrequency } from '@core/domain-classes/reminder-frequency';
import { User } from '@core/domain-classes/user';
import { UserAuth } from '@core/domain-classes/user-auth';
import { SecurityService } from '@core/security/security.service';
import { CommonService } from '@core/services/common.service';
import { TranslateModule } from '@ngx-translate/core';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';

import { ToastrService } from '@core/services/toastr-service';
import { BaseComponent } from '../../base.component';
import { ReminderService } from '../../reminder/reminder.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  templateUrl: './document-reminder.component.html',
  styleUrls: ['./document-reminder.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    PageHelpTextComponent,
    MatCheckboxModule,
    MatSelectModule,
    MatRadioModule,
    NgxMaterialTimepickerModule,
    MatDatepickerModule,
    TranslateModule,
    MatDialogModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ]
})
export class DocumentReminderComponent extends BaseComponent implements OnInit {
  reminderFrequencies: ReminderFrequency[] = [];
  reminderForm: UntypedFormGroup;
  minDate = new Date();
  selectedUsers: User[] = [];
  reminder: Reminder;
  isLoading = false;
  securityObject: UserAuth;
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
    private reminderService: ReminderService,
    private commonService: CommonService,
    private toastrService: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: DocumentInfo,
    private dialogRef: MatDialogRef<DocumentReminderComponent>,
    private securityService: SecurityService
  ) {
    super();
  }

  ngOnInit(): void {
    for (let i = 1; i <= 31; i++) {
      this.days.push(i);
    }
    this.getReminderFrequency();
    this.createReminderForm();
    this.getAuthObj();
  }

  getAuthObj() {
    this.sub$.sink = this.securityService.SecurityObject.subscribe((c) => {
      if (c) {
        this.securityObject = c;
      }
    });
  }

  closeDialog() {
    this.dialogRef.close();
  }

  getReminderFrequency() {
    this.sub$.sink = this.commonService
      .getReminderFrequency()
      .subscribe((f) => (this.reminderFrequencies = [...f]));
  }

  createReminderForm() {
    var currentDate = new Date();
    this.reminderForm = this.fb.group(
      {
        id: [''],
        subject: ['', [Validators.required]],
        message: ['', [Validators.required]],
        frequency: [''],
        isRepeated: [false],
        isEmailNotification: [false],
        startDate: [currentDate, [Validators.required]],
        startTime: ['12:00', [Validators.required]],
        endDate: [null],
        endTime: [null],
        dayOfWeek: [2],
        documentId: [this.data.id],
      },
      { validators: this.dateAndTimeValidator }
    );
  }

  dateAndTimeValidator(control: AbstractControl) {
    const endDate = control.get('endDate')?.value;
    const endTime = control.get('endTime')?.value;

    if (endDate && !endTime) {
      control.get('endTime')?.setErrors({ required: true });
    } else {
      control.get('endTime')?.setErrors(null);
    }
    return null;
  }

  checkData(event: MatCheckboxChange) {
    if (event.checked) {
      this.reminderForm.get('frequency')?.setValidators([Validators.required]);
    } else {
      this.reminderForm.get('frequency')?.setValidators([]);
    }
    this.reminderForm.get('frequency')?.updateValueAndValidity();
    this.reminderForm.markAllAsTouched();
  }

  createReminder() {
    if (!this.reminderForm.valid) {
      this.reminderForm.markAllAsTouched();
      return;
    }
    let startDate = new Date(this.reminderForm.get('startDate')?.value);
    let startTime = this.reminderForm.get('startTime')?.value;
    const [hours, minutes] = startTime.split(':');
    const combinedDate = new Date(
      startDate.getFullYear(),
      startDate.getMonth(),
      startDate.getDate(),
      hours,
      minutes
    );
    let endDate = this.reminderForm.get('endDate')?.value;
    let combinedEndDate;
    if (endDate) {
      endDate = new Date(endDate); // Ensure it's a Date object
      let endTime = this.reminderForm.get('endTime')?.value;

      if (endTime && typeof endTime === 'string' && endTime.includes(':')) {
        let [hours, minutes] = endTime.split(':').map(Number); // Convert time parts to numbers
        combinedEndDate = new Date(
          endDate.getFullYear(),
          endDate.getMonth(),
          endDate.getDate(),
          hours,
          minutes
        );
      }
    }

    let reminder: Reminder = this.reminderForm.value;
    reminder.startDate = combinedDate;
    reminder.endDate = combinedEndDate;
    reminder.reminderUsers = [
      {
        reminderId: reminder.id ?? '',
        userId: this.securityObject.id ?? '',
      },
    ];

    if (!reminder.isRepeated) {
      reminder.dailyReminders = [];
      reminder.quarterlyReminders = [];
      reminder.halfYearlyReminders = [];
    }

    if (!this.reminder) {
      this.isLoading = true;
      this.sub$.sink = this.reminderService.addReminder(reminder).subscribe(
        (d) => {
          this.toastrService.success(
            this.translationService.getValue('REMINDER_CREATED_SUCCESSFULLY')
          );
          // this.route.navigate(['/reminders']);
          this.dialogRef.close();
          this.isLoading = false;
        },
        () => (this.isLoading = false)
      );
    } else {
      if (reminder.dailyReminders) {
        reminder.dailyReminders = reminder.dailyReminders.map((c) => {
          c.reminderId = this.reminder.id ?? '';
          return c;
        });
      }
      if (reminder.quarterlyReminders) {
        reminder.quarterlyReminders = reminder.quarterlyReminders.map((c) => {
          c.reminderId = this.reminder.id ?? '';
          return c;
        });
      }
      if (reminder.halfYearlyReminders) {
        reminder.halfYearlyReminders = reminder.halfYearlyReminders.map((c) => {
          c.reminderId = this.reminder.id ?? '';
          return c;
        });
      }
      this.isLoading = true;
      this.sub$.sink = this.reminderService.updateReminder(reminder).subscribe(
        (d) => {
          this.toastrService.success(
            this.translationService.getValue('REMINDER_UPDATED_SUCCESSFULLY')
          );
          // this.route.navigate(['/reminders']);
          this.dialogRef.close();
          this.isLoading = false;
        },
        () => (this.isLoading = false)
      );
    }
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
      var formArray = this.fb.array([]);
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
      var formArray = this.fb.array([]);
      var firstQuaterMonths = this.months.filter(
        (c) => [1, 2, 3].indexOf(c.id) >= 0
      );
      var secondQuaterMonths = this.months.filter(
        (c) => [4, 5, 6].indexOf(c.id) >= 0
      );
      var thirdQuaterMonths = this.months.filter(
        (c) => [7, 8, 9].indexOf(c.id) >= 0
      );
      var forthQuaterMonths = this.months.filter(
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
      var formArray = this.fb.array([]);
      var firstQuaterMonths = this.months.filter(
        (c) => [1, 2, 3, 4, 5, 6].indexOf(c.id) >= 0
      );
      var secondQuaterMonths = this.months.filter(
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
      isActive: [true],
      name: [DayOfWeek[dayOfWeek]],
    });
  }

  createQuarterlyReminderFormGroup(
    quater: Quarter,
    name: string,
    monthValues: any[]
  ) {
    return this.fb.group({
      id: [''],
      reminderId: [''],
      quarter: [quater],
      day: [this.getCurrentDay()],
      month: [monthValues[0].id],
      name: [name],
      monthValues: [monthValues],
    });
  }

  createHalfYearlyReminderFormGroup(
    quater: Quarter,
    name: string,
    monthValues: any[]
  ) {
    return this.fb.group({
      id: [''],
      reminderId: [''],
      quarter: [quater],
      day: [this.getCurrentDay()],
      month: [monthValues[0].id],
      name: [name],
      monthValues: [monthValues],
    });
  }

  getCurrentDay(): number {
    return new Date().getDate();
  }

  onDateChange(formGroup: any) {
    const day = formGroup.get('day')?.value;
    const month = formGroup.get('month')?.value;
    var daysInMonth = new Date(
      new Date().getFullYear(),
      Number.parseInt(month),
      0
    ).getDate();
    if (day > daysInMonth) {
      if (formGroup) {
        formGroup?.setErrors({
          invalidDate: 'Invalid Date',
        });
        formGroup?.markAllAsTouched();
      }
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
