import { Component, OnInit } from '@angular/core';
import { HttpClientModule, HttpClient, HttpResponse } from '@angular/common/http';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import { Dados } from 'src/app/dados';
import { ChartDataSets, ChartType, RadialChartOptions } from 'chart.js';


@Component({
  selector: 'app-professor',
  templateUrl: './Professor.component.html',
  styleUrls: ['./Professor.component.css']
})

export class ProfessorComponent implements OnInit {

  professores;
  integral: number;

  dados: Dados;

  constructor(private http: HttpClient) {}

  public barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      display: true,
      text: 'Professores por Regime de Trabalho'
  }
  };

  public barChartColors: Color[] = [{
    backgroundColor: 'rgba(23,58,86,1)'
  }];

  public barChartLabels = ['Tempo Integral', 'Tempo Parcial', 'Horista'];
  public barChartType = 'bar';
  public barChartLegend = false;
  public barChartData: any[];

  public radarChartOptions: RadialChartOptions = {
    responsive: true,
  };
  public radarChartLabels: Label[] = ['Doutores',
                                      'Mestres + Doutores',
                                      'Regime Tempo Integral',
                                      'Regime Tempo Integral + Parcial',
                                      'CHZ/Afastado'];

  public radarChartData: ChartDataSets[] = [
    { data: [65, 59, 70, 81, 20], label: 'Unesa' }
  ];
  public radarChartType: ChartType = 'radar';

  ngOnInit() {

    this.getProfessores();
  }

  getProfessores() {
      this.http.get('http://localhost:5000/api/Professor')
      .subscribe(
        response => {
          this.professores = response;
        },
        error => {console.log(error);
        },
        () => {
          this.barChartData =  [{data: [ this.professores.qtdTempoIntegral,
                                          this.professores.qtdTempoParcial,
                                          this.professores.qtdHorista], label: 'Qtd Professores'}];
      });

      }

}

