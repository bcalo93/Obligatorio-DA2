import { Component, OnInit } from '@angular/core';
import { User } from 'src/models';
import { UserService } from 'src/services';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {

  users: Array<User>;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getAllUsers()
      .subscribe(
        users => this.users = users,
        error => console.log(error)
      );
  }

  deleteUser(id: string) {
    this.userService.deleteUser(id).subscribe(
      () => {
        const index = this.users.findIndex(x => x.id === id);
        this.users.splice(index, 1);
        console.log('User deleted');
      },
      error => console.log(error)
    );
  }
}
