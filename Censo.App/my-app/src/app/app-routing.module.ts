
import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';

/*teste*/
import { TesteGraficoComponent } from './teste/teste-grafico/teste-grafico.component';
import { TesteTelaComponent } from './teste/teste-tela/teste-tela.component';
/* fim teste */

import { ProfessorComponent } from './Professor/Professor.component';
import { ProfessorConsultaComponent } from './professor-consulta/professor-consulta.component';
import { AppCensoComponent } from './app-censo/app-censo.component';
import { AppCorpoDocenteComponent } from './app-corpo-docente/app-corpo-docente.component';
import { AppDadosCensoComponent } from './app-dados-censo/app-dados-censo.component';
import { PaginaNaoEncontradaComponent } from './PaginaNaoEncontrada/PaginaNaoEncontrada.component';
import { AppHomeComponent } from './app-home/app-home.component';
import { AppComposicaoComponent} from './app-composicao/app-composicao.component';
import { AppResultadosComponent } from './app-resultados/app-resultados.component';
import { DetalheResultadoComponent } from './detalhe-resultado/detalhe-resultado.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthGuard } from './auth/auth.guard';
import { RegulatorioComponent } from './regulatorio/regulatorio.component';
import { CorpoDocenteComponent } from './regulatorio-corpo-docente/corpo-docente.component';
import { ProfessorConsultaDetalheComponent } from './professor-consulta-detalhe/professor-consulta-detalhe.component';
import { RegulatorioProfessorIesComponent } from './regulatorio-professor-ies/regulatorio-professor-ies.component';
import { RegulatorioProfessorCursoComponent } from './regulatorio-professor-curso/regulatorio-professor-curso.component';
import { RegulatorioProfessorForaSedeComponent } from './regulatorio-professor-fora-sede/regulatorio-professor-fora-sede.component';
import { RegulatorioGapCargaHorariaComponent } from './regulatorio-gap-carga-horaria/regulatorio-gap-carga-horaria.component';
import { RegulatorioTermoTiTpComponent } from './regulatorio-termo-ti-tp/regulatorio-termo-ti-tp.component';
import { AppCompararComponent } from './app-resultados/app-comparar/app-comparar.component';
import { CalculadoraResultadosComponent } from './calculadora-resultados/calculadora-resultados.component';
import { QuadroDoceComponent } from './quadro-doce/quadro-doce.component';
import {ExportacaoComponent  } from './exportacao/exportacao.component';
import { ExpProfessorComponent } from './exp-professor/exp-professor.component';
import { ExpProfessorAddComponent } from './exp-professor-add/exp-professor-add.component';
import { ExpProfessorCursoComponent } from './exp-professor-curso/exp-professor-curso.component';
import { ExpProfessorTitulacaoComponent } from './exp-professor-titulacao/exp-professor-titulacao.component';
import { ExpProfessorDadosSalvosComponent } from './exp-professor-dadosSalvos/exp-professor-dadosSalvos.component';
import { EnadeComponent } from './enade/enade.component';
import { CursoEnadeComponent } from './curso-enade/curso-enade.component';
import { ManutencaoCursoEnadeComponent } from './manutencao-curso-enade/manutencao-curso-enade.component';
import { AreaEnadeComponent } from './area-enade/area-enade.component';
import { ErrorComponent } from './error/error.component';
import { RelatorioComponent } from './relatorio/relatorio.component';
import { PerformanceAlunosComponent } from './performanceAlunos/performanceAlunos.component';
import { ProducaoQuestoesComponent } from './producaoQuestoes/producaoQuestoes.component';
import { DependenciaOnlineComponent } from './dependencia-online/dependencia-online.component';
import { AvaliandoAprendizagemComponent } from './avaliando-aprendizagem/avaliando-aprendizagem.component';
import { ManutencaoCursoComponent } from './manutencao-curso/manutencao-curso.component';



const routes: Routes = [

  /*carregamento de rotas  autenticadas*/
  {path: 'user', component: UserComponent,
    children:[
      {path: 'login', component: LoginComponent },
      {path: 'registration', component: RegistrationComponent },
    ]
  },

  {path: 'Professor', component: ProfessorComponent , canActivate: [AuthGuard]},

  /* TESTE */
  {path: 'TesteGrafico', component: TesteGraficoComponent  ,canActivate: [AuthGuard]},
  {path: 'TelaTeste', component: TesteTelaComponent  ,canActivate: [AuthGuard]},
  {path: 'MQD', component: QuadroDoceComponent  ,canActivate: [AuthGuard]},

  /*ERROR*/
  {path: 'Error', component: ErrorComponent  ,canActivate: [AuthGuard]},


  /* ABA CONSULTA*/
  {path: 'ProfessorConsulta', component: ProfessorConsultaComponent  ,canActivate: [AuthGuard]},
  {path: 'ProfessorConsultaDetalhe/:id', component: ProfessorConsultaDetalheComponent ,canActivate: [AuthGuard]},


  /* ABA CENSO */
  {path: 'Censo', component: AppCensoComponent  ,canActivate: [AuthGuard]},
  {path: 'DadosCenso', component: AppDadosCensoComponent  ,canActivate: [AuthGuard]},
  {path: 'CorpoDocente', component: AppCorpoDocenteComponent  ,canActivate: [AuthGuard]},
  {path: 'ComposicaoProfessor', component: AppComposicaoComponent  ,canActivate: [AuthGuard]},
  {path: 'Resultados/Comparar', component: AppCompararComponent ,pathMatch: 'full', canActivate: [AuthGuard]},
  {path: 'Resultados/:id', component: DetalheResultadoComponent ,pathMatch: 'full',canActivate: [AuthGuard]},
  {path: 'Resultados',   component: AppResultadosComponent ,canActivate: [AuthGuard]},
  {path: 'CalculadoraResultado/:id',   component:  CalculadoraResultadosComponent,canActivate: [AuthGuard]}, 


  /* ABA REGULATORIO */
  {path: 'Regulatorio', component: RegulatorioComponent ,canActivate: [AuthGuard]},
  {path: 'RegulatorioProfessorIes', component: RegulatorioProfessorIesComponent ,canActivate: [AuthGuard]},
  {path: 'RegulatorioProfessorCurso', component: RegulatorioProfessorCursoComponent ,canActivate: [AuthGuard]},
  {path: 'RegulatorioProfessorForaDeSede', component: RegulatorioProfessorForaSedeComponent ,canActivate: [AuthGuard]},
  {path: 'RegulatorioProfessorGapCargaHoraria', component: RegulatorioGapCargaHorariaComponent ,canActivate: [AuthGuard]},
  {path: 'RegulatorioProfessorTermoTiTp', component: RegulatorioTermoTiTpComponent ,canActivate: [AuthGuard]},
  {path: 'RegulatorioCorpoDocente', component: CorpoDocenteComponent ,canActivate: [AuthGuard]},

  /* ABA EXPORTAÇÃO*/
  {path: 'Exportacao', component: ExportacaoComponent , canActivate: [AuthGuard]},

    /* SUB DIVISAO */
    {path: 'Exportacao/professores', component: ExpProfessorComponent , canActivate: [AuthGuard]},
    {path: 'Exportacao/professores/add', component: ExpProfessorAddComponent , canActivate: [AuthGuard]},
    {path: 'Exportacao/professores/add/exp-professor-dadosSalvos', component: ExpProfessorDadosSalvosComponent , canActivate: [AuthGuard]},
    {path: 'Exportacao/professores/professor-curso', component: ExpProfessorCursoComponent , canActivate: [AuthGuard]},
    {path: 'Exportacao/professores/professor-titulacao', component: ExpProfessorTitulacaoComponent , canActivate: [AuthGuard]},


  /* ABA ENADE */
  {path: 'Enade', component: EnadeComponent , canActivate: [AuthGuard]},
  {path: 'CursoEnade', component: CursoEnadeComponent , canActivate: [AuthGuard]},
  {path: 'ManutencaoCicloEnade', component: ManutencaoCursoEnadeComponent , canActivate: [AuthGuard]},
  {path: 'AreaEnade', component: AreaEnadeComponent , canActivate: [AuthGuard]},
  {path: 'ManutencaoCurso', component: ManutencaoCursoComponent , canActivate: [AuthGuard]},

  /* ABA REGULATORIO */
  {path: 'Relatorio', component: RelatorioComponent , canActivate: [AuthGuard]},
  {path: 'ProducaoQuestoes', component: ProducaoQuestoesComponent , canActivate: [AuthGuard]},
  {path: 'PerformanceAlunos', component: PerformanceAlunosComponent , canActivate: [AuthGuard]},
  {path: 'DependenciaOnline', component: DependenciaOnlineComponent , canActivate: [AuthGuard]},
  {path: 'AvaliandoAprendizagem', component: AvaliandoAprendizagemComponent , canActivate: [AuthGuard]},


  /*INICIO*/
  {path: 'Inicio', component: AppHomeComponent , canActivate: [AuthGuard]},
  {path: '', component: LoginComponent},
  {path: '**', component: PaginaNaoEncontradaComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
