import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})
export class DeviceListComponent implements OnInit {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);
  devices: any[] = [];
  isAdmin = false;

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin(); // <-- 4. Set the flag when the page loads!
    this.loadDevices();
  }
  loadDevices() {
    this.apiService.getDevices().subscribe({
      next: (data) => this.devices = data,
      error: (err) => console.error('API Error:', err)
    });
  }

  deleteDevice(id: number) {
    if (confirm('Are you sure you want to delete this device?')) {
      this.apiService.deleteDevice(id).subscribe({
        next: () => this.loadDevices(),
        error: (err) => console.error('Error deleting device:', err)
      });
    }
  }
}