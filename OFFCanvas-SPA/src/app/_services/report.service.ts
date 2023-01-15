import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Report} from 'src/app/_models/Report';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  baseUrl = environment.apiURL+"reports/";

  constructor(private http: HttpClient) {}

  getReport(id: number): Observable<Report>{
    return this.http.get<Report>(this.baseUrl+id/*+'post',httpOptions*/);
  }

  getReports(): Observable<Report[]>{
    return this.http.get<Report[]>(this.baseUrl/*+'post',httpOptions*/);
  }

  postReport(model: any){
    console.log('Post Attempt: report');
    //return null;
    return this.http.post(this.baseUrl+'post', model);
  }

  removeReport(model: any){
    console.log(model);
    return this.http.post(this.baseUrl+'remove', model);
    //console.log("rs:Dissolving: "+this.baseUrl+"remove/"+uid+"/"+pid);
    //return this.http.get(this.baseUrl+"remove/"+uid+"/"+pid);
  }
}
