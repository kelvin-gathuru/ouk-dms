import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageStorageSettingComponent } from './manage-storage-setting.component';

describe('ManageStorageSettingComponent', () => {
  let component: ManageStorageSettingComponent;
  let fixture: ComponentFixture<ManageStorageSettingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageStorageSettingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageStorageSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
