import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RegisterComponent } from './register.component';

describe('RegisterComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(RegisterComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('form should be invalid when empty', () => {
    const fixture = TestBed.createComponent(RegisterComponent);
    expect(fixture.componentInstance.form.invalid).toBeTrue();
  });

  it('password without digit should be invalid', () => {
    const fixture = TestBed.createComponent(RegisterComponent);
    const { form } = fixture.componentInstance;
    form.setValue({
      displayName: 'Tester',
      email: 'user@example.com',
      password: 'NoDigitHere!',
      confirmPassword: 'NoDigitHere!'
    });
    expect(form.controls.password.invalid).toBeTrue();
  });

  it('password without special character should be invalid', () => {
    const fixture = TestBed.createComponent(RegisterComponent);
    const { form } = fixture.componentInstance;
    form.setValue({
      displayName: 'Tester',
      email: 'user@example.com',
      password: 'NoSpecial1',
      confirmPassword: 'NoSpecial1'
    });
    expect(form.controls.password.invalid).toBeTrue();
  });

  it('form should be valid with matching strong passwords', () => {
    const fixture = TestBed.createComponent(RegisterComponent);
    const { form } = fixture.componentInstance;
    form.setValue({
      displayName: 'Tester',
      email: 'user@example.com',
      password: 'Password1!',
      confirmPassword: 'Password1!'
    });
    expect(form.valid).toBeTrue();
  });

  it('form should be invalid when passwords do not match', () => {
    const fixture = TestBed.createComponent(RegisterComponent);
    const { form } = fixture.componentInstance;
    form.setValue({
      displayName: 'Tester',
      email: 'user@example.com',
      password: 'Password1!',
      confirmPassword: 'Different1!'
    });
    expect(form.hasError('passwordMismatch')).toBeTrue();
  });
});
