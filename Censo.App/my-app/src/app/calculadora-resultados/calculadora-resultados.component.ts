import { Component, OnInit } from '@angular/core';
import { ProfessorService } from '../_services/professor.service';

@Component({
  selector: 'app-calculadora-resultados',
  templateUrl: './calculadora-resultados.component.html',
  styleUrls: ['./calculadora-resultados.component.css']
})
export class CalculadoraResultadosComponent implements OnInit {

  constructor(private professorService: ProfessorService) { }
  resultado: any;
  listaCampus: any;
  listaCursos: any;
  curso: any;
  errodados = false;
  professores: any;
  infoCurso: any;
  resultados: any;

  ngOnInit() {
    this.professorService.getDados().subscribe(
      response => {
        this.resultado = response;
        this.listaCampus = this.resultado.campus;
        this.listaCursos = this.resultado.cursos;
      },
      error => {
        console.log(error);
      });
  }

  getCampus(codigo: string) {
    this.curso = this.listaCursos.filter(x => x.codCampus.toString() === codigo.toString());
    // this.curso = this.listaCursos;
  }

  getInfoCurso(codigo: any) {
    this.professores = null;
    this.professorService.getInfoCurso(codigo).subscribe(
      response => {
        this.errodados = false;
        this.infoCurso = response;
        this.professores = this.infoCurso.cursoProfessor;
        //console.log(response);
      },
      error => {
        this.errodados = true;
        this.infoCurso = null;
        console.log(error);
      });
  }

  resetar() {
  }

  valorInput() {
    var input: any;
    input = document.getElementById('doutorUm');
    input.value = input;
    console.log(input);
    //document.getElementById('doutorUm').value;
  }

  getCalcularResultado() {
    this.professorService.getCalculadoraResultado().subscribe(
      response => {
        this.resultados = response;
      },
      error => {
        console.log(error);
      });
  }







}
