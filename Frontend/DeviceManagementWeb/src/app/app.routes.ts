import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'users', component: UserListComponent },
  { path: 'devices', component: DeviceListComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];