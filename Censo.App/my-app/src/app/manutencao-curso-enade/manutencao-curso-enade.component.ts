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

  cicloFiltrado: any;

  /*cicloSelecionado*/
  ciclo: any;
  idSelecionado: any;
  descricaoCiclo: any;

  outros = 99;
  arrayCiclo: any;


  /*FILTRO POR AREA DO CICLO*/
  cicloFiltradoSelecionado: any;

  /*adicionar e remover */
  palavrasDestino = [];
  arrayAuxiliarOrigem: any = [];
  arrayAuxiliarDestino: any = [];
  /*adicionar e remover */

  ngOnInit() {
    this.todosOsCiclos();
    this.cicloSelecionado();
  }

  /* ANO I II III - Selecione um Ciclo */
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

  /*Combo 2 */
  cicloSelecionado() {
    this.enadeService.cicloSelecionadodeObterCiclos().subscribe(
      response => {
        this.ciclo = response;
       // console.log(this.ciclo) //trago a api2
      },
      error => {
        console.log(error);
      }
    );
  }


  filtrarCiclo(id: number) {
    this.cicloFiltrado = this.todosCiclos.filter(x => x.idCiclo == id)[0] // [0] para pegar o primeira posicao do array o filtre pega só um array por veze para acessar esse obj dentro do array
    // console.log(this.cicloFiltrado[0].descArea)
    this.cicloFiltradoSelecionado = this.ciclo.filter(x => x.idciclo == id);

    if (id !== this.outros) {
      this.arrayCiclo = this.ciclo.filter(x => x.idciclo == this.outros);
      //console.log(this.cicloFiltradoSelecionado); //vem filtrado pelo id (SEM 99) Variavel que trás todos os cursos do ciclo do box 2
    }
    this.arrayCiclo.forEach(element => {
      this.cicloFiltradoSelecionado.push(element); //COM 99
     // console.log(this.cicloFiltradoSelecionado);
    });
  }

  /*TESTE*/
  transfereItens() {
    this.cicloFiltradoSelecionado = this.cicloFiltradoSelecionado.filter(
      itemFonte => !this.arrayAuxiliarOrigem.includes(itemFonte)
    );
    this.palavrasDestino = [
      ...this.palavrasDestino,
      ...this.arrayAuxiliarOrigem
    ];
    this.arrayAuxiliarOrigem = [];
  }

  devolveItens() {
    this.palavrasDestino = this.palavrasDestino.filter(
      itemDestino => !this.arrayAuxiliarDestino.includes(itemDestino)
    );
    this.cicloFiltradoSelecionado = [
      ...this.cicloFiltradoSelecionado,
      ...this.arrayAuxiliarDestino
    ];
    this.arrayAuxiliarDestino = [];
  }


  pegaPalavraOrigem(palavra, target) {
    target.classList.toggle("selected");
     if (this.arrayAuxiliarOrigem.includes(palavra)) {
      let index = this.arrayAuxiliarOrigem.indexOf(palavra);
      this.arrayAuxiliarOrigem.splice(index, 1);
      return;
    }

    this.arrayAuxiliarOrigem.push(palavra);
  }

  pegaPalavraDestino(palavra, target) {
    target.classList.toggle("selected");
    if (this.arrayAuxiliarDestino.includes(palavra)) {
      let index = this.arrayAuxiliarDestino.indexOf(palavra);
      this.arrayAuxiliarDestino.splice(index, 1);
      return;
    }

    this.arrayAuxiliarDestino.push(palavra);
  }
}

  

 




