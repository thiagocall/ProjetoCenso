import { Component, OnInit } from '@angular/core';
import { ProfessorService } from '../_services/professor.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-calculadora-resultados',
  templateUrl: './calculadora-resultados.component.html',
  styleUrls: ['./calculadora-resultados.component.css']
})
export class CalculadoraResultadosComponent implements OnInit {

  constructor(private professorService: ProfessorService , private route: ActivatedRoute) { }
  resultado: any;
  listaCampus: any;
  listaCursos: any;
  curso: any;
  errodados = false;
  professores: any;
  infoCurso: any;
  resultados: any;
  cod:any;
  codcurso: any;

  notaContinua: any;



  /* biding com tela da calculadora*/
  // qtdDR: any;
  // qtdDH: any;
  // qtdMR: any;
  // qtdMH: any;
  // qtdER: any;
  // qtdEH: any;
  // qtd: any;
  // nota_Doutor: any;
  // nota_Mestre: any;
  // nota_Regime: any;


  dados = new dados();

  calcula = new calcula();
  calculaAnt = new calcula();

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

  CalcularResultado() {

    this.dados._idEmec = this.codcurso;
    this.dados._idResultado = this.route.snapshot.paramMap.get('id');
    // console.log(this.dados);
    this.professorService.postCalculadoraResultado(this.dados).subscribe(
      response => {
        this.calcula = Object.assign(response);
        // this.resultados = response;
        console.log(this.calcula);
      },
      error => {
        console.log(error);
      });
  }

  Recalcula(valor:number){
    console.log(this.calcula);
    this.calculaAnt = Object.assign(this.calcula);
    this.calcula.qtdDR = valor;
    this.calculaNotaContinua();

  }


  calculaNotaContinua() {

    let percDoutorAnt = (this.calculaAnt.qtdDH + this.calculaAnt.qtdDR) / this.calculaAnt.qtd;
    let percDoutorNovo = (this.calcula.qtdDH + this.calcula.qtdDR) / this.calcula.qtd;
    let notaDoutorNova = this.calculaAnt.nota_Doutor * percDoutorNovo / percDoutorAnt;

    let percMestreAnt = (this.calculaAnt.qtdMH + this.calculaAnt.qtdMR + this.calculaAnt.qtdDH + this.calculaAnt.qtdDR) / this.calculaAnt.qtd;
    let percMestreNovo = (this.calcula.qtdMH + this.calcula.qtdMR + this.calcula.qtdDH + this.calcula.qtdDR) / this.calcula.qtd;
    let notaMestreNova = this.calculaAnt.nota_Mestre * percMestreNovo / percMestreAnt;

    let percRegimeAnt = (this.calculaAnt.qtdMR + this.calculaAnt.qtdDR + this.calculaAnt.qtdER) / this.calculaAnt.qtd;
    let percRegimeNovo = (this.calcula.qtdMR + this.calcula.qtdDR + this.calcula.qtdER) / this.calcula.qtd;
    let notaRegimeNova = this.calculaAnt.nota_Regime * percRegimeNovo / percRegimeAnt;

    //console.log(this.calculaAnt);


    this.notaContinua = notaDoutorNova * 0.5 + notaMestreNova * 0.25 + notaRegimeNova * 0.25;

  }

  


}

class dados {
  constructor() { }
  _idEmec: number;
  _idResultado: string;
}

class calcula {

  constructor() {}

  qtdDR: any;
  qtdDH: any;
  qtdMR: any;
  qtdMH: any;
  qtdER: any;
  qtdEH: any;
  qtd: any;
  nota_Doutor: any;
  nota_Mestre: any;
  nota_Regime: any;

}





