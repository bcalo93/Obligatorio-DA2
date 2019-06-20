import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { AuthGuard } from '../guards/auth.guard';
import { LoginComponent } from './login/login.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UsersListComponent } from './users-list/users-list.component';
import { ConditionEditComponent } from './condition-edit/condition-edit.component';
import { AreaEditComponent } from './area-edit/area-edit.component';
import { ManagerAssignmentComponent } from './manager-assignment/manager-assignment.component';
import { IndicatorEditComponent } from './indicator-edit/indicator-edit.component';
import { AreaListComponent } from './area-list/area-list.component';
import { ManagerAssignmentListComponent } from './manager-assignment-list/manager-assignment-list.component';
import { UserIndicatorConfigComponent } from './user-indicator-config/user-indicator-config.component';
import { ImporterComponent } from './importer/importer.component';
import { ReportsFrequentUserComponent } from './reports-frequent-user/reports-frequent-user.component';
import { ReportsTopHiddenIndicatorsComponent } from './reports-top-hidden-indicators/reports-top-hidden-indicators.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },

  { path: 'users', component: UsersListComponent, canActivate: [AuthGuard] },
  { path: 'users/add', component: UserEditComponent, canActivate: [AuthGuard] },
  { path: 'users/:id', component: UserEditComponent, canActivate: [AuthGuard] },

  { path: 'areas', component: AreaListComponent, canActivate: [AuthGuard] },
  { path: 'areas/add', component: AreaEditComponent, canActivate: [AuthGuard] },
  { path: 'areas/managers', component: ManagerAssignmentComponent, canActivate: [AuthGuard] },
  { path: 'areas/managers/:id', component: ManagerAssignmentListComponent, canActivate: [AuthGuard] },


  { path: 'areas/:id', component: AreaEditComponent, canActivate: [AuthGuard] },

  { path: 'indicator/config', component: UserIndicatorConfigComponent, canActivate: [AuthGuard] },

  { path: 'indicator/:id', component: IndicatorEditComponent, canActivate: [AuthGuard] },
  { path: 'indicator/:id/condition/add', component: ConditionEditComponent, canActivate: [AuthGuard] },

  { path: 'importer', component: ImporterComponent, canActivate: [AuthGuard]},

  { path: 'reports/frequent-users', component: ReportsFrequentUserComponent, canActivate: [AuthGuard]},
  { path: 'reports/hidden-indicators', component: ReportsTopHiddenIndicatorsComponent, canActivate: [AuthGuard]},
  { path: 'reports/log', component: ReportsFrequentUserComponent, canActivate: [AuthGuard]},

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
