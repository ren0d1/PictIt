import { Component, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-lockout',
  templateUrl: './lockout.component.html',
  styleUrls: ['./lockout.component.css']
})
export class LockoutComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private http: HttpClient, public oauthService: OAuthService) {}

  email = '';
  actualTime: number;
  lockoutEnd: number;

  timeDif: number;
  hoursLeft: number;
  minLeft: number;
  secLeft: number;

  ngOnInit() {
    this.activeRoute.queryParams.subscribe(params => {
      this.email = params['email'];
    });

    this.http.get<string>('/api/user/lockout', { params : new HttpParams().set('email', this.email) }).subscribe(timeLeft => {
      this.actualTime = new Date().getTime();
      this.lockoutEnd = new Date(timeLeft).getTime();
      this.timeDif = this.lockoutEnd - this.actualTime;

      this.hoursLeft = Math.floor((this.timeDif % (1000 * 60 * 60 * 24)) / (1000*60*60));
      this.minLeft = Math.floor((this.timeDif % (1000 * 60 * 60)) / (1000 * 60));
      this.secLeft = Math.floor((this.timeDif % (1000 * 60)) / 1000);

      let interval = setInterval(() => {
        this.actualTime = new Date().getTime();
        this.timeDif = this.lockoutEnd - this.actualTime;

        this.hoursLeft = Math.floor((this.timeDif % (1000 * 60 * 60 * 24)) / (1000*60*60));
        this.minLeft = Math.floor((this.timeDif % (1000 * 60 * 60)) / (1000 * 60));
        this.secLeft = Math.floor((this.timeDif % (1000 * 60)) / 1000);

        if(this.timeDif === 0){
          clearInterval(interval);
        }
      }, 1000) // Every 1s

      setTimeout(() => {
        this.oauthService.initImplicitFlow();
      }, this.timeDif);
    });
  }
}
