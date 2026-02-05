import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorageSettingListComponent } from './storage-setting-list.component';

describe('StorageSettingListComponent', () => {
  let component: StorageSettingListComponent;
  let fixture: ComponentFixture<StorageSettingListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StorageSettingListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StorageSettingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
