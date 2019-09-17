import { Component, OnInit } from '@angular/core';
import { HttpClientModule, HttpClient, HttpResponse } from '@angular/common/http';
import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-app-corpo-docente',
  templateUrl: './app-corpo-docente.component.html',
  styleUrls: ['./app-corpo-docente.component.css']
})
export class AppCorpoDocenteComponent implements OnInit {

  constructor(private http: HttpClient) { }
  resultado: any;
  listaCampus: any;
  listaCursos: any;
  curso: any;
  cod: any;

  ngOnInit() {
    this.http.get('http://localhost:5000/api/v1/dados').subscribe(
      response => {
        this.resultado = response;
        this.listaCampus = this.resultado.campus;
        this.listaCursos = this.resultado.cursos;
        console.log(response);
      },
      error => {
        console.log(error);
      }
    );
  }

  getCampus(codigo: string) {
    this.curso = this.listaCursos.filter(x => x.codCampus.toString() === codigo.toString());
    // this.curso = this.listaCursos;
  }


}
