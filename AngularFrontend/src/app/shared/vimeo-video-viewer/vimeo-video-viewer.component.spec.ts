import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VimeoVideoViewerComponent } from './vimeo-video-viewer.component';

describe('VimeoVideoViewerComponent', () => {
  let component: VimeoVideoViewerComponent;
  let fixture: ComponentFixture<VimeoVideoViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VimeoVideoViewerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VimeoVideoViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
