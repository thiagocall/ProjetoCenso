import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
 //apiUrl: 'http://localhost:5000/api/'
 
export class ProfessorService {

constructor(private http: HttpClient) { }

baseUrl = environment.apiUrl;
getToken() {
  const tokenHeader = new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('token')}`});
  return tokenHeader;
}

getProfessores() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Professor', {headers: tokenHeader}); /*v1*/
}

buscarProfessores(campo: string) {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Professor/Busca/' + campo, {headers: tokenHeader}); /*v1*/
}

professorConsultaDetalhe(campo: string) {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Professor/BuscaDetalhe/' + campo, {headers: tokenHeader}); /*v1*/
} 

getDados() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/dados/' ,{headers: tokenHeader});
}

getInfoCurso(codigo: string) {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/censo/cursoEmec/obterInfoCurso/' + codigo, {headers: tokenHeader});
}

/*EXCEL */
getProfessorExcel() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Professor/ProfessorCenso/Excel/', {responseType: 'blob', headers: tokenHeader}); /*v1*/
}

/*CENSO - RESULTADOS  */
postCalculadoraResultado(dados:any){
const tokenHeader = this.getToken();
  return this.http.post(this.baseUrl + 'v1/censo/CursoEmec/GetDadosCalculadora', dados, {headers: tokenHeader});
}

/*REGULATORIO / GERA TERMO TI-TP /  MQD*/
pesquisaDocente() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Professor/PesquisaCPFDOCENTE/', {headers: tokenHeader}); /*v1*/
}



}

