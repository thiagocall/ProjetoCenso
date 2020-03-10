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

  /*salvar*/


  ngOnInit() {
    this.campus();
  }


  campus() {
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
  }

  filtrarCursos(valor: any) {
    this.cursoFiltrado = this.listaCursos.filter(c => c.codCampus == valor);
    //console.log(this.cursoFiltrado);
  }

  salvarCiclo() {
    
  }




}
