import { Routes, RouterModule } from '@angular/router';

import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { InternalServerErrorComponent } from './pages/internal-server-error/internal-server-error.component';
import { ExternalLoginErrorComponent } from './pages/external-login-error/external-login-error.component';

import { LogoutComponent } from './pages/logout/logout.component';

import { HomeComponent } from './pages/home/home.component';
import { GalleryComponent } from './pages/gallery/gallery.component';

import { AuthGuardService } from './shared/guards/auth.guard';
import { AuthCallbackComponent } from './pages/auth-callback/auth-callback.component';

const appRoutes: Routes = [
  { path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'gallery',
    component: GalleryComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'unauthorized',
    component: UnauthorizedComponent
  },
  {
    path: 'forbidden',
    component: ForbiddenComponent
  },
  {
    path: 'not-found',
    component: NotFoundComponent
  },
  {
    path: 'internal-server-error',
    component: InternalServerErrorComponent
  },
  {
    path: 'external-login-error',
    component: ExternalLoginErrorComponent
  },
  {
    path: 'logout',
    component: LogoutComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'auth-callback',
    component: AuthCallbackComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];

export const Routing = RouterModule.forRoot(appRoutes);
