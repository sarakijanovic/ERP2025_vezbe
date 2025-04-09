import { Component, OnInit, ElementRef } from '@angular/core';
import { ROUTESAll, ROUTESEmployee, ROUTESUser } from '../sidebar/sidebar.component';
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { Router } from '@angular/router';
import { ClientService } from 'src/app/services/clientService/client.service';
import { ClientDTO } from 'src/app/DTOs/clientDTO';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  public focus;
  role: string;
  username: string;
  public listTitles: any[];
  public location: Location;
  constructor(location: Location,  private element: ElementRef, private router: Router
  ) {
    this.location = location;
  }

  ngOnInit() {
    this.username=localStorage.getItem("korisnickoIme")
    this.listTitles = ROUTESAll.filter(listTitle => listTitle);
    this.listTitles = ROUTESUser.filter(listTitle => listTitle);
    this.listTitles = ROUTESEmployee.filter(listTitle => listTitle);
    this.role = localStorage.getItem("uloga")
  }
  getTitle(){
    var titlee = this.location.prepareExternalUrl(this.location.path());
    if(titlee.charAt(0) === '#'){
        titlee = titlee.slice( 1 );
    }

    for(var item = 0; item < this.listTitles.length; item++){
        if(this.listTitles[item].path === titlee){
            return this.listTitles[item].title;
        }
    }
    return 'Dashboard';
  }
  logOut(){
    localStorage.clear();
  }

}
