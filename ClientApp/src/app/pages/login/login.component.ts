import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';

import { ActivatedRoute } from '@angular/router';
import { ExternalLogin } from '../../shared/models/external-login.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpErrorHandlerService } from '../../shared/services/http-error-handler.service';
import { LoginUser } from '../../shared/models/login-user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private http: HttpClient, private errorHandler: HttpErrorHandlerService) {}

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
      this.returnUrl = params['ReturnUrl'];
    });

    this.http.get<ExternalLogin[]>('/api/user/externallogin/providers').subscribe(providers => {
      this.availableProviders = providers;
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  sendForm() {
    if(this.loginForm.valid){
      const loginUser = new LoginUser(
        this.emailFormControl.value,
        this.passwordFormControl.value,
        this.returnUrl
      );
      this.sent = true;
      this.http.post('/api/user/login', loginUser).subscribe(() => {}, (error: HttpErrorResponse) => {
        this.errorHandler.handleFormError(error, this.emailFormControl);
        this.sent = false;
      });
    }
  }

  externalLogin(provider: string) {
    const encodeGetParams = p => Object.entries(p).map(kv => kv.map(encodeURIComponent).join('=')).join('&');
    const params = {
      provider: provider,
      returnUrl: this.returnUrl
    };

    // Using window.location instead of router plays loading screen animation
    window.location.href = window.location.origin + '/api/user/externallogin?' + encodeGetParams(params);
  }
}
