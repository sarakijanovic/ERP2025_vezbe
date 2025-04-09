import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { OrderDTO } from 'src/app/DTOs/orderDTO';
import { Order } from 'src/app/models/order';
import { OrderDetail } from 'src/app/models/orderDetail';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = "http://localhost:5235/api/porudzbina"

  constructor(private http: HttpClient) { }

  public getAllOrders() : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl(), header)
      .pipe(map((response: Response) => response));
  }

  public getOrderDetails(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get("http://localhost:5235/api/aparatPorudzbina"+`/${id}`, header)
      .pipe(map((response: Response) => response));
  }

  public createOrderDetails(orderDetails: OrderDetail) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.post("http://localhost:5235/api/aparatPorudzbina", orderDetails, header)
      .pipe(map((response: Response) => response));
  }

  public getOrderById(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl()+`/${id}`, header)
      .pipe(map((response: Response) => response));
  }

  public getOrderByKlijentId(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl()+`/klijent/${id}`, header)
      .pipe(map((response: Response) => response));
  }

  public createOrder(td: OrderDTO) : Observable<any> {
    console.log(td)
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.post(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Order created successfully!"); return response}));
  }

  public updateOrder(td: Order) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.put(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Order updated successfully!"); return response}));
  }

  public deleteOrder(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.delete(this.getUrl()+`/${id}`, header)
      .pipe(map((response: Response) => {console.log("Order deleted successfully!"); return response}));
  }

  private getUrl() {
    return `${this.baseUrl}`;
  }
}
