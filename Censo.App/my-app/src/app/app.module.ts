import { BrowserModule} from '@angular/platform-browser';
import { NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { ChartsModule } from 'ng2-charts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, TooltipModule } from 'ngx-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AccordionModule } from 'ngx-bootstrap';
import { ExportAsModule } from 'ngx-export-as';
import { DatePipe } from '@angular/common';


// import {JwPaginationComponent} from 'jw-angular-pagination';

/*teste*/
import { TesteGraficoComponent } from './teste/teste-grafico/teste-grafico.component';
import { TesteTelaComponent } from "./teste/teste-tela/teste-tela.component";
/*fim teste */ 

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InicioComponent } from './nav/nav.component';
import { ProfessorComponent } from './Professor/Professor.component';
import { ProfessorConsultaComponent } from './professor-consulta/professor-consulta.component';
import { AppCensoComponent } from './app-censo/app-censo.component';
import { AppCorpoDocenteComponent } from './app-corpo-docente/app-corpo-docente.component';
import { AppDadosCensoComponent } from './app-dados-censo/app-dados-censo.component';
import { PaginaNaoEncontradaComponent } from './PaginaNaoEncontrada/PaginaNaoEncontrada.component';
import { AppHomeComponent } from './app-home/app-home.component';
import { AppFooterComponent } from './app-footer/app-footer.component';
import { AppComposicaoComponent } from './app-composicao/app-composicao.component';
import { AppResultadosComponent } from './app-resultados/app-resultados.component';
import { DetalheResultadoComponent } from './detalhe-resultado/detalhe-resultado.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { RegulatorioComponent } from './regulatorio/regulatorio.component';
import { CorpoDocenteComponent } from './regulatorio-corpo-docente/corpo-docente.component';
import { ProfessorConsultaDetalheComponent } from './professor-consulta-detalhe/professor-consulta-detalhe.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { RegulatorioProfessorIesComponent } from './regulatorio-professor-ies/regulatorio-professor-ies.component';
import { RegulatorioProfessorCursoComponent } from './regulatorio-professor-curso/regulatorio-professor-curso.component';
import { RegulatorioProfessorForaSedeComponent } from './regulatorio-professor-fora-sede/regulatorio-professor-fora-sede.component';
import { RegulatorioGapCargaHorariaComponent } from './regulatorio-gap-carga-horaria/regulatorio-gap-carga-horaria.component';
import { RegulatorioTermoTiTpComponent } from './regulatorio-termo-ti-tp/regulatorio-termo-ti-tp.component';
import { AppCompararComponent } from './app-resultados/app-comparar/app-comparar.component';
import { CalculadoraResultadosComponent } from './calculadora-resultados/calculadora-resultados.component';
import { QuadroDoceComponent } from './quadro-doce/quadro-doce.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ExportacaoComponent } from './exportacao/exportacao.component';
import { ExpProfessorComponent } from './exp-professor/exp-professor.component';
import { ExpProfessorCursoComponent } from './exp-professor-curso/exp-professor-curso.component';
import { ExpProfessorTitulacaoComponent } from './exp-professor-titulacao/exp-professor-titulacao.component';
import { ExpProfessorAddComponent } from './exp-professor-add/exp-professor-add.component';
import { ExpProfessorDadosSalvosComponent } from './exp-professor-dadosSalvos/exp-professor-dadosSalvos.component';

@NgModule({
   declarations: [
      AppComponent,
      TesteGraficoComponent,
      /*teste*/TesteTelaComponent,
      /*teste*/InicioComponent,
      ProfessorComponent,
      ProfessorConsultaComponent,
      AppCensoComponent,
      AppCorpoDocenteComponent,
      AppDadosCensoComponent,
      PaginaNaoEncontradaComponent,
      AppHomeComponent,
      AppFooterComponent,
      AppComposicaoComponent,
      AppResultadosComponent,
      DetalheResultadoComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent,
      RegulatorioComponent,
      CorpoDocenteComponent,
      ProfessorConsultaDetalheComponent,
      RegulatorioProfessorIesComponent,
      RegulatorioProfessorCursoComponent,
      RegulatorioProfessorForaSedeComponent,
      RegulatorioGapCargaHorariaComponent,
      RegulatorioTermoTiTpComponent,
      AppCompararComponent,
      CalculadoraResultadosComponent,
      QuadroDoceComponent,
      ExportacaoComponent,
      ExpProfessorComponent,
      ExpProfessorCursoComponent,
      ExpProfessorTitulacaoComponent,
      ExpProfessorAddComponent,
      ExpProfessorDadosSalvosComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      ChartsModule,
      FormsModule,
      ReactiveFormsModule,
      ModalModule.forRoot(),
      TooltipModule.forRoot(),
      BrowserAnimationsModule,
      ToastrModule.forRoot(),
      AccordionModule.forRoot(),
      NgxPaginationModule,
      ExportAsModule,
      BsDatepickerModule.forRoot(),
      //NgxChartsModule
   ],
   providers: [DatePipe],
   bootstrap: [
      AppComponent
   ]
})

export class AppModule { }
