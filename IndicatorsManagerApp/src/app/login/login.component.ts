import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/services';
import { Router } from '@angular/router';
import { Credentials } from 'src/models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required]);
  errorMessage: string;
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.clearErrorMsg();
  }

  login() {
    const username = this.username.value;
    const password = this.password.value;
    const credentials = new Credentials(username, password);
    this.authService.login(credentials)
      .subscribe(
      () => {
        this.router.navigate(['/']);
        this.clearErrorMsg();
      },
      (error: any) => this.errorMessage = error
    );
  }

  getUsernameErrorMessage() {
    return this.username.hasError('required') ? 'You must enter a value' : '';
  }

  getPasswordErrorMessage() {
    return this.password.hasError('required') ? 'You must enter a value' : '';
  }

  isFormInvalid() {
    return this.username.invalid || this.password.invalid;
  }

  clearErrorMsg() {
    this.errorMessage = '';
  }
}
