import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageUserCategoryPermissionComponent } from './manage-user-category-permission.component';

describe('ManageUserCategoryPermissionComponent', () => {
  let component: ManageUserCategoryPermissionComponent;
  let fixture: ComponentFixture<ManageUserCategoryPermissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageUserCategoryPermissionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageUserCategoryPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
