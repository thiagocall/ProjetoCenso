import { Component, OnInit, Input} from '@angular/core';
import { OtimizacaoService } from '../_services/otimizacao.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-app-dados-censo',
  templateUrl: './app-dados-censo.component.html',
  styleUrls: ['./app-dados-censo.component.css'],
})

export class AppDadosCensoComponent implements OnInit {

  constructor(private otmService: OtimizacaoService) { }

  public parametro: Parametro;

  app = new AppComponent();

  selecionado = 'true';


  ngOnInit() {
    this.parametro = new Parametro();
  }

  SalvaParametro() {
    
  }

  Restaurar() {

    this.parametro = new Parametro();
    this.selecionado = 'true';

  }


  Otimizar() {

    this.otmService.Otimizar(this.parametro).subscribe(
      response => {
        // alert("Ok");
          this.app.ShowToast();
      },
      error => {(error);
      }
    );

    
  }

  


}

class Parametro {

  constructor() {

        this.DTI = true;
        this.DTP = true;
        this.DH = true;
        this.MTI = true;
        this.MTP = true;
        this.MH = true;
        this.ETI = true;
        this.ETP = true;
        this.EH = false;
        this.Perclimite = 15;
        this.otimiza20p = true;
        this.usoProfessor = 10;
        this.usoProfessorGeral = 15;
        this.PercReduProf = 20;
        this.Metodo = -1;

  }

  DTI: boolean;
  DTP: boolean;
  DH: boolean;
  MTI: boolean;
  MTP: boolean;
  MH: boolean;
  ETI: boolean;
  ETP: boolean;
  EH: boolean;
  Perclimite: number;
  otimiza20p: boolean;
  usoProfessor: number;
  usoProfessorGeral: number;
  PercReduProf: number;
  Metodo: number;

}
