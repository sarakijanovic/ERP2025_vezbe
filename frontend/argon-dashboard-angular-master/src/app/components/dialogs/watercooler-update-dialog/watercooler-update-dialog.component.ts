import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-watercooler-update-dialog',
  templateUrl: './watercooler-update-dialog.component.html',
  styleUrls: ['./watercooler-update-dialog.component.scss']
})
export class WatercoolerUpdateDialogComponent implements OnInit {

  watercoolerForm: FormGroup;

  constructor(    
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<WatercoolerUpdateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) { 
      if (!data.watercooler) {
        this.data.watercooler = {
          model: '',
          proizvodjac: '',
          opis: '',
          slikaURL: '',
          cena: '',
          kolicinaNaStanju: '',
          tipAparataNaziv: '',
          tipAparataID: ''
        };
      }
    }

  ngOnInit(): void {
    this.watercoolerForm = this.fb.group({
      model: [this.data.watercooler.model, Validators.required],
      proizvodjac: [this.data.watercooler.proizvodjac, Validators.required],
      opis: [this.data.watercooler.opis],
      slikaURL: [this.data.watercooler.slikaURL],
      cena: [
        this.data.watercooler.cena,
        [Validators.required, Validators.pattern('^[0-9]+(\\.[0-9]{1,2})?$')]
      ],
      kolicinaNaStanju: [this.data.watercooler.kolicinaNaStanju,
        [Validators.required, Validators.pattern('^[0-9]*$')]
      ],
      tipAparataID: [this.data.watercooler.tipAparataID, Validators.required],
    });
  }

  onClose() {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.watercoolerForm.valid) {
      this.dialogRef.close(this.watercoolerForm.value);
    }
  }

}
