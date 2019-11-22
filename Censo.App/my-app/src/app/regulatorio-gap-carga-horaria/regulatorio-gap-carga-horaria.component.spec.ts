/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegulatorioGapCargaHorariaComponent } from './regulatorio-gap-carga-horaria.component';

describe('RegulatorioGapCargaHorariaComponent', () => {
  let component: RegulatorioGapCargaHorariaComponent;
  let fixture: ComponentFixture<RegulatorioGapCargaHorariaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulatorioGapCargaHorariaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulatorioGapCargaHorariaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
