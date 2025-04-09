import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { clientCreationDTO } from 'src/app/DTOs/clientCreationDTO';
import { Credentials } from 'src/app/models/credentials';
import { AuthenticationService } from 'src/app/services/authService/authentication.service';
import { ClientService } from 'src/app/services/clientService/client.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  hide = true;
  date: Date;
  clientCreation = new clientCreationDTO();
  privacyPolicyChecked: boolean = false;
  name = new FormControl('', [Validators.required]);
  surname = new FormControl('', [Validators.required]);
  contact = new FormControl('', [Validators.required, Validators.pattern(/^\+381\d+$/)]);
  address = new FormControl('', [Validators.required]);
  dateOfBirth = new FormControl('', [Validators.required]);
  email = new FormControl('', [Validators.required, Validators.email]);
  username = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required]);

  constructor(private clientService: ClientService,
              private authService: AuthenticationService,
              private router: Router,
              public snackBar: MatSnackBar) { }

  register() {
    if (!this.privacyPolicyChecked) {
      confirm("You must accept terms of service!");
    } else {
      this.convertDateString();
      this.clientService.createClient(this.clientCreation).subscribe({
        next: (v) => {
          console.log('You are successfully signed up!', v);
          this.openSnackBar('Successfully signed up!');
          const creds = new Credentials();
          creds.korisnickoIme = this.clientCreation.korisnickoImeKlijenta;
          creds.lozinka = this.clientCreation.lozinkaKlijenta;
          this.authService.authenticateUser(creds).subscribe(res => {
            localStorage.setItem("korisnickoIme", creds.korisnickoIme)
            this.router.navigateByUrl('/dashboard');
          })
        },
        error: (e) => {
          this.openSnackBar('Error signing up!');
          console.error('Error signing up!', e);
        },
        complete: () => console.info('complete')
      })
    }
  }

  getErrorMessageName() {
    if (this.name.hasError('required')) {
      return 'You must enter a name';
    }
    return '';
  }

  getErrorMessageSurname() {
    if (this.surname.hasError('required')) {
      return 'You must enter a surname';
    }
    return '';
  }

  getErrorMessageContact() {
    if (this.contact.hasError('required')) {
      return 'You must enter a contact';
    }
    if (this.contact.hasError('pattern')) {
      return 'Contact must start with +381';
    }
    return '';
  }

  getErrorMessageAddress() {
    if (this.address.hasError('required')) {
      return 'You must enter a address';
    }
    return '';
  }

  getErrorMessageDateOfBirth() {
    if (this.dateOfBirth.hasError('required')) {
      return 'You must enter a date of birth';
    }
    return '';
  }

  getErrorMessageEmail() {
    if (this.email.hasError('required')) {
      return 'You must enter a email';
    }
    if (this.email.hasError('email')) {
      return 'Invalid email adress';
    }
    return '';
  }

  getErrorMessageUsername() {
    if (this.username.hasError('required')) {
      return 'You must enter a username';
    }
    return '';
  }

  getErrorMessagePassword() {
    if (this.password.hasError('required')) {
      return 'You must enter a password';
    }
    return '';
  }

  openSnackBar(message: string) {
    this.snackBar.open(message, "Close", {
      duration: 3000,
    });
  }

  convertDateString() {
    const year = this.date.getUTCFullYear();
    const month = String(this.date.getUTCMonth() + 1).padStart(2, '0');
    const day = String(this.date.getUTCDate()).padStart(2, '0');

    this.clientCreation.datumRodjenja = `${year}-${month}-${day}`;
  }

}
