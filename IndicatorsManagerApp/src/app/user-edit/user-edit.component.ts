import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { UserService } from '../../services';
import { User } from 'src/models';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  user: User;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private location: Location) { }

  ngOnInit() {
    this.userService.getAllUsers()
      .subscribe(users => {
        this.user = users[0];
        console.log(this.user);
      });
  }

  goBack(): void {
    this.location.back();
  }

}
