import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ModGuard implements CanActivate {
  /*canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }*/
  
  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  canActivate(): boolean {
    if(this.authService.loggedIn() && this.authService.isStaff()){
      return true;
    }

    this.alertify.error('You do not have access');
    this.router.navigate(['#']);
  }
  
}
