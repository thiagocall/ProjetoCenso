import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})

export class ProfessorService {

constructor(private http: HttpClient) { }

baseUrl = environment.apiUrl;
getToken() {
  const tokenHeader = new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('token')}`});
  return tokenHeader;
}

getProfessores() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'Professor', {headers: tokenHeader});
}

buscarProfessores(campo: string) {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'Professor/Busca/' + campo, {headers: tokenHeader});
}

/* Consulta - Professores */
professorConsultaDetalhe(campo: string) {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/api/Professor/BuscaDetalhe/' + campo, {headers: tokenHeader});
} 

getDados() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/dados/' ,{headers: tokenHeader});
}

getInfoCurso(codigo: string) {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/censo/cursoEmec/obterInfoCurso/' + codigo, {headers: tokenHeader});
}

/*EXCEL - PROFESSOR - EXPORTAR */
getProfessorExcel() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Professor/ProfessorCenso/Excel/', {responseType: 'blob', headers: tokenHeader});
}

/*CENSO - RESULTADOS  */
postCalculadoraResultado(dados:any){
const tokenHeader = this.getToken();
  return this.http.post(this.baseUrl + 'v1/censo/CursoEmec/GetDadosCalculadora', dados, {headers: tokenHeader});
}

/*MQD - REGULATORIO - GERA TERMO TI/TP*/
pesquisaDocente() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'Professor/PesquisaCPFDOCENTE/', {headers: tokenHeader});
}



}

