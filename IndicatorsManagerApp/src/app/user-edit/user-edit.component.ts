import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { UserService } from '../../services';
import { User } from 'src/models';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  user: User;
  currentUserName: string;

  name = new FormControl('', [Validators.required]);
  lastName = new FormControl('', [Validators.required]);
  username = new FormControl('', [Validators.required]);
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required]);
  roles = ['Admin', 'Manager'];
  selectedRole: string;
  errorMessage: string;

  constructor(
    private userService: UserService,
    private location: Location) { }

  ngOnInit() {
    this.userService.getAllUsers()
      .subscribe(users => {
        this.user = users[0];
        console.log(this.user);
        this.currentUserName = this.user.name;
      });
  }

  save() {
    const name = this.name.value;
    const lastName = this.lastName.value;
    const username = this.username.value;
    const email = this.email.value;
    const password = this.password.value;
    const user = new User(name, lastName, username, email, this.selectedRole, password);
    console.log(user);
    this.userService.addUser(user)
    .subscribe(
      (data: User) => {
        this.clearErrorMsg();
        console.log(data);
      },
      (error: any) => {
        this.errorMessage = error;
        console.log(this.errorMessage);
      }
    );
  }

  goBack(): void {
    this.location.back();
  }

  keyEnter() {
    return this.isFormInvalid() ? null : this.save();
  }

  isFormInvalid() {
    return this.name.invalid
      || this.lastName.invalid
      || this.username.invalid
      || this.password.invalid
      || this.email.invalid;
  }

  clearErrorMsg() {
    this.errorMessage = '';
  }
}

