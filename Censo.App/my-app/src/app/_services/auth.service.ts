import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  
  baseURL = 'http://localhost:5000/api/v1/usuarios/';
  jwtHelper = new JwtHelperService();
  decodedToken: any; //token que será decodificado

  constructor(private http: HttpClient) { }

  login(model: any) {
    //console.log(model)
    return this.http
      .post(`${this.baseURL}login`, model).pipe( //post em 'http://localhost:5000/api/v1/usuarios/'
        map((response: any) => {
          const user = response;
         // console.log(user);
          if (user) {
            localStorage.setItem('token', user.token); //salvando o token dentro do localStorage
            this.decodedToken = this.jwtHelper.decodeToken(user.token); //decodificar o token 
            sessionStorage.setItem('username', this.decodedToken.unique_name);
          }
        })
      );
  }

  register(model: any) {
    return this.http.post(`${this.baseURL}criar`, model);
  }

  loggedIn() {
    const token = localStorage.getItem('token'); // vai pegar o token do localStorage
    return !this.jwtHelper.isTokenExpired(token);
  }

  logout() {
    localStorage.removeItem('token');
  }

}
