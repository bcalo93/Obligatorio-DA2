import { Component, OnInit } from '@angular/core';
import { Indicator } from 'src/models';
import { ReportsService } from 'src/services';

@Component({
  selector: 'app-reports-top-hidden-indicators',
  templateUrl: './reports-top-hidden-indicators.component.html',
  styleUrls: ['./reports-top-hidden-indicators.component.css']
})
export class ReportsTopHiddenIndicatorsComponent implements OnInit {

  indicators: Array<Indicator> = [];
  errorMessage = '';

  constructor(private reportsService: ReportsService) { }

  ngOnInit() {
    this.reportsService.getTopHiddenReports().subscribe(
      response => {
        this.indicators = response;
      },
      error => this.errorMessage = error
    );
  }

}
