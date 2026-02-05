import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharePermissionComponent } from './share-permission.component';

describe('SharePermissionComponent', () => {
  let component: SharePermissionComponent;
  let fixture: ComponentFixture<SharePermissionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SharePermissionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharePermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
