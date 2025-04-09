import { Component, OnInit } from '@angular/core';
import { ClientDTO } from 'src/app/DTOs/clientDTO';
import { ClientService } from 'src/app/services/clientService/client.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  user: ClientDTO;

  constructor(private clientService: ClientService) { }

  ngOnInit() {
    this.clientService.getClientByUsername(localStorage.getItem("korisnickoIme")).subscribe(res => {
      this.user= res
    })
  }

}
