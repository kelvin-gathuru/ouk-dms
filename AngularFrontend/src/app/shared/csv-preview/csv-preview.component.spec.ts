import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CSVPreviewComponent } from './csv-preview.component';

describe('CSVPreviewComponent', () => {
  let component: CSVPreviewComponent;
  let fixture: ComponentFixture<CSVPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CSVPreviewComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CSVPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
