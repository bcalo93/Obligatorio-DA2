import { Component, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { User, Indicator } from 'src/models';
import { UserService, AuthService } from 'src/services';
import { IndicatorConfig } from 'src/models/indicatorConfig';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';


const helpMessage = 'In this section you will be able to:' +
 '<ul style="list-style: none;">' +
 '<li><span>&#10003;</span> <strong>Drag and drop</strong> indicators to alter the order of them in main page</li>' +
 '<li><span>&#10003;</span> <strong>Edit</strong> indicator current name</li>' +
 '<li><span>&#10003;</span> <strong>Toggle</strong> indicator visibility</li>' +
 '</ul>';

@Component({
  selector: 'app-user-indicator-config',
  templateUrl: './user-indicator-config.component.html',
  styleUrls: ['./user-indicator-config.component.css']
})
export class UserIndicatorConfigComponent implements OnInit {

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


  drop(event: CdkDragDrop<any>) {
    if (event.previousIndex !== event.currentIndex) {
      moveItemInArray(this.indicators, event.previousIndex, event.currentIndex);
      const newList = Array<IndicatorConfig>();
      const aux = [...this.indicators].map((item, index) => {
        const newIndicatorConfig = {
          indicatorId: item.id,
          position: index,
          isVisible: item.isVisible,
          alias: (item.alias) ? item.alias : item.name
        };
        return newIndicatorConfig;
      });
      this.userService.updateIndicatorConfiguration(aux as Array<IndicatorConfig>)
      .subscribe(
        () => {
          console.log('newList', newList);
        },
        error => {
          moveItemInArray(this.indicators, event.currentIndex, event.previousIndex);
          this.errorMessage = error;
        }
      );
    }
  }

  getName(item: any) {
    if (item.alias) {
      return item.alias;
    } else { return item.name; }
  }

  changeIndicatorVisibility(indicatorConfig: any) {
    const newIndicatorConfig = {
        indicatorId: indicatorConfig.id,
        position: indicatorConfig.position,
        isVisible: indicatorConfig.isVisible,
        alias: (indicatorConfig.alias) ? indicatorConfig.alias : indicatorConfig.name
      };
    const updateConfig = new IndicatorConfig(newIndicatorConfig as IndicatorConfig);
    updateConfig.toggleVisibility();
    const aux = new Array<IndicatorConfig>();
    aux.push(updateConfig);
    this.userService.updateIndicatorConfiguration(aux)
    .subscribe(
      () => {
        indicatorConfig.isVisible = !indicatorConfig.isVisible;
      },
      error => this.errorMessage = error
    );
  }

  updateIndicatorAlias(indicatorConfig: any) {
    console.log('updateIndicatorAlias', indicatorConfig);
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '400px',
      height: '250px',
      data: {
        message: 'Please enter a new name for your indicator: ',
        currentUser: this.authService.getCurrentUser(),
        showInput: true,
        alias: indicatorConfig.name
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('indicatorConfig', indicatorConfig)
        const currentPosition = (indicatorConfig.position) ? indicatorConfig.position : 0;
        const newIndicatorConfig = {
          indicatorId: indicatorConfig.id,
          position: currentPosition,
          isVisible: indicatorConfig.isVisible,
          alias: result
        };
        const updateConfig = new IndicatorConfig(newIndicatorConfig as IndicatorConfig);
        debugger
        const aux = new Array<IndicatorConfig>();
        aux.push(updateConfig);
        console.log(aux);
        this.userService.updateIndicatorConfiguration(aux)
        .subscribe(
          () => {
            indicatorConfig.alias = result;
            console.log('Indicator alias config updated ');
          },
          error => this.errorMessage = error
        );
      }
    });
  }


  openHelp() {
    this.dialog.open(DialogComponent, {
      width: '400px',
      height: '300px',
      data: {
        message: helpMessage,
        currentUser: this.authService.getCurrentUser(),
      }
    });
  }
}

