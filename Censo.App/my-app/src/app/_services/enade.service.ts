import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnadeService {

constructor(private http: HttpClient) { }
baseUrl = environment.apiUrl;

getToken() {
  const tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${localStorage.getItem('token')}` });
  return tokenHeader;
}


/*CAMPUS E CURSO*/
getDados() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/dados/' ,{headers: tokenHeader});
}

/* ObterCiclos  (TODOS - ANO I, ANO II, ANO III) */
obterCiclos() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Enade/ObterCiclos', {headers: tokenHeader});
}

/* cicloSelecionado  (é o curso selecionado do obterCiclos) */
cicloSelecionadodeObterCiclos() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Enade/SelecionaCiclos', {headers: tokenHeader});
}


/* SOMENTE CAMPUS */
selectCampus() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Enade/TodosCampus', {headers: tokenHeader});
}

/*RESULTADO TABELA - TESTE*/
resultadoTabela(codCampus:any) {
 /* const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Enade/ObtemDadosEnade'  , {headers: tokenHeader}); */
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Enade/ObtemDadosEnade/' + codCampus , {headers: tokenHeader});
}

salvarIdCiclo(data:any){
  const tokenHeader = this.getToken();
  return this.http.post(this.baseUrl + 'v1/Enade/salvarIdCiclo/' , data , {headers: tokenHeader});
}



}