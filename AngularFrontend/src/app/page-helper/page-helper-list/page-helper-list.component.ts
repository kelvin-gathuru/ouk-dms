import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { PageHelperService } from '../page-helper.service';
import { Observable } from 'rxjs';
import { PageHelper } from '@core/domain-classes/pageHelper';
import { PageHelperListPresentationComponent } from '../page-helper-list-presentation/page-helper-list-presentation.component';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-page-helper-list',
  templateUrl: './page-helper-list.component.html',
  styleUrls: ['./page-helper-list.component.scss'],
  standalone: true,
  imports: [
    PageHelperListPresentationComponent,
    AsyncPipe
  ],
})
export class PageHelperListComponent extends BaseComponent implements OnInit {
  pageHelpers$: Observable<PageHelper[]> | null = null;

  constructor(
    private pageHelperService: PageHelperService
  ) {
    super();
  }
  ngOnInit(): void {
    this.getPageHelpers();
  }

  getPageHelpers(): void {
    this.pageHelpers$ = this.pageHelperService.getPageHelpers();
  }
}

