import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../../_services/regulatorio.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-regulatorio-inicio',
  templateUrl: './regulatorio-inicio.component.html',
  styleUrls: ['./regulatorio-inicio.component.css']
})
export class RegulatorioInicioComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }

  exportarResultadoExcel() {
    let blob;
    this.regulatorioService.getRegulatorioCorpoDocenteExcel().subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, 'CorpoDocente.xlsx');
    });
  }

  ngOnInit() {
  }

}
