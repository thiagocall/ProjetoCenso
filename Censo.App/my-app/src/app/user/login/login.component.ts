import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/auth.service';
import { $ } from 'protractor';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';



@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  titulo = 'Login';
  model: any = {};

  mostrarSpinner = false;

  registerForm: FormGroup;

  constructor(private authService: AuthService
    , public fb: FormBuilder
    , public router: Router
    , private toastr: ToastrService) { }

  ngOnInit()
   {

    this.validation();
    
    if (this.authService.loggedIn()) {
      this.router.navigate(['Inicio']);
    }
  }

  login() {
    this.mostrarSpinner = true;
    
    // document.getElementById('btnLog').classList.replace("","");
    this.authService.login(this.model)
      .subscribe(
        () => {
          this.router.navigate(['Inicio']);
          this.toastr.success('Logado com Sucesso', null, {
            timeOut: 2000
          });
        },
        error => {
          this.toastr.error('Falha ao tentar Logar', null, {
            timeOut: 2000
          });
          this.mostrarSpinner = false;
        }
      );
      
      

    console.log(this.authService.decodedToken);
  }






validation() {
  this.registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });
}


}