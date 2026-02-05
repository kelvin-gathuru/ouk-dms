import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YoutubeVideoViewerComponent } from './youtube-video-viewer.component';

describe('YoutubeVideoViewerComponent', () => {
  let component: YoutubeVideoViewerComponent;
  let fixture: ComponentFixture<YoutubeVideoViewerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YoutubeVideoViewerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YoutubeVideoViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
