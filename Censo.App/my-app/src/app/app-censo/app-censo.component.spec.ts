import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppCensoComponent } from './app-censo.component';

describe('AppCensoComponent', () => {
  let component: AppCensoComponent;
  let fixture: ComponentFixture<AppCensoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppCensoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppCensoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
