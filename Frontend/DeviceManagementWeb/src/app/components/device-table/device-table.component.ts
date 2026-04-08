import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './device-table.component.html',
  styleUrl: './device-table.component.scss'
})
export class DeviceTableComponent {
  @Input() devices: Device[] = [];
  @Input() selectedDeviceId?: number;
  
  // NEW: Tells the table which buttons to show!
  @Input() mode: 'admin' | 'my-devices' | 'available' = 'admin'; 

  @Output() viewDevice = new EventEmitter<Device>();
  @Output() assign = new EventEmitter<Device>();   // NEW EVENT
  @Output() unassign = new EventEmitter<Device>(); // NEW EVENT
}