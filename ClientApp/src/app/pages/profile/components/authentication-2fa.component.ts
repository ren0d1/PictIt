import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Authentication2faInformation } from '../../../shared/models/authentication-2fa-information.model';
import { Router } from '@angular/router';
import { HttpErrorHandlerService } from '../../../shared/services/http-error-handler.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-authentication-2fa',
  templateUrl: './authentication-2fa.component.html',
  styleUrls: ['./authentication-2fa.component.css']
})
export class Authentication2faComponent implements OnInit {
  constructor(private http: HttpClient, private router: Router, private errorHandler: HttpErrorHandlerService, private snackBar: MatSnackBar) {}

  authentication2faInformation: Authentication2faInformation;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  ngOnInit() {
    this.http.get<Authentication2faInformation>('/api/user/authentication2fa').subscribe(information => {
      this.authentication2faInformation = information;
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  forgetBrowser() {
    this.http.post('/api/user/authentication2fa/ForgetClient', null).subscribe(() => {
      this.snackBar.open('The current browser has been', 'forgotten', {
        duration: 2000
      });

      window.location.href = window.location.origin + '/profile';
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  generateRecoveryCodes() {
    this.http.post<string[]>('/api/user/authentication2fa/GenerateRecoveryCodes', null).subscribe(codes => {
      this.router.navigate(['show-codes'], { queryParams: {recoveryCodes: codes} });
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  disable2fa() {
    this.http.post('/api/user/authentication2fa/Disable', null).subscribe(() => {
      this.snackBar.open('2fa has been', 'disabled', {
        duration: 2000
      });

      window.location.href = window.location.origin + '/profile';
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  resetAuthenticator() {
    this.http.post('/api/user/authentication2fa/Reset', null).subscribe(() => {
      this.snackBar.open('Your authenticator app key has been', 'reset', {
        duration: 2000
      });

      this.router.navigate(['authenticator']);
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  download(content: any) {
    const element = document.createElement('a');
    element.setAttribute('href', 'data:text/json;charset=utf-8,' + encodeURIComponent(JSON.stringify(content)));
    element.setAttribute('download', 'personalData.json');

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
  }

  downloadPersonalData() {
    this.http.get('/api/user/personalData').subscribe(data => {
      this.download(data);
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  deleteAccount() {
    this.router.navigate(['delete-account']);
  }
}
