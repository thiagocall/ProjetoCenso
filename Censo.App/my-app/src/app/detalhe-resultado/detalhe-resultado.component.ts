import { Component, OnInit } from '@angular/core';
import { OtimizacaoService } from '../_services/otimizacao.service';
import { Router, ActivatedRoute } from '@angular/router';
import { saveAs } from 'file-saver';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomElementSchemaRegistry } from '@angular/compiler';
//  import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-detalhe-resultado',
  templateUrl: './detalhe-resultado.component.html',
  styleUrls: ['./detalhe-resultado.component.css']
})

export class DetalheResultadoComponent implements OnInit {

  title = 'checkbox';
  disableBotao: boolean;

  form: FormGroup;

  constructor(private otimizacaoService: OtimizacaoService,
    private router: Router,
    private thisRoute: ActivatedRoute,
    private formBuilder: FormBuilder) { }

  dados: any;
  dadosJsonAtual: any;
  dadosJsonOtm: any;
  resultadoAtual: any;
  resultadoOtimizado: any;
  fileUrl;
  resultadoOficial: any;
  id: number;
  indOficial: any;
  pacote: any;

  getDados() {

    this.id = Number(this.thisRoute.snapshot.paramMap.get('id'));
    this.otimizacaoService.obterDetalheResultado(this.id).subscribe(

      response => {
        this.dados = response;
        this.resultadoAtual = this.dados.resultadoAtual;
        this.resultadoOtimizado = this.dados.resultado;
        this.dadosJsonAtual = JSON.parse(this.resultadoAtual.resumo);
        this.dadosJsonOtm = JSON.parse(this.resultadoOtimizado.resumo);
        this.indOficial = this.dados.resultado.indOficial;
        if (this.indOficial == 0) {
          this.form.get('indOficial').patchValue(false);
        } else {
          this.form.get('indOficial').patchValue(true);
        }

      },
      error => {
        console.log(error);
      }
    );

  }

  exportarResultadoExcel() {
    let thefile;
    let blob;

    this.otimizacaoService.exportarResultadoExcel(this.id).subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, `ResultadoCenso_${this.id}.xlsx`);
    });
    // window.open(window.URL.createObjectURL(thefile));
  };

  
  salvarOficial(value:any) {
    this.disableBotao = true
    this.otimizacaoService.salvarOficial(Object.assign({ "id": this.id, valor: value })).subscribe(
      response => {
        this.resultadoOficial = response;

        if (this.form.get('indOficial').value) {
          this.form.get('indOficial').patchValue(1);
          // console.log(Object.assign({ "id": this.id }));

        } else {
          this.form.get('indOficial').patchValue(0);
          // console.log(Object.assign({ "id": this.id }));
        }

        this.disableBotao = false
        //console.log("isso foi enviado", this.form.value)

      }, error => {
        console.log(error)
        this.disableBotao = false
      }
    )

    console.log(Object.assign({ "id": this.id, valor: value}));
  }

  
  ngOnInit() {
    this.getDados();
    this.form = this.formBuilder.group({
      indOficial: [false, Validators.nullValidator]
    })

  }



}
