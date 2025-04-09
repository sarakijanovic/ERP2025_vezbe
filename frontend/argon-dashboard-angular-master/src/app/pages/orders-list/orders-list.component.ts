import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ClientDTO } from 'src/app/DTOs/clientDTO';
import { EmployeeDTO } from 'src/app/DTOs/employeeDTO';
import { OrderDTO } from 'src/app/DTOs/orderDTO';
import { OrderDetailsComponent } from 'src/app/components/dialogs/order-details/order-details.component';
import { OrderDialogComponent } from 'src/app/components/dialogs/order-dialog/order-dialog.component';
import { ClientService } from 'src/app/services/clientService/client.service';
import { EmployeeService } from 'src/app/services/employeeService/employee.service';
import { OrderService } from 'src/app/services/orderService/order.service';
import { WatercoolerService } from 'src/app/services/watercoolerService/watercooler.service';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit {

  orders: OrderDTO[];
  orderInfo: OrderDTO;
  employees: EmployeeDTO[];
  clients: ClientDTO[];
  role: string;
  
  constructor(private ordersService: OrderService,
              private clientService: ClientService,
              private employeeService: EmployeeService,
              private watercoolerService: WatercoolerService,
              private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.role = localStorage.getItem("uloga")
    if(localStorage.getItem("uloga") == 'Customer') {
      this.startSubscriptionForUser();
    } else {    
      this.startSubscriptionForEmployee();
    }
  }

  startSubscriptionForUser() {
    this.clientService.getClientByUsername(localStorage.getItem("korisnickoIme")).subscribe(result => {
      this.ordersService.getOrderByKlijentId(result.klijentID).subscribe(res => 
        {
          this.orders = res;
          this.orders.forEach(element => {
            element.klijent = result.imeKlijenta + " " + result.prezimeKlijenta
            this.employeeService.getEmployeeById(element.zaposleniID).subscribe(empl=> {
              element.zaposleni = empl.imeZaposlenog + " " + empl.prezimeZaposlenog
            })
          });
          this.loadOrderDetails();
        });
    })
    
  }

  loadOrderDetails() {
    this.orders.forEach(element => {
      element.aparati = []
      this.ordersService.getOrderDetails(element.porudzbinaID).subscribe(res => {
        res.forEach(ordWat => {
          this.watercoolerService.getWatercoolerById(ordWat.aparatID).subscribe( wat => {
            element.aparati.push(wat)
            }
          )
        });
      })
    });
  }
  

  startSubscriptionForEmployee() {
    this.ordersService.getAllOrders().subscribe(res => 
      {
        this.orders = res;
        this.orders.forEach(element => {
          this.clientService.getClientById(element.klijentID).subscribe(result=> {
            element.klijent = result.imeKlijenta + " " + result.prezimeKlijenta
          })
          this.employeeService.getEmployeeById(element.zaposleniID).subscribe(result=> {
            element.zaposleni = result.imeZaposlenog + " " + result.prezimeZaposlenog
          })
        });
        this.loadOrderDetails();
      });
      this.employeeService.getAllEmployees().subscribe(res => 
        {
          this.employees = res;
        })
      this.clientService.getAllClients().subscribe(res => 
        {
          this.clients = res;
        })
  }

  updateOrder(order: OrderDTO) {
    this.orderInfo = order;
    let dialog = this.dialog.open(OrderDialogComponent, {
      width: '600px',
      data: {
        order: this.orderInfo,
        employees: this.employees
      }
    });
    dialog.afterClosed().subscribe(result => {
      if (result) {
        result.porudzbinaID = this.orderInfo.porudzbinaID
        result.klijentID = this.orderInfo.klijentID
        result.datumPorudzbine = this.orderInfo.datumPorudzbine
        result.iznos = this.orderInfo.iznos
        this.ordersService.updateOrder(result).subscribe(res => {
          const orderIndex = this.orders.findIndex( (obj: OrderDTO) => obj.porudzbinaID == order.porudzbinaID);
          this.orders[orderIndex] = result;
          this.employees.forEach(element => {
            if(element.zaposleniID == result.zaposleniID) {
              this.orders[orderIndex].zaposleni = element.imeZaposlenog + " " + element.prezimeZaposlenog
            }
          });
          this.clients.forEach(element => {
            if(element.klijentID == result.klijentID) {
              this.orders[orderIndex].klijent = element.imeKlijenta + " " + element.prezimeKlijenta
            }
          });
        });
      }
    });
  }

  showWatercoolers(order : OrderDTO) {
    this.dialog.open(OrderDetailsComponent, {
      width: '1000px',
      data: {
        watercoolers: order.aparati
      }
    });
  }

}
