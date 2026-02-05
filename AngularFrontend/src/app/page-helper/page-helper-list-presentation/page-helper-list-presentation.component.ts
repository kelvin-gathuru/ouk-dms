import { Component, Input, OnInit } from '@angular/core';
import { PageHelper } from '@core/domain-classes/pageHelper';
import { BaseComponent } from '../../base.component';
import { Router, RouterModule } from '@angular/router';
import { PageHelpPreviewComponent } from '@shared/page-help-preview/page-help-preview.component';
import { CommonService } from '@core/services/common.service';
import { MatDialog } from '@angular/material/dialog';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { MatTableModule } from '@angular/material/table';
import { TranslateModule } from '@ngx-translate/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-page-helper-list-presentation',
  templateUrl: './page-helper-list-presentation.component.html',
  styleUrls: ['./page-helper-list-presentation.component.scss'],
  standalone: true,
  imports: [
    PageHelpTextComponent,
    MatTableModule,
    TranslateModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ]
})
export class PageHelperListPresentationComponent
  extends BaseComponent
  implements OnInit {
  @Input() pageHelpers: PageHelper[] | null = [];
  columnsToDisplay: string[] = ['action', 'name', 'code'];

  constructor(
    private router: Router,
    private commonService: CommonService,
    private dialog: MatDialog
  ) {
    super();
  }

  ngOnInit(): void { }

  viewPageHelper(pageHelper: PageHelper): void {
    this.commonService
      .getPageHelperText(pageHelper.code ?? '')
      .subscribe((help: PageHelper) => {
        this.dialog.open(PageHelpPreviewComponent, {
          width: '100%',
          maxWidth: '60vw',
          data: Object.assign({}, help),
        });
      });
  }

  managePageHelper(pageHelper: PageHelper) {
    this.router.navigate(['/page-helper/manage', pageHelper.id]);
  }
}
