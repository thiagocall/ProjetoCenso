import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  titulo = 'Login';
  model: any = {};

  constructor(private authService: AuthService
    , public router: Router
    , private toastr: ToastrService) { }

  ngOnInit() {
    if (this.authService.loggedIn()) {
      this.router.navigate(['Inicio']);
    }
  }

  login() {
    this.authService.login(this.model)
      .subscribe(
        () => {
          this.router.navigate(['Inicio']);
          this.toastr.success('Logado com Sucesso',null, {
            timeOut: 2000
          });
        },
        error => {
          this.toastr.error('Falha ao tentar Logar',null, {
            timeOut: 2000
          });
        }
      );

      console.log(this.authService.decodedToken);
  }

}