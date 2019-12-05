import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';


@Component({
  selector: 'app-regulatorio-gap-carga-horaria',
  templateUrl: './regulatorio-gap-carga-horaria.component.html',
  styleUrls: ['./regulatorio-gap-carga-horaria.component.css']
})
export class RegulatorioGapCargaHorariaComponent implements OnInit {

  constructor(private regulatorio: RegulatorioService) { }

  p: any;
  resultado: any;


  /*filtro lista*/
  dadosFiltrados: any[];
  filtroProfessor: []
  

  /*pesquisa com a lista completa ao recarregar a pagina */
  getProfessores() {
    this.regulatorio.PesquisaProfessores().subscribe(
      response => {
        this.resultado = response;
        this.dadosFiltrados = this.resultado;
        console.log(this.resultado);
      },
      error => {
      });
  }


  /* funcionando */
  filtrarItem(value: any) {
    if(value.length > 4 ) {
       this.dadosFiltrados = this.resultado;
       this.dadosFiltrados = this.dadosFiltrados.filter( x => x.cpfProfessor.search(value.toLocaleUpperCase()) !== -1 ||
                                                         x.titulacao.search(value.toLocaleUpperCase()) !== -1 ||
                                                         x.nomProfessor.search(value.toLocaleUpperCase()) !== -1 ||
                                                         x.regime.search(value.toLocaleUpperCase()) !== -1);
    }

    else {
      this.dadosFiltrados = [];
    }
    
  }



ngOnInit() {
  this.getProfessores();
}



}
