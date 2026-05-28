import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { LoginComponent } from './login.component';

describe('LoginComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('form should be invalid when empty', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    expect(fixture.componentInstance.form.invalid).toBeTrue();
  });

  it('form should be valid with a valid email and password', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    const { form } = fixture.componentInstance;
    form.setValue({ email: 'user@example.com', password: 'Password1!' });
    expect(form.valid).toBeTrue();
  });
});
