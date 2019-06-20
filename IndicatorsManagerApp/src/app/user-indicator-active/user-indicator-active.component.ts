import { Component, OnInit } from '@angular/core';
import { UserService, AuthService } from 'src/services';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';
import { IndicatorService } from 'src/services/indicator.service';

const helpMessage = 'In this section you will see:' +
 '<ul style="list-style: none;">' +
 '<li><span>&#10003;</span> <strong>Active</strong> indicators with current configuration</li>' +
 '<li><span>&#10003;</span> <strong>Colour</strong> of the condition which activate the indicator</li>' +
 '<li><span>&#10003;</span> <strong>Condition</strong> evaluated in plain text</li>' +
 '</ul>';

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
    private indicatorService: IndicatorService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.userService.getManagerActiveIndicators().subscribe(
      response => {
        if (response.length === 0) {
          this.errorMessage =
          'Currently you do not have active indicators.' +
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
    const name = item.items[0].name;
    return name === 'YELLOW' ? 'ORANGE' : name;
  }

  openInformation(item: any) {
    let message = '';
    this.indicatorService.getIndicator(item.id).subscribe(
      response => {
        const itemId = item.items[0].id;
        const aux: Array<any> = response.itemsResult.map( x => x);
        message = aux.find(x => x.id === itemId).result.conditionToString;
        this.openDialog(message);
      },
      () => this.openDialog('Currently the condition result is unavailable.')
    );
  }

  openDialog(message: string) {
    this.dialog.open(DialogComponent, {
      width: 'fit-content',
      maxWidth: '80%',
      height: 'auto',
      data: {
        header: 'The actual condition detail: ',
        message,
      }
    });
  }

  openHelp() {
    this.dialog.open(DialogComponent, {
      width: '500px',
      height: '300px',
      data: {
        message: helpMessage,
        currentUser: this.authService.getCurrentUser(),
      }
    });
  }
}