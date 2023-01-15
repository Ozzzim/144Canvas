import { Component, OnInit } from '@angular/core';
import {Picture} from '../_models/picture';
import { AlertifyService } from '../_services/alertify.service';
import { PictureService } from '../_services/picture.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  pictures: Picture[];

  constructor(private pictureService: PictureService,private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadPictures();
  }

  loadPictures(){
    this.pictureService.getPictures().subscribe((pictures: Picture[]) => {
      this.pictures= pictures;
    }, error =>{
      this.alertify.error(error);
    }
    );
  }

  loggedIn():boolean{
    return this.authService.loggedIn();
  }
}
