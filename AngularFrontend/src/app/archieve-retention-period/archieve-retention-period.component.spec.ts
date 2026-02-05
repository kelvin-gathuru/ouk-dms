import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchieveRetentionPeriodComponent } from './archieve-retention-period.component';

describe('ArchieveRetentionPeriodComponent', () => {
  let component: ArchieveRetentionPeriodComponent;
  let fixture: ComponentFixture<ArchieveRetentionPeriodComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArchieveRetentionPeriodComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArchieveRetentionPeriodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
