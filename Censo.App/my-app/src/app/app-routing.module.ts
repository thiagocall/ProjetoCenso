import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import {ProfessorComponent} from './Professor/Professor.component';
import {ProfessorIESComponent} from './ProfessorIES/ProfessorIES.component';
import {ProfessorConsultaComponent} from './professor-consulta/professor-consulta.component';
import {AppCensoComponent } from './app-censo/app-censo.component';
import {AppCorpoDocenteComponent  } from './app-corpo-docente/app-corpo-docente.component';
import {AppDadosCensoComponent  } from './app-dados-censo/app-dados-censo.component';


const routes: Routes = [

  // {path: '', redirectTo: 'localhost:4200'},
  {path: 'Professor', component: ProfessorComponent },
  {path: 'ProfessorIES', component: ProfessorIESComponent },
  {path: 'ProfessorConsulta', component: ProfessorConsultaComponent },
  {path: 'Censo', component: AppCensoComponent },
  {path: 'CorpoDocente', component: AppCorpoDocenteComponent },
  {path: 'DadosCenso', component: AppDadosCensoComponent },

  // {path: '**', component: PageNotFoundComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
