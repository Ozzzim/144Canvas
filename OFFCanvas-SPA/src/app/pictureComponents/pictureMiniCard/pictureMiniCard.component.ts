import { Component, Input, OnInit } from '@angular/core';
import { Picture } from 'src/app/_models/Picture';
import { PictureService } from 'src/app/_services/picture.service';

@Component({
  selector: 'app-pictureMiniCard',
  templateUrl: './pictureMiniCard.component.html',
  styleUrls: ['./pictureMiniCard.component.css']
})
export class PictureMiniCardComponent implements OnInit {
  @Input() picture: Picture;

  constructor(private pictureService: PictureService) { }

  ngOnInit(): void {
  }
  getPath():string{
    return this.pictureService.getImagePath(this.picture);
  }
}
