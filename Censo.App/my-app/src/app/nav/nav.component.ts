import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-inicio',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class InicioComponent implements OnInit {

  constructor() { }

  professores = [];

  ngOnInit() {

    this.professores = ['Thiago Caldas', 'Marcus Sales', 'Cesar Augusto'];
  }

}
