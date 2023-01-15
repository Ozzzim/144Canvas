import { Component, OnInit } from '@angular/core';

import {User} from '../../_models/User';
import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-userList',
  templateUrl: './userList.component.html',
  styleUrls: ['./userList.component.css']
})
export class UserListComponent implements OnInit {
  users: User[];
  searchModel: any = {};
  searched: boolean=false;

  constructor(private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers(){
    this.userService.getUsers().subscribe((users: User[]) => {
      this.users= users;
    }, error =>{
      this.alertify.error(error);
    }
    );
  }

  search(){
    if(!this.searchModel.query)
      this.searchModel.query="*";
    this.userService.getUsersSearch(this.searchModel).subscribe((users: User[]) => {
      this.users = users;
      this.searched= true;
    }, error =>{
      this.alertify.error("Error while searching for users");
    }
    );
  }

  reset(){
    //console.log("reset");
    if(this.searched){
      this.loadUsers();
      this.searched=false;
    }
  }
}
