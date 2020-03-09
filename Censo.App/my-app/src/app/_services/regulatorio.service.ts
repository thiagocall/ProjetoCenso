import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class RegulatorioService {

  constructor(private http: HttpClient) { }

  //apiUrl: 'http://localhost:5000/api/'
  baseUrl = environment.apiUrl;
  getToken() {
    const tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${localStorage.getItem('token')}` });
    return tokenHeader;
  }

  // REGULATÓRIO CORPO DOCENTE 

  getRegulatorioCorpoDocenteExcel() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/BuscaIes/excel', { responseType: 'blob', headers: tokenHeader }); /*v1*/
  }

  // REGULATÓRIO PROFESSOR IES

  getRegulatorioProfessorIesExcel(_ies: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/buscaiesID/excel/' + _ies, { responseType: 'blob', headers: tokenHeader }); 
  }

  
  getRegulatorioBuscaIes(codigo: string) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/buscaies/' + codigo, { headers: tokenHeader }); /*v1*/
  }

  getIes() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/dados/geties/', { headers: tokenHeader });
  }

  // REGULATÓRIO PROFESSOR CURSO

  getRegulatorioProfessorCurso(codigo: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/Emec/' + codigo, { headers: tokenHeader }); /*v1*/
  }

  getRegulatorioProfessorCursoExcel(codigo: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/Emec/Excel/' + codigo, { responseType: 'blob', headers: tokenHeader }); /*v1*/
  }

  getRegulatorioProfessorCampusExcel(codigo: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/Emec/ExcelCampus/' + codigo, { responseType: 'blob', headers: tokenHeader }); /*v1*/
  }

  getCampus() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/dados/getCampus/', { headers: tokenHeader });
  }
 
  // REGULATÓRIO PROFESSOR FORA DE SEDE

  getRegulatorioProfessorForaSedeExcel(_campus: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/foradesede/excel/' + _campus, { responseType: 'blob', headers: tokenHeader }); /*v1*/
  }


  getResultadoProfessorForaSede(codigo: string) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/foradesede/' + codigo, { headers: tokenHeader }); /*v1*/
  }

  getCampusForaSede() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/Buscacampus', { headers: tokenHeader }); /*v1*/
  }

  // REGULATÓRIO GAP CARGA HORÁRIA

  PesquisaProfessores() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/regulatorio/BuscaProfessor', { headers: tokenHeader }); /*v1*/
  }

  /*post */
  postCalculaGapProf(professores: any[]) {
    const tokenHeader = this.getToken();
    return this.http.post(this.baseUrl + 'v1/regulatorio/CalculaGapProf', professores, { headers: tokenHeader }); /*v1*/
  }


}

