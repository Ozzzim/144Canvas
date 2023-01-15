import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Like} from 'src/app/_models/Like';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl = environment.apiURL+"likes/";

  constructor(private http: HttpClient) {}

  getLike(userID: number, pictureID: number): Observable<Like>{
    return this.http.get<Like>(this.baseUrl+userID+'/'+pictureID/*,httpOptions*/);
  }

  getLikeCount(pictureID: number): Observable<number>{
    return this.http.get<number>(this.baseUrl+'count/'+pictureID);
  }

  like(model: any){
    return this.http.post(this.baseUrl+'like', model)
  }

  unlike(model: any){
    return this.http.post(this.baseUrl+'unlike', model)
  }
}
