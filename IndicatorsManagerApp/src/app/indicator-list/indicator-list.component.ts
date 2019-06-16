import { Component, OnInit, Input } from '@angular/core';
import { Indicator } from 'src/models';
import { AreaService } from 'src/services';
import { IndicatorService } from 'src/services/indicator.service';
import { FormControl, Validators } from '@angular/forms';

const indicatorsTEST = [{name: "indicator 1"},{name: "indicator 2"},{name: "indicator 3"}]

@Component({
  selector: 'app-indicator-list',
  templateUrl: './indicator-list.component.html',
  styleUrls: ['./indicator-list.component.css']
})
export class IndicatorListComponent implements OnInit {

  @Input() areaId: string;
  indicators: Array<Indicator>;
  name = new FormControl('', [Validators.required]);

  constructor(
    private areaService: AreaService,
    private indicatorService: IndicatorService ) { }

  ngOnInit() {
    this.areaService.getIndicators(this.areaId)
      .subscribe(
        indicators => this.indicators = indicators,
        error => {
          this.indicators = indicatorsTEST as Array<Indicator>,
          console.log(error);
        }
      );
  }

  deleteUser(indicatorId: string) {
    this.indicatorService.deleteIndicator(indicatorId).subscribe(
      () => {
        const index = this.indicators.findIndex(x => x.id === indicatorId);
        this.indicators.splice(index, 1);
        console.log('Indicator deleted');
      },
      error => console.log(error)
    );
  }

  save() {
    const indicator = new Indicator();
    indicator.name = this.name.value;
    this.indicators.splice(0, 0, indicator);
    this.name.setValue('');
    // this.areaService.addIndicators(this.areaId, indicator).subscribe(
    //   () => { },
    //   error => console.log(error)
    // );
  }

}
