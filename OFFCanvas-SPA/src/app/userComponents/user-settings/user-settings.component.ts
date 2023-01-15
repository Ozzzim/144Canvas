import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/_models/User';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import {AuthService} from '../../_services/auth.service'

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {
  backgroundModel: any = {};
  passwordModel: any = {};
  aboutModel: any = {};

  currentBackground: string;
  user: User;

  constructor(private authService: AuthService, private alertify: AlertifyService, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    if(this.loggedIn){
      this.userService.getUser(this.authService.decodedToken.nameid).subscribe((user: User) => {
        this.user=user;
        if(user.backgroundURL=="0")
          this.currentBackground=null;
        else
          this.currentBackground=UserService.getProfilePicturePath(user);
      }, error => {
        this.alertify.error(error);
      });
    }
  }

  loggedIn(){
    //const token = localStorage.getItem('token');
    if(!this.authService.loggedIn()){
      this.alertify.error("You're not logged in!");
      this.router.navigate(['/home'])
      return false;
    }
    return true;
  }

  pickPicture(id: number){
    this.backgroundModel.newPicID=id;
  }

  postBGChange(){
    if(this.loggedIn){
      this.backgroundModel.userID=+this.authService.decodedToken.nameid;
      this.userService.changeBG(this.backgroundModel).subscribe(next =>{
        this.userService.getUser(this.authService.decodedToken.nameid).subscribe((user: User) => {
          this.user=user;
          this.currentBackground=UserService.getProfilePicturePath(user);
        }, error => {
          this.alertify.error("Error while updating the view!");
        });
        this.alertify.success('Background successfully changed!');//console.log('Login successful!');
      }, error => {
        this.alertify.error("Couldn't change the background...");
      });
    } else
      this.alertify.error("You're not logged in!");
  }

  postAboutChange(){
    if(this.loggedIn){
      this.aboutModel.userID=+this.authService.decodedToken.nameid;
      this.userService.changeAbout(this.aboutModel).subscribe(next =>{
        this.userService.getUser(this.authService.decodedToken.nameid).subscribe((user: User) => {
          this.user=user;
        }, error => {
          this.alertify.error("Error while updating the view!");
        });
        this.alertify.success('About successfully changed!');//console.log('Login successful!');
      }, error => {
        this.alertify.error("Couldn't update about...");
      });
    } else
      this.alertify.error("You're not logged in!");
  }

  postPasswordChange(){
    if(this.loggedIn){
      if(this.passwordModel.nPass!=this.passwordModel.nPassR){
        this.alertify.error("New password doesn't match the repeated password!");
      } else {
        this.passwordModel.userID=+this.authService.decodedToken.nameid;
        this.authService.changePassword(this.passwordModel).subscribe(next =>{
          this.alertify.success('Password successfully changed!');//console.log('Login successful!');
        }, error => {
          this.alertify.error("Couldn't update the password...");
        });
      }
    } else
      this.alertify.error("You're not logged in!");
  }

  getPath():string{
    //console.log(UserService.getProfilePicturePath(this.user)+"");
    return UserService.getProfilePicturePath(this.user);
  }
}
