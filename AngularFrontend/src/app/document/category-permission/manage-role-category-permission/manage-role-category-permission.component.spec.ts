import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageRoleCategoryPermissionComponent } from './manage-role-category-permission.component';

describe('ManageRoleCategoryPermissionComponent', () => {
  let component: ManageRoleCategoryPermissionComponent;
  let fixture: ComponentFixture<ManageRoleCategoryPermissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageRoleCategoryPermissionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageRoleCategoryPermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
