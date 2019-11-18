import { Component, OnInit } from '@angular/core';
import { ProfessorService } from '../_services/professor.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-professor-consulta-detalhe',
  templateUrl: './professor-consulta-detalhe.component.html',
  styleUrls: ['./professor-consulta-detalhe.component.css']
})

export class ProfessorConsultaDetalheComponent implements OnInit {

  constructor(private professorService: ProfessorService, private router: Router, private thisRoute: ActivatedRoute) { }

  professor:any;
  public campo;
  id: any;


  /*//funcionando
  buscarCpfProfessores() {
    this.id = this.thisRoute.snapshot.paramMap.get('id');
    this.professorService.buscarProfessores(this.id).subscribe(
      response => {
        this.professores = response;
      },
      error => {
        console.log(error);
      }
    );
  } */

  buscarCpfProfessor() {
    this.id = this.thisRoute.snapshot.paramMap.get('id');
    this.professorService.professorConsultaDetalhe(this.id).subscribe(

      response => {
        this.professor = response;
        console.log('PROFESSOR : ' + this.professor);
      },
      error => {
        console.log(error);
      }
    );
  } 

  ngOnInit() {
    this.buscarCpfProfessor();
  }

}
