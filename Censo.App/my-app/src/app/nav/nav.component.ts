import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-inicio',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class InicioComponent implements OnInit {

  constructor(private authService: AuthService
  , public router: Router
  , private toastr: ToastrService) { }

  professores = [];

  ngOnInit() {

    this.professores = ['Thiago Caldas', 'Marcus Sales', 'Cesar Augusto'];
  }
  
  loggedIn() {
    return this.authService.loggedIn();
  }

  showMenu() {
    return this.router.url !== '/user/login';
  }

  logout() {
    localStorage.removeItem('token');
    this.toastr.show('Log Out');
    this.router.navigate(['/user/login']);
  }

  userName() {
    return sessionStorage.getItem('username');
  }

}
