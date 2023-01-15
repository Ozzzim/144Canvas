import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-userCard',
  templateUrl: './userCard.component.html',
  styleUrls: ['./userCard.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user: User;

  constructor() { }

  ngOnInit() {
  }

  getPath():string{
    return UserService.getProfilePicturePath(this.user);
  }

}
