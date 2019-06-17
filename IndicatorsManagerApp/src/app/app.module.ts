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
  MatNativeDateModule

} from '@angular/material';

import {DragDropModule} from '@angular/cdk/drag-drop';


import { HomeManagerComponent } from './home-manager/home-manager.component';
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

@NgModule({
  declarations: [
    AppComponent,
    HomeManagerComponent,
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
    AreaListComponent
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
  ],
  providers: [],
  bootstrap: [AppComponent],

})
export class AppModule { }
