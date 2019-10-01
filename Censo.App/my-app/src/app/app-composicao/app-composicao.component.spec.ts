/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AppComposicaoComponent } from './app-composicao.component';

describe('AppComposicaoComponent', () => {
  let component: AppComposicaoComponent;
  let fixture: ComponentFixture<AppComposicaoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppComposicaoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComposicaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
