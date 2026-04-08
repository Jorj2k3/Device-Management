import { Component, Input, Output, EventEmitter, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Device } from '../../models/device.model';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './device-form.component.html',
  styleUrl: './device-form.component.scss'
})
export class DeviceFormComponent implements OnInit, OnChanges {
  @Input() initialData: Device | null = null;
  @Input() isEditing = false;
  
  @Output() save = new EventEmitter<Device>();
  @Output() cancel = new EventEmitter<void>();

  deviceForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.deviceForm = this.fb.group({
      id: [0], // Default to 0
      name: ['', Validators.required],
      manufacturer: ['', Validators.required],
      type: ['Laptop', Validators.required],
      operatingSystem: ['Windows', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ramAmountGb: [null, [Validators.required, Validators.min(1)]],
      status: ['Available', Validators.required],
      description: [''],
      // ADD THESE SO WE DON'T LOSE THE OWNER WHEN EDITING!
      assignedUserID: [null], 
      assignedUserName: [null]
    });
  }

  ngOnInit() {
    this.updateForm();
  }

  ngOnChanges() {
    this.updateForm();
  }

  updateForm() {
    if (this.initialData) {
      this.deviceForm.patchValue(this.initialData); 
    } else {
      // THE FIX: Explicitly set 'id: 0' so it doesn't become 'null'!
      this.deviceForm.reset({ 
        id: 0, 
        type: 'Laptop', 
        operatingSystem: 'Windows', 
        status: 'Available',
        assignedUserID: null,
        assignedUserName: null
      });
    }
  }

  onSubmit() {
    if (this.deviceForm.valid) {
      this.save.emit(this.deviceForm.value as Device);
    } else {
      this.deviceForm.markAllAsTouched(); 
    }
  }
}