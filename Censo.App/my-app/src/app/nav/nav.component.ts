import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { UsuarioService } from 'src/app/_services/usuarios.service';
import { ToastrService } from 'ngx-toastr';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-inicio',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class InicioComponent implements OnInit {

  constructor(
                private authService: AuthService
              , private usuarioService: UsuarioService
              , public router: Router
              , private toastr: ToastrService) { }

  professores = [];
  JwtHelper = new JwtHelperService();

  ngOnInit() {

    this.professores = ['Thiago Caldas', 'Marcus Sales', 'Cesar Augusto'];
  }
  loggedIn() {
    return this.authService.loggedIn();
  }

  showMenu() {
    return localStorage.getItem('token') !== null;
  }

  logout() {
    localStorage.removeItem('token');
    this.toastr.success('Log Out');
    this.router.navigate(['/user/login']);
  }

  userName() {
    // return sessionStorage.getItem('username').split('.')[0];
    const token = this.JwtHelper.decodeToken(localStorage.getItem('token'));
    return token.unique_name.split('.')[0];
  }

  IsMaster(): boolean {
    return this.usuarioService.IsMaster();
  }

}
