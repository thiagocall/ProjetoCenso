import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-regulatorio-professor-curso',
  templateUrl: './regulatorio-professor-curso.component.html',
  styleUrls: ['./regulatorio-professor-curso.component.css']
})
export class RegulatorioProfessorCursoComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }
  campi:any;
  curso:any[];
  dados: any;
  cursoFiltrado:any;
  resultado:any;
  p: any;
  errodados = false;
  selecione: any;
  campoSelecionado: any;

  
  ngOnInit() {
  this.getCampus();
  }

  
  getCampus() {
    this.regulatorioService.getCampus().subscribe(
      response => {
        this.dados = response;
        this.curso = this.dados.cursos;
        this.campi = this.dados.campi;

        //campus
        this.campi.sort(function (a,b) {
          if (a.nomCampus > b.nomCampus) {
            return 1;
          }
          if (a.nomCampus < b.nomCampus) {
            return -1;
          }
          return 0;
        });
        //console.log(this.dados.curso);
        
        //curso
        this.curso.sort(function (a,b) {
          if (a.nomCursoCenso > b.nomCursoCenso) {
            return 1;
          }
          if (a.nomCursoCenso < b.nomCursoCenso) {
            return -1;
          }
          return 0;
        });
        //console.log(this.dados.curso);
        //console.log(this.curso);
      },
      error => {
        console.log('curso' + error);
      }
    );
  } 




  exportarResultadoExcel() {

    
    let blob;
    let id;
    console.log(this.selecione);

    if (this.selecione == 0) {

      id = this.campoSelecionado;
      // console.log(this.selecione);
      this.regulatorioService.getRegulatorioProfessorCampusExcel(id)
      .subscribe(
        response => {
          blob = new Blob([response], { type: 'application/octet-stream' });
          saveAs(blob, `Professor_Curso_${id}.xlsx`);
        }
      )
      
    } else {
      id = this.selecione;

      this.regulatorioService.getRegulatorioProfessorCursoExcel(id)
          .subscribe(
            response => {
              blob = new Blob([response], { type: 'application/octet-stream' });
              saveAs(blob, `Professor_Curso_${id}.xlsx`);
            }
          )
    }

  }


 
  getCurso(valor: any){
   // console.log(valor);
   this.campoSelecionado = valor;
   console.log(this.campoSelecionado);
   this.cursoFiltrado = this.curso.filter(c => c.codCampus == valor);
  }

  getResultado(valor: number){
    this.regulatorioService.getRegulatorioProfessorCurso(valor).subscribe(
      response => {
        this.errodados = false;
        this.resultado = response;
        // console.log(this.resultado);
        console.log(this.errodados);
      },
      error => {
        this.errodados = true;
        console.log(error);
      }
    );
  }
 

}
