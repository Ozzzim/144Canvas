import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {User} from 'src/app/_models/User';
import {Picture} from 'src/app/_models/Picture';


/*const httpOptions={
  headers: new HttpHeaders({
    'Authorization': 'Bearer '+ localStorage.getItem('token')
  })
};*/

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiURL+'users/';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl/*,httpOptions*/);
  }

  getUsersSearch(model: any): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl+"search/"+model.query+"/"+model.order/*,httpOptions*/);
  }

  getUser(id): Observable<User>{
    return this.http.get<User>(this.baseUrl+id/*,httpOptions*/);
  }

  getUsersPictures(id): Observable<Picture[]>{
    return null;//PLACEHOLDER, service doesn't exist at  the moment
    //return this.http.get<Picture[]>(this.baseUrl+'users/pictures/'+id/*,httpOptions*/);
  }

  getUsersComments(id): Observable<Comment[]>{
    return this.http.get<Comment[]>(this.baseUrl+'comments/'+id/*,httpOptions*/);
  }

  changeBG(model: any){
    return this.http.post(this.baseUrl+'changeBG', model)
  }
  changeAbout(model: any){
    return this.http.post(this.baseUrl+'changeAbout',model)
  }

  removeUser(model: any){
    return this.http.post(this.baseUrl+'remove',model)
  }

  static getProfilePicturePath(u:User):string{
    return environment.apiURL+"pictures/storage/"+u.backgroundURL;
  }
}
