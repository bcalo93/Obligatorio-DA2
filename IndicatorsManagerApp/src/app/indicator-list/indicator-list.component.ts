import { Component, OnInit, Input } from '@angular/core';
import { Indicator } from 'src/models';
import { AreaService } from 'src/services';
import { IndicatorService } from 'src/services/indicator.service';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-indicator-list',
  templateUrl: './indicator-list.component.html',
  styleUrls: ['./indicator-list.component.css']
})
export class IndicatorListComponent implements OnInit {

  @Input() areaId: string;
  indicators: Array<Indicator>;
  name = new FormControl('', [Validators.required]);
  errorMessage: string;

  constructor(
    private areaService: AreaService,
    private indicatorService: IndicatorService,
    private router: Router) { }

  ngOnInit() {
    this.areaService.getIndicators(this.areaId)
      .subscribe(
        indicators => {
          console.log(indicators)
          const orderedList = indicators.reverse();
          this.indicators = orderedList;
        },
        error => this.errorMessage = error
      );
  }

  deleteIndicator(indicatorId: string) {
    this.indicatorService.deleteIndicator(indicatorId).subscribe(
      () => {
        const index = this.indicators.findIndex(x => x.id === indicatorId);
        this.indicators.splice(index, 1);
        console.log('Indicator deleted');
      },
      error => this.errorMessage = error
    );
  }

  editIndicator(indicatorId: string) {
    this.router.navigate(['/indicator', indicatorId]);
  }


  save() {
    const indicator = new Indicator();
    indicator.name = this.name.value;
    this.areaService.addIndicators(this.areaId, indicator).subscribe(
      response => {
        document.getElementById('indicator-name').blur();
        this.name.reset();
        this.indicators.splice(0, 0, response);
      },
      error => this.errorMessage = error
    );
  }

}
