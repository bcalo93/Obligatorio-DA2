import { Component, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { User, Indicator } from 'src/models';
import { UserService, AuthService } from 'src/services';
import { IndicatorConfig } from 'src/models/indicatorConfig';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';

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
    const aux = [...this.indicators];
    const newList = Array<IndicatorConfig>();
    aux.reverse().forEach(
      (item, index) => {
        const newItem = new IndicatorConfig(item);
        newItem.setPosition(index);
        newList.push(newItem);
      }
    );
    this.userService.updateIndicatorConfiguration(newList)
    .subscribe(
      () => {
        console.log('newList', newList);
        moveItemInArray(aux, event.previousIndex, event.currentIndex);
      },
    );
  }

  getName(item: any) {
    if (item.alias) {
      return item.alias;
    } else { return item.name; }
  }

  changeIndicatorVisibility(indicatorConfig: any) {
    const currentPosition = (indicatorConfig.position) ? indicatorConfig.position : 0;
    const newIndicatorConfig = {
        indicatorId: indicatorConfig.id,
        // position: currentPosition,
        isVisible: indicatorConfig.isVisible,
        // alias: indicatorConfig.name
      };
    const updateConfig = new IndicatorConfig(newIndicatorConfig as IndicatorConfig);
    console.log('ANTES', updateConfig);

    updateConfig.toggleVisibility();
    console.log('DESPUES', updateConfig);
    const aux = new Array<IndicatorConfig>();
    aux.push(updateConfig);
    console.log('changeIndicatorVisibility BODY', aux);
    this.userService.updateIndicatorConfiguration(aux)
    .subscribe(
      () => {
        indicatorConfig.isVisible = !indicatorConfig.isVisible;
        console.log('Indicator visibility config updated')
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
}
