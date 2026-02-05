import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { take, mergeMap } from 'rxjs/operators';
import { Reminder } from '@core/domain-classes/reminder';
import { CommonService } from '@core/services/common.service';


@Injectable({ providedIn: 'root' })
export class ReminderDetailResolverService {
  constructor(private cs: CommonService, private router: Router) { }
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<Reminder> | null {
    const id = route.paramMap.get('id');
    if (id === 'add') {
      return null;
    }
    return this.cs.getReminder(id ?? '').pipe(
      take(1),
      mergeMap((reminder: Reminder) => {
        if (reminder) {
          if (reminder.startDate) {
            reminder.startTime = new Date(reminder.startDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });
          }
          if (reminder.endDate) {
            reminder.endTime = new Date(reminder.endDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });
          }
          return of(reminder);
        } else {
          this.router.navigate(['/reminders']);
          return of(null as any);
        }
      })
    );
  }
}
