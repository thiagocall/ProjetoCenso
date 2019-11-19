import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class OtimizacaoService {

  constructor(private http: HttpClient) { }

  private baseURL = environment.apiUrl + 'v1/censo/CursoEmec/';

  getToken() {
    const tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${localStorage.getItem('token')}` });
    return tokenHeader;
  }

  Otimizar(obj: any) {
    const tokenHeader = this.getToken();
    return this.http.post(`${this.baseURL}Otimizar`, obj, { headers: tokenHeader });
  }

  obterResultadosOtimizados() { // resultado da tabela TbResultado
    const tokenHeader = this.getToken();
    return this.http.get(this.baseURL + 'ObterResultados', { headers: tokenHeader });
  }

  excluirResultadosOtimizados(idResultado: number) {
    console.log('idResultado');
    const tokenHeader = this.getToken();
    return this.http.delete(this.baseURL + idResultado, { headers: tokenHeader });
  }


  obterDetalheResultado(id: number) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseURL + 'ObterResultados/' + id, { headers: tokenHeader });
  }

  exportarResultadoExcel(id: number) {
    const tokenHeader = this.getToken();
    return this.http.get(this.baseURL + 'Resultado/Excel/' + id, {responseType: 'blob', headers: tokenHeader});
  }
}
