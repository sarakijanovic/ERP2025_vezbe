import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { WatercoolerDTO } from 'src/app/DTOs/watercoolerDTO';
import { Watercooler } from 'src/app/models/watercooler';

@Injectable({
  providedIn: 'root'
})
export class WatercoolerService {

  baseUrl = "http://localhost:5235/api/aparatZaVodu"

  constructor(private http: HttpClient) { }

  public getAllWatercoolers(page?: number, pageSize?: number) : Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get(this.getUrl(), { params })
      .pipe(map((response: Response) => response));
  }

  public getAllWatercoolersSorting(page?: number, pageSize?: number, sortByCenaOrder?: string, typeId?: string, company?: string) : Observable<any> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if(sortByCenaOrder) {
      params = params.set('sortByCena', true)
                    .set('sortOrder', sortByCenaOrder);
    }

    if(typeId) {
      params = params.set('tipAparataID', typeId);
    }

    if(company) {
      params = params.set('proizvodjac', company);
    }
      
    return this.http.get(this.getUrl(), { params })
      .pipe(map((response: Response) => response));
  }

  public getWatercoolerById(id: string) : Observable<any> {
    return this.http.get(this.getUrl()+`/${id}`)
      .pipe(map((response: Response) => response));
  }

  public createWatercooler(td: WatercoolerDTO) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.post(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Watercooler created successfully!"); return response}));
  }

  public updateWatercooler(td: Watercooler) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.put(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Watercooler updated successfully!"); return response}));
  }

  public deleteWatercooler(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.delete(this.getUrl()+`/${id}`, header)
      .pipe(map((response: Response) => {console.log("Watercooler deleted successfully!"); return response}));
  }

  private getUrl() {
    return `${this.baseUrl}`;
  }
}
