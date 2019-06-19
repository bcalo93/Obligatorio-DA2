import { Component, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { User } from 'src/models';
import { UserService } from 'src/services';

@Component({
  selector: 'app-user-indicator-config',
  templateUrl: './user-indicator-config.component.html',
  styleUrls: ['./user-indicator-config.component.css']
})
export class UserIndicatorConfigComponent implements OnInit {

  indicators = new Array<User>();
  errorMessage = '';

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getManagerIndicators().subscribe(
      response => {
        console.log(response);
        if (response.length === 0) {
          this.errorMessage = 'Currently you are not assigned to any area, or the area to which you are assigned does not have any associated information yet. Please contact with your administrator for more information.'
        }
      },
      error => this.errorMessage = error
    );
  }


  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.indicators, event.previousIndex, event.currentIndex);
  }

}
