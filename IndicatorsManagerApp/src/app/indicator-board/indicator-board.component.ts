import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-indicator-board',
  templateUrl: './indicator-board.component.html',
  styleUrls: ['./indicator-board.component.css']
})
export class IndicatorBoardComponent implements OnInit {

  @Input() indicatorsList: Array<any> = [];
  greenCount = 0;
  yellowCount = 0;
  redCount = 0;

  constructor() { 
  }

  ngOnInit() {
    console.log('ngOnInit', this.indicatorsList);
    console.log('ngOnInit', this.indicatorsList.length);
    this.indicatorsList.forEach(item => this.addCount(item.activeItems[0]));
  }

  addCount(color: string) {
    switch (color) {
      case 'GREEN':
        this.greenCount++;
        break;
      case 'YELLOW':
        this.yellowCount++;
        break;
      case 'RED':
        this.redCount++;
        break;
    }
  }
}
