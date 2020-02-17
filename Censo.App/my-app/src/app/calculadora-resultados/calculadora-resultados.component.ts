import { Component, OnInit, ComponentFactoryResolver } from '@angular/core';
import { ProfessorService } from '../_services/professor.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-calculadora-resultados',
  templateUrl: './calculadora-resultados.component.html',
  styleUrls: ['./calculadora-resultados.component.css']
})
export class CalculadoraResultadosComponent implements OnInit {

  constructor(private professorService: ProfessorService , private route: ActivatedRoute) { }
  resultado: any;
  listaCampus: any;
  listaCursos: any;
  curso: any;
  errodados = false;
  professores: any;
  infoCurso: any;
  resultados: any;
  resultadoCalculo: any;
  cod:any;
  codcurso: any;
  regua: previsao;

  notaContinua: any;
  notaFaixa: any;

  dados = new dados();

  calcula: calcula;
  calculaAnt= new calcula();
  calculaOriginal= new calcula();


  ngOnInit() {
    this.CalcularResultado(); //teste
    this.professorService.getDados().subscribe(
      response => {
        this.resultado = response;
        this.listaCampus = this.resultado.campus;
        this.listaCursos = this.resultado.cursos;
      },
      error => {
        console.log(error);
      });
  }

  getCampus(codigo: string) {
    this.curso = this.listaCursos.filter(x => x.codCampus.toString() === codigo.toString());
    // this.curso = this.listaCursos;
  }


  getInfoCurso(codigo: any) {
    this.professores = null;
    this.professorService.getInfoCurso(codigo).subscribe(
      response => {
        this.errodados = false;
        this.infoCurso = response;
        this.professores = this.infoCurso.cursoProfessor;
        //console.log(response);
      },
      error => {
        this.errodados = true;
        this.infoCurso = null;
        console.log(error);
      });
  }

  
  CalcularResultado() {

    this.dados._idEmec = this.codcurso;
    this.dados._idResultado = this.route.snapshot.paramMap.get('id');
    
    this.professorService.postCalculadoraResultado(this.dados).subscribe(
      response => {
        this.resultadoCalculo = response;
        this.calculaAnt = new calcula();
        this.calculaAnt.qtdDR = this.resultadoCalculo.qtdDR;
        this.calculaAnt.qtdDH = this.resultadoCalculo.qtdDH;
        this.calculaAnt.qtdMR = this.resultadoCalculo.qtdMR;
        this.calculaAnt.qtdMH = this.resultadoCalculo.qtdMH;
        this.calculaAnt.qtdER = this.resultadoCalculo.qtdER;
        this.calculaAnt.qtdEH = this.resultadoCalculo.qtdEH;
        this.calculaAnt.qtd = this.resultadoCalculo.qtd;
        this.calculaAnt.nota_Doutor = this.resultadoCalculo.nota_Doutor;
        this.calculaAnt.nota_Mestre = this.resultadoCalculo.nota_Mestre;
        this.calculaAnt.nota_Regime = this.resultadoCalculo.nota_Regime;

        this.calculaOriginal.qtdDR = this.resultadoCalculo.qtdDR;
        this.calculaOriginal.qtdDH = this.resultadoCalculo.qtdDH;
        this.calculaOriginal.qtdMR = this.resultadoCalculo.qtdMR;
        this.calculaOriginal.qtdMH = this.resultadoCalculo.qtdMH;
        this.calculaOriginal.qtdER = this.resultadoCalculo.qtdER;
        this.calculaOriginal.qtdEH = this.resultadoCalculo.qtdEH;
        this.calculaOriginal.qtd = this.resultadoCalculo.qtd;
        this.calculaOriginal.nota_Doutor = this.resultadoCalculo.nota_Doutor;
        this.calculaOriginal.nota_Mestre = this.resultadoCalculo.nota_Mestre;
        this.calculaOriginal.nota_Regime = this.resultadoCalculo.nota_Regime;
        
        
        this.calcula = Object.assign(response);
        this.regua  = new previsao();
        this.regua = Object.assign(this.resultadoCalculo.listaPrevisaoSKU);
        this.calculaNotaContinua();
      
      },
      error => {
        console.log(error);
      });
  }

  Recalcula(valor:number, tipo: string){


    let QDR = (tipo == 'DR') ? Number(valor) : this.calcula.qtdDR;
    let QDH = (tipo == 'DH') ? Number(valor) : this.calcula.qtdDH;
    let QMR = (tipo == 'MR') ? Number(valor) : this.calcula.qtdMR;
    let QMH = (tipo == 'MH') ? Number(valor) : this.calcula.qtdMH;
    let QER = (tipo == 'ER') ? Number(valor) : this.calcula.qtdER;
    let QEH = (tipo == 'EH') ? Number(valor) : this.calcula.qtdEH;
    
    this.calcula.qtd += QDR - this.calcula.qtdDR + 
                        QDH - this.calcula.qtdDH +
                        QMR - this.calcula.qtdMR + 
                        QMH - this.calcula.qtdMH + 
                        QER - this.calcula.qtdER + 
                        QEH - this.calcula.qtdEH;

    this.calcula.qtdDR = QDR;
    this.calcula.qtdDH = QDH;
    this.calcula.qtdMR = QMR;
    this.calcula.qtdMH = QMH;
    this.calcula.qtdER = QER;
    this.calcula.qtdEH = QEH;


    this.calculaNotaContinua();


    this.calculaAnt = new calcula();
    this.calculaAnt.qtdDR = this.calcula.qtdDR;
    this.calculaAnt.qtdDH = this.calcula.qtdDH;
    this.calculaAnt.qtdMR = this.calcula.qtdMR;
    this.calculaAnt.qtdMH = this.calcula.qtdMH;
    this.calculaAnt.qtdER = this.calcula.qtdER;
    this.calculaAnt.qtdEH = this.calcula.qtdEH;
    this.calculaAnt.qtd = this.calcula.qtd;
    this.calculaAnt.nota_Doutor = this.calcula.nota_Doutor;
    this.calculaAnt.nota_Mestre = this.calcula.nota_Mestre;
    this.calculaAnt.nota_Regime = this.calcula.nota_Regime;

  }

  resetar() {

    //this.calcula = null;
    this.calcula.qtdDR = this.calculaOriginal.qtdDR;
    this.calcula.qtdDH = this.calculaOriginal.qtdDH;
    this.calcula.qtdMR = this.calculaOriginal.qtdMR;
    this.calcula.qtdMH = this.calculaOriginal.qtdMH;
    this.calcula.qtdER = this.calculaOriginal.qtdER;
    this.calcula.qtdEH = this.calculaOriginal.qtdEH;
    this.calcula.qtd = this.calculaOriginal.qtd;
    this.calcula.nota_Doutor = this.calculaOriginal.nota_Doutor;
    this.calcula.nota_Mestre = this.calculaOriginal.nota_Mestre;
    this.calcula.nota_Regime = this.calculaOriginal.nota_Regime;

    console.log(this.calcula.nota_Doutor)

    this.calculaNotaContinua();

  }


  calculaNotaContinua() {

    let percDoutor = (this.calcula.qtdDH + this.calcula.qtdDR) / this.calcula.qtd;
    let notaDoutor = this.N_Escala(this.regua.p_Min_Doutor, this.regua.p_Max_Doutor, percDoutor);

    console.log(this.regua.p_Max_Doutor)

    let percMestre = (this.calcula.qtdMH + this.calcula.qtdMR + this.calcula.qtdDH + this.calcula.qtdDR) / this.calcula.qtd;
    let notaMestre = this.N_Escala(this.regua.p_Min_Mestre, this.regua.p_Max_Mestre, percMestre);

    // console.log(notaMestre)

    let percRegime = (this.calcula.qtdMR + this.calcula.qtdDR + this.calcula.qtdER) / this.calcula.qtd;
    let notaRegime = this.N_Escala(this.regua.p_Min_Regime, this.regua.p_Max_Regime, percRegime);

    
    // console.log(percRegime)
    this.notaContinua = notaDoutor * 0.5 + notaMestre * 0.25 + notaRegime * 0.25;

    console.log([percDoutor, percRegime, percMestre])

    this.notaFaixa = this.MontaFaixa(this.notaContinua);

  }

  
  N_Escala (lim_min: number,  lim_max: number,  percent: number)
        {

            let n: number;

                n = (percent - lim_min) / (lim_max - lim_min) * 5;

                if (n < 0)
                {
                    return 0;
                }
                else if (n > 5)
                {
                    return 5;
                }

                else
                {
                    let n1 = (n == null) ? 0 : n;
                    return  n1;
                }
           
        }
      
  MontaFaixa(nota: number) {

    switch (true) {
      case nota < 0.945:
        return 1;
       
      case nota < 1.945:
        return 2;
      
      case nota < 2.945:
        return 3;
       
      case nota < 3.945:
         return 4;

      default:
        return 5;
    }

  }

  


}

class dados {
  constructor() { }
  _idEmec: number;
  _idResultado: string;
}

class calcula {

  constructor() {}

  qtdDR: number;
  qtdDH: number;
  qtdMR: number;
  qtdMH: number;
  qtdER: number;
  qtdEH: number;
  qtd : number;
  nota_Doutor: number;
  nota_Mestre: number;
  nota_Regime: number;

}

class previsao {

  constructor() {}

  codArea: number;
  p_Min_Mestre: number;
  p_Max_Mestre: number;
  p_Min_Doutor: number;
  p_Max_Doutor: number;
  p_Min_Regime: number;
  p_Max_Regime: number;
}





