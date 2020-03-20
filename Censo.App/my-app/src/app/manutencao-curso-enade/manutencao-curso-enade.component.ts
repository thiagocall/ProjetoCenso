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
  }

  todosOsCiclos() {
    this.enadeService.obterCiclos().subscribe(
      response => {
        this.todosCiclos = response;
        this.id = this.todosCiclos.idCiclo;
        //console.log(this.todosCiclos);
        //console.log(this.todosCiclos[2]);
      },
      error => {
        console.log(error);
      }
    );
  }

  segundaLista() {
    this.enadeService.obterCiclos().subscribe(
      response => {
        this.todosCiclos = response;
        this.id = this.todosCiclos.idCiclo;
        //console.log(this.todosCiclos);
        //console.log(this.todosCiclos[2]);
      },
      error => {
        console.log(error);
      }
    );
  }


  filtrarCiclo(id:number){
   this.cicloFiltrado = this.todosCiclos.filter(x => x.idCiclo == id)[0] // [0] para pegar o primeira posicao do array o filtre pega só um array por veze para acessar esse obj dentro do array
  // console.log(this.cicloFiltrado[0].descArea)
  }

 adicionarArea() {
  console.log('Adicionar');
 }

 removerArea() {
  console.log('Remover');
 }

 salvar(){
   console.log('Salvar');
 }


}
