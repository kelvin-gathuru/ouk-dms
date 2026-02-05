import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowLogListComponent } from './workflow-log-list.component';

describe('WorkflowLogListComponent', () => {
  let component: WorkflowLogListComponent;
  let fixture: ComponentFixture<WorkflowLogListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkflowLogListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowLogListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
