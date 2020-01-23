/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ExpProfessorDadosSalvosComponent } from './exp-professor-dadosSalvos.component';

describe('ExpProfessorDadosSalvosComponent', () => {
  let component: ExpProfessorDadosSalvosComponent;
  let fixture: ComponentFixture<ExpProfessorDadosSalvosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpProfessorDadosSalvosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpProfessorDadosSalvosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
