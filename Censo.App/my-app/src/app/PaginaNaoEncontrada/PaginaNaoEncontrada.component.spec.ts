/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PaginaNaoEncontradaComponent } from './PaginaNaoEncontrada.component';

describe('PaginaNaoEncontradaComponent', () => {
  let component: PaginaNaoEncontradaComponent;
  let fixture: ComponentFixture<PaginaNaoEncontradaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaginaNaoEncontradaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaginaNaoEncontradaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
