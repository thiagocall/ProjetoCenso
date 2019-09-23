import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppCorpoDocenteComponent } from './app-corpo-docente.component';

describe('AppCorpoDocenteComponent', () => {
  let component: AppCorpoDocenteComponent;
  let fixture: ComponentFixture<AppCorpoDocenteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppCorpoDocenteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppCorpoDocenteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
