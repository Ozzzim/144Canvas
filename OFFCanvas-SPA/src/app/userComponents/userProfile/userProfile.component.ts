import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/User';
import { Picture } from 'src/app/_models/Picture';
import {Comment} from 'src/app/_models/Comment';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-userProfile',
  templateUrl: './userProfile.component.html',
  styleUrls: ['./userProfile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: User;
  pictures: Picture[];
  comments: Comment[];

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadUser();
  }

  loadUser(){
    
    this.userService.getUser(+this.route.snapshot.params['id'])
      .subscribe((user: User) => {
        //console.log("Loading user: "+user.username);
        this.user=user;
      }, error => {
        this.alertify.error(error);
      }
    );
    //this.comments=this.user.comments;
    //this.pictures=this.user.pictures;

    /*this.userService.getUsersPictures(+this.route.snapshot.params['id']).subscribe((pictures: Picture[]) => {
      this.pictures= pictures;
    }, error =>{
      this.alertify.error(error);
    }
    );
    this.userService.getUsersComments(+this.route.snapshot.params['id']).subscribe((comments: Comment[]) => {
      this.comments= comments;
    }, error =>{
      this.alertify.error(error);
    }
    );*/    
  }

  getPath():string{
    //console.log(UserService.getProfilePicturePath(this.user)+"");
    return UserService.getProfilePicturePath(this.user);
  }

}
