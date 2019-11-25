import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-regulatorio-professor-ies',
  templateUrl: './regulatorio-professor-ies.component.html',
  styleUrls: ['./regulatorio-professor-ies.component.css']
})
export class RegulatorioProfessorIesComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }
  resultado: any;
  resultadoId: any;
  codigo: any;
  ies: any;
  p: any;
  
  ngOnInit() {
   this.getIes();
    //this.buscaId(this.codIes);
  }

   //codInstituicao
   buscaId(codIes: string) {
    this.regulatorioService.getRegulatorioBuscaIes(codIes).subscribe(
      response => {
        this.resultadoId = response;
      },
      error => {
        console.log(error);
      }
    );
  }

  getIes() {
    this.regulatorioService.getIes().subscribe(
      response => {
        //ordenação com sort
        this.resultado = response;
        this.ies = this.resultado.ies;
        this.ies.sort(function (a,b) {
          if (a.nomIes > b.nomIes) {
            return 1;
          }
          if (a.nomIes < b.nomIes) {
            return -1;
          }
          return 0;
        });

        console.log(this.resultado.ies);
      },
      error => {
        console.log(error);
      }
    );
  } 

  exportarResultadoExcel(codies: any) {
    let blob;
    this.regulatorioService.getRegulatorioProfessorIesExcel(codies).subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, `Professor_IES_${codies}.xlsx`);
    }
    );
  }

}
