import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Credentials } from 'src/app/models/credentials';
import { AuthenticationService } from 'src/app/services/authService/authentication.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  hide = true;
  creds = new Credentials();
  employeeChecked: boolean = false;
  username = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required]);

  constructor(private authService: AuthenticationService,
              private router: Router,
              public snackBar: MatSnackBar) { }

  logIn() {
    if(this.employeeChecked) {
      this.authService.authenticateEmployee(this.creds).subscribe({
        next: (v) => {
          console.log('You are successfully logged in!', v);
          localStorage.setItem('korisnickoIme', this.creds.korisnickoIme)
          this.openSnackBar('Successfully logged in!');
          this.router.navigateByUrl('/dashboard');
        },
        error: (e) => {
          this.openSnackBar('Invalid credentials!');
          console.error('Error logging in!', e);
        },
        complete: () => console.info('complete')
      });
    } else {
      this.authService.authenticateUser(this.creds).subscribe({
        next: (v) => {
          console.log('You are successfully logged in!', v);
          localStorage.setItem('korisnickoIme', this.creds.korisnickoIme)
          this.openSnackBar('Successfully logged in!');
          this.router.navigateByUrl('/dashboard');
        },
        error: (e) => {
          this.openSnackBar('Invalid credentials!');
          console.error('Error logging in!', e);
        },
        complete: () => console.info('complete')
      });
    }
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

}
