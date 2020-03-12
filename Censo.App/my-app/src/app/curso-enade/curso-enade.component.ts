import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { ProfessorService } from '../_services/professor.service';
import { EnadeService } from '../_services/enade.service';

@Component({
  selector: 'app-curso-enade',
  templateUrl: './curso-enade.component.html',
  styleUrls: ['./curso-enade.component.css']
})

export class CursoEnadeComponent implements OnInit {

  constructor(private enadeService: EnadeService) { }

  /* campus */
  resultado: any;
  listaCampus: any;
  listaCursos: any;

  /* filtrar cursos */
  cursoFiltrado: any;

  /*filtro input*/
  dadosFiltrados: any[];
  dadosFiltradosCursos: any[];

  /*select campus*/
  resultadoCampus: any;
  campi: any;
  codCampus: any;
  nomeCampus: any;
  codigoCampus:any; /*teste*/

  /*Tabela - resultadoCampusCurso*/
  resultadoCursos:any;
  cursos:any;

  ngOnInit() {
    //this.campus();
    this.selectCampus();
    this.resultadoCampusCurso();
  }

  selectCampus() { /* select só com os campus */
    this.enadeService.selectCampus().subscribe(
      response => {
        this.resultadoCampus = response;
        this.campi = this.resultadoCampus.campi;
        this.codigoCampus = this.resultadoCampus.codCampus;
      },
      error => {
        console.log(error);
      }
    );
  }

  resultadoCampusCurso() { /*resultado tabela*/
    this.enadeService.resultadoTabela().subscribe(
      response => {
        this.resultadoCursos = response;
        this.cursos = this.resultadoCursos.cursos;
        this.codCampus = this.resultadoCursos.codCampus;
      },
      error => {
        console.log(error);
      }
    );
  }


  filtrarCursos(valor: any) {
    this.cursoFiltrado = this.cursos.filter(c => c.codCampus == valor); 
    //this.cursoFiltrado = this.listaCursos.filter(c => c.codCampus == valor); // curso filtrado 
    console.log(this.cursoFiltrado);
  }

  pesquisarAno() {

  }

  /* filtro input */
  pesquisarCursos(value: any) {
    if (value.length > 4) {
      this.dadosFiltradosCursos = this.cursoFiltrado;
      this.dadosFiltradosCursos = this.dadosFiltradosCursos.filter(x =>
        x.nomCursoCenso.search(value.toLocaleUpperCase()) !== -1).slice(0, 5); // os 5 primeiros da lista;
      console.log(this.dadosFiltradosCursos); // está trazendo direito do curso filtrado
    }
    else {
      this.dadosFiltradosCursos = [];
    }
  }


 /*campus() {
    this.enadeService.getDados().subscribe(
      response => {
        this.resultado = response;
        this.listaCampus = this.resultado.campus;
        this.listaCursos = this.resultado.cursos;
        //console.log(this.resultado)
      },
      error => {
        console.log(error);
      }
    );
  } */





}
