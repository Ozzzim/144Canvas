import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiURL + 'auth/'//'http://localhost:5050/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl+'login', model)
    .pipe(map((response: any)=> {
      
      const user = response;
      if(user){
        localStorage.setItem('token', user.token);
        this.decodedToken=this.jwtHelper.decodeToken(user.token);
      }
    }));
  }

  register(model: any){
    return this.http.post(this.baseUrl+'register',model);
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  changePassword(model: any){
    return this.http.post(this.baseUrl+'password',model);
  }

  isRoles(allowedRoles):boolean{
    const userRoles = this.decodedToken.role as Array<string>;
    allowedRoles.array.forEach(role => {
      if(userRoles.includes(role))
        return true;
    });
    return false;
  }

  isStaff():boolean{
    const userRoles = this.decodedToken.role as Array<string>;
    return userRoles.includes("Mod") || userRoles.includes("Admin");
  }

  isAdmin():boolean{
    const userRoles = this.decodedToken.role as Array<string>;
    return userRoles.includes("Admin");
  }
}
