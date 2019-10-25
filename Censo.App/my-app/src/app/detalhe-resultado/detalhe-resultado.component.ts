import { Component, OnInit } from '@angular/core';
import { OtimizacaoService } from '../_services/otimizacao.service';

@Component({
  selector: 'app-detalhe-resultado',
  templateUrl: './detalhe-resultado.component.html',
  styleUrls: ['./detalhe-resultado.component.css']
})
export class DetalheResultadoComponent implements OnInit {
  resultadoOtimizado: any; //resultado da tabela TbResultado
  dados: number;


  constructor(private OtimizacaoService: OtimizacaoService) { }

  ngOnInit() {
    this.getResultado();
  }


  /* posso apagar é só um teste para passar os valores na tela */
  getResultado() {
    this.OtimizacaoService.obterResultadosOtimizados().subscribe(
      response => {
        this.resultadoOtimizado = response;
        this.dados = this.resultadoOtimizado.length;
        // console.log(this.resultadoOtimizado.length);
        var a: any = this.resultadoOtimizado;
        //console.log(typeof a[0].id);
      }, error => {
        console.log(error);
      });
  }


}
