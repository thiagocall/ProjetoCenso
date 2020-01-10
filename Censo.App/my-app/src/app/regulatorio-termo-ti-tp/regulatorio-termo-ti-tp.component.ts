import { Component, OnInit } from '@angular/core';
import { ExportAsService, ExportAsConfig } from 'ngx-export-as';

/*datepicker*/
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap';
import { ProfessorService } from '../_services/professor.service';
defineLocale('pt-br', ptBrLocale);


@Component({
  selector: 'app-regulatorio-termo-ti-tp',
  templateUrl: './regulatorio-termo-ti-tp.component.html',
  styleUrls: ['./regulatorio-termo-ti-tp.component.css']
})

export class RegulatorioTermoTiTpComponent implements OnInit {

  constructor(private exportAsService: ExportAsService, private localeService: BsLocaleService, private professorService: ProfessorService) {
    localeService.use('pt-br');
  }

  /*
  exportAsConfig: ExportAsConfig = {
    type: 'pdf', // the type you want to download
    elementId: 'pdf_View', // the id of html/table element
    options: { // html-docx-js document options
      top: 40,
      bottom: 60,
      left: 40,
      width: 1000,
    }
  }; */


 
exportAsConfig: ExportAsConfig = {
    type: 'pdf', // the type you want to download
    //elementId: 'balance-sheet-preview', // the id of html/table element
    elementId: 'pdf_View', // the id of html/table element
    options: { // html-docx-js document options
      margins: {
        top: '170',
        bottom: '5',
      },
      orientation: 'landscape',
      unit: 'in',
      format: [4, 2]
    }
  } 

  export(nome: string) {
    this.exportAsService.save(this.exportAsConfig, 'MQD - ' + nome).subscribe(() => {
    }); 
    
  } 

  ngOnInit() {
    this.pesquisaDocentesMqd();
  }
  
  p: any;
  campo: any;
  resultado: any;
  listaMatricula: any;
  listaDocente: any;
  value: any;
  matSec: any;

  /*pesquisaDocentesMqd*/
  cpfProfessor: any;
  nomProfessor: any;
  numMatricula: any;
  dtAdmissao: any;
  listaCarga: any;
  cpfCarga: any;
  cargaDs: any;
  cargaFs: any;
  cargaTotal: any;

  /*filtro lista */
  dadosFiltrados: any[];
  filtroProfessor: [];
  matriculasFiltradas: any[];
  resposta: any;

  ProfessorAdicionado: any;
  dataAdmissao: any;
  listaCargaHoraria: any;


  /*pesquisa com a lista completa ao recarregar a pagina */
  pesquisaDocentesMqd() {
    this.professorService.pesquisaDocente().subscribe(
      response => {
        this.resultado = response;

        /*lista docente*/
        this.listaDocente = this.resultado.listaDocente;
        this.cpfProfessor = this.resultado.listaDocente.cpfProfessor;
        this.nomProfessor = this.resultado.listaDocente.nomProfessor;

        /*lista matricula*/
        this.listaMatricula = this.resultado.listaMatricula;
        this.numMatricula = this.resultado.listaMatricula.numMatricula;
        this.dtAdmissao = this.resultado.listaMatricula.dtAdmissao;

        /*lista carga*/
        this.listaCarga = this.resultado.listaCarga;
      },
      error => {
      });
  }


  /* filtro input "pesquisar por cpf e nome" */
  filtrarItem(value: any) {
    if (value.length > 4) {
      this.dadosFiltrados = this.listaDocente;
      this.dadosFiltrados = this.dadosFiltrados.filter(x => x.cpfProfessor.search(value.toLocaleUpperCase()) !== -1 ||
        x.nomProfessor.search(value.toLocaleUpperCase()) !== -1).slice(0, 8); //slice "mostro só os 8 da lista"
    }
    else {
      this.dadosFiltrados = [];
    }
  }



  /* adiciono o professor no formulario */
  addProfessor(professor: any) {
    let prof = {
      cpfProfessor: professor.cpfProfessor,
      nomProfessor: professor.nomProfessor,
    }

    this.matriculasFiltradas = [];


    this.ProfessorAdicionado = prof; // {{ProfessorAdicionado.nomProfessor}} "mostro" o prof no front
    this.matriculasFiltradas = this.listaMatricula.filter(x => x.cpfProfessor == prof.cpfProfessor);
    this.listaCargaHoraria = this.listaCarga.filter(x => x.cpfCarga == prof.cpfProfessor)[0];
    console.log(this.listaCargaHoraria);
    this.cargaDs = this.listaCargaHoraria.cargaDs;
    this.cargaFs = this.listaCargaHoraria.cargaFs;
    this.cargaTotal = this.cargaDs + this.cargaFs;
    // this.dadosFiltrados = [];
  }



}










