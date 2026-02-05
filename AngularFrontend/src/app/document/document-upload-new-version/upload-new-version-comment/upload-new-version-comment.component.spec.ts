import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadNewVersionCommentComponent } from './upload-new-version-comment.component';

describe('UploadNewVersionCommentComponent', () => {
  let component: UploadNewVersionCommentComponent;
  let fixture: ComponentFixture<UploadNewVersionCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadNewVersionCommentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadNewVersionCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
