import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-area-enade',
  templateUrl: './area-enade.component.html',
  styleUrls: ['./area-enade.component.css']
})
export class AreaEnadeComponent implements OnInit {

  constructor() { }
  todosCiclos: any;
  listaProfessorAdicionado: any;

  filtrarCiclo(item: any) {}
  dadosFiltrados() {}
  addProfessor(professor: any) {}
  limparLista() {}
  salvarDadosprofessor() {}
  removerProfessor(professor: any) {}

  ngOnInit() {
  }

}
