/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegulatorioProfessorIesComponent } from './regulatorio-professor-ies.component';

describe('RegulatorioProfessorIesComponent', () => {
  let component: RegulatorioProfessorIesComponent;
  let fixture: ComponentFixture<RegulatorioProfessorIesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulatorioProfessorIesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulatorioProfessorIesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
