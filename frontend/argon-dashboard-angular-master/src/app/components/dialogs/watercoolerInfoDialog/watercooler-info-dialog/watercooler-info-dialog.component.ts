import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CartItem } from 'src/app/models/cartItem';

@Component({
  selector: 'app-watercooler-info-dialog',
  templateUrl: './watercooler-info-dialog.component.html',
  styleUrls: ['./watercooler-info-dialog.component.scss']
})
export class WatercoolerInfoDialogComponent implements OnInit {

  cartItems: CartItem[] = [];
  quantity: number = 1;
  role: string;

  constructor(
    public dialogRef: MatDialogRef<WatercoolerInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) { }

  ngOnInit(): void {
    this.role = localStorage.getItem("uloga");
  }

  onClick(): void {
    this.dialogRef.close();
  }

  addToCart(){
    if(this.quantity != 0){
      if(localStorage.getItem("cartItems") != null)
      {
        let existingItems = JSON.parse(localStorage.getItem("cartItems"));
        let existingItem = existingItems.find(item => item.aparatID === this.data.watercooler.aparatID);
        if (existingItem) {
          existingItem.kolicina += this.quantity;
        } else {
          this.cartItems.push({
            aparatID: this.data.watercooler.aparatID,
            slikaURL: this.data.watercooler.slikaURL,
            model: this.data.watercooler.model,
            proizvodjac: this.data.watercooler.proizvodjac,
            cena: this.data.watercooler.cena,
            kolicina: this.quantity
          });
        }
        existingItems.forEach(element => {
          this.cartItems.push(element);
        });
      } else {
        this.cartItems.push({
          aparatID: this.data.watercooler.aparatID,
          slikaURL: this.data.watercooler.slikaURL,
          model: this.data.watercooler.model,
          proizvodjac: this.data.watercooler.proizvodjac,
          cena: this.data.watercooler.cena,
          kolicina: this.quantity
        });
      }
      localStorage.setItem("cartItems", JSON.stringify(this.cartItems));
    }
    this.onClick();
  }    

}
