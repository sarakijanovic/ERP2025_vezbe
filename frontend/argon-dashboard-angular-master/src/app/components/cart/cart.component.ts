import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderDTO } from 'src/app/DTOs/orderDTO';
import { CartItem } from 'src/app/models/cartItem';
import { OrderDetail } from 'src/app/models/orderDetail';
import { ClientService } from 'src/app/services/clientService/client.service';
import { EmployeeService } from 'src/app/services/employeeService/employee.service';
import { OrderService } from 'src/app/services/orderService/order.service';
import { PaymentService } from 'src/app/services/payment-service.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

  cartItems: CartItem[] = []
  totalPrice: number = 0;
  handler:any = null;
  order: OrderDTO = new OrderDTO();

  constructor(private paymentService: PaymentService,
    private orderService: OrderService,
    private clientService: ClientService,
    private employeeService: EmployeeService,
    private router: Router,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.cartItems = JSON.parse(localStorage.getItem("cartItems"));
    this.calculateTotal();
    this.loadStripe();
    this.clientService.getClientByUsername(localStorage.getItem("korisnickoIme")).subscribe(res => {
      this.order.klijentID = res.klijentID
      this.setOrderDate();
      this.order.dostavljena = false;
    })
    this.employeeService.getAllEmployees().subscribe(res => {
      this.order.zaposleniID = res[0].zaposleniID
    })
  }

  calculateTotal() {
    this.totalPrice = 0;
    this.cartItems.forEach(item => {
      this.totalPrice += item.kolicina*item.cena
    });
    this.totalPrice = parseFloat(this.totalPrice.toFixed(2));
  }

  removeFromCart(id: string) {
    const cartItemIndex = this.cartItems.findIndex( (obj: CartItem) => obj.aparatID == id);
    this.cartItems.splice(cartItemIndex, 1);
    this.calculateTotal();
    if(this.cartItems.length = 0)
    {
      localStorage.removeItem("cartItems");
    } else {
      localStorage.setItem("cartItems", JSON.stringify(this.cartItems));
    }
  }

  pay() {
    this.order.iznos = this.totalPrice;
    console.log(this.order)
    this.orderService.createOrder(this.order).subscribe(res => {
      console.log("after method")
      console.log(res)
      this.order.porudzbinaID = res.porudzbinaID
      this.cartItems.forEach(element => {
        let od = new OrderDetail();
        od.aparatID = element.aparatID;
        od.porudzbinaID = res.porudzbinaID;
        od.kolicina = element.kolicina;
        this.orderService.createOrderDetails(od).subscribe();
      });
    })
    const handler = (<any>window).StripeCheckout.configure({
      key: 'pk_test_51L60ysC5Xo5PQs5TJ4M8XTkoiGKZ5jL5Btjla41HlEGgMujgY0lpmqtStDWrfatlyfX2kx3IyJumOioS4My5UWtQ001ihuuekN',
      locale: 'auto',
      token: (token: any) => {
        this.processPayment(token);
      }
    });

    handler.open({
      name: 'Payment',
      description: 'Enter details about your card.',
      amount: this.totalPrice * 100,
      currency: 'rsd'
    });
  }

  processPayment(token: any) {
    const paymentData = {
      token: token.id,
      email: token.email,
      amount: Math.round(this.totalPrice * 100),

    };
    this.http.post('http://localhost:5235/api/pay/create-payment-intent', paymentData)
      .subscribe(response => {
        console.log('Payment Intent created:', response);
        alert('Payment successfull!');
        this.order.dostavljena = true;
        this.orderService.updateOrder(this.order).subscribe();
        localStorage.removeItem("cartItems")
        this.router.navigateByUrl('/dashboard');
      }, error => {
        console.error('Payment Error:', error);
        alert('Payment error!');
      });
  }

  loadStripe() {
    if (!window.document.getElementById('stripe-script')) {
      const s = window.document.createElement("script");
      s.id = "stripe-script";
      s.type = "text/javascript";
      s.src = "https://checkout.stripe.com/checkout.js";
      s.onload = () => {
        this.handler = (<any>window).StripeCheckout.configure({
          key: 'pk_test_51L60ysC5Xo5PQs5TJ4M8XTkoiGKZ5jL5Btjla41HlEGgMujgY0lpmqtStDWrfatlyfX2kx3IyJumOioS4My5UWtQ001ihuuekN',
          locale: 'auto',
          token: (token: any) => {
            // Access the token ID with token.id
            alert('Plaćanje je uspešno izvršeno!');
          }
        });
      }

      window.document.body.appendChild(s);
    }
  }
  
  setOrderDate() {
    let date = new Date();
    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(date.getUTCDate()).padStart(2, '0');
  
    this.order.datumPorudzbine = `${year}-${month}-${day}`;
  }
}
