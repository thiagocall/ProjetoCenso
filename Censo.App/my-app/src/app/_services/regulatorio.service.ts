import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class RegulatorioService {

constructor(private http: HttpClient) { }

baseUrl = environment.apiUrl;
getToken() {
  const tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${localStorage.getItem('token')}` });
  return tokenHeader;
}


getRegulatorioCorpoDocenteExcel() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'regulatorio/BuscaIes/excel', {responseType: 'blob', headers: tokenHeader});
}

}
