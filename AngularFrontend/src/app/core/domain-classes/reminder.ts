import { ReminderUsers } from "./reminder-users";

export class Reminder {
  id?: string;
  subject: string;
  description: string;
  startDate?: Date;
  startTime: string;
  endDate?: Date;
  endTime: string;
  documentId?: string;
  documentName?: string;
  frequencyId: string;
  frequency?: string;
  reminderDate: Date;
  isRepeated: boolean;
  isEmailNotification: boolean;
  isActive: boolean;
  reminderUsers: ReminderUsers[];
  dailyReminders: DailyReminders[];
  quarterlyReminders: QuarterlyReminders[];
  halfYearlyReminders: HalfYearlyReminders[];
}

export class DailyReminders {
  id: string;
  reminderId: string;
  dayOfWeek: number;
  isActive: boolean;
}

export class QuarterlyReminders {
  id?: string;
  reminderId: string;
  day: number;
  month: number;
  quarter: number;
}

export class HalfYearlyReminders {
  id?: string;
  reminderId: string;
  day: number;
  month: number;
  quarter: number;
}
