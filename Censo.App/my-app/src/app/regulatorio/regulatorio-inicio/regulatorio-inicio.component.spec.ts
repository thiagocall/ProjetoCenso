/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegulatorioInicioComponent } from './regulatorio-inicio.component';

describe('RegulatorioInicioComponent', () => {
  let component: RegulatorioInicioComponent;
  let fixture: ComponentFixture<RegulatorioInicioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulatorioInicioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulatorioInicioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
