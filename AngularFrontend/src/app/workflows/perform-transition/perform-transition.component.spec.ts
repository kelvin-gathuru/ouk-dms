import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformTransitionComponent } from './perform-transition.component';

describe('PerformTransitionComponent', () => {
  let component: PerformTransitionComponent;
  let fixture: ComponentFixture<PerformTransitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PerformTransitionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PerformTransitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
