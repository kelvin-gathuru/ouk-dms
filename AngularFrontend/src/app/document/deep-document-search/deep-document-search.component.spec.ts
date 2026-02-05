import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeepDocumentSearchComponent } from './deep-document-search.component';

describe('DeepDocumentSearchComponent', () => {
  let component: DeepDocumentSearchComponent;
  let fixture: ComponentFixture<DeepDocumentSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeepDocumentSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeepDocumentSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
