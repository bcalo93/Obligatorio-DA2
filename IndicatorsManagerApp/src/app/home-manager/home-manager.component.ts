import { Component, OnInit } from '@angular/core';
import { User } from 'src/models/user';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-home-manager',
  templateUrl: './home-manager.component.html',
  styleUrls: ['./home-manager.component.css']
})
export class HomeManagerComponent implements OnInit {
  currentUserName: string;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.currentUserName = this.authService.getCurrentUser().name;
  }

}
