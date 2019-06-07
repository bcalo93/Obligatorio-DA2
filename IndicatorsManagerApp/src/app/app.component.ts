import { Component } from '@angular/core';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'IndicatorsManagerApp';

  constructor(private auth: AuthService) {}

  isUserLoggedIn() {
    return this.auth.getToken() !== null;
  }

  logout() {
    return this.auth.logout();
  }
}
