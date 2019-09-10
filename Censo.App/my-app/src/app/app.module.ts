import { BrowserModule} from '@angular/platform-browser';
import { NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { ChartsModule } from 'ng2-charts';
import {FormsModule} from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InicioComponent } from './nav/nav.component';
import { ProfessorComponent } from './Professor/Professor.component';
import { ProfessorIESComponent } from './ProfessorIES/ProfessorIES.component';
import { ProfessorConsultaComponent } from './professor-consulta/professor-consulta.component';

@NgModule({
   declarations: [
      AppComponent,
      InicioComponent,
      ProfessorComponent,
      ProfessorIESComponent,
      ProfessorConsultaComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      ChartsModule,
      FormsModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
