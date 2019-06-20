import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {
  MatButtonModule,
  MatCheckboxModule,
  MatFormFieldModule,
  MatInputModule,
  MatRippleModule,
  MatSelectModule,
  MatRadioModule,
  MatIconModule,
  MatCardModule,
  MatMenuModule,
  MatToolbarModule,
  MatSnackBarModule,
  MatListModule,
  MatTableModule,
  MatTreeModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatTooltipModule,
  MatDialogModule

} from '@angular/material';

import {DragDropModule} from '@angular/cdk/drag-drop';

import { LoginComponent } from './login/login.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { ErrorMessageComponent } from './error-message/error-message.component';
import { HeaderComponent } from './header/header.component';
import { UsersListComponent } from './users-list/users-list.component';
import { ConditionEditComponent } from './condition-edit/condition-edit.component';
import { UserIndicatorConfigComponent } from './user-indicator-config/user-indicator-config.component';
import { ConditionDropdownComponent } from './condition-dropdown/condition-dropdown.component';
import { AreaEditComponent } from './area-edit/area-edit.component';
import { ManagerAssignmentComponent } from './manager-assignment/manager-assignment.component';
import { IndicatorListComponent } from './indicator-list/indicator-list.component';
import { IndicatorEditComponent } from './indicator-edit/indicator-edit.component';
import { ConditionListComponent } from './condition-list/condition-list.component';
import { AreaListComponent } from './area-list/area-list.component';
import { ManagerAssignmentListComponent } from './manager-assignment-list/manager-assignment-list.component';
import { DialogComponent } from './dialog/dialog.component';
import { NoDataComponent } from './no-data/no-data.component';
import { HomeComponent } from './home/home.component';
import { UserIndicatorActiveComponent } from './user-indicator-active/user-indicator-active.component';
import { IndicatorBoardComponent } from './indicator-board/indicator-board.component';
import { ImporterComponent } from './importer/importer.component';
import { ReportsFrequentUserComponent } from './reports-frequent-user/reports-frequent-user.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UserEditComponent,
    ErrorMessageComponent,
    HeaderComponent,
    UsersListComponent,
    ConditionEditComponent,
    UserIndicatorConfigComponent,
    ConditionDropdownComponent,
    AreaEditComponent,
    ManagerAssignmentComponent,
    IndicatorListComponent,
    IndicatorEditComponent,
    ConditionListComponent,
    AreaListComponent,
    ManagerAssignmentListComponent,
    DialogComponent,
    NoDataComponent,
    HomeComponent,
    UserIndicatorActiveComponent,
    IndicatorBoardComponent,
    ImporterComponent,
    ReportsFrequentUserComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatRippleModule,
    MatSelectModule,
    MatRadioModule,
    MatIconModule,
    MatCardModule,
    MatMenuModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatListModule,
    DragDropModule,
    MatTableModule,
    MatTreeModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTooltipModule,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [DialogComponent]

})
export class AppModule { }
