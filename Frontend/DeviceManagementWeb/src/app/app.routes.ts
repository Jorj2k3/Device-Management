import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { DeviceListComponent } from './components/device-list/device-list.component';

export const routes: Routes = [
  { path: 'users', component: UserListComponent },
  { path: 'devices', component: DeviceListComponent },
  { path: '', redirectTo: '/users', pathMatch: 'full' } // Default route redirects to users
];