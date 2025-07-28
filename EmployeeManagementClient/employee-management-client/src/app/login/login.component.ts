import { Component } from '@angular/core';
import { AuthService } from '../core/auth.service';
import { Router } from '@angular/router';
import { MatInputModule } from "@angular/material/input";
import { MatCard, MatCardModule } from "@angular/material/card";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  imports: [MatInputModule, MatCard, FormsModule, CommonModule, MatCardModule, MatButtonModule  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  email = '';
  password = '';
  constructor(private authService: AuthService, private router: Router) { }

  login() {
    this.authService.login(this.email, this.password).subscribe({
      next: () => this.router.navigate(['/employees']),
      error: () => alert('Credenciais inválidas, tente novamente.'),
    });
  }
}
