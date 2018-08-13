import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';

import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService implements CanActivate {
  constructor(private oauthService: OAuthService) { }
  canActivate(): boolean {
    if (this.oauthService.hasValidIdToken()) {
      return true;
    }

    this.oauthService.initImplicitFlow();
    return false;
  }
}
