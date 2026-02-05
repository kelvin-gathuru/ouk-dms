import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { DashboradService } from './dashboard.service';
import { DocumentByCategoryChartComponent } from './document-by-category-chart/document-by-category-chart.component';
import { CalenderViewComponent } from './calender-view/calender-view.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  standalone: true,
  imports: [
    DocumentByCategoryChartComponent,
    CalenderViewComponent
  ]
})
export class DashboardComponent extends BaseComponent implements OnInit {
  totalAssignDocumentCount = 0;
  expireSoonDocumentCount = 0
  constructor() {
    super();
  }

  ngOnInit() {

  }
}


