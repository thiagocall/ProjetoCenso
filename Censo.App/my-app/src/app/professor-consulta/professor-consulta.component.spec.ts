import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfessorConsultaComponent } from './professor-consulta.component';

describe('ProfessorConsultaComponent', () => {
  let component: ProfessorConsultaComponent;
  let fixture: ComponentFixture<ProfessorConsultaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfessorConsultaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfessorConsultaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
