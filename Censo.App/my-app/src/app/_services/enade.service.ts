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

campus() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/dados/getCampus/', { headers: tokenHeader });
}

getDados() {
  const tokenHeader = this.getToken();
  return this.http.get(this.baseUrl + 'v1/dados/' ,{headers: tokenHeader});
}

}
