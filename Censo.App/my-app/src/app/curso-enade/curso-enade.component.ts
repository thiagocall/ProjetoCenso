import { Component, OnInit, ɵConsole } from '@angular/core';
import { EnadeService } from '../_services/enade.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-curso-enade',
  templateUrl: './curso-enade.component.html',
  styleUrls: ['./curso-enade.component.css']
})

export class CursoEnadeComponent implements OnInit {

  selectAno: string;
  arraySlectedAno: string;
  arrayCursoFiltrado: any;

  constructor(private enadeService: EnadeService,
    public formBuilder: FormBuilder) {

    this.campusForm = this.formBuilder.group({
      codCampus: ['', [Validators.required, Validators.maxLength(80)]],
    });

    this.anoForm = this.formBuilder.group({
      anoCampus: ['', [Validators.required, Validators.maxLength(80)]],
    });

  }

  public campusForm: FormGroup;
  public anoForm: FormGroup;
  anoCampus: any;

  /*campus*/
  resultado: any;
  listaCampus: any;
  listaCursos: any;

  /*filtrar cursos*/
  cursoFiltrado: any;

  /*filtro input*/
  dadosFiltrados: any[];
  dadosFiltradosCursos: any[];

  /*select campus*/
  resultadoCampus: any;
  campi: any;
  codCampus: any;
  nomeCampus: any;
  codigoCampus: any; /*teste*/

  /*Tabela - resultadoCampusCurso*/
  resultadoCursos: any;
  cursos: any;
  value: any

  salvarCursos: any

  ngOnInit() {
    this.selectCampus();
  }


  /*SELECT COM OS CAMPUS - selecione o campus*/
  selectCampus() {
    if (this.campusForm.value != null || this.campusForm.value != undefined) {
      this.enadeService.selectCampus().subscribe(
        response => {
          this.resultadoCampus = response;
          this.campi = this.resultadoCampus.campi; //dentro de campi eu tenho codcampus e nomCampus
        },
        error => {
          console.log(error);
        }
      );
    }

  }


  /*RESULTADO DA TABELA -CURSOS */
  resultadoCampusCurso(valor: any) {
    this.arraySlectedAno = '';
    // this.cursos = this.resultadoCursos; //testar
    this.enadeService.resultadoTabela(valor).subscribe(
      response => {
        this.resultadoCursos = response;
        this.cursos = this.resultadoCursos;
        this.codCampus = this.resultadoCursos.codCampus;
        /*ordem alfabetica cursos pesquisa tabela*/
        let tempArray = this.cursos;
        tempArray.sort((a, b) => {
          if (a.nomecurso > b.nomecurso) {
            return 1;
          }
          if (a.nomecurso < b.nomecurso) {
            return -1;
          }
          return 0;
        });
        /*fim ordem alfabetica cursos pesquisa tabela*/

      },
      error => {
        console.log(error);
      }
    );
  }


  /*BOTÃO PESQUISAR*/
  botaoPesquisar(valor: any) {
    if (valor != null || valor != undefined) {
      this.resultadoCampusCurso(valor);
    }
    else {
      console.log('valor botao pesquisar' + valor)
    }
  }

  /* select ANO */
  filtroAno(ano: any) {
    if (ano == '0') {
      this.arraySlectedAno = this.cursos;
      //console.log(this.arraySlectedAno)
    } else {
      this.arraySlectedAno = this.cursos.filter(x => // o filtro esta dentro de select ano
        x.idciclo == ano);
    }
  }

  /* filtro do menu pesquisar input */
  /* pesquisarCursos(value: any) {
     //this.arrayCursoFiltrado = [];//apagar
     if (value.length > 4) {
       this.arrayCursoFiltrado = this.cursos.filter(x => // o filtro esta dentro de select ano
         x.nomecurso === value);
     }
     else {
       this.arrayCursoFiltrado = [];
     }
   } */



}
