
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
import { RegulatorioInicioComponent } from './regulatorio/regulatorio-inicio/regulatorio-inicio.component';
import { CorpoDocenteComponent } from './regulatorio-corpo-docente/corpo-docente.component';
import { ProfessorConsultaDetalheComponent } from './professor-consulta-detalhe/professor-consulta-detalhe.component';
import { RegulatorioProfessorIesComponent } from './regulatorio/regulatorio-professor-ies/regulatorio-professor-ies.component';
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
import { AdminUsuarioComponent } from './admin-usuario/admin-usuario.component';

const nivel0 = ['User'];
const nivel1 = ['Master', 'Reg', 'Adm', 'CSC'];
const nivel2 = ['Master', 'Reg', 'Adm'];
const nivel3 = ['Master', 'Adm'];
const nivel4 = ['Master'];

const routes: Routes = [

  /*carregamento de rotas  autenticadas*/
  {path: 'user', component: UserComponent,
    children: [
      {path: 'login', component: LoginComponent },
      {path: 'registration', component: RegistrationComponent },
    ]
  },

  {path: 'Professor', component: ProfessorComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel0
  }},

  /* TESTE */
  {path: 'TesteGrafico', component: TesteGraficoComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel4
  }},
  {path: 'TelaTeste', component: TesteTelaComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel4
  }},
  {path: 'MQD', component: QuadroDoceComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},

  /*ERROR*/
  {path: 'Error', component: ErrorComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel0
  }},

  /* ABA CONSULTA*/
  {path: 'ProfessorConsulta', component: ProfessorConsultaComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel1
  }},
  {path: 'ProfessorConsultaDetalhe/:id', component: ProfessorConsultaDetalheComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel1
  }},

  /* ABA CENSO */
  {path: 'Censo', component: AppCensoComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel0
  }},
  {path: 'DadosCenso', component: AppDadosCensoComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},
  {path: 'CorpoDocente', component: AppCorpoDocenteComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel0
  }},
  {path: 'ComposicaoProfessor', component: AppComposicaoComponent  , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel0
  }},
  {path: 'Resultados/Comparar', component: AppCompararComponent , pathMatch: 'full',  canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},
  {path: 'Resultados/:id', component: DetalheResultadoComponent , pathMatch: 'full', canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},
  {path: 'Resultados',   component: AppResultadosComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},
  {path: 'CalculadoraResultado/:id',   component:  CalculadoraResultadosComponent, canActivate: [AuthGuard], data: {
    expectedRole: nivel3},

  },

  /* ABA REGULATORIO */
  {path: 'Regulatorio', component: RegulatorioComponent , canActivate: [AuthGuard],
  children: [
    {path: 'Inicio', component: RegulatorioInicioComponent},
    {path: 'ProfessorIes', component: RegulatorioProfessorIesComponent},
  ],
    data: {
    expectedRole: nivel2
  }},
  // {path: 'RegulatorioProfessorIes', component: RegulatorioProfessorIesComponent , canActivate: [AuthGuard],
  //   data: {
  //   expectedRole: ['User']
  // }},
  {path: 'RegulatorioProfessorCurso', component: RegulatorioProfessorCursoComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},
  {path: 'RegulatorioProfessorForaDeSede', component: RegulatorioProfessorForaSedeComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},
  {path: 'RegulatorioProfessorGapCargaHoraria', component: RegulatorioGapCargaHorariaComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},
  {path: 'RegulatorioProfessorTermoTiTp', component: RegulatorioTermoTiTpComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},
  {path: 'RegulatorioCorpoDocente', component: CorpoDocenteComponent , canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},

  /* ABA EXPORTAÇÃO*/
  {path: 'Exportacao', component: ExportacaoComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},

    /* SUB DIVISAO */
    {path: 'Exportacao/professores', component: ExpProfessorComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
    {path: 'Exportacao/professores/add', component: ExpProfessorAddComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
    {path: 'Exportacao/professores/add/exp-professor-dadosSalvos', component: ExpProfessorDadosSalvosComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
    {path: 'Exportacao/professores/professor-curso', component: ExpProfessorCursoComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
    {path: 'Exportacao/professores/professor-titulacao', component: ExpProfessorTitulacaoComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},

  /* ABA ENADE */
  {path: 'Enade', component: EnadeComponent ,  canActivate: [AuthGuard] ,
    data: {
    expectedRole: nivel1
  }},
  {path: 'CursoEnade', component: CursoEnadeComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: nivel2
  }},
  {path: 'ManutencaoCicloEnade', component: ManutencaoCursoEnadeComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},
  {path: 'AreaEnade', component: AreaEnadeComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},
  {path: 'ManutencaoCurso', component: ManutencaoCursoComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: nivel3
  }},

  /* ABA REGULATORIO */
  {path: 'Relatorio', component: RelatorioComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
  {path: 'ProducaoQuestoes', component: ProducaoQuestoesComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
  {path: 'PerformanceAlunos', component: PerformanceAlunosComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
  {path: 'DependenciaOnline', component: DependenciaOnlineComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},
  {path: 'AvaliandoAprendizagem', component: AvaliandoAprendizagemComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: ['User']
  }},

  {path: 'Admin', component: AdminUsuarioComponent, canActivate: [AuthGuard],
  data: {
    expectedRole: nivel4
  }
},


  /*INICIO*/
  {path: 'Inicio', component: AppHomeComponent ,  canActivate: [AuthGuard],
    data: {
    expectedRole: nivel0
  }},
  {path: '', component: LoginComponent},
  {path: '**', component: PaginaNaoEncontradaComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
