import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-client-dialog',
  templateUrl: './client-dialog.component.html',
  styleUrls: ['./client-dialog.component.scss']
})
export class ClientDialogComponent implements OnInit {

  clientForm: FormGroup;
  hide = true;
  date: Date;
  constructor(    
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ClientDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {}

  ngOnInit(): void {
    this.date = new Date(this.data.client.datumRodjenja);
    this.convertDateString();
    this.clientForm = this.fb.group({
      imeKlijenta: [this.data.client.imeKlijenta, Validators.required],
      prezimeKlijenta: [this.data.client.prezimeKlijenta, Validators.required],
      adresa: [this.data.client.adresa, Validators.required],
      datumRodjenja: [this.data.client.datumRodjenja, Validators.required],
      kontakt: [this.data.client.kontakt, Validators.required],
      lozinkaKlijenta: [this.data.client.lozinkaKlijenta, Validators.required],
      emailKlijenta: [this.data.client.emailKlijenta, [Validators.required, Validators.email]],
    });
  }

  onClose() {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.clientForm.valid) {
      this.dialogRef.close(this.clientForm.value);
    }
  }

  convertDateString() {
    const year = this.date.getUTCFullYear();
    const month = String(this.date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(this.date.getUTCDate()).padStart(2, '0');

    this.data.client.datumRodjenja = `${year}-${month}-${day}`;
  }

}
