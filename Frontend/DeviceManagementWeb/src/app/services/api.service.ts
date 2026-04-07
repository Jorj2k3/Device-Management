import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);

  private baseUrl = 'https://localhost:7177/api'; 

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Users`);
  }
  getDevices(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Devices`);
  }
}