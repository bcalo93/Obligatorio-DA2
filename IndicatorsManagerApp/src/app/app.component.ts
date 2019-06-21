import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services/auth.service';
import { UserRole } from 'src/enums';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'IndicatorsManagerApp';

  constructor(private auth: AuthService) {}

  isManagerLoggedIn() {
    return this.auth.isManagerLoggedIn();
  }

  isAdminLoggedIn() {
    return this.auth.isAdminLoggedIn();
  }

  isUserLoggedIn() {
    return this.auth.getToken() !== null;
  }
}
