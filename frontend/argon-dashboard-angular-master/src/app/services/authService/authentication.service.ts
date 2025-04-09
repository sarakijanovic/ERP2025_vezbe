import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { JWT } from 'src/app/models/JWT';
import { Credentials } from 'src/app/models/credentials';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl = "http://localhost:5235/api/auth"

  constructor(private http: HttpClient) { }

  public authenticateEmployee(employeeLog: Credentials) : Observable<any> {
    return this.http.post<JWT>(this.getUrl()+"/zaposleni", employeeLog)
      .pipe(map(response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('uloga', response.uloga);
        return response;
      }));
  }

  public authenticateUser(userLog: Credentials) : Observable<any> {
    return this.http.post<JWT>(this.getUrl()+"/klijent", userLog)
      .pipe(map(response => {
        localStorage.setItem('token', response.token);
        localStorage.setItem('uloga', response.uloga);
        return response;
      }));
  }

  private getUrl() {
    return `${this.baseUrl}`;
  }
}
