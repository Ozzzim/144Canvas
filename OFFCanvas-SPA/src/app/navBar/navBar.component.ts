import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service'

@Component({
  selector: 'app-navBar',
  templateUrl: './navBar.component.html',
  styleUrls: ['./navBar.component.scss']
})
export class NavBarComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login(){
    //console.log('USERNAME:'+this.model.username);
    this.authService.login(this.model).subscribe(next =>{
      this.alertify.success('Login successful!');//console.log('Login successful!');
    }, error => {
      this.alertify.error('Login failed...');
    });
    //console.log(this.model);
  }

  loggedIn(){
    //const token = localStorage.getItem('token');
    return this.authService.loggedIn();//!!token;
  }

  logout(){
    localStorage.removeItem('token');
    this.alertify.message('See you!');
    this.router.navigate(['/home'])
    //console.log('Log out!');
  }

  getUsername(){
    return this.authService.decodedToken.unique_name;
  }

  isStaff():boolean{
    return this.authService.isStaff();
  }
}
