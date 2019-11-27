import { Component, OnInit } from '@angular/core';
import { RegulatorioService } from '../_services/regulatorio.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-regulatorio-professor-fora-sede',
  templateUrl: './regulatorio-professor-fora-sede.component.html',
  styleUrls: ['./regulatorio-professor-fora-sede.component.css']
})
export class RegulatorioProfessorForaSedeComponent implements OnInit {

  constructor(private regulatorioService: RegulatorioService) { }
  input: any;
  ies: any;
  resultadoCodCampus: any;

  ngOnInit() {
    this.inputCampus();
  }

  inputCampus() {
    this.regulatorioService.getCampusForaSede().subscribe(
      response => {
        //ordenação
        this.input = response;
        this.ies = this.input.ies;
        this.ies.sort(function (a, b) {
          if (a.nomIes > b.nomIes) {
            return 1;
          }
          if (a.nomIes < b.nomIes) {
            return -1;
          }
          return 0;
        });

        console.log(this.input.ies);
      },
      error => {
        console.log(error);
      });
  }


  resultadoProfessorForaSede(codCampus: string) {
      this.regulatorioService.getResultadoProfessorForaSede(codCampus).subscribe(
        response => {
          this.resultadoCodCampus = response;
        },
        error => {
          console.log(error);
        });
  }

  exportarResultadoExcel(codcampus: any) {
    let blob;
    this.regulatorioService.getRegulatorioProfessorForaSede(codcampus).subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, `Professor_IES_${codcampus}.xlsx`);
    });
  } 


}
