import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { authGuard } from './shared/guards/auth.guard';
import { guestGuard } from './shared/guards/guest.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'shop',
    loadChildren: () => import('./shop/shop.routes').then(m => m.SHOP_ROUTES)
  },
  {
    path: 'blog',
    loadChildren: () => import('./blog/blog.routes').then(m => m.BLOG_ROUTES)
  },
  {
    path: 'login',
    loadComponent: () => import('./shared/components/login/login.component').then(m => m.LoginComponent),
    canActivate: [guestGuard]
  },
  {
    path: 'register',
    loadComponent: () => import('./shared/components/register/register.component').then(m => m.RegisterComponent),
    canActivate: [guestGuard]
  },
  {
    path: 'account',
    loadComponent: () => import('./shared/components/account/account.component').then(m => m.AccountComponent),
    canActivate: [authGuard]
  },
  { path: '**', component: NotFoundComponent }
];
