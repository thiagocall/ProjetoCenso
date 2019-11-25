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

  getRegulatorioCorpoDocenteExcel() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/BuscaIes/excel', { responseType: 'blob', headers: tokenHeader });
  }

  getRegulatorioProfessorIesExcel(codigo: string) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/buscaies/' + codigo, { responseType: 'blob', headers: tokenHeader });
  }

  getRegulatorioBuscaIes(codigo: string) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'regulatorio/buscaies/' + codigo, { headers: tokenHeader });
  }

  getIes() {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + 'v1/dados/geties/', { headers: tokenHeader });
  }



}
