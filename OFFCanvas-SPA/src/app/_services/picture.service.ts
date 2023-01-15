import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Picture} from 'src/app/_models/Picture';

@Injectable({
  providedIn: 'root'
})
export class PictureService {
  baseUrl = environment.apiURL+"pictures/";

  constructor(private http: HttpClient) {}

  getPicture(id: number): Observable<Picture>{
    return this.http.get<Picture>(this.baseUrl+id/*,httpOptions*/);
  }

  getRandomPicture(): Observable<Picture>{
    return this.http.get<Picture>(this.baseUrl+'/random'/*,httpOptions*/);
  }
  getPictures(): Observable<Picture[]> {
    return this.http.get<Picture[]>(this.baseUrl/*+'post',httpOptions*/);
  }
  getPicturesSearch(model: any): Observable<Picture[]> {
    return this.http.get<Picture[]>(this.baseUrl+"search/"+model.title+"/"+model.tags+"/"+model.user+"/"+model.order/*,httpOptions*/);
  }

  postPicture(model: any){
    console.log('Post Attempt:'+model.title);
    //return null;
    return this.http.post(this.baseUrl+'post', model)
  }

  removePicture(model: any){
    console.log('Remove Attempt:'+model.url);
    return this.http.post(this.baseUrl+'remove',model)
  }

  getImagePath(p: Picture):string{
    return this.baseUrl+"storage/"+p.imagedataId;
  }
}
