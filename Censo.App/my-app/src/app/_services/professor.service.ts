import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProfessorService {

constructor(private http: HttpClient) { }

baseUrl = 'http://localhost:5000/api/';

getProfessores() {

  return this.http.get(this.baseUrl + 'Professor');

}

buscarProfessores(campo: string) {

  return this.http.get(this.baseUrl + 'Professor/Busca/' + campo);

}

getDados() {

  return this.http.get(this.baseUrl + 'v1/dados/');

}

getInfoCurso(codigo: string) {

  return this.http.get(this.baseUrl + 'v1/censo/cursoEmec/obterInfoCurso/' + codigo);

}


}
