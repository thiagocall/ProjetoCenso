import { Component, OnInit } from '@angular/core';
import { HttpClientModule, HttpClient, HttpResponse } from '@angular/common/http';
import { ProfessorService } from '../_services/professor.service';

@Component({
  selector: 'app-professor-consulta',
  templateUrl: './professor-consulta.component.html',
  styleUrls: ['./professor-consulta.component.css']
})
export class ProfessorConsultaComponent implements OnInit {

  constructor(private professorService: ProfessorService) {   }

  professores;
  public campo;

  ngOnInit() {
  }

  buscarProfessores() {

    this.professorService.buscarProfessores(this.campo).subscribe(

    response => {
      this.professores = response;
    },
    error => {
      console.log(error);
    });

  }

}
