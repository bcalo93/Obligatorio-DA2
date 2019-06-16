import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeManagerComponent } from './home-manager/home-manager.component';
import { AuthGuard } from '../guards/auth.guard';
import { LoginComponent } from './login/login.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UsersListComponent } from './users-list/users-list.component';
import { ConditionEditComponent } from './condition-edit/condition-edit.component';
import { AreaEditComponent } from './area-edit/area-edit.component';
import { ManagerAssignmentComponent } from './manager-assignment/manager-assignment.component';
import { IndicatorEditComponent } from './indicator-edit/indicator-edit.component';

const routes: Routes = [
  { path: '', component: HomeManagerComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'users/add', component: UserEditComponent },
  { path: 'users/:id', component: UserEditComponent },
  { path: 'users', component: UsersListComponent },
  { path: 'areas/add', component: AreaEditComponent },
  { path: 'areas/:id', component: AreaEditComponent },
  { path: 'indicator/:id', component: IndicatorEditComponent },
  { path: 'condition/add', component: ConditionEditComponent },
  { path: 'areas/managers', component: ManagerAssignmentComponent },

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
