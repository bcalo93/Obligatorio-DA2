import { Component, OnInit } from '@angular/core';
import { Indicator } from 'src/models';
import { FormControl, Validators } from '@angular/forms';
import { IndicatorService } from 'src/services/indicator.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-indicator-edit',
  templateUrl: './indicator-edit.component.html',
  styleUrls: ['./indicator-edit.component.css']
})
export class IndicatorEditComponent implements OnInit {

  indicatorId: string;
  errorMessage: string;

  name = new FormControl('', [Validators.required]);

  constructor(
    private indicatorService: IndicatorService,
    private location: Location,
    private currentRoute: ActivatedRoute,
    private router: Router) {}

  ngOnInit() {
    const id = this.currentRoute.snapshot.paramMap.get('id');
    if (id) {
      this.indicatorId = id;
      this.loadIndicatorDetails(id);
    }
  }

  updateIndicator() {
    console.log(this.indicatorId);
    console.log(this.name.value);
    const indicator = new Indicator();
    indicator.id = this.indicatorId;
    indicator.name = this.name.value;
    this.indicatorService.updateIndicatorName(this.indicatorId, indicator)
    .subscribe(
      (data: Indicator) => {
        this.clearErrorMsg();
        console.log('Indicator Updated', data);
      },
      (error: any) => {
        console.log(error);
        this.errorMessage = error;
      }
    );
  }

  loadIndicatorDetails(id: string) {
    this.indicatorService.getIndicator(id)
      .subscribe(indicator => {
        console.log(indicator)
        this.name.setValue(indicator.name);
        this.clearErrorMsg();
    });
  }

  clearErrorMsg() {
    this.errorMessage = '';
  }

  goBack(): void {
    this.location.back();
  }
}
