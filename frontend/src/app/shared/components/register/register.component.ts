import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

const passwordMatchValidator: ValidatorFn = (control: AbstractControl) => {
  const password = control.get('password')?.value;
  const confirm = control.get('confirmPassword')?.value;
  return password === confirm ? null : { passwordMismatch: true };
};

// Combined pattern: must contain at least one digit AND at least one special character
const PASSWORD_COMPLEXITY = /^(?=.*\d)(?=.*[^a-zA-Z0-9]).+$/;

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly form = this.fb.group({
    displayName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(PASSWORD_COMPLEXITY)
    ]],
    confirmPassword: ['', Validators.required]
  }, { validators: passwordMatchValidator });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.loading.set(true);
    this.errorMessage.set('');

    const { email, password, displayName } = this.form.getRawValue();
    this.authService.register({ email: email!, password: password!, displayName: displayName! }).subscribe({
      next: () => this.router.navigate(['/login']),
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Registration failed. The email may already be in use.');
      }
    });
  }
}
