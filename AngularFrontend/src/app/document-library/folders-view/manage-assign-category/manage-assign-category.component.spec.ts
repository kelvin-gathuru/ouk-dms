import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageAssignCategoryComponent } from './manage-assign-category.component';

describe('ManageAssignCategoryComponent', () => {
  let component: ManageAssignCategoryComponent;
  let fixture: ComponentFixture<ManageAssignCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManageAssignCategoryComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageAssignCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
