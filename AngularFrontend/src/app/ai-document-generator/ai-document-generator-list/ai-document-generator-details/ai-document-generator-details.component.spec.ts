import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AiDocumentGeneratorDetailsComponent } from './ai-document-generator-details.component';

describe('AiDocumentGeneratorDetailsComponent', () => {
  let component: AiDocumentGeneratorDetailsComponent;
  let fixture: ComponentFixture<AiDocumentGeneratorDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AiDocumentGeneratorDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AiDocumentGeneratorDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
