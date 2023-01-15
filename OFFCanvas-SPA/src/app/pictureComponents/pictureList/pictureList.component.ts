import { Component, OnInit } from '@angular/core';

import {Picture} from '../../_models/picture';
import { AlertifyService } from '../../_services/alertify.service';
import { PictureService } from '../../_services/picture.service';

@Component({
  selector: 'app-pictureList',
  templateUrl: './pictureList.component.html',
  styleUrls: ['./pictureList.component.css']
})
export class PictureListComponent implements OnInit {
  pictures: Picture[];
  searchModel: any = {};
  searched: boolean=false;

  constructor(private pictureService: PictureService, private alertify: AlertifyService) { }

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

  search(){
    if(!this.searchModel.title)
      this.searchModel.title="*";
    if(!this.searchModel.tags)
      this.searchModel.tags="*";
    if(!this.searchModel.user)
      this.searchModel.user="*";
     if(!this.searchModel.order)
      this.searchModel.order="date";
    this.pictureService.getPicturesSearch(this.searchModel).subscribe((pictures: Picture[]) => {
      this.pictures = pictures;
      this.searched= true;
    }, error =>{
      this.alertify.error("Error while searching for users");
    }
    );
  }

  reset(){
    console.log("reset");
    if(this.searched){
      this.loadPictures();
      this.searched=false;
    }
  }
}
