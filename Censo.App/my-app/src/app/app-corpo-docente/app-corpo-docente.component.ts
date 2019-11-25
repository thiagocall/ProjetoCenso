import { Component, OnInit } from '@angular/core';
import {Location} from '@angular/common/';
import { HttpClientModule, HttpClient, HttpResponse } from '@angular/common/http';
import { resolveSanitizationFn } from '@angular/compiler/src/render3/view/template';
import { ProfessorService } from '../_services/professor.service';
// import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-app-corpo-docente',
  templateUrl: './app-corpo-docente.component.html',
  styleUrls: ['./app-corpo-docente.component.css']
})
export class AppCorpoDocenteComponent implements OnInit {

  constructor(private professorService: ProfessorService, private Loc: Location) { }
  resultado: any;
  listaCampus: any;
  listaCursos: any;
  curso: any;
  cod: any;
  codcurso: any;
  errodados = false;
  notaM;
  notaD;
  notaR;
  qtdM;
  qtdD;
  qtdR;
  professores: any;
  notaFaixa;
  pageOfItems: Array<any>;
  infoCurso: any;


  ngOnInit() {
    this.professorService.getDados().subscribe(
      response => {
      this.resultado = response;
      this.listaCampus = this.resultado.campus;
      this.listaCursos = this.resultado.cursos;
    },
    error => {
      console.log(error);
    }
    );
  }

  getCampus(codigo: string) {
    this.curso = this.listaCursos.filter(x => x.codCampus.toString() === codigo.toString());
    // this.curso = this.listaCursos;
  }

  getInfoCurso(codigo: any) {
    this.professores = null;
    this.notaM = null;
    this.notaD = null;
    this.notaR = null;
    this.notaFaixa = null;

    this.professorService.getInfoCurso(codigo).subscribe(
    response => {
      this.errodados = false;
      this.infoCurso = response;
      this.professores = this.infoCurso.cursoProfessor;
      this.notaM = this.infoCurso.notaM;
      this.notaD = this.infoCurso.notaD;
      this.notaR = this.infoCurso.notaR;
      this.qtdM = this.infoCurso.qtdM;
      this.qtdD = this.infoCurso.qtdD;
      this.qtdR = this.infoCurso.qtdR;

      this.notaFaixa = this.faixa();
      //console.log(response);

    },
    error => {
      this.errodados = true;
      this.infoCurso = null;
      console.log(error);
    }
    );

  }

  faixa(): number {
    const nota = (this.notaD * 2 + this.notaM + this.notaR) / 4;
    switch (true) {
      case nota < 0.945:
      return 1;
      case nota < 1.945:
      return 2;
      case nota < 2.945:
      return 3;
      case nota < 3.945:
      return 4;
      default:
      return 5;
      break;
    }
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }
}