import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { Device } from '../../models/device.model';

// Import our new Dumb components!
import { DeviceTableComponent } from '../device-table/device-table.component';
import { DeviceDetailsComponent } from '../device-details/device-details.component';
import { DeviceFormComponent } from '../device-form/device-form.component';

@Component({
  selector: 'app-device-list',
  standalone: true,
  // Declare that this Smart component uses these Dumb components
  imports: [CommonModule, DeviceTableComponent, DeviceDetailsComponent, DeviceFormComponent], 
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})
export class DeviceListComponent implements OnInit {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  
  // 1. Strictly typed state!
  devices: Device[] = [];
  isAdmin = false;
  
  selectedDevice: Device | null = null;
  isCreating = false;
  isEditing = false;

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();
    this.loadDevices();
  }

  loadDevices() {
    this.apiService.getDevices().subscribe({
      next: (data) => this.devices = data,
      error: (err) => console.error('API Error:', err)
    });
  }

  // --- Handlers for events emitted by Dumb Components ---

  onViewDevice(device: Device) {
    this.isCreating = false;
    this.isEditing = false;
    this.selectedDevice = device;
  }

  onCloseDetails() {
    this.selectedDevice = null;
  }

  onOpenCreate() {
    this.selectedDevice = null; 
    this.isEditing = false;
    this.isCreating = true;
  }

  onOpenEdit() {
    this.isCreating = false;
    this.isEditing = true;
  }

  onCloseForm() {
    this.isCreating = false;
    this.isEditing = false;
  }

  onSaveDevice(deviceData: Device) {
    if (this.isCreating) {
      this.apiService.createDevice(deviceData).subscribe({
        next: () => {
          this.onCloseForm();
          this.loadDevices();
        },
        error: (err) => console.error('Error creating device:', err)
      });
    } else if (this.isEditing) {
      this.apiService.updateDevice(deviceData.id, deviceData).subscribe({
        next: () => {
          this.onCloseForm();
          this.selectedDevice = deviceData; 
          this.loadDevices();
        },
        error: (err) => console.error('Error updating device:', err)
      });
    }
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
}