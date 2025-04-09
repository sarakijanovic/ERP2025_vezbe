import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  baseUrl = "http://localhost:5235/api/placanje"

  constructor(private http: HttpClient) { }

  public pay(amount: number) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.post(this.getUrl(), amount, header)
      .pipe(map((response: Response) => response));
  }
  
  private getUrl() {
    return `${this.baseUrl}`;
  }
}
