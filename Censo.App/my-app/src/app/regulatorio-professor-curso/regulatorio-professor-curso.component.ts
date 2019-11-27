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

  
  ngOnInit() {
  this.getCampus();
  }

  
  getCampus() {
    this.regulatorioService.getCampus().subscribe(
      response => {
        this.dados = response;
        this.curso = this.dados.cursos;
        //console.log(this.curso);
        this.campi = this.dados.campi;
      },
      error => {
        console.log(error);
      }
    );
  } 

 
  getCurso(valor: number){
    console.log(this.curso);
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
