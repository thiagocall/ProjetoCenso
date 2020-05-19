import { Component, OnInit } from '@angular/core';
import { UsuarioService} from '../_services/usuarios.service';

@Component({
  selector: 'app-admin-usuario',
  templateUrl: './admin-usuario.component.html',
  styleUrls: ['./admin-usuario.component.css']
})
export class AdminUsuarioComponent implements OnInit {

  constructor(private usuarioService: UsuarioService) { }

  usuarios: any;

  getUsuarios() {
    this.usuarioService.getUsuarios().subscribe(
      response => {
        this.usuarios = response;
        console.log(this.usuarios);
      },
      error => {}
    );


  }

  ngOnInit() {

    this.getUsuarios();
  }

}
