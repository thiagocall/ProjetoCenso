import { Component, OnInit } from '@angular/core';
import { OtimizacaoService } from '../_services/otimizacao.service';
import { Router, ActivatedRoute } from '@angular/router';
import { saveAs } from 'file-saver';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
//  import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-detalhe-resultado',
  templateUrl: './detalhe-resultado.component.html',
  styleUrls: ['./detalhe-resultado.component.css']
})

export class DetalheResultadoComponent implements OnInit {

  title = 'checkbox';

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
  id: any;
  indOficial: any;
  pacote: any;

  getDados() {

    this.id = this.thisRoute.snapshot.paramMap.get('id');
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

  check() {
    console.log(this.form.value)
    //enviar para o back

    if (this.form.get('indOficial').value) {
      this.form.get('indOficial').patchValue(1);

    } else {
      this.form.get('indOficial').patchValue(0);
    }

    console.log("isso foi enviado", this.form.value)
  }

  /** TESTE */
  /*salvarOficial() {
    this.OtimizacaoService.obterResultadosOtimizados().subscribe(
      response => {
        this.resultadoOtimizado = response;

      }, error => {
        console.log(error);
      });
  }

  /** teste */

  
  /*Comparar() {
    console.log(this.comparar);
    this.OtimizacaoService.getComparaResultado(this.comparar)
      .subscribe(
        response => {
          this.resultados = response;
           console.log(this.resultados);
        },
        error => {

        }
      );
  } */

  /*
  sendToBackEnd() {
    if (this.form.get('indOficial').value) {
      this.form.get('indOficial').patchValue(1);

    } else {
      this.form.get('indOficial').patchValue(0);
    }

    console.log("isso foi enviado", this.form.value)

  } */


  ngOnInit() {
    this.getDados();
    this.form = this.formBuilder.group({
      indOficial: [false, Validators.nullValidator]
    })

  }



}
