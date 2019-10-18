import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import {ProfessorComponent} from './Professor/Professor.component';
import {ProfessorIESComponent} from './ProfessorIES/ProfessorIES.component';
import {ProfessorConsultaComponent} from './professor-consulta/professor-consulta.component';
import {AppCensoComponent } from './app-censo/app-censo.component';
import {AppCorpoDocenteComponent  } from './app-corpo-docente/app-corpo-docente.component';
import {AppDadosCensoComponent  } from './app-dados-censo/app-dados-censo.component';
import { PaginaNaoEncontradaComponent } from './PaginaNaoEncontrada/PaginaNaoEncontrada.component';
import {AppHomeComponent  } from './app-home/app-home.component';
import {AppComposicaoComponent} from './app-composicao/app-composicao.component';
import { AppResultadosComponent } from './app-resultados/app-resultados.component';
import { DetalheResultadoComponent } from './detalhe-resultado/detalhe-resultado.component';


const routes: Routes = [

  {path: 'Professor', component: ProfessorComponent },
  {path: 'ProfessorIES', component: ProfessorIESComponent },
  {path: 'ProfessorConsulta', component: ProfessorConsultaComponent },
  {path: 'Censo', component: AppCensoComponent },
  {path: 'CorpoDocente', component: AppCorpoDocenteComponent },
  {path: 'DadosCenso', component: AppDadosCensoComponent },
  {path: 'ComposicaoProfessor', component: AppComposicaoComponent },
  {path: 'Resultados',   component: AppResultadosComponent},
  {path: 'Resultados/:id', component: DetalheResultadoComponent},
  {path: '', component: AppHomeComponent},
  {path: '**', component: PaginaNaoEncontradaComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
