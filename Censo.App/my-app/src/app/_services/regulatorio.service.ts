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
    return this.http.get(this.baseUrl + 'regulatorio/BuscaIes/excel', { responseType: 'blob', headers: tokenHeader });
  }

  // REGULATÓRIO PROFESSOR IES

  getRegulatorioProfessorIesExcel(_ies: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/buscaiesID/excel/' + _ies, { responseType: 'blob', headers: tokenHeader });
  }

  getRegulatorioBuscaIes(codigo: string) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/buscaies/' + codigo, { headers: tokenHeader });
  }

  getIes() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/dados/geties/', { headers: tokenHeader });
  }

  // REGULATÓRIO PROFESSOR CURSO

  getRegulatorioProfessorCurso(codigo: number) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/Emec/' + codigo, { headers: tokenHeader });
  }

  getCampus() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/dados/getCampus/', { headers: tokenHeader });
  }

  // REGULATÓRIO PROFESSOR FORA DE SEDE

  getRegulatorioProfessorForaSedeExcel(_campus: any) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/foradesede/excel/' + _campus, { responseType: 'blob', headers: tokenHeader });
  }


  getResultadoProfessorForaSede(codigo: string) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/foradesede/' + codigo, { headers: tokenHeader });
  }

  getCampusForaSede() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/Buscacampus', { headers: tokenHeader });
  }

}
