import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReminderDetailComponent } from './reminder-detail.component';

describe('ReminderDetailComponent', () => {
  let component: ReminderDetailComponent;
  let fixture: ComponentFixture<ReminderDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReminderDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReminderDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
