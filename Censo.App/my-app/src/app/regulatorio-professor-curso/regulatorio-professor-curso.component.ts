import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-regulatorio-professor-curso',
  templateUrl: './regulatorio-professor-curso.component.html',
  styleUrls: ['./regulatorio-professor-curso.component.css']
})
export class RegulatorioProfessorCursoComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }
  campi:any;
  curso:any[];
  dados: any;
  cursoFiltrado:any;
  resultado:any;
  p: any;
  errodados = false;
  selecione: any;

  
  ngOnInit() {
  this.getCampus();
  }

  
  getCampus() {
    this.regulatorioService.getCampus().subscribe(
      response => {
        this.dados = response;
        this.curso = this.dados.cursos;
        this.campi = this.dados.campi;

        //campus
        this.campi.sort(function (a,b) {
          if (a.nomCampus > b.nomCampus) {
            return 1;
          }
          if (a.nomCampus < b.nomCampus) {
            return -1;
          }
          return 0;
        });
        //console.log(this.dados.curso);
        
        //curso
        this.curso.sort(function (a,b) {
          if (a.nomCursoCenso > b.nomCursoCenso) {
            return 1;
          }
          if (a.nomCursoCenso < b.nomCursoCenso) {
            return -1;
          }
          return 0;
        });
        //console.log(this.dados.curso);
        //console.log(this.curso);
      },
      error => {
        console.log('curso' + error);
      }
    );
  } 


  




  exportarResultadoExcel(){
    
  }









 
  getCurso(valor: number){
   // console.log(this.curso);
    this.cursoFiltrado = this.curso.filter(c => c.codCampus == valor);
  }

  getResultado(valor: number){
    this.regulatorioService.getRegulatorioProfessorCurso(valor).subscribe(
      response => {
        this.errodados = false;
        this.resultado = response;
        //console.log(this.resultado);
        console.log(this.errodados);
      },
      error => {
        this.errodados = true;
        console.log(error);
      }
    );
  }
 

}
