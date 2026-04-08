import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  newUser = { 
    name: '', 
    email: '', 
    password: '', 
    location: '' 
  };
  
  errorMessage = '';

  onSubmit() {
    this.authService.register(this.newUser).subscribe({
      next: () => {
        alert('Account created successfully! Please log in.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to create account. Please try again.';
        console.error('Registration error:', err);
      }
    });
  }
}