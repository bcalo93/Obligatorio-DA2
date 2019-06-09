import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { UserService } from '../../services';
import { User } from 'src/models';
import { FormControl, Validators } from '@angular/forms';
import { UserRole } from 'src/enums';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  title: string;
  user: User;
  currentUserName: string;

  name =  new FormControl('', [Validators.required]);
  lastName = new FormControl('', [Validators.required]);
  username = new FormControl('', [Validators.required]);
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required]);

  roles = [UserRole.ADMIN, UserRole.MANAGER];
  selectedRole: UserRole;
  errorMessage: string;

  constructor(
    private userService: UserService,
    private location: Location,
    private currentRoute: ActivatedRoute) {}


  ngOnInit() {
    const id = this.currentRoute.snapshot.paramMap.get('id');
    if (id) {
      this.loadCurrentUserDetails(id);
      this.title = 'Edit User Details';
    } else {
      this.title = 'Create New User';
    }
  }

  loadCurrentUserDetails(id: string) {
    this.userService.getAllUsers()
      .subscribe(users => {
        this.user = users.find(x => x.id === id);
        this.name.setValue(this.user.name);
        this.lastName.setValue(this.user.lastName);
        this.username.setValue(this.user.username);
        this.email.setValue(this.user.email);
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

