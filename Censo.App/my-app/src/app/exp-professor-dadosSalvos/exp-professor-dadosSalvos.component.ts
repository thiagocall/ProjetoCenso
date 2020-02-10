import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-exp-professor-dadosSalvos',
  templateUrl: './exp-professor-dadosSalvos.component.html',
  styleUrls: ['./exp-professor-dadosSalvos.component.css']
})
export class ExpProfessorDadosSalvosComponent implements OnInit {

  item: any;
  cpfProfessor: any;
  nomProfessor: any;
  regime: any;
  titulacao: any;

  constructor() { }

  ngOnInit() {
  }

}
