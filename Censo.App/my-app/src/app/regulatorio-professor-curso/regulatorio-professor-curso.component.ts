import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';

@Component({
  selector: 'app-regulatorio-professor-curso',
  templateUrl: './regulatorio-professor-curso.component.html',
  styleUrls: ['./regulatorio-professor-curso.component.css']
})
export class RegulatorioProfessorCursoComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }
  resultado: any;
  resultadoId: any;
  codigo: any;
  campus: any;  //ies = campus // codIes codcampus
  p: any;
  
  ngOnInit() {
   //this.getCampus(); 
  }

   //codInstituicao
   buscaId(codCampus: string) {
    this.regulatorioService.getRegulatorioProfessorCurso(codCampus).subscribe(
      response => {
        this.resultadoId = response;
      },
      error => {
        console.log(error);
      }
    );
  }

  /*
  getCampus() {
    this.regulatorioService.getCampus().subscribe(
      response => {
        //ordenação com sort
        this.resultado = response;
        this.campus = this.resultado.ies;
        this.campus.sort(function (a,b) {
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
  } */


 

}
