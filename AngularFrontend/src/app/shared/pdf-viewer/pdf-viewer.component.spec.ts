import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PdfDataViewerComponent } from './pdf-viewer.component';

describe('PdfViewerComponent', () => {
  let component: PdfDataViewerComponent;
  let fixture: ComponentFixture<PdfDataViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PdfDataViewerComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PdfDataViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
