import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentMetaTagListComponent } from './document-meta-tag-list.component';

describe('DocumentMetaTagListComponent', () => {
  let component: DocumentMetaTagListComponent;
  let fixture: ComponentFixture<DocumentMetaTagListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocumentMetaTagListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DocumentMetaTagListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
