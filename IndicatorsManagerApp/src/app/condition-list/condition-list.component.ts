import { Component, OnInit, Input } from '@angular/core';
import { Indicator, IndicatorItem } from 'src/models';
import { Router } from '@angular/router';
import { IndicatorService } from 'src/services/indicator.service';

@Component({
  selector: 'app-condition-list',
  templateUrl: './condition-list.component.html',
  styleUrls: ['./condition-list.component.css']
})
export class ConditionListComponent implements OnInit {

  @Input() indicatorId: string;
  conditions: Array<IndicatorItem>;
  errorMessage: string;

  constructor(
    private indicatorService: IndicatorService,
    private router: Router) { }

  ngOnInit() {
    this.indicatorService.getIndicator(this.indicatorId)
      .subscribe(
        response => {
          const indicator = response;
          this.conditions = !!indicator.itemsResult ? indicator.itemsResult.reverse() : [];
        },
        error => this.errorMessage = error
      );
  }

  deleteCondition(indicatorId: string) {
    this.indicatorService.deleteIndicatorItem(indicatorId).subscribe(
      () => {
        const index = this.conditions.findIndex(x => x.id === indicatorId);
        this.conditions.splice(index, 1);
        console.log('Condition deleted');
      },
      error => this.errorMessage = error
    );
  }

  addCondition() {
    const actualUrl = this.router.url;
    this.router.navigate([actualUrl + '/condition/add']);
  }

  getValue(conditionResult: any) {
    if (typeof conditionResult === 'string') {
      return 'string';
    } else {
      return conditionResult ? 'true' :  'false';
    }
  }

  getColour(name: string) {
    return name === 'YELLOW' ? 'ORANGE' : name;
  }
}
