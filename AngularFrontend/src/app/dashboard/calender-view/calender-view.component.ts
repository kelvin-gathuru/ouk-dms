import { Component, inject, OnInit, ViewChild, Renderer2 } from '@angular/core';
import { CalenderReminderDto } from '@core/domain-classes/calender-reminder';
import { forkJoin } from 'rxjs';
import { DashboradService } from '../dashboard.service';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { FullCalendarComponent } from '@fullcalendar/angular';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ReminderDetailComponent } from '@shared/reminder-detail/reminder-detail.component';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { CalenderViewModule } from './calender-view.module';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-calender-view',
  templateUrl: './calender-view.component.html',
  styleUrls: ['./calender-view.component.scss'],
  standalone: true,
  imports: [
    MatDialogModule,
    PageHelpTextComponent,
    TranslateModule,
    CalenderViewModule,
    MatCardModule
  ]
})
export class CalenderViewComponent implements OnInit {
  @ViewChild('calendar') calendarComponent: FullCalendarComponent;
  events: any[] = [];
  renderer = inject(Renderer2);

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: (arg) => this.handleDateClick(arg),
    datesSet: (arg) => this.handleDatesSet(arg),
    eventClick: (info) => this.handleEventClick(info),
    eventDidMount: this.addTooltip.bind(this), // Attach tooltip when events are rendered,
  };

  dashboardService = inject(DashboradService);
  dialog = inject(MatDialog);

  ngOnInit(): void {
    const currentDate = new Date();
  }

  handleDateClick(arg: any) { }

  handleDatesSet(arg: any) {
    const currentDate = this.calendarComponent
      ?.getApi()
      .getCurrentData().currentDate;
    this.gerReminders(currentDate.getMonth() + 1, currentDate.getFullYear());
  }

  gerReminders(month: number, year: number) {
    this.events = [];
    const dailyReminders = this.dashboardService.getDailyReminders(month, year);
    const weeklyReminders = this.dashboardService.getWeeklyReminders(
      month,
      year
    );
    const monthlyReminders = this.dashboardService.getMonthlyReminders(
      month,
      year
    );
    const quarterlyReminders = this.dashboardService.getQuarterlyReminders(
      month,
      year
    );
    const halfYearlyReminders = this.dashboardService.getHalfYearlyReminders(
      month,
      year
    );
    const yearlyReminders = this.dashboardService.getYearlyReminders(
      month,
      year
    );
    const oneTimeReminders = this.dashboardService.getOneTimeReminders(
      month,
      year
    );

    const allEvents$ = [
      dailyReminders,
      weeklyReminders,
      monthlyReminders,
      quarterlyReminders,
      halfYearlyReminders,
      yearlyReminders,
      oneTimeReminders,
    ];

    forkJoin(allEvents$).subscribe((results) => {
      this.addEvent(results[0] as CalenderReminderDto[]);
      this.addEvent(results[1] as CalenderReminderDto[]);
      this.addEvent(results[2] as CalenderReminderDto[]);
      this.addEvent(results[3] as CalenderReminderDto[]);
      this.addEvent(results[4] as CalenderReminderDto[]);
      this.addEvent(results[5] as CalenderReminderDto[]);
      this.addEvent(results[6] as CalenderReminderDto[]);
    });
  }

  addEvent(calenderReminder: CalenderReminderDto[]) {
    const event = calenderReminder.map((c) => {
      return {
        title: c.title,
        start: new Date(c.start.toString()),
        end: new Date(c.end.toString()),
        extendedProps: {
          remiderId: c?.remiderId,
          description: c?.title, // Tooltip content
        },
      };
    });
    this.events = this.events.concat(event);
  }

  handleEventClick(info: any) {
    this.dialog.open(ReminderDetailComponent, {
      data: info.event?.extendedProps.remiderId,
      width: '80vw',
    });
  }

  // Add tooltip on hover
  addTooltip(info: any) {
    const tooltipContent =
      info.event.extendedProps.description || info.event.title;

    // Create a div element to hold the tooltip
    const tooltipDiv = this.renderer.createElement('div');
    this.renderer.addClass(tooltipDiv, 'tooltip-custom');
    this.renderer.setProperty(tooltipDiv, 'textContent', tooltipContent);

    // Style the tooltip to appear inside the same date box
    this.renderer.setStyle(tooltipDiv, 'position', 'absolute');
    this.renderer.setStyle(tooltipDiv, 'background-color', '#000');
    this.renderer.setStyle(tooltipDiv, 'color', '#fff');
    this.renderer.setStyle(tooltipDiv, 'padding', '5px');
    this.renderer.setStyle(tooltipDiv, 'border-radius', '5px');
    this.renderer.setStyle(tooltipDiv, 'white-space', 'nowrap');
    this.renderer.setStyle(tooltipDiv, 'display', 'none');
    this.renderer.setStyle(tooltipDiv, 'z-index', '1000');

    // Append the tooltip to the event element
    this.renderer.appendChild(info.el, tooltipDiv);

    // Position tooltip relative to the event box
    this.renderer.listen(info.el, 'mouseenter', (event) => {
      const rect = info.el.getBoundingClientRect();

      // Set tooltip position to stay within the same box
      this.renderer.setStyle(tooltipDiv, 'display', 'block');
      this.renderer.setStyle(tooltipDiv, 'top', `${rect.height + 5}px`); // 5px below the event box
      this.renderer.setStyle(tooltipDiv, 'left', `0px`); // Align with the left side of the event
    });

    // Hide tooltip on mouseleave
    this.renderer.listen(info.el, 'mouseleave', () => {
      this.renderer.setStyle(tooltipDiv, 'display', 'none');
    });

    // Use a timeout to ensure the DOM is fully rendered
    setTimeout(() => {
      const timeElement = info.el.querySelector('.fc-event-time');
      if (timeElement) {
        timeElement.style.display = 'none';
      }
    }, 0);
  }
}
