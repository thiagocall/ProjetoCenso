import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import decode from 'jwt-decode';
import { AuthService } from 'src/app/_services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {

  constructor(private router: Router,
              private authService: AuthService,
              private toastr: ToastrService) {

  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean  {
    const expectedRoles = next.data.expectedRole;
    const token = localStorage.getItem('token');
    const decodedToken = decode(token);
    const roles =  decodedToken.Roles;
    // console.log('regras rota: ', expectedRoles);
    // console.log('regras usuário: ', roles);
    // roles = decodedToken.Roles;
    if (this.authService.loggedIn() && ((expectedRoles.some(r => roles.includes(r))) || expectedRoles === undefined)) {
      return true;
    } else {
        this.toastr.warning('Usuário não possui acesso', null, {
          timeOut: 2000,
          });
        return false; // this.router.navigate(['/Inicio']);
    }
  }

}
