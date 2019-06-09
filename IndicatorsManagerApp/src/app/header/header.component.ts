import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services';
import { User } from 'src/models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  currentUser: User;

  constructor(private auth: AuthService) { }

  ngOnInit() {
    this.currentUser = this.auth.getCurrentUser();
  }

  logout() {
    return this.auth.logout();
  }

  isManagerLoggedIn() {
    return this.auth.isManagerLoggedIn();
  }

  isAdminLoggedIn() {
    return this.auth.isAdminLoggedIn();
  }
}
