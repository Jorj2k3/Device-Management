import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'users', component: UserListComponent, canActivate: [authGuard] },
  { path: 'devices', component: DeviceListComponent, canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];