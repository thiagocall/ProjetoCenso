import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

constructor(private http: HttpClient) { }
baseUrl = environment.apiUrl;

getToken() {
  const tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${localStorage.getItem('token')}` });
  return tokenHeader;
}

getUsuarios() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/Usuarios/getUsuarios', {headers: tokenHeader});
}

}
