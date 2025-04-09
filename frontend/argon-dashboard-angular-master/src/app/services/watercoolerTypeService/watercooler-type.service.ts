import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { WatercoolerTypeDTO } from 'src/app/DTOs/watercoolerTypeDTO';
import { WatercoolerType } from 'src/app/models/watercoolerType';

@Injectable({
  providedIn: 'root'
})
export class WatercoolerTypeService {

  baseUrl = "http://localhost:5235/api/tipAparata"

  constructor(private http: HttpClient) { }

  public getAllWatercoolerTypes() : Observable<any> {
    return this.http.get(this.getUrl())
      .pipe(map((response: Response) => response));
  }

  public getWatercoolerTypeById(id: string) : Observable<any> {
    return this.http.get(this.getUrl()+`/${id}`)
      .pipe(map((response: Response) => response));
  }

  public createWatercoolerType(td: WatercoolerTypeDTO) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.post(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Watercooler type created successfully!"); return response}));
  }

  public updateWatercoolerType(td: WatercoolerType) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.put(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Watercooler type updated successfully!"); return response}));
  }

  public deleteWatercoolerType(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.delete(this.getUrl()+`/${id}`, header)
      .pipe(map((response: Response) => {console.log("Watercooler type deleted successfully!"); return response}));
  }

  private getUrl() {
    return `${this.baseUrl}`;
  }
}
