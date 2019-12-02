import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-app-censo',
  templateUrl: './app-censo.component.html',
  styleUrls: ['./app-censo.component.css']
})
export class AppCensoComponent implements OnInit {

  constructor( private router: Router) { }


  AcessaCorpoDocente() {

    this.router.navigate(['/CorpoDocente']);

  }


  ngOnInit() {
  }




}
