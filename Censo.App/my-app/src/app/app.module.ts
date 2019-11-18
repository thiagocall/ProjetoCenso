import { BrowserModule} from '@angular/platform-browser';
import { NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { ChartsModule } from 'ng2-charts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, TooltipModule } from 'ngx-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AccordionModule } from 'ngx-bootstrap';



// import {JwPaginationComponent} from 'jw-angular-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InicioComponent } from './nav/nav.component';
import { ProfessorComponent } from './Professor/Professor.component';
import { ProfessorIESComponent } from './ProfessorIES/ProfessorIES.component';
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
import { TesteGraficoComponent } from './teste-grafico/teste-grafico.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { RegulatorioComponent } from './nav-regulatorio/regulatorio/regulatorio.component';
import { CorpoDocenteComponent } from './nav-regulatorio/corpo-docente/corpo-docente.component';
import { ProfessorConsultaDetalheComponent } from './professor-consulta-detalhe/professor-consulta-detalhe.component';

@NgModule({
   declarations: [
      AppComponent,
      InicioComponent,
      ProfessorComponent,
      ProfessorIESComponent,
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
      TesteGraficoComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent,
      RegulatorioComponent,
      CorpoDocenteComponent,
      ProfessorConsultaDetalheComponent
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
      AccordionModule.forRoot()
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
