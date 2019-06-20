import { Component, OnInit } from '@angular/core';
import { User } from 'src/models';
import { ReportsService } from 'src/services';

@Component({
  selector: 'app-reports-frequent-user',
  templateUrl: './reports-frequent-user.component.html',
  styleUrls: ['./reports-frequent-user.component.css']
})
export class ReportsFrequentUserComponent implements OnInit {

  users: Array<User> = [];
  errorMessage = '';

  constructor(private reportsService: ReportsService) { }

  ngOnInit() {
    this.reportsService.getTopFrequentUsers().subscribe(
      response => {
        this.users = response;
      },
      error => this.errorMessage = error
    );
  }

}
