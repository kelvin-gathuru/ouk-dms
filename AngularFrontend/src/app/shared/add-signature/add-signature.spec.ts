import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSignature } from './add-signature';

describe('AddSignature', () => {
  let component: AddSignature;
  let fixture: ComponentFixture<AddSignature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddSignature]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddSignature);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
