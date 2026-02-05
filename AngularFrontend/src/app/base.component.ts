import { Direction } from '@angular/cdk/bidi';
import { Component, Directive, inject, OnDestroy } from '@angular/core';
import { TranslationService } from '@core/services/translation.service';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-base',
  template: ``,
  standalone: false
})
export class BaseComponent implements OnDestroy {
  sub$: SubSink;
  langDir: Direction = 'ltr';
  translationService = inject(TranslationService);
  constructor() {
    this.sub$ = new SubSink();
    this.getLangDir();
  }

  getLangDir() {
    this.sub$.sink = this.translationService.lanDir$.subscribe(
      (c: Direction) => {
        this.langDir = c;
      }
    );
  }
  ngOnDestroy(): void {
    this.sub$.unsubscribe();
  }
}
