/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegulatorioTermoTiTpComponent } from './regulatorio-termo-ti-tp.component';

describe('RegulatorioTermoTiTpComponent', () => {
  let component: RegulatorioTermoTiTpComponent;
  let fixture: ComponentFixture<RegulatorioTermoTiTpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulatorioTermoTiTpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulatorioTermoTiTpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
