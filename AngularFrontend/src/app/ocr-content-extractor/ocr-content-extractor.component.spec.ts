import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OcrContentExtractorComponent } from './ocr-content-extractor.component';

describe('OcrContentExtractorComponent', () => {
  let component: OcrContentExtractorComponent;
  let fixture: ComponentFixture<OcrContentExtractorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OcrContentExtractorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OcrContentExtractorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
