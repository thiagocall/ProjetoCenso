import { Component, OnInit } from '@angular/core';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import { Dados } from 'src/app/dados';
import { ChartDataSets, ChartType, RadialChartOptions } from 'chart.js';
import { R3TargetBinder } from '@angular/compiler';
import { ProfessorService } from '../_services/professor.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-professor',
  templateUrl: './Professor.component.html',
  styleUrls: ['./Professor.component.css']
})

export class ProfessorComponent implements OnInit {

  professores;
  integral: number;

  dados: Dados;

  constructor(private professorService: ProfessorService) { }

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

  public radarChartOptions = {
    responsive: true,
    title: {
      display: true,
      text: 'Distribuição de Professores (%)'
    },
    elements: {
      line: {
        tension: 0,
        borderWidth: 2
      }
    },
    tooltips: {
      enabled: true,
      callbacks: {
        label: function (tooltipItem, data) {
          return data.datasets[tooltipItem.datasetIndex].label + ' : ' + data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
        }
      }
    }
  };
  public radarChartLabels = ['Doutores',
    'Titulados',
    'Regime Tempo Integral',
    'Regime Tempo Integral + Parcial',
    'Especialista'];

  public radarChartData: any[];
  public radarChartType = 'radar';
  public radarChartLegend = false;
  public radarChartColor = [{ backgroundColor: 'rgba(30,184,222,0.4)', borderColor: 'rgba(30,184,222,0.9)' }];

  ngOnInit() {

    this.getProfessores();
  }

  getProfessores() {
    this.professorService.getProfessores()
      .subscribe(
        response => {
          this.professores = response;
        },
        error => {
          console.log(error);
        },
        () => {
          this.barChartData = [{
            data: [this.professores.qtdTempoIntegral,
            this.professores.qtdTempoParcial,
            this.professores.qtdHorista], label: 'Qtd Professores'
          }];
          // this.professores.qtdDoutor / this.professores.qtdProfessores * 100
          this.radarChartData = [{
            label: '% Professores',
            color: 'rgb(255, 255, 0)',
            data: [(this.professores.qtdDoutor / this.professores.qtdProfessores * 100).toFixed(2),
            ((this.professores.qtdMestre + this.professores.qtdDoutor) /
              this.professores.qtdProfessores * 100).toFixed(2),
            (this.professores.qtdTempoIntegral /
              this.professores.qtdProfessores * 100).toFixed(2),
            (this.professores.qtdRegime /
              this.professores.qtdProfessores * 100).toFixed(2),
            (this.professores.qtdEspecialista /
              this.professores.qtdProfessores * 100).toFixed(2)],
            fill: true,
            backgroundColor: 'rgba(54, 162, 235, 0.2)',
            borderColor: 'rgb(54, 162, 235)',
            pointBackgroundColor: 'rgb(54, 162, 235)',
          }
          ];


        });

  }

  exportarResultadoExcel() {
    let blob;
    this.professorService.getProfessorExcel().subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, 'Professores.xlsx');
    }
    );
  }

}