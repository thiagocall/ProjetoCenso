import { Component, OnInit, ɵConsole } from '@angular/core';
import { OtimizacaoService } from 'src/app/_services/otimizacao.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-app-comparar',
  templateUrl: './app-comparar.component.html',
  styleUrls: ['./app-comparar.component.css']
})
export class AppCompararComponent implements OnInit {

  constructor(private OtimizacaoService: OtimizacaoService, private router: Router, private route: ActivatedRoute) { }

  comparar: any;
  resultados: any;
  resultadosJson: any;
  indOficial: any;

  ngOnInit() {
    this.route.queryParams
      .subscribe(params =>
        this.comparar = params.res);
    this.Comparar();
  }


  Comparar() {
    //console.log(this.comparar);
    this.OtimizacaoService.getComparaResultado(this.comparar)
      .subscribe(
        response => {
          this.resultados = response;
          this.indOficial = this.resultados.indOficial;
        },
        error => {

        }
      );
  }

  JSONParse_(js: any) {
    return JSON.parse(js);
  }

}
