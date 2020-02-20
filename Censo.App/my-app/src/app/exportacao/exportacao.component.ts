import { Component, OnInit } from '@angular/core';
import { ExportacaoService } from '../_services/exportacao.service';
import { saveAs } from 'file-saver';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-exportacao',
  templateUrl: './exportacao.component.html',
  styleUrls: ['./exportacao.component.css']
})

export class ExportacaoComponent implements OnInit {

  titulo = 'Longin'
  mostrarSpinner = false;

  constructor(private exportacaoService: ExportacaoService, private datePipe: DatePipe) { }

  exportacaoCensoExcel() {
    this.mostrarSpinner = true;
    let blob;
    const data = this.datePipe.transform(Date(), 'yyyy-MM-dd');
    this.exportacaoService.getExportarCensoExcel().subscribe(
      response => {
        this.mostrarSpinner = false;
        blob = new Blob([response], { type: 'application/octet-stream' });
        saveAs(blob, `Arquivo_Censo_Oficial_${data}.xlsx`);
      });
  }

  ngOnInit() {

  }

}
