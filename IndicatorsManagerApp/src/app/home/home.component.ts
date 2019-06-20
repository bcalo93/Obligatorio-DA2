import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services';
import { User } from 'src/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthService) { }

  currentUser: User;

  ngOnInit() {
    this.currentUser = this.authService.getCurrentUser();
  }

  isManagerLoggedIn() {
    return this.authService.isManagerLoggedIn();
  }

  isAdminLoggedIn() {
    return this.authService.isAdminLoggedIn();
  }
}
