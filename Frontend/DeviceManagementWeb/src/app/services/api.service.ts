import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Device } from '../models/device.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);

  private baseUrl = '/api'; 

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Users`);
  }

  getUser(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Users/${id}`);
  }

  updateUser(id: number, user: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/Users/${id}`, user);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Users/${id}`);
  }

  getDevices(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Devices`);
  }

  getDevice(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Devices/${id}`);
  }

  createDevice(device: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Devices`, device);
  }

  updateDevice(id: number, device: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/Devices/${id}`, device);
  }

  deleteDevice(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Devices/${id}`);
  }

  generateDescription(deviceData: any): Observable<{description: string}> {
    return this.http.post<{description: string}>(`${this.baseUrl}/Devices/GenerateDescription`, deviceData);
  }

  searchDevices(query: string): Observable<Device[]> {
    if (!query || query.trim() === '') {
      return this.getDevices();
    }
    return this.http.get<Device[]>(`${this.baseUrl}/Devices/Search?q=${encodeURIComponent(query)}`);
  }
}