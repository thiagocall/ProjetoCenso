import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})

export class ExportacaoService {

  constructor(private http: HttpClient) { }

  // apiUrl: 'http://localhost:5000/api/'
  baseUrl = environment.apiUrl;
  getToken() {
    const tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${localStorage.getItem('token')}` });
    return tokenHeader;
  }

  // EXPORTAÇÃO 

  /* ADICIONAR PROFESSOR */
  exportacaoProfessor(professores: any) {
    const tokenHeader = this.getToken();
    return this.http.post(this.baseUrl + 'v1/Exportacao/DevolveProf', professores, { headers: tokenHeader }); /*v1*/
  }

  /* GERAR EXPORTAÇÃO CENSO */
  getExportarCensoExcel(id = 0) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseUrl + `v1/Exportacao/Geracao/Excel/${id}`, { responseType: 'blob', headers: tokenHeader }); /*v1*/
  }

}


