import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageWorkflowInstanceComponent } from './manage-workflow-instance.component';

describe('ManageWorkflowInstanceComponent', () => {
  let component: ManageWorkflowInstanceComponent;
  let fixture: ComponentFixture<ManageWorkflowInstanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageWorkflowInstanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageWorkflowInstanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
