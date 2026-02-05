import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchiveFoldersComponent } from './archive-folders.component';

describe('ArchiveFoldersComponent', () => {
  let component: ArchiveFoldersComponent;
  let fixture: ComponentFixture<ArchiveFoldersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ArchiveFoldersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ArchiveFoldersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
