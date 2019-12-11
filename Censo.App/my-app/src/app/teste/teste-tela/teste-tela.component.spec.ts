/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TesteTelaComponent } from './teste-tela.component';

describe('TesteTelaComponent', () => {
  let component: TesteTelaComponent;
  let fixture: ComponentFixture<TesteTelaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TesteTelaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TesteTelaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
