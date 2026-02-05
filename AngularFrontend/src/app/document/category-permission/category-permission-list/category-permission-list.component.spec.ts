import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryPermissionListComponent } from './category-permission-list.component';

describe('CategoryPermissionListComponent', () => {
  let component: CategoryPermissionListComponent;
  let fixture: ComponentFixture<CategoryPermissionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryPermissionListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryPermissionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
