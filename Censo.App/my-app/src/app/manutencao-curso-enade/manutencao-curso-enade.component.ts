import { Component, OnInit } from '@angular/core';
import { EnadeService } from '../_services/enade.service';

@Component({
  selector: 'app-manutencao-curso-enade',
  templateUrl: './manutencao-curso-enade.component.html',
  styleUrls: ['./manutencao-curso-enade.component.css']
})
export class ManutencaoCursoEnadeComponent implements OnInit {

  constructor(private enadeService: EnadeService) { }

  todosCiclos: any;
  id: any;
  cicloId: any;

 cicloFiltrado:any;


  ngOnInit() {
    this.todosOsCiclos();
    //this.ciclosId(); // teste
  }

  todosOsCiclos() {
    this.enadeService.obterCiclos(this.id).subscribe(
      response => {
        this.todosCiclos = response;
        this.id = this.todosCiclos.idCiclo;
        console.log(this.todosCiclos);
        //console.log(this.todosCiclos[2]);
      },
      error => {
        console.log(error);
      }
    );
  }


  filtrarCiclo(id:number){
   this.cicloFiltrado = this.todosCiclos.filter(x => x.idCiclo == id)[0]
  // console.log(this.cicloFiltrado[0].descArea)
  }

 
  /*?*/
 /* ciclosId() {
    this.enadeService.obterCiclosId(this.id).subscribe(
      response => {
        this.cicloId = response;
        this.idCiclo = this.cicloId.idCiclo;
        console.log(this.cicloId)
       
      },
      error => {
        console.log(error);
      }
    );
  } */


}
