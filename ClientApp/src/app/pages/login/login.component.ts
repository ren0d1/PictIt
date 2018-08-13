import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';

import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute) {}

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.maxLength(255)
  ]);

  showPassword = false;

  passwordFormControl = new FormControl('', [
    Validators.required,
    Validators.pattern(XRegExp('(?=.*\\pN)(?=.*\\p{Ll})(?=.*\\p{Lu})(?=.*(\\pP|\\pS)).{8,}', 'A')),
    Validators.minLength(8)
  ]);

  loginForm: FormGroup;
  returnUrl = '';

  ngOnInit() {
    this.loginForm = new FormGroup ({
      emailFormControl: this.emailFormControl,
      passwordFormControl: this.passwordFormControl
    });

    this.activeRoute.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    });
  }

  // tryFacebook() {
  //   console.log('trying facebook');

  //   this.http.get('api/user/externalLogin', { params: new HttpParams().set('provider', 'Facebook').set('returnUrl', this.returnUrl) }).subscribe((result: HttpResponseBase) => {
  //       this.sent = true;
  //       console.log('came back');
  //       console.log(result);
  //     }, (error: HttpErrorResponse) => {
  //       console.log('Status: ' + error.status + '\nStatusText: ' + error.statusText);
  //       console.log('Ok? ' + error.ok + '\nType: ' + error.type);
  //       console.log('Url: ' + error.url);
  //       if (error.status === 200 && this.router.url !== error.url) {
  //         this.router.navigateByUrl(error.url);
  //       }
  //       if (error.status === 404) {
  //         if (error.url.includes('consent')) {
  //           this.router.navigateByUrl(error.url);
  //         }
  //       }
  //     }
  //   );
  // }

  tryFacebook() {
    console.log('trying facebook');

    window.location.href = 'https://www.facebook.com/v3.1/dialog/oauth?&response_type=token' +
    '&client_id=185375462328710&redirect_uri=https://localhost:44399/auth-callback&scope=email';
  }
}
