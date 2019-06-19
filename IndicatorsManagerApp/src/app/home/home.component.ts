import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  isManagerLoggedIn() {
    return this.authService.isManagerLoggedIn();
  }

  isAdminLoggedIn() {
    return this.authService.isAdminLoggedIn();
  }
}
