import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { analyzeAndValidateNgModules } from '@angular/compiler';


@Component({
  selector: 'app-regulatorio-gap-carga-horaria',
  templateUrl: './regulatorio-gap-carga-horaria.component.html',
  styleUrls: ['./regulatorio-gap-carga-horaria.component.css']
})

export class RegulatorioGapCargaHorariaComponent implements OnInit {

  constructor(private regulatorio: RegulatorioService) {


   }

  p: any;
  resultado: any;

  /*filtro lista*/
  dadosFiltrados: any[];
  filtroProfessor: [];

  /*selecao tabela */
  listaProfessorAdicionado: Array<any> = new Array<any>();


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
  addProfessor(professor : any) {

    if (this.listaProfessorAdicionado.indexOf(professor) == -1 ) {
        this.listaProfessorAdicionado.push(professor);
        console.log(this.listaProfessorAdicionado);  
    } else {
      return;
    }
  }


  /*remover professores na tabela */
  removerProfessor(professor : any) {

    this.listaProfessorAdicionado.splice(this.listaProfessorAdicionado.indexOf(professor),1);
    // console.log(this.listaProfessorAdicionado);
    
  }

  ngOnInit() {
    this.getProfessores();
  }


}
