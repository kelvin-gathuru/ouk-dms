import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AiDocumentGeneratorPreviewComponent } from './ai-document-generator-preview.component';

describe('AppAiDocumentGeneratorPreviewComponent', () => {
  let component: AiDocumentGeneratorPreviewComponent;
  let fixture: ComponentFixture<AiDocumentGeneratorPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AiDocumentGeneratorPreviewComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AiDocumentGeneratorPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
