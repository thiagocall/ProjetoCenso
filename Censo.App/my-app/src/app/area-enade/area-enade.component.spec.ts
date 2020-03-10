/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AreaEnadeComponent } from './area-enade.component';

describe('AreaEnadeComponent', () => {
  let component: AreaEnadeComponent;
  let fixture: ComponentFixture<AreaEnadeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AreaEnadeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AreaEnadeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
