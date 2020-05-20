import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {environment } from '../../environments/environment';
import decode from 'jwt-decode';

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

Delete(name: string) {
  const tokenHeader = this.getToken();
  return this.http.delete(this.baseUrl + `v1/Usuarios/delUsuarios/${name}`, {headers: tokenHeader});
}

AddRole(name: string, role: string) {
  const tokenHeader = this.getToken();
  return this.http.delete(this.baseUrl + `v1/Usuarios/addRole/${name}/${role}`, {headers: tokenHeader});
}

DeleteRole(name: string, role: string) {
  const tokenHeader = this.getToken();
  return this.http.delete(this.baseUrl + `v1/Usuarios/delRole/${name}/${role}`, {headers: tokenHeader});
}

IsMaster(): boolean {
 
  const decodedToken = decode(localStorage.getItem('token'));
  const roles =  decodedToken.Roles;
  if (roles.includes('Master')) {
    return true;
  } else {
    return false;
  }
}

}
