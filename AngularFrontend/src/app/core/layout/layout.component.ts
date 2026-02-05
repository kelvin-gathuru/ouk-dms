import {
  AfterViewInit,
  Component,
  Inject,
  Renderer2,
  DOCUMENT
} from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { RouterModule } from '@angular/router';
import { FooterComponent } from '@core/footer/footer.component';
import { HeaderComponent } from '@core/header/header.component';
import { InConfiguration } from '@core/services/config.interface';
import { TranslationService } from '@core/services/translation.service';
import { SidebarComponent } from '@core/sidebar/sidebar.component';
import { LoadingIndicatorComponent } from '@shared/loading-indicator/loading-indicator.component';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  standalone: true,
  imports: [
    LoadingIndicatorComponent,
    HeaderComponent,
    SidebarComponent,
    RouterModule,
    FooterComponent,
    MatDialogModule
  ],
})
export class LayoutComponent implements AfterViewInit {
  direction!: string;
  public config!: InConfiguration;
  constructor(
    private translationService: TranslationService,
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
  ) {
    this.getLangDir();
  }

  getLangDir() {
    this.translationService.lanDir$.subscribe(
      (c: string) => {
        this.direction = c;
        if (this.direction == 'rtl') {
          this.setRTLSettings();
        } else {
          this.setLTRSettings();
        }
      }
    );
  }
  ngAfterViewInit(): void {

    if (localStorage.getItem('isRtl')) {
      if (localStorage.getItem('isRtl') === 'true') {
        this.setRTLSettings();
      } else if (localStorage.getItem('isRtl') === 'false') {
        this.setLTRSettings();
      }
    } else {
      if (this.config?.layout.rtl == true) {
        this.setRTLSettings();
      } else {
        this.setLTRSettings();
      }
    }

    if (localStorage.getItem('collapsed_menu')) {
      if (localStorage.getItem('collapsed_menu') === 'true') {
        this.renderer.addClass(this.document.body, 'sidebar-closed');
        this.renderer.addClass(this.document.body, 'sidebarsubmenu-closed');
      }
    } else {
      if (this.config?.layout?.sidebar?.collapsed == true) {
        this.renderer.addClass(this.document.body, 'sidebar-closed');
        this.renderer.addClass(this.document.body, 'sidebarsubmenu-closed');
        localStorage.setItem('collapsed_menu', 'true');
      } else {
        this.renderer.removeClass(this.document.body, 'sidebar-closed');
        this.renderer.removeClass(this.document.body, 'sidebarsubmenu-closed');
        localStorage.setItem('collapsed_menu', 'false');
      }
    }
  }

  setRTLSettings() {
    document.getElementsByTagName('html')[0].setAttribute('dir', 'rtl');
    this.renderer.addClass(this.document.body, 'rtl');

    localStorage.setItem('isRtl', 'true');
  }
  setLTRSettings() {
    document.getElementsByTagName('html')[0].removeAttribute('dir');
    this.renderer.removeClass(this.document.body, 'rtl');

    localStorage.setItem('isRtl', 'false');
  }



}
