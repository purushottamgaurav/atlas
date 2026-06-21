import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  isRegisterMode = false;
  email = '';
  password = '';
  username = '';
  error = '';
  busy = false;

  constructor(private auth: AuthService, private router: Router) {}

  toggle(): void {
    this.isRegisterMode = !this.isRegisterMode;
    this.error = '';
  }

  submit(): void {
    this.busy = true;
    this.error = '';
    const obs = this.isRegisterMode
      ? this.auth.register({ username: this.username, email: this.email, password: this.password })
      : this.auth.login({ email: this.email, password: this.password });

    obs.subscribe({
      next: () => this.router.navigate(['/lobby']),
      error: (e: { error?: { error?: string }; message?: string }) => {
        this.error = e.error?.error ?? e.message ?? 'An error occurred';
        this.busy = false;
      }
    });
  }
}
