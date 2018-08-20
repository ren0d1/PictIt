import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';
import { HttpErrorHandlerService } from '../../../shared/services/http-error-handler.service';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-2fa',
  templateUrl: './login-2fa.component.html',
  styleUrls: ['./login-2fa.component.css']
})
export class Login2faComponent implements OnInit {
  constructor(private http: HttpClient, private activeRoute: ActivatedRoute, private errorHandler: HttpErrorHandlerService) {}

  twoFactorCodeFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(8)
  ]);

  login2faForm: FormGroup;
  sent = false;
  returnUrl = '';

  ngOnInit() {
    this.login2faForm = new FormGroup ({
      emailFormControl: this.twoFactorCodeFormControl
    });

    this.activeRoute.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    });
  }

  sendForm(form: HTMLFormElement) {
    this.sent = true;
    if (this.login2faForm.valid) {
      this.http.post('/api/user/Login2fa', {'TwoFactorCode': this.twoFactorCodeFormControl.value}, { params : new HttpParams().set('returnUrl', this.returnUrl)}).subscribe(() => {}, (error: HttpErrorResponse) => {
        this.errorHandler.handleFormError(error, this.twoFactorCodeFormControl);
        this.sent = false;
      });
    }
  }
}
