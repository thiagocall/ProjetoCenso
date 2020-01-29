/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ExportacaoService } from './exportacao.service';

describe('Service: Exportacao', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExportacaoService]
    });
  });

  it('should ...', inject([ExportacaoService], (service: ExportacaoService) => {
    expect(service).toBeTruthy();
  }));
});
