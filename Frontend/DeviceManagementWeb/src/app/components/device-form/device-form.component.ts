import { Component, Input, Output, EventEmitter, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
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
  @Input() existingDevices: Device[] = [];
  
  @Output() save = new EventEmitter<Device>();
  @Output() cancel = new EventEmitter<void>();

  deviceForm: FormGroup;

  duplicateNameValidator = (control: AbstractControl): ValidationErrors | null => {
    if (!control.value || !this.existingDevices) return null;
    
    const currentId = this.deviceForm?.get('id')?.value || 0;
   
    const exists = this.existingDevices.some(
      d => d.name.toLowerCase() === control.value.toLowerCase() && d.id !== currentId
    );

    return exists ? { duplicateName: true } : null; 
  };

  constructor(private fb: FormBuilder) {
    this.deviceForm = this.fb.group({
      id: [0],
      name: ['', [Validators.required, this.duplicateNameValidator]],
      manufacturer: ['', Validators.required],
      type: ['Laptop', Validators.required],
      operatingSystem: ['Windows', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ramAmountGb: [null, [Validators.required, Validators.min(1)]],
      status: ['Available', Validators.required],
      description: [''],
      assignedUserId: [null],
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
      this.deviceForm.reset({ 
        id: 0, 
        type: 'Laptop', 
        operatingSystem: 'Windows', 
        status: 'Available',
        assignedUserId: null,
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