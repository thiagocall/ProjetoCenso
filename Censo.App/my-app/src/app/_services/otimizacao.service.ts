import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OtimizacaoService {

constructor(private http: HttpClient) { }

private baseURL = 'http://localhost:5000/api/v1/censo/CursoEmec/';

Otimizar(obj: any) {
  return this.http.post(`${this.baseURL}Otimizar`, obj);
}

obterResultadosOtimizados() { //resultado da tabela TbResultado
  return this.http.get(this.baseURL + 'ObterResultados');
}

excluirResultadosOtimizados(idResultado: number) {
  console.log("idResultado")
   return this.http.delete(this.baseURL + idResultado);
} 


}
