import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { analyzeAndValidateNgModules } from '@angular/compiler';
import { RegistrationComponent } from '../user/registration/registration.component';


@Component({
  selector: 'app-regulatorio-gap-carga-horaria',
  templateUrl: './regulatorio-gap-carga-horaria.component.html',
  styleUrls: ['./regulatorio-gap-carga-horaria.component.css']
})

export class RegulatorioGapCargaHorariaComponent implements OnInit {

  constructor(private regulatorio: RegulatorioService) { }

  p: any;
  resultado: any;
 

  /*filtro lista*/
  dadosFiltrados: any[];
  filtroProfessor: [];
  resposta: any;

  /*selecao tabela */
  listaProfessorAdicionado: Array<any> = new Array<any>();

  /*calcula carga horaria gap */
  professor: professor = new professor();

  /* input target */
  selecionado = true;
  TEMPO_INTEGRAL = false;
  TEMPO_PARCIAL = false;
  professorTarget: any[];

  /* filtro input */
  filtrarItem(value: any) {
    if (value.length > 4) {
      this.dadosFiltrados = this.resultado;
      this.dadosFiltrados = this.dadosFiltrados.filter(x => x.cpfProfessor.search(value.toLocaleUpperCase()) !== -1 ||
        x.titulacao.search(value.toLocaleUpperCase()) !== -1 ||
        x.nomProfessor.search(value.toLocaleUpperCase()) !== -1 ||
        x.regime.search(value.toLocaleUpperCase()) !== -1);
    }
    else {
      this.dadosFiltrados = [];
    }
  }

  /*pesquisa com a lista completa ao recarregar a pagina */
  getProfessores() {
    this.regulatorio.PesquisaProfessores().subscribe(
      response => {
        this.resultado = response;
        //console.log(this.resultado);
      },
      error => {
      });
  }

  /*adicionar professores na tabela */
  addProfessor(professor: any) {

    let prof = {
      cpfProfessor: professor.cpfProfessor,
      nomProfessor: professor.nomProfessor,
      qtdHorasDs: professor.qtdHorasDs,
      qtdHorasFs: professor.qtdHorasFs,
      regime: professor.regime,
      titulacao: professor.titulacao,
      target: ""
    }

    //if (this.listaProfessorAdicionado.filter(x => x.cpf == professor.cpfProfessor).length == 0) {
    if (this.listaProfessorAdicionado
      .filter(x => x.cpfProfessor == prof.cpfProfessor)
      .length == 0) {
      this.listaProfessorAdicionado.push(prof);
      //console.log(prof);
      //console.log(this.listaProfessorAdicionado);
    } else {
      return;
    }
  }

  /*remover professores na tabela */
  removerProfessor(professor: any) {
    this.listaProfessorAdicionado.splice(this.listaProfessorAdicionado.indexOf(professor), 1);
    // console.log(this.listaProfessorAdicionado);
  }

  CalculaCargaHoraria() {
    //console.log(this.listaProfessorAdicionado);
    var listaProfessor: any;
    let listaProfessorResposta: any;

    /** Map transformando em objeto */
    listaProfessor = this.listaProfessorAdicionado.map(x =>
      ({
        cpf: x.cpfProfessor,
        ds: x.qtdHorasDs,
        fs: x.qtdHorasFs,
        target: x.target,
      }))
    // console.log(listaProfessor); // sem complemento

    this.regulatorio.postCalculaGapProf(listaProfessor).subscribe(
      response => {
        // console.log(response); //complemento

        listaProfessorResposta = response;
        var resposta = listaProfessorResposta.map(x =>
          ({
            cpfProfessor: x.cpf,
            nomProfessor: this.listaProfessorAdicionado.find(p => p.cpfProfessor == x.cpf).nomProfessor,
            qtdHorasDs: x.ds,
            qtdHorasFs: x.fs,
            regime: this.listaProfessorAdicionado.find(p => p.cpfProfessor == x.cpf).regime,
            titulacao: this.listaProfessorAdicionado.find(p => p.cpfProfessor == x.cpf).titulacao,
            complemento: x.complemento,
            target: this.listaProfessorAdicionado.find(p => p.cpfProfessor == x.cpf).target
          }));

        //console.log(resposta);
        this.resposta = resposta; //com todos os dados + complemento
        //console.log(listaProfessorResposta) //não tem nome do prof e tem complemento
      },
      error => {
        console.log(error);
      });
  }

  /* selecao input */
  selecaoPadrao(value: string, professor: any): boolean {
     console.log(value);
     let indicacao = (professor.regime == "TEMPO INTEGRAL") ? "TEMPO INTEGRAL" : 
     (professor.regime == "TEMPO PARCIAL") ? "TEMPO INTEGRAL" : "TEMPO PARCIAL";

    if (indicacao == value) {
      return true;
    } else {
      return false;
    }
  }

  limparLista() {
    this.listaProfessorAdicionado = [];
    this.resposta = [];
  }


  ngOnInit() {
    this.getProfessores();
  }
}

class professor {
  constructor() { }
  nome;
  cpf;
  ds;
  fs;
  target;
}