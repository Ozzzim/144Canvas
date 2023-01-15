import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/User';
import { Picture } from 'src/app/_models/Picture';
import { Like } from 'src/app/_models/Like';
import {Comment} from 'src/app/_models/Comment';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PictureService } from 'src/app/_services/picture.service';
import { AuthService } from 'src/app/_services/auth.service';
import { CommentService } from 'src/app/_services/comment.service';
import { LikeService } from 'src/app/_services/like.service';

@Component({
  selector: 'app-picture',
  templateUrl: './picture.component.html',
  styleUrls: ['./picture.component.css']
})
export class PictureComponent implements OnInit {
  commentModel: any = {};
  likeModel : any = {};
  
  picture: Picture;
  comments: Comment[];
    
  liked: boolean=false;
  likesCount: number=0;

  constructor(private authService: AuthService, private pictureService: PictureService, private commentService: CommentService, private likeService: LikeService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPicture();
    this.loadComments();
    this.updateLikesCount();

    this.likeService.getLike(+this.authService.decodedToken.nameid,+this.route.snapshot.params['id'])
    .subscribe((like: Like) => {
      this.liked=(like!=null);
    }, error => {
      this.liked=false;
    });
  }

  loadPicture(){
    this.pictureService.getPicture(+this.route.snapshot.params['id'])
      .subscribe((picture: Picture) => {
        //console.log("User should have this many comments: "+picture.comments);
        this.picture=picture;
                                  //this.comments=picture.comments;
        //this.user=picture.user;
      }, error => {
        this.alertify.error(error);
      }
    );
  }

  loadComments(){
    this.commentService.getCommentsForPicture(+this.route.snapshot.params['id'])
      .subscribe((comments: Comment[]) => {
        console.log("Reloading comments: "+comments.length);
        this.comments=comments;
      }, error => {
        this.alertify.error(error);
      }
    );
  }
  pictureReady():boolean{
    return this.picture!=null;
  }
  loggedIn(){
    return this.authService.loggedIn();
  }
  hasComments():boolean{
    if(this.comments!=null)
      return this.comments.length>0
    return false;
  }

  postComment(){
    if(this.loggedIn){
      this.commentModel.userID=+this.authService.decodedToken.nameid;
      this.commentModel.pictureID=+this.route.snapshot.params['id'];
      this.commentService.postComment(this.commentModel).subscribe(next =>{
        this.alertify.success('Comment posted');//console.log('Login successful!');
        this.loadComments();
      }, error => {
        this.alertify.error("Couldn't post the comment...");
        //console.log('Login failed!');
      });
    } else {
      this.alertify.error("You're not logged in!");
    }
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
    this.likeService.getLikeCount(+this.route.snapshot.params['id'])
      .subscribe((likeCountReq: number) => {
        this.likesCount=likeCountReq;
        
      }, error => {
        this.alertify.error("Error: Failed to update like counter");
        this.likesCount=0;
      });
  }
}
