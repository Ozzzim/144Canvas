import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  register(){
    //console.log(this.model);
    this.authService.register(this.model).subscribe(()=>{
      this.alertify.success('Registration was successful!');
        this.authService.login(this.model).subscribe(//MIGHT NOT WORK AFTER USER MODEL UPGRADE
        next =>{}, 
        error => {this.alertify.error('Login failed though.');});
      this.router.navigate(['#'])
    }, error => {
      this.alertify.error('Register Error: Unknown');
    }
    );
  }

  cancel(){
    console.log("Cancel");
  }
}
