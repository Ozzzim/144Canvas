import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Comment} from 'src/app/_models/Comment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  baseUrl = environment.apiURL+"comment/";

  constructor(private http: HttpClient) {}

  getComment(id: number): Observable<Comment>{
    return this.http.get<Comment>(this.baseUrl+id/*,httpOptions*/);
  }

  getCommentsForPicture(id: number):Observable<Comment[]>{
    return this.http.get<Comment[]>(this.baseUrl+"picture/"+id/*,httpOptions*/);
  }

  getCommentsForUser(id: number):Observable<Comment[]>{
    return this.http.get<Comment[]>(this.baseUrl+"user/"+id/*,httpOptions*/);
  }

  postComment(model: any){
    console.log('Post Attempt:'+model.content);
    //return null;
    return this.http.post(this.baseUrl+'post', model)
  }
}
