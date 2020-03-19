import { Component, OnInit, ɵConsole } from '@angular/core';
import { EnadeService } from '../_services/enade.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-curso-enade',
  templateUrl: './curso-enade.component.html',
  styleUrls: ['./curso-enade.component.css']
})

export class CursoEnadeComponent implements OnInit {

  selectAno:string;
  arraySlectedAno:string;
  arrayCursoFiltrado:any;

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
  anoCampus:any;

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

  salvarCursos:any

  ngOnInit() {
    this.selectCampus();
  }


  /*SELECT COM OS CAMPUS*/
  selectCampus() {
    if (this.campusForm.value != null || this.campusForm.value != undefined) {
      console.log('CodCampus' + this.campusForm.value.codCampus)
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


  /*RESULTADO DA TABELA */
  resultadoCampusCurso(valor:any) {
    
    this.enadeService.resultadoTabela(valor).subscribe(
      response => {
        this.resultadoCursos = response;
        this.cursos = this.resultadoCursos;      
        this.codCampus = this.resultadoCursos.codCampus;
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


  filtroAno(ano:any) {
    //console.log('ANO ' + JSON.stringify(ano))
    this.arraySlectedAno = this.cursos.filter(x => // o filtro esta dentro de select ano
      x.idciclo == ano);
    //console.log('filterCURSOS ' + JSON.stringify(this.arraySlectedAno))
  }

  /* filtro input */
  pesquisarCursos(value: any) {
   // console.log('CURSOS' +JSON.stringify(this.cursos))
   console.log('SALVAR CURSOS ' + this.salvarCursos)
    if (value.length > 4) {
      this.arrayCursoFiltrado = this.cursos.filter(x => // o filtro esta dentro de select ano
        x.nomecurso === value);
        console.log('ARRAY SELECT' + this.arraySlectedAno);
        //console.log('VALUE' + value);

    }
    else {
      //this.arrayCursoFiltrado = [];
      console.log('ELSE' + this.arrayCursoFiltrado)
    }
  }  



}
