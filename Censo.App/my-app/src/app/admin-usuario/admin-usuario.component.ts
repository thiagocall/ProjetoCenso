import { Component, OnInit } from '@angular/core';
import { UsuarioService} from '../_services/usuarios.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-usuario',
  templateUrl: './admin-usuario.component.html',
  styleUrls: ['./admin-usuario.component.css']
})
export class AdminUsuarioComponent implements OnInit {

  constructor(private usuarioService: UsuarioService,
              private toastr: ToastrService,
              private modalService: BsModalService ) { }

  resultado: any;
  usuarios: any;
  usuarioSelecionado: any;
  modalRef: BsModalRef;
  remUsuario: any;
  roles: any;

  getUsuarios() {
    this.usuarioService.getUsuarios().subscribe(
      response => {
        this.resultado = response;
        this.usuarios = this.resultado.usersInfo;
        console.log(this.resultado.roles);
      },
      error => {}
    );

  }

  removerUsuario() {
    this.usuarioService.Delete(this.remUsuario).subscribe(
      response => {
        this.toastr.success('Usuário removido', null, {
          timeOut: 2000,
          });
        this.cancelar();
        this.getUsuarios();

      },
      error => {}
    );
  }

  removerRole(name: string, role: string) {
    this.usuarioService.DeleteRole(name, role).subscribe(
      response => {
        this.toastr.success('Perfil removido', null, {
          timeOut: 2000,
          });
        this.getUsuarios();
      },
      error => {
        this.toastr.warning('Algo deu errado', null, {
          timeOut: 2000,
          });
      }
    );
  }

  addRole(name: string, role: string) {
    this.usuarioService.AddRole(name, role).subscribe(
      response => {
        this.toastr.success('Perfil adicionado', null, {
          timeOut: 2000,
          });
        this.getUsuarios();

      },
      error => {
        this.toastr.warning('Algo deu errado', null, {
          timeOut: 2000,
          });
      }
    );
  }

  selecionaUsuario(name: string){

    this.usuarioSelecionado = this.usuarios.filter(x => x.email === name)[0];
    this.roles = this.resultado.roles;

  }

  modalExcluir(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  cancelar(): void {
    this.modalRef.hide(); // fecha o modal
  }

  remover(template: any, name: string){
    this.remUsuario = name;
    this.modalExcluir(template);
  }

  ngOnInit() {

    this.getUsuarios();
  }

}
