import { Component } from '@angular/core';
import { AuthService } from 'src/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(
    private authService: AuthService,
    private router: Router) {}

  login(form: any) {
    const credentials = form.value;
    this.authService.login(credentials)
      .subscribe(
      () => this.router.navigate(['/']),
      (error: any) => console.log(error)
    );
  }
}
