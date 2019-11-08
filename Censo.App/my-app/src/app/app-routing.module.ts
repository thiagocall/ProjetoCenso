import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import {ProfessorComponent} from './Professor/Professor.component';
import {ProfessorIESComponent} from './ProfessorIES/ProfessorIES.component';
import {ProfessorConsultaComponent} from './professor-consulta/professor-consulta.component';
import {AppCensoComponent } from './app-censo/app-censo.component';
import {AppCorpoDocenteComponent  } from './app-corpo-docente/app-corpo-docente.component';
import {AppDadosCensoComponent  } from './app-dados-censo/app-dados-censo.component';
import {PaginaNaoEncontradaComponent } from './PaginaNaoEncontrada/PaginaNaoEncontrada.component';
import {AppHomeComponent  } from './app-home/app-home.component';
import {AppComposicaoComponent} from './app-composicao/app-composicao.component';
import {AppResultadosComponent } from './app-resultados/app-resultados.component';
import {DetalheResultadoComponent } from './detalhe-resultado/detalhe-resultado.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthGuard } from './auth/auth.guard';



const routes: Routes = [

  /*carregamento de rotas  autenticadas*/
  {path: 'user', component: UserComponent,
    children:[
      {path: 'login', component: LoginComponent },
      {path: 'registration', component: RegistrationComponent },
    ]
  },

  {path: 'Professor', component: ProfessorComponent , canActivate: [AuthGuard]},
  {path: 'ProfessorIES', component: ProfessorIESComponent ,canActivate: [AuthGuard]},
  {path: 'ProfessorConsulta', component: ProfessorConsultaComponent  ,canActivate: [AuthGuard]},
  {path: 'Censo', component: AppCensoComponent  ,canActivate: [AuthGuard]},
  {path: 'CorpoDocente', component: AppCorpoDocenteComponent  ,canActivate: [AuthGuard]},
  {path: 'DadosCenso', component: AppDadosCensoComponent  ,canActivate: [AuthGuard]},
  {path: 'ComposicaoProfessor', component: AppComposicaoComponent  ,canActivate: [AuthGuard]},
  {path: 'Resultados',   component: AppResultadosComponent ,canActivate: [AuthGuard]},
  {path: 'Resultados/:id', component: DetalheResultadoComponent ,canActivate: [AuthGuard]},
  {path: 'Inicio', component: AppHomeComponent ,canActivate: [AuthGuard]},
  {path: '', component: LoginComponent},
  {path: '**', component: PaginaNaoEncontradaComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
