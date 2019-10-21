import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { OtimizacaoService } from '../_services/otimizacao.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-app-resultados',
  templateUrl: './app-resultados.component.html',
  styleUrls: ['./app-resultados.component.css']
})
export class AppResultadosComponent implements OnInit {

  modalRef: BsModalRef;
  message: string;
  resultadoOtimizado: any; //resultado da tabela TbResultado
  isCollapsed = true;

  constructor(private modalService: BsModalService, private OtimizacaoService: OtimizacaoService, private router: Router) { }

  confirma: boolean;
  idSelecionado: number;


  ngOnInit() {
    this.getResultado();
  }

  getResultado() {
    this.OtimizacaoService.obterResultadosOtimizados().subscribe(
      response => {
        this.resultadoOtimizado = response;
        // console.log(this.resultadoOtimizado);
        var a: any = this.resultadoOtimizado;
        //console.log(typeof a[0].id);
      }, error => {
        console.log(error);
      });
  }

  //formatar data dia/mes/ano
  getData(id: number) {

    var dia = (String(id)).substr(-8, 2)
    var mes = (String(id)).substr(4, 2)
    var ano = (String(id)).substr(0, 4)
    return `${dia}/${mes}/${ano}`;

  }

  getResumo(res: any) {
    const resumo = JSON.parse(res);
    // console.log(resumo);
    return resumo;
  

  }

  modalExcluir(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  excluir(idResultado: number, template: TemplateRef<any>): void {

    //fecha o modal
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    this.idSelecionado = idResultado;
    console.log(this.confirma);


  }


  ConfirmaExclusao() {

    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate(['/Resultados']);
    // Atualiza a tela da tabela
    this.OtimizacaoService.excluirResultadosOtimizados(this.idSelecionado).subscribe(
      response => {
        this.modalRef.hide();
        this.getResultado();
        // console.log()
      },
      error => {
        console.log(error);
      });

  }

  cancelar(): void {
    this.modalRef.hide(); // fehca o modal
  }




  toggleCol(id: any) {

      

  }
}
