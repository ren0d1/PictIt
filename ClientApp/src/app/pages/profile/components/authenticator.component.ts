import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';

import { AuthenticatorUser } from '../../../shared/models/authenticator-user.model';

@Component({
  selector: 'app-authenticator',
  templateUrl: './authenticator.component.html',
  styleUrls: ['./authenticator.component.css']
})
export class AuthenticatorComponent implements OnInit {
  constructor(private http: HttpClient) {}

  twoFactorCodeFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(8)
  ]);

  authenticator: AuthenticatorUser;
  authenticatorForm: FormGroup;

  ngOnInit() {
    this.authenticatorForm = new FormGroup ({
      emailFormControl: this.twoFactorCodeFormControl
    });

    this.http.get<AuthenticatorUser>('/api/user/Authenticator').subscribe(authenticator => {
      this.authenticator = authenticator;
    });
  }

  sendForm() {
    if (this.authenticatorForm.valid) {
      this.http.post('/api/user/authenticator', null,  { params: new HttpParams().set('twoFactorCode', this.twoFactorCodeFormControl.value) }).subscribe(result => {

      }, (error: HttpErrorResponse) => {
        if (error.status === 422) {
          this.twoFactorCodeFormControl.setErrors({wrongcode: true});
        }
      });
    }
  }
}
