import { Component, Input, OnInit } from '@angular/core';
import { Picture } from 'src/app/_models/picture';
import { Like } from 'src/app/_models/Like';
import { LikeService } from 'src/app/_services/like.service';
import { PictureService } from 'src/app/_services/picture.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-pictureCard',
  templateUrl: './pictureCard.component.html',
  styleUrls: ['./pictureCard.component.css']
})
export class PictureCardComponent implements OnInit {
  @Input() picture: Picture;

  likeModel : any = {};
  liked: boolean=false;
  likesCount: number=0;
  
  constructor(private pictureService: PictureService,private likeService: LikeService, private alertify: AlertifyService, private authService: AuthService) { }  

  ngOnInit() {
    this.updateLikesCount();
    this.likeService.getLike(+this.authService.decodedToken.nameid,this.picture.id)
      .subscribe((like: Like) => {
        this.liked=(like!=null);
      }, error => {
        this.liked=false;
      });
    
  }
  getPath():string{
    return this.pictureService.getImagePath(this.picture);
  }
  likeAction(){
    if(this.authService.loggedIn()){
      this.likeModel.userID=+this.authService.decodedToken.nameid;
      this.likeModel.pictureID=this.picture.id;
      if(this.liked){
        this.likeService.unlike(this.likeModel).subscribe(next =>{
          this.liked=false;
          this.updateLikesCount();
        }, error => {
          this.alertify.error("Error: Failed to unlike the picture!");
        });
      }else{
        this.likeService.like(this.likeModel).subscribe(next =>{
          this.liked=true;
          this.updateLikesCount();
        }, error => {
          this.alertify.error("Error: Failed to like the picture!");
        });
      }
    } else {
      this.alertify.error("You're not logged in!");
    }
  }

  updateLikesCount(){
    this.likeService.getLikeCount(this.picture.id)
      .subscribe((likeCountReq: number) => {
        this.likesCount=likeCountReq;
      }, error => {
        this.alertify.error("Error: Failed to update like counter");
        this.likesCount=0;
      });
  }

  loggedIn(){
    //const token = localStorage.getItem('token');
    return this.authService.loggedIn();//!!token;
  }

}
