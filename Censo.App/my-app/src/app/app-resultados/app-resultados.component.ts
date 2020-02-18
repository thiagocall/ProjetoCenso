import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { OtimizacaoService } from '../_services/otimizacao.service';
import { Router } from '@angular/router';
import { saveAs } from 'file-saver';
import { collectExternalReferences } from '@angular/compiler';


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
  dados: number;

  constructor(private modalService: BsModalService, 
    private OtimizacaoService: OtimizacaoService, 
    private router: Router) { }

  confirma: boolean;
  idSelecionado: number;
  listaCompara: Array<any> = new Array<any>();


  ngOnInit() {
    this.getResultado();
  }

  addCompara(id: number, ind: any) {
    if (ind) {
      this.listaCompara.push(id);
    } else {
      this.listaCompara.splice(this.listaCompara.indexOf(id), 1);
    }

  }


  getResultado() {
    this.OtimizacaoService.obterResultadosOtimizados().subscribe(
      response => {
        this.resultadoOtimizado = response;
        this.dados = this.resultadoOtimizado.length;
        console.log(this.resultadoOtimizado.length);
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
    this.modalRef.hide(); // fecha o modal
  }

  toggleCol(id: any) {
    
  }

  exportarResultadoExcel(item: any) {
    // const id = item.substr(2, item.length);
    console.log(item);
    let thefile;
    let blob;
    this.OtimizacaoService.exportarResultadoExcel(item).subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, `ResultadoCenso_${item}.xlsx`);
    }
    );
  }
}
