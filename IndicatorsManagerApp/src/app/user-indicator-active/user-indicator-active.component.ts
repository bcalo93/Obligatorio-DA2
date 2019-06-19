import { Component, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { User, Indicator } from 'src/models';
import { UserService, AuthService } from 'src/services';
import { IndicatorConfig } from 'src/models/indicatorConfig';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-user-indicator-active',
  templateUrl: './user-indicator-active.component.html',
  styleUrls: ['./user-indicator-active.component.css']
})
export class UserIndicatorActiveComponent implements OnInit {


  indicators = new Array<any>();
  errorMessage = '';

  constructor(
    private userService: UserService,
    private authService: AuthService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.userService.getManagerIndicators().subscribe(
      response => {
        console.log(response);
        if (response.length === 0) {
          this.errorMessage =
          'Currently you are not assigned to any area, ' +
          'or the area to which you are assigned does not have any associated information yet. ' +
          'Please contact with your administrator for more information.';
        } else {
          this.indicators = response;
        }
      },
      error => this.errorMessage = error
    );
  }

  getName(item: any) {
    if (item.alias) {
      return item.alias;
    } else { return item.name; }
  }

  getColour(item: any) {
    return item.items[0].name;
  }

  openInformation(){
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '500px',
      height: '500px',
      data: {
        header: 'The actual condition detail: ',
        message: 'The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:The actual condition detail:',
        currentUser: this.authService.getCurrentUser()
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {}});
  }
}
