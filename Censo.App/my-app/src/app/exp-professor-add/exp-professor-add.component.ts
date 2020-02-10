import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { ExportacaoService } from "../_services/exportacao.service";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-exp-professor-add',
  templateUrl: './exp-professor-add.component.html',
  styleUrls: ['./exp-professor-add.component.css']
})

export class ExpProfessorAddComponent implements OnInit {

  constructor(private regulatorio: RegulatorioService, private exportacao: ExportacaoService, private toast: ToastrService) { }

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
  professorTarget: any[];

  /* filtro input */
  filtrarItem(value: any) {
    if (value.length > 4) {
      this.dadosFiltrados = this.resultado;
      this.dadosFiltrados = this.dadosFiltrados.filter(x => x.cpfProfessor.search(value.toLocaleUpperCase()) !== -1 ||
        x.titulacao.search(value.toLocaleUpperCase()) !== -1 ||
        x.nomProfessor.search(value.toLocaleUpperCase()) !== -1 ||
        x.regime.search(value.toLocaleUpperCase()) !== -1).slice(0, 5); // os 5 primeiros da lista;
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

    if (this.listaProfessorAdicionado
      .filter(x => x.cpfProfessor == prof.cpfProfessor)
      .length == 0) {
      this.listaProfessorAdicionado.push(prof);
    } else {
      return;
    }
  }

  /*remover professores na tabela */
  removerProfessor(professor: any) {
    this.listaProfessorAdicionado.splice(this.listaProfessorAdicionado.indexOf(professor), 1);
    //console.log(this.listaProfessorAdicionado);
  }


   //dados salvos na tabela "DOM" 
   /*dadosProfessor() {
    this.resposta = this.listaProfessorAdicionado;
    //console.log(this.resposta); 
  } */


  

  //"POST -- TESTE"
  salvarDadosprofessor() {
    this.exportacao.exportacaoProfessor(this.listaProfessorAdicionado).subscribe(
      response => { //post response ok ou error
        console.log(this.listaProfessorAdicionado)
        this.toast.warning('Não foi possível salvar ou o professor já foi adicionado!', null, {
          timeOut: 1000
        });
      },
      error => {
        console.log(this.listaProfessorAdicionado)
        this.toast.success('Professor Adicionado com Sucesso!', null, {
          timeOut: 1000
        });
      })
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