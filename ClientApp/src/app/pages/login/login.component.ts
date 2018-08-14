import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';

import { ActivatedRoute } from '@angular/router';
import { ExternalLogin } from '../../shared/models/external-login.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private http: HttpClient) {}

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
  sent = false;
  returnUrl = '';
  availableProviders: ExternalLogin[];

  ngOnInit() {
    this.loginForm = new FormGroup ({
      emailFormControl: this.emailFormControl,
      passwordFormControl: this.passwordFormControl
    });

    this.activeRoute.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    });

    this.http.get<ExternalLogin[]>('/api/user/externallogin/providers').subscribe(providers => {
      this.availableProviders = providers;
    });
  }

  doSomething(form: HTMLFormElement) {
    this.sent = true;
    form.submit();
  }

  externalLogin(provider: string) {
    const encodeGetParams = p => Object.entries(p).map(kv => kv.map(encodeURIComponent).join('=')).join('&');
    const params = {
      provider: provider,
      returnUrl: this.returnUrl
    };

    window.location.href = 'https://localhost:44399/api/user/externallogin?' + encodeGetParams(params);
  }
}
