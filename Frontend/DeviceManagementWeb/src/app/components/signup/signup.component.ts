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

  validateEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }

  onSubmit() {
    if (!this.newUser.name || !this.newUser.email || !this.newUser.password || !this.newUser.location) {
      this.errorMessage = 'All fields are required.';
      return;
    }
    if (!this.validateEmail(this.newUser.email)) {
      this.errorMessage = 'Please enter a valid email address.';
      return;
    }
    if (this.newUser.password.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters.';
      return;
    }
    this.errorMessage = '';
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