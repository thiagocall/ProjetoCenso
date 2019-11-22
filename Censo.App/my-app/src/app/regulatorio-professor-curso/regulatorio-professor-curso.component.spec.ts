/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegulatorioProfessorCursoComponent } from './regulatorio-professor-curso.component';

describe('RegulatorioProfessorCursoComponent', () => {
  let component: RegulatorioProfessorCursoComponent;
  let fixture: ComponentFixture<RegulatorioProfessorCursoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulatorioProfessorCursoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulatorioProfessorCursoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
