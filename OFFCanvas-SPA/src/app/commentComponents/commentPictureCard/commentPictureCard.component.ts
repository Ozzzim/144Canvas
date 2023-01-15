import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/_models/Comment';

@Component({
  selector: 'app-commentPictureCard',
  templateUrl: './commentPictureCard.component.html',
  styleUrls: ['./commentPictureCard.component.css']
})
export class CommentPictureCardComponent implements OnInit {
  @Input() comment: Comment;

  constructor() { }

  ngOnInit() {
  }
}
