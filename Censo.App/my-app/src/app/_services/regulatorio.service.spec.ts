/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RegulatorioService } from './regulatorio.service';

describe('Service: Regulatorio', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RegulatorioService]
    });
  });

  it('should ...', inject([RegulatorioService], (service: RegulatorioService) => {
    expect(service).toBeTruthy();
  }));
});
