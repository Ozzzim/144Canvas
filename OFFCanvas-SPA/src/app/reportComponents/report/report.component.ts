import { Component, OnInit } from '@angular/core';
import { Picture } from 'src/app/_models/Picture';
import { User } from 'src/app/_models/User';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { PictureService } from 'src/app/_services/picture.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ReportService } from 'src/app/_services/report.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  reportModel: any = {};
  reportedPicture: Picture;

  constructor( private route: ActivatedRoute, private reportService: ReportService, private alertify: AlertifyService, private userService: UserService, private authService: AuthService, private pictureService: PictureService, private router: Router) { }

  ngOnInit(): void {
    this.pictureService.getPicture(+this.route.snapshot.params['id'])
      .subscribe((picture: Picture) => {
      console.log("Loading picture: "+picture.title);
      this.reportedPicture=picture;
    }, error => {
      this.alertify.error(error);
    });    
  }

  submitReport(){
    //console.log('Post Attempt:'+this.reportModel.Category);
    if(this.authService.loggedIn()){
      
      this.reportModel.pictureID=+this.reportedPicture.id;
      this.reportModel.userID=+this.authService.decodedToken.nameid;
      if(this.reportModel.descryption==null)
        this.reportModel.descryption="";
      console.log("Your report:\n"+this.reportModel.pictureID+"\n"+
                                      this.reportModel.userID+"\n"+
                                      this.reportModel.category+"\n"+
                                      this.reportModel.descryption);
      this.reportService.postReport(this.reportModel).subscribe(next =>{
        this.alertify.success('Picture reported');
        this.router.navigate(['/pictures'])
      }, error => {
        this.alertify.error("Couldn't post the report...");
      });
    } else
      this.alertify.error("You're not logged in!");
  }

  getPath():string{
    return this.pictureService.getImagePath(this.reportedPicture);
  }

}
