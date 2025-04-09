import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { EmployeeDTO } from 'src/app/DTOs/employeeDTO';
import { Employee } from 'src/app/models/employee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  baseUrl = "http://localhost:5235/api/zaposleni"

  constructor(private http: HttpClient) { }

  public getAllEmployees() : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl(), header)
      .pipe(map((response: Response) => response));
  }

  public getEmployeeById(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.get(this.getUrl()+`/id/${id}`, header)
      .pipe(map((response: Response) => response));
  }

  public createEmployee(td: EmployeeDTO) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    console.log(td)
    return this.http.post(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Employee created successfully!"); return response}));
  }

  public updateEmployee(td: Employee) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.put(this.getUrl(), td, header)
      .pipe(map((response: Response) => {console.log("Employee updated successfully!"); return response}));
  }

  public deleteEmployee(id: string) : Observable<any> {
    const token = localStorage.getItem('token');
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${token}`)
    }
    return this.http.delete(this.getUrl()+`/${id}`, header)
      .pipe(map((response: Response) => {console.log("Employee deleted successfully!"); return response}));
  }

  private getUrl() {
    return `${this.baseUrl}`;
  }
}
