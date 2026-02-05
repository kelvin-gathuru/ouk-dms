import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestDocumentThroughWorkflowComponent } from './request-document-through-workflow.component';

describe('RequestDocumentThroughWorkflowComponent', () => {
  let component: RequestDocumentThroughWorkflowComponent;
  let fixture: ComponentFixture<RequestDocumentThroughWorkflowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RequestDocumentThroughWorkflowComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequestDocumentThroughWorkflowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
