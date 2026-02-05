import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NLog } from '@core/domain-classes/n-log';
import { NLogResource } from '@core/domain-classes/n-log-resource';
import { ResponseHeader } from '@core/domain-classes/response-header';
import { fromEvent, merge, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { BaseComponent } from '../../base.component';
import { NLogDataSource } from '../n-log-datasource';
import { NLogService } from '../n-log.service';
import { BreakpointsService } from '@core/services/breakpoints.service';
import { MatTableModule } from '@angular/material/table';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { TranslateModule } from '@ngx-translate/core';
import { UTCToLocalTime } from '@shared/pipes/utc-to-localtime.pipe';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AsyncPipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-n-log-list',
  templateUrl: './n-log-list.component.html',
  styleUrls: ['./n-log-list.component.scss'],
  standalone: true,
  imports: [
    MatPaginator,
    MatSort,
    MatTableModule,
    PageHelpTextComponent,
    TranslateModule,
    UTCToLocalTime,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    AsyncPipe,
    MatProgressSpinnerModule
  ],
})
export class NLogListComponent
  extends BaseComponent
  implements OnInit, AfterViewInit {
  dataSource: NLogDataSource;
  logs: NLog[] = [];
  displayedColumns: string[] = ['action', 'logged', 'level', 'message'];
  isLoadingResults = true;
  nLogResource: NLogResource;
  loading$: Observable<boolean>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;
  footerToDisplayed = ['footer'];
  constructor(
    private nLogService: NLogService,
    private breakpointsService: BreakpointsService
  ) {
    super();
    this.nLogResource = new NLogResource();
    this.nLogResource.pageSize = 10;
    this.nLogResource.orderBy = 'logged desc';
  }

  ngOnInit(): void {
    this.dataSource = new NLogDataSource(this.nLogService);
    this.dataSource.loadNLogs(this.nLogResource);
    this.getResourceParameter();
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.sub$.sink = merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap((c: any) => {
          this.nLogResource.skip =
            this.paginator.pageIndex * this.paginator.pageSize;
          this.nLogResource.pageSize = this.paginator.pageSize;
          this.nLogResource.orderBy =
            this.sort.active + ' ' + this.sort.direction;
          this.dataSource.loadNLogs(this.nLogResource);
        })
      )
      .subscribe();

    this.sub$.sink = fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(() => {
          this.paginator.pageIndex = 0;
          this.nLogResource.message = this.input.nativeElement.value;
          this.dataSource.loadNLogs(this.nLogResource);
        })
      )
      .subscribe();
  }

  getResourceParameter() {
    this.sub$.sink = this.dataSource.responseHeaderSubject$.subscribe(
      (c: ResponseHeader) => {
        if (c) {
          this.nLogResource.pageSize = c.pageSize;
          this.nLogResource.skip = c.skip;
          this.nLogResource.totalCount = c.totalCount;
        }
      }
    );
  }
}
