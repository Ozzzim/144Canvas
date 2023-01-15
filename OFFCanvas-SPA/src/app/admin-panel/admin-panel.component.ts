import { Component, OnInit } from '@angular/core';

import {Report} from '../_models/Report';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { ReportService } from '../_services/report.service';
import { PictureService } from '../_services/picture.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  reports: Report[];
  displayedReport: Report;

  constructor(private reportService: ReportService, private pictureService: PictureService, private userService: UserService, private alertify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    //if(this.canMod())
      this.loadReports();
  }

  loadReports(){
    this.reportService.getReports().subscribe((reports: Report[]) => {
      this.reports= reports;
      console.log("Reports: "+reports.length);
    }, error =>{
      this.alertify.error(error);
    }
    );
  }

  dissolveReport(){
    //deletes report
    var dissolveModel: any = {};
    dissolveModel.UserID=+this.displayedReport.userID;
    dissolveModel.PictureID=+this.displayedReport.pictureID;
    console.log("Reportdissolve");
    this.reportService.removeReport(dissolveModel).subscribe(next =>{
      this.alertify.success('Success!');
    }, error => {
      this.alertify.error("Couldn't remove the report...");
    });
    //this.reportService.removeReport(this.displayedReport.userID,this.displayedReport.pictureID);
    this.displayedReport=null;
  }

  loadDetails(report: Report){
    this.displayedReport=report;
  }

  getPath():string{
    return this.pictureService.getImagePath(this.displayedReport.reportedPicture);
  }

  reject(){
    if(this.authService.isStaff()){
      this.dissolveReport();
      this.loadReports();
    }else
    this.alertify.error("I told you, you can't use that!");
  }

  deletePic(){
    if(this.authService.isStaff()){
      var deleteModel: any = {};
      deleteModel.UserID=-1;
      deleteModel.PictureID=+this.displayedReport.pictureID;

      this.pictureService.removePicture(deleteModel).subscribe(next =>{
        //this.dissolveReport();
        this.displayedReport=null;
        this.loadReports();
        
        this.alertify.success('Success!');
      }, error => {
        this.alertify.error("Couldn't remove the picture...");
      });
    }else
    this.alertify.error("I told you, you can't use that!");
  }

  deleteUser(){
    if(this.authService.isAdmin()){
      var deleteModel: any = {};
      deleteModel.UserID=+this.displayedReport.reportedPicture.user.id;
      deleteModel.PictureID=-1;

      
      this.userService.removeUser(deleteModel).subscribe(next =>{
        //this.dissolveReport();
        this.displayedReport=null;
        this.loadReports();
        this.alertify.success('Success!');
      }, error => {
        console.log(error);
        this.alertify.error("Couldn't ban the user...");
      });
    }else
    this.alertify.error("Moderators cannot ban the users");
  }

  isStaff():boolean{
    return this.authService.isStaff();
  }
  isAdmin():boolean{
    return this.authService.isAdmin();
  }
}
