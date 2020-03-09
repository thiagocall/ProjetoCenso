/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ManutencaoCursoEnadeComponent } from './manutencao-curso-enade.component';

describe('ManutencaoCursoEnadeComponent', () => {
  let component: ManutencaoCursoEnadeComponent;
  let fixture: ComponentFixture<ManutencaoCursoEnadeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManutencaoCursoEnadeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManutencaoCursoEnadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
