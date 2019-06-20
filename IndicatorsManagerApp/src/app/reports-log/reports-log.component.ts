import { Component, OnInit } from '@angular/core';
import { User } from 'src/models';
import { ReportsService } from 'src/services';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-reports-log',
  templateUrl: './reports-log.component.html',
  styleUrls: ['./reports-log.component.css']
})
export class ReportsLogComponent implements OnInit {

  logs: Array<any> = [];
  errorMessage = '';
  startDate = new FormControl('', [Validators.required]);
  endDate = new FormControl('', [Validators.required]);

  constructor(private reportsService: ReportsService) { }

  ngOnInit() {
  }

  transformDate(date: any) {
    return date.toISOString().split('T')[0];
  }

  transformDateByString(date: string) {
    return date.split('T')[0];
  }


  onChangeStartDate(event: any) {
    let value = event.target.value;
    if (value instanceof Date) {
      value = value.toISOString().split('T')[0];
    } else { this.startDate.setErrors({incorrect: true}); }
  }

  onChangeEndDate(event: any) {
    let value = event.target.value;
    if (value instanceof Date) {
      value = value.toISOString().split('T')[0];
    } else { this.endDate.setErrors({incorrect: true}); }
  }


  isValidDate() {
    return this.endDate.valid && this.startDate.valid;
  }

  showLog(){

    // console.log(this.startDate)
    // console.log(this.endDate)
    this.reportsService.getSystemActions(
      this.transformDate(this.startDate.value),
      this.transformDate(this.endDate.value)
    ).subscribe(
      response => {
        this.logs = response;
        console.log(this.logs);
      },
      error => this.errorMessage = error
    );
  }
}
