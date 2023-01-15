import { Component, Input, OnInit } from '@angular/core';
import { Comment } from 'src/app/_models/Comment';

@Component({
  selector: 'app-commentProfileCard',
  templateUrl: './commentProfileCard.component.html',
  styleUrls: ['./commentProfileCard.component.css']
})
export class CommentProfileCardComponent implements OnInit {
  @Input() comment: Comment;

  constructor() { }

  ngOnInit() {
  }

}
