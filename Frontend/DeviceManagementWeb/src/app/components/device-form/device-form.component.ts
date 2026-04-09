import { Component, Input, Output, EventEmitter, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Device } from '../../models/device.model';
import { ApiService } from '../../services/api.service';

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

  constructor(private fb: FormBuilder, private apiService: ApiService) {
    this.deviceForm = this.fb.group({
      id: [0],
      name: ['', [Validators.required, this.duplicateNameValidator]],
      manufacturer: ['', Validators.required],
      type: ['Laptop', Validators.required],
      operatingSystem: ['Windows', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ramAmountGb: [null, [Validators.required, Validators.min(1)]],
      status: ['Available'],
      description: [''],
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

  isGeneratingAi = false;
  onGenerateDescription() {
    this.isGeneratingAi = true;
    const data = this.deviceForm.value;
  
    if (!data.name || !data.manufacturer || !data.processor || !data.ramAmountGb) {
      alert('Please fill out the Name, Manufacturer, Processor, and RAM first so the AI has data to work with!');
      this.isGeneratingAi = false;
      return; 
    }
    
    this.apiService.generateDescription(data).subscribe({
      next: (res) => {
        console.log("API Response:", res);
        this.deviceForm.patchValue({ description: res.description });
        this.isGeneratingAi = false;
      },
      error: (err) => {
        console.error('AI Error:', err);
        this.isGeneratingAi = false;
      }
    });
  }
}