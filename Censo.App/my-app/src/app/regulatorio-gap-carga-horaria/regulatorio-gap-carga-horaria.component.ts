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
  campus: any;
  cursos: any;




  getProfessores() {
    this.regulatorio.PesquisaProfessores().subscribe(
      response => {
       this.resultado = response;
       this.campus = this.resultado.campus;
       this.cursos = this.resultado.cursos;
        //console.log(this.cursos);
      },
      error => {
      });
  } 

  



  ngOnInit() {
    this.getProfessores();
  }

}
