import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppDadosCensoComponent } from './app-dados-censo.component';

describe('AppDadosCensoComponent', () => {
  let component: AppDadosCensoComponent;
  let fixture: ComponentFixture<AppDadosCensoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppDadosCensoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppDadosCensoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
