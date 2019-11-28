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
  codigoCampus: any;
  resultadoCodCampus: any;
 
  ngOnInit() {
    this.inputCampus();
  }

  inputCampus() {
    this.regulatorioService.getCampusForaSede().subscribe(
      response => {
        this.input = response;
        this.codigoCampus = this.input;
        console.log(this.codigoCampus);
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
    console.log(codcampus)
    this.regulatorioService.getRegulatorioProfessorForaSedeExcel(codcampus).subscribe(response => {
      blob = new Blob([response], { type: 'application/octet-stream' });
      saveAs(blob, `Professor_IES_${codcampus}.xlsx`);
    });
  } 


}
