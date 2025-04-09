import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { clientCreationDTO } from 'src/app/DTOs/clientCreationDTO';
import { ClientDTO } from 'src/app/DTOs/clientDTO';
import { Client } from 'src/app/models/client';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  baseUrl = "http://localhost:5235/api/klijent"

  constructor(private http: HttpClient) { }

  public getAllClients() : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl(), header)
      .pipe(map((response: Response) => response));
  }

  public getClientById(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl()+`/id/${id}`, header)
      .pipe(map((response: Response) => response));
  }

  public getClientByUsername(username: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl()+`/${username}`, header)
      .pipe(map((response: Response) => response));
  }

  public createClient(client: clientCreationDTO) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.post(this.getUrl(), client, header)
      .pipe(map((response: Response) => {console.log("Client created successfully!"); return response}));
  }

  public updateClient(client: Client) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.put(this.getUrl(), client, header)
      .pipe(map((response: Response) => {console.log("Client updated successfully!"); return response}));
  }

  public deleteClient(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.delete(this.getUrl()+`/${id}`, header)
      .pipe(map((response: Response) => {console.log("Client deleted successfully!"); return response}));
  }

  private getUrl() {
    return `${this.baseUrl}`;
  }
}
