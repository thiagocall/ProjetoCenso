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

  resultado: any;
  listaCampus: any;
  listaCursos: any;

  cursoFiltrado: any;
  campoSelecionado: any;
  curso: any[];

  ngOnInit() {
    this.campus();
  }


 campus(){
  this.enadeService.getDados().subscribe(
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

 getCurso(valor: any) {
  // console.log(valor);
  this.campoSelecionado = valor;
  //console.log(this.campoSelecionado);
  this.cursoFiltrado = this.curso.filter(c => c.codCampus == valor);
}


  

}
