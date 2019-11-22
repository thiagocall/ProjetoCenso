/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegulatorioProfessorForaSedeComponent } from './regulatorio-professor-fora-sede.component';

describe('RegulatorioProfessorForaSedeComponent', () => {
  let component: RegulatorioProfessorForaSedeComponent;
  let fixture: ComponentFixture<RegulatorioProfessorForaSedeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulatorioProfessorForaSedeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulatorioProfessorForaSedeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
