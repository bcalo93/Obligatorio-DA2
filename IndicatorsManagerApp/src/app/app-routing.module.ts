import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { HomeManagerComponent } from './home-manager/home-manager.component';
import { AuthGuard } from '../guards/auth.guard';
import { LoginCustomComponent } from './login-custom/login-custom.component';

const routes: Routes = [
  { path: '', component: HomeManagerComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'loginNew', component: LoginCustomComponent },
  { path: '**', redirectTo: '' },


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
