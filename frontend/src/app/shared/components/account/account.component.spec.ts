import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { AccountComponent } from './account.component';

describe('AccountComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccountComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(AccountComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('currentUser should reflect AuthService signal', () => {
    const fixture = TestBed.createComponent(AccountComponent);
    // No token in localStorage → user should be null
    expect(fixture.componentInstance.currentUser()).toBeNull();
  });
});
