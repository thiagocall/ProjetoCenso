import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../../_services/regulatorio.service';
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
  ies: any;
  p: any;
  mostrarBusca = false;
  mostrarExcel = false;

  ngOnInit() {
    this.getIes();
    //this.buscaId(this.codIes);
  }

  getIes() {
    this.regulatorioService.getIes().subscribe(
      response => {
        // ordenação com sort
        this.resultado = response;
        //console.log(this.resultado)
        this.ies = this.resultado.ies;
        this.ies.sort((a, b) => {
          if (a.nomIes > b.nomIes) {
            return 1;
          }
          if (a.nomIes < b.nomIes) {
            return -1;
          }
          return 0;
        });

      },
      error => {
        // console.log(error);
      }
    );

  }

  // codInstituicao
  buscaId(codIes: string) {
    if (codIes === '-1') {
      return null;
    }
    this.mostrarBusca = true;
    //console.log(this.resultadoId); //dados professor e dados instituição
    this.regulatorioService.getRegulatorioBuscaIes(codIes).subscribe(
      response => {
        // console.log(response);
        this.resultadoId = [];
        this.resultadoId = response;
        this.resultadoId.sort((a, b) => {
          if (a.nomProfessor > b.nomProfessor) {
            return 1;
          }
          if (a.nomProfessor < b.nomProfessor) {
            return -1;
          }

          return 0;
        })
        this.mostrarBusca = false;
      },
      error => {
        // console.log(error);
      }
    );
  }

  /** exporta excel */
  exportarResultadoExcel(codies: any) {
    if (codies == '-1') {
      return null;
    }
    let blob;
    this.mostrarExcel = true;
    this.regulatorioService.getRegulatorioProfessorIesExcel(codies).subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      this.mostrarExcel = false;
      saveAs(blob, 'Professor_IES' + ((codies == 0) ? '' : '_' + codies) + '.xlsx');
    }
    );
  }

}

