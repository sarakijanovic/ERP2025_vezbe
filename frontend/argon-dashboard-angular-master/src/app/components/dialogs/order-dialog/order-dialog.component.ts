import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-order-dialog',
  templateUrl: './order-dialog.component.html',
  styleUrls: ['./order-dialog.component.scss']
})
export class OrderDialogComponent implements OnInit {

  orderForm: FormGroup;

  constructor(    
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<OrderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {}

  ngOnInit(): void {
    this.orderForm = this.fb.group({
      dostavljena: [this.data.order.dostavljena],
      zaposleniID: [this.data.order.zaposleniID, Validators.required],
    });
  }

  onClose() {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.orderForm.valid) {
      this.dialogRef.close(this.orderForm.value);
    }
  }

}
