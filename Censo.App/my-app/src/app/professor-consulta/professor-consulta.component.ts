import { Component, OnInit } from '@angular/core';
import { HttpClientModule, HttpClient, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-professor-consulta',
  templateUrl: './professor-consulta.component.html',
  styleUrls: ['./professor-consulta.component.css']
})
export class ProfessorConsultaComponent implements OnInit {

  constructor(private http: HttpClient) {   }

  professores;
  public campo;

  ngOnInit() {
  }

  buscarProfessores() {

    this.http.get('http://10.200.0.9/api/Professor/Busca/' + this.campo).subscribe(

    response => {
      this.professores = response;
    },
    error => {
      console.log(error);
    });

  }

}
