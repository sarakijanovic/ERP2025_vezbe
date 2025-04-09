import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ClientDTO } from 'src/app/DTOs/clientDTO';
import { ClientDialogComponent } from 'src/app/components/dialogs/client-dialog/client-dialog.component';
import { ClientService } from 'src/app/services/clientService/client.service';

@Component({
  selector: 'app-clients-list',
  templateUrl: './clients-list.component.html',
  styleUrls: ['./clients-list.component.scss']
})
export class ClientsListComponent implements OnInit {

  clients: ClientDTO[] = [];
  clientInfo: ClientDTO;
  role: string;

  constructor(private clientService: ClientService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.role = localStorage.getItem("uloga")
    this.clientService.getAllClients().subscribe(
      (response: any) => {
        this.clients = response;
      },
      (error: any) => {
        console.log('Error occurred while loading clients:', error);
      }
    );
  }

  updateClient(client: ClientDTO) {
    this.clientInfo = client;
    this.clientInfo.lozinkaKlijenta = client.korisnickoImeKlijenta
    let dialog = this.dialog.open(ClientDialogComponent, {
      width: '600px',
      data: {
        client: this.clientInfo,
      }
    });
    dialog.afterClosed().subscribe(result => {
      if (result) {
        result.klijentID = this.clientInfo.klijentID
        result.korisnickoImeKlijenta = this.clientInfo.korisnickoImeKlijenta
        result.datumRodjenja = this.convertDateString(result.datumRodjenja)
        this.clientService.updateClient(result).subscribe(res => {
          const clientIndex = this.clients.findIndex( (obj: ClientDTO) => obj.klijentID == client.klijentID);
          this.clients[clientIndex] = result;
        });
      }
    });
  }

  convertDateString(date: Date) : string {
    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(date.getUTCDate() + 1).padStart(2, '0');

    return`${year}-${month}-${day}`;
  }

  deleteClient(client: ClientDTO) {
    if(confirm("Are you sure you want to delete client " + client.korisnickoImeKlijenta + "?")) {
      const clientIndex = this.clients.findIndex( (obj: ClientDTO) => obj.klijentID == client.klijentID);
      this.clients.splice(clientIndex, 1);
      this.clientService.deleteClient(client.klijentID).subscribe();
    }
  }

}
