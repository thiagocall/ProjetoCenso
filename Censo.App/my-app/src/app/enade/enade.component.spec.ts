/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { EnadeComponent } from './enade.component';

describe('EnadeComponent', () => {
  let component: EnadeComponent;
  let fixture: ComponentFixture<EnadeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EnadeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EnadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
