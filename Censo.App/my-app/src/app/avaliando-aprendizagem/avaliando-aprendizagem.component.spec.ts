/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AvaliandoAprendizagemComponent } from './avaliando-aprendizagem.component';

describe('AvaliandoAprendizagemComponent', () => {
  let component: AvaliandoAprendizagemComponent;
  let fixture: ComponentFixture<AvaliandoAprendizagemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvaliandoAprendizagemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvaliandoAprendizagemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
