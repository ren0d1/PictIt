import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';

import { AuthenticatorUser } from '../../../shared/models/authenticator-user.model';
import { HttpErrorHandlerService } from '../../../shared/services/http-error-handler.service';

@Component({
  selector: 'app-authenticator',
  templateUrl: './authenticator.component.html',
  styleUrls: ['./authenticator.component.css']
})
export class AuthenticatorComponent implements OnInit {
  constructor(private http: HttpClient, private errorHandler: HttpErrorHandlerService) {}

  twoFactorCodeFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(8)
  ]);

  authenticator: AuthenticatorUser;
  authenticatorForm: FormGroup;

  ngOnInit() {
    this.authenticatorForm = new FormGroup ({
      twoFactorCodeFormControl: this.twoFactorCodeFormControl
    });

    this.http.get<AuthenticatorUser>('/api/user/Authenticator').subscribe(authenticator => {
      this.authenticator = authenticator;
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  sendForm() {
    if (this.authenticatorForm.valid) {
      this.http.post('/api/user/authenticator', null,  { params: new HttpParams().set('twoFactorCode', this.twoFactorCodeFormControl.value) }).subscribe(() => {}, (error: HttpErrorResponse) => this.errorHandler.handleFormError(error, this.twoFactorCodeFormControl));
    }
  }
}
