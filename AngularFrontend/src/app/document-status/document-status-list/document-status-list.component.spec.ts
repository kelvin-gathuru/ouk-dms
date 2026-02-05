import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentStatusListComponent } from './document-status-list.component';

describe('DoucmentStatusListComponent', () => {
  let component: DocumentStatusListComponent;
  let fixture: ComponentFixture<DocumentStatusListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocumentStatusListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocumentStatusListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
