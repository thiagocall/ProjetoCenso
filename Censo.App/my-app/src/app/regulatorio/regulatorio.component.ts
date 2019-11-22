import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-regulatorio',
  templateUrl: './regulatorio.component.html',
  styleUrls: ['./regulatorio.component.css']
})
export class RegulatorioComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }

  ngOnInit() {
    
  }

  exportarResultadoExcel() {
    let blob;
    this.regulatorioService.getRegulatorioCorpoDocenteExcel().subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, 'CorpoDocente.xlsx');
    }
    );
  }

}
