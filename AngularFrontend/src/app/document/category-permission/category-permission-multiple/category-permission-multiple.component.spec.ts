import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryPermissionMultipleComponent } from './category-permission-multiple.component';

describe('CategoryPermissionMultipleComponent', () => {
  let component: CategoryPermissionMultipleComponent;
  let fixture: ComponentFixture<CategoryPermissionMultipleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryPermissionMultipleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryPermissionMultipleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
