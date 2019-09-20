import { Component, OnInit } from '@angular/core';
import { HttpBackend } from '@angular/common/http';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-app-dados-censo',
  templateUrl: './app-dados-censo.component.html',
  styleUrls: ['./app-dados-censo.component.css']
})
export class AppDadosCensoComponent implements OnInit {

  constructor(private http: HttpClient) { }

  public parametro: Parametro;

  ngOnInit() {
    this.parametro = new Parametro();
  }

  SalvaParametro() {

  }


  Otimizar() {

    this.http.post('http://localhost:5000/api/v1/censo/CursoEmec/Otimizar', this.parametro).subscribe(
      response => { console.log(response);
      },
      error => { console.log(error);
      }
    );
  }


}

class Parametro {
  constructor() {

  this.DTI = true;
  this.DTP = true;
  this.DH = true;
  this.MTI = true;
  this.MTP = true;
  this.MH = true;
  this.ETI = true;
  this.ETP = true;
  this.EH = false;
  this.Perclimite = 15;
  this.otimiza20p = true;
  this.usoProfessor = 10;
  this.usoProfessorGeral = 15;
  this.PercReduProf = 20;

  }

  DTI: boolean;
  DTP: boolean;
  DH: boolean;
  MTI: boolean;
  MTP: boolean;
  MH: boolean;
  ETI: boolean;
  ETP: boolean;
  EH: boolean;
  Perclimite: number;
  otimiza20p: boolean;
  usoProfessor: number;
  usoProfessorGeral: number;
  PercReduProf: number;

}
