/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { EnadeService } from './enade.service';

describe('Service: Enade', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EnadeService]
    });
  });

  it('should ...', inject([EnadeService], (service: EnadeService) => {
    expect(service).toBeTruthy();
  }));
});
