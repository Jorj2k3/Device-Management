import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Device } from '../../models/device.model';
import { ActivatedRoute, Router } from '@angular/router';

import { DeviceTableComponent } from '../device-table/device-table.component';
import { DeviceDetailsComponent } from '../device-details/device-details.component';
import { DeviceFormComponent } from '../device-form/device-form.component';

import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule, DeviceTableComponent, DeviceDetailsComponent, DeviceFormComponent, ReactiveFormsModule], 
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})

export class DeviceListComponent implements OnInit {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  
  devices: Device[] = [];
  isAdmin = false;
  
  currentUserId: number | null = null;
  myDevices: Device[] = [];
  availableDevices: Device[] = [];

  activeTab: 'my-devices' | 'available' = 'my-devices';

  selectedDevice: Device | null = null;
  isCreating = false;
  isEditing = false;

  searchControl = new FormControl('');

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();
    this.currentUserId = this.authService.getCurrentUserId(); 

    this.route.queryParams.subscribe(params => {
      if (params['tab']) {
        this.activeTab = params['tab'] as 'my-devices' | 'available';
      } else if (!this.isAdmin) {
        this.router.navigate([], { queryParams: { tab: 'my-devices' } });
      }
    });

    this.loadDevices();

    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(query => {
      this.apiService.searchDevices(query || '').subscribe(data => this.devices = data);
    });
  }

  loadDevices() {
    this.apiService.getDevices().subscribe({
      next: (data) => {
        this.devices = data;
        
        if (!this.isAdmin && this.currentUserId) {
          this.myDevices = this.devices.filter(d => d.assignedUserID === this.currentUserId);
          this.availableDevices = this.devices.filter(d => d.assignedUserID === null && d.status === 'Available');
        }
      },
      error: (err) => console.error('API Error:', err)
    });
  }

  onAssign(device: Device) {
    if (!this.currentUserId) return;
    const updatedDevice: Device = { ...device, assignedUserID: this.currentUserId, status: 'In Use' };

    this.apiService.updateDevice(device.id, updatedDevice).subscribe({
      next: () => {
        // Automatically jump to the My Devices tab when they claim a device!
        this.router.navigate([], { queryParams: { tab: 'my-devices' } }); 
        this.loadDevices();
      },
      error: (err) => console.error('Error assigning device', err)
    });
  }

  onUnassign(device: Device) {
    const updatedDevice: Device = { ...device, assignedUserID: null, status: 'Available' };

    this.apiService.updateDevice(device.id, updatedDevice).subscribe({
      next: () => {
        this.router.navigate([], { queryParams: { tab: 'available' } }); 
        this.loadDevices();
      },
      error: (err) => console.error('Error returning device', err)
    });

    this.apiService.updateDevice(device.id, updatedDevice).subscribe({
      next: () => this.loadDevices(), 
      error: (err) => console.error('Error returning device', err)
    });
  }



  onDeleteDevice(id: number) {
    if (confirm('Are you sure you want to delete this device?')) {
      this.apiService.deleteDevice(id).subscribe({
        next: () => {
          this.selectedDevice = null;
          this.loadDevices();
        },
        error: (err) => console.error('Error deleting device:', err)
      });
    }
  }

  onViewDevice(device: Device) {
    this.isCreating = false;
    this.isEditing = false;
    this.selectedDevice = device;
  }

  onOpenCreate() {
    this.selectedDevice = null;
    this.isEditing = false;
    this.isCreating = true;
  }

  onCloseDetails() {
    this.selectedDevice = null;
  }

  onOpenEdit() {
    this.isEditing = true;
  }

  onCloseForm() {
    this.isCreating = false;
    this.isEditing = false;
  }

  onSaveDevice(device: Device) {
    if (this.isEditing) {
      // Preserve the assignedUserID from the original device since the form doesn't expose it
      const updatedDevice: Device = {
        ...device,
        assignedUserID: this.selectedDevice?.assignedUserID ?? device.assignedUserID
      };
      this.apiService.updateDevice(updatedDevice.id, updatedDevice).subscribe({
        next: () => {
          this.isEditing = false;
          this.selectedDevice = null;
          this.loadDevices();
        },
        error: (err) => console.error('Error updating device:', err)
      });
    } else {
      this.apiService.createDevice(device).subscribe({
        next: () => {
          this.isCreating = false;
          this.loadDevices();
        },
        error: (err) => console.error('Error creating device:', err)
      });
    }
  }
}