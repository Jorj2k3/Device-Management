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

  @Output() viewDevice = new EventEmitter<Device>();

  onViewClick(device: Device) {
    this.viewDevice.emit(device);
  }
}