import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import {ProfessorComponent} from './Professor/Professor.component';
import {ProfessorIESComponent} from './ProfessorIES/ProfessorIES.component';
import {ProfessorConsultaComponent} from './professor-consulta/professor-consulta.component';


const routes: Routes = [

  // {path: '', redirectTo: 'localhost:4200'},
  {path: 'Professor', component: ProfessorComponent },
  {path: 'ProfessorIES', component: ProfessorIESComponent },
  {path: 'ProfessorConsulta', component: ProfessorConsultaComponent },
  // {path: '**', component: PageNotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
