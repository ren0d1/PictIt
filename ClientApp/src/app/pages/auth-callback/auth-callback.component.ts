import { Component, OnInit } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  constructor(private oauthService: OAuthService, private router: Router) { }

  claims;

  ngOnInit() {
    this.claims = this.oauthService.getIdentityClaims();
    let wait = setInterval(() => {
      if(window.location.href.indexOf('#') === -1){
        this.router.navigate(['gallery']);
        clearInterval(wait);
      }
    }, 1000);
  }
}
