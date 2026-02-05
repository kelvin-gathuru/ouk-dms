import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchiveDocumentsComponent } from './archive-documents.component';

describe('ArchiveDocumentsComponent', () => {
  let component: ArchiveDocumentsComponent;
  let fixture: ComponentFixture<ArchiveDocumentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArchiveDocumentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArchiveDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
