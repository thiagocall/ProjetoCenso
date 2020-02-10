import { Component, OnInit } from '@angular/core';
import { ExportacaoService } from "../_services/exportacao.service";
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-exportacao',
  templateUrl: './exportacao.component.html',
  styleUrls: ['./exportacao.component.css']
})

export class ExportacaoComponent implements OnInit {

  constructor(private exportacaoService: ExportacaoService) { }

  exportacaoCensoExcel() {
    let blob;
    this.exportacaoService.getExportarCensoExcel().subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, 'CorpoDocente.xlsx');
    });
  }

  ngOnInit() {
  }

}
