import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDocumentMetaTagComponent } from './manage-document-meta-tag.component';

describe('ManageDocumentMetaTagComponent', () => {
  let component: ManageDocumentMetaTagComponent;
  let fixture: ComponentFixture<ManageDocumentMetaTagComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageDocumentMetaTagComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageDocumentMetaTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
