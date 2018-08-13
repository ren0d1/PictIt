import { Component, OnInit } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  constructor(private oauthService: OAuthService, private activeRoute: ActivatedRoute) { }

  claims;

  ngOnInit() {
    this.activeRoute.queryParams.subscribe(params => {
      console.log(params);
    });

    this.claims = this.oauthService.getIdentityClaims();
  }
}
