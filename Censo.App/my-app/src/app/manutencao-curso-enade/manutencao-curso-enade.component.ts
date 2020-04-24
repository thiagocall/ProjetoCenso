import { Component, OnInit, ɵConsole } from '@angular/core';
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

  noventaENove = 99;
  um = 1;
  dois = 2;
  tres = 3;

  arrayCiclo: any;


  /*FILTRO POR AREA DO CICLO*/
  cicloFiltradoSelecionado: any;

  /*adicionar e remover */
  palavrasDestino = [];
  arrayAuxiliarOrigem: any = [];
  arrayAuxiliarDestino: any = [];
  /*adicionar e remover */

  /*salvar*/
  saveIdCiclo: number;

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
        //console.log('TODOS OS CICLOS ' ,this.todosCiclos);
        //console.log(this.todosCiclos[2]);
      },
      error => {
        console.log(error);
      }
    );
  }

  /*Combo 2 */
  cicloSelecionado() {
    this.enadeService.cicloSelecionadodeObterCiclos()
    .subscribe(
      response => {
        this.ciclo = response;
        //console.log('THIS CICLO', this.ciclo) //trago a api2
      },
      error => {
        console.log(error);
      }
    );
  }


  filtrarCiclo(id: number) {
    this.saveIdCiclo = id;
    this.palavrasDestino = [];
    this.cicloFiltrado = this.todosCiclos.filter(x => x.idCiclo == id)[0] // [0] para pegar o primeira posicao do array o filtre pega só um array por veze para acessar esse obj dentro do array

    /*COMBO 2*/
    this.cicloFiltradoSelecionado = this.ciclo.filter(x => x.idciclo == id);
    /*ordem alfabetica combo 2 - quando seleciona no select trás os cursos no combo2*/
    let tempArray = this.cicloFiltradoSelecionado;
    tempArray.sort((a, b) => {
      if (a.descricaoarea > b.descricaoarea) {
        return 1;
      }
      if (a.descricaoarea < b.descricaoarea) {
        return -1;
      }
      return 0;
    });
    //console.log('COMBO 2 ', tempArray) //
    /*fim ordem alfabetica combo 2*/

    this.ciclo.forEach(element => {
      if (element.idciclo != id) {
        this.palavrasDestino.push(element);
        /*ordem alfabetica combo 3*/
        let tempArray = this.palavrasDestino;
        tempArray.sort((a, b) => {
          if (a.descricaoarea > b.descricaoarea) {
            return 1;
          }
          if (a.descricaoarea < b.descricaoarea) {
            return -1;
          }
          return 0;
        });

        /*fim ordem alfabetica combo 3*/
        // console.log('Array Aux', this.palavrasDestino) //todos os ciclos
      }
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
    //console.log('CICLO FILTRADO SELECIONADO' , this.cicloFiltradoSelecionado)
  }


  /*combo 2*/
  pegaPalavraOrigem(palavra, target) {
    target.classList.toggle("selected"); // se o alvo do clique tiver a classe selected, remove, do contrário, adiciona
    if (this.arrayAuxiliarOrigem.includes(palavra)) { //se o arrayAuxiliarOrigem já contem o item
      let index = this.arrayAuxiliarOrigem.indexOf(palavra); //Pego o indice desse item do array
      this.arrayAuxiliarOrigem.splice(index, 1); //removo ele
      return;
    }

    this.arrayAuxiliarOrigem.push(palavra); // como eu botei o retorno no if, se não cair no if somente, vai ser executar aqui, adicionando o item
   // console.log('arrayAuxiliarOrigem', this.arrayAuxiliarOrigem.push(palavra));
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

  salvar(){
    let localCiclo = [];
    this.cicloFiltradoSelecionado.forEach(element => {
      localCiclo.push(element.codareaemec);
    });
    let data = {
      'id': this.saveIdCiclo,
      'ciclos':localCiclo
    }
    console.log('save data ' ,data);
    this.enadeService.salvarIdCiclo(data).subscribe(
      response => {
        console.log('salvar api' ,response);
      },
      error => {
        console.log(error);
      }
    );
  }

}








