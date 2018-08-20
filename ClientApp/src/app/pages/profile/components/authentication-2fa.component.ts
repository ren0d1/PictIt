import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Authentication2faInformation } from '../../../shared/models/authentication-2fa-information.model';
import { Router } from '@angular/router';
import { HttpErrorHandlerService } from '../../../shared/services/http-error-handler.service';

@Component({
  selector: 'app-authentication-2fa',
  templateUrl: './authentication-2fa.component.html',
  styleUrls: ['./authentication-2fa.component.css']
})
export class Authentication2faComponent implements OnInit {
  constructor(private http: HttpClient, private router: Router, private errorHandler: HttpErrorHandlerService) {}

  authentication2faInformation: Authentication2faInformation;

  ngOnInit() {
    this.http.get<Authentication2faInformation>('/api/user/authentication2fa').subscribe(information => {
      this.authentication2faInformation = information;
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  forgetBrowser() {
    this.http.post('/api/user/authentication2fa/ForgetClient', null).subscribe(result => {
      this.router.navigate(['profile']);
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  generateRecoveryCodes() {
    this.http.post<string[]>('/api/user/authentication2fa/GenerateRecoveryCodes', null).subscribe(codes => {
      this.router.navigate(['show-codes'], { queryParams: {recoveryCodes: codes} });
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  disable2fa() {
    this.http.post('/api/user/authentication2fa/Disable', null).subscribe(result => {
      this.router.navigate(['profile']);
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  resetAuthenticator() {
    this.http.post('/api/user/authentication2fa/Reset', null).subscribe(result => {
      this.router.navigate(['authenticator']);
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }
}
