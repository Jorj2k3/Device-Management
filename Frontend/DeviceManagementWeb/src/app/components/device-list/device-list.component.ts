import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.scss'
})
export class DeviceListComponent implements OnInit {
  private apiService = inject(ApiService);
  
  devices: any[] = [];

  ngOnInit() {
    this.apiService.getDevices().subscribe({
      next: (data) => {
        this.devices = data;
        console.log('Devices loaded:', this.devices);
      },
      error: (err) => console.error('API Error:', err)
    });
  }
}