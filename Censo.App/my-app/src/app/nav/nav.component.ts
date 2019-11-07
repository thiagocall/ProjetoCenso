import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-inicio',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class InicioComponent implements OnInit {

  constructor(private authService: AuthService
  , public router: Router) { }

  professores = [];

  ngOnInit() {

    this.professores = ['Thiago Caldas', 'Marcus Sales', 'Cesar Augusto'];
  }
  
  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['user/login']);
  }

}
