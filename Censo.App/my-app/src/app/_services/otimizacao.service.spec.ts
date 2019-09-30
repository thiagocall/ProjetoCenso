/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { OtimizacaoService } from './otimizacao.service';

describe('Service: Otimizacao', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OtimizacaoService]
    });
  });

  it('should ...', inject([OtimizacaoService], (service: OtimizacaoService) => {
    expect(service).toBeTruthy();
  }));
});
