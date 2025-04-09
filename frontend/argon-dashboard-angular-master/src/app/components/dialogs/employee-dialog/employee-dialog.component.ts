import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-employee-dialog',
  templateUrl: './employee-dialog.component.html',
  styleUrls: ['./employee-dialog.component.scss']
})
export class EmployeeDialogComponent implements OnInit {

  employeeForm: FormGroup;
  hide = true;

  constructor(    
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EmployeeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {
      if (!data.employee) {
        this.data.employee = {
          imeZaposlenog: '',
          prezimeZaposlenog: '',
          jmbg: '',
          korisnickoImeZaposlenog: '',
          lozinkaZaposlenog: '',
          emailZaposlenog: '',
          uloga: '',
          ulogaID: ''
        };
      }
    }

  ngOnInit(): void {
    this.employeeForm = this.fb.group({
      korisnickoImeZaposlenog: [this.data.employee.korisnickoImeZaposlenog, Validators.required],
      imeZaposlenog: [this.data.employee.imeZaposlenog, Validators.required],
      prezimeZaposlenog: [this.data.employee.prezimeZaposlenog, Validators.required],
      jmbg: [this.data.employee.jmbg, [Validators.pattern('^[0-9]*$'), Validators.maxLength(13)]],
      ulogaID: [this.data.employee.ulogaID, Validators.required],
      lozinkaZaposlenog: [this.data.employee.lozinkaZaposlenog, Validators.required],
      emailZaposlenog: [this.data.employee.emailZaposlenog, [Validators.required, Validators.email]] 
    });
  }

  onClose() {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.employeeForm.valid) {
      this.dialogRef.close(this.employeeForm.value);
    }
  }

}
