import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; //
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})
export class DeviceListComponent implements OnInit {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  
  devices: any[] = [];
  isAdmin = false;
  selectedDevice: any = null;
  
  isCreating = false;
  newDevice: any = {};

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

  viewDetails(device: any) {
    this.isCreating = false;
    this.selectedDevice = device;
  }

  closeDetails() {
    this.selectedDevice = null;
  }

  openCreateForm() {
    this.selectedDevice = null; 
    this.isCreating = true;

    this.newDevice = {
      name: '',
      manufacturer: '',
      type: 'Laptop',
      operatingSystem: 'Windows',
      osVersion: '',
      processor: '',
      ramAmountGb: null,
      description: '',
      status: 'Available',
      assignedUserID: null
    };
  }

  closeCreateForm() {
    this.isCreating = false;
  }

  saveNewDevice() {
    this.apiService.createDevice(this.newDevice).subscribe({
      next: () => {
        this.isCreating = false;
        this.loadDevices();
      },
      error: (err) => {
        console.error('Error creating device:', err);
        alert('Failed to create device. Check console for details.');
      }
    });
  }

  deleteDevice(id: number) {
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