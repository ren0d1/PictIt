import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from '../../../shared/services/http-error-handler.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  constructor(private http: HttpClient, private errorHandler: HttpErrorHandlerService){}

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
    Validators.maxLength(255)
  ]);

  forgotPassword: FormGroup;
  sent = false;

  ngOnInit() {
    this.forgotPassword = new FormGroup ({
      emailFormControl: this.emailFormControl
    });
  }

  sendForm() {
    this.sent = true;

    if (this.forgotPassword.valid) {
      this.http.post('/api/user/ForgotPassword', null, {params: new HttpParams().set('email', this.emailFormControl.value)}).subscribe(() => {}, (error: HttpErrorResponse) => {
        this.sent = false;
        this.errorHandler.handleError(error);
      });
    }
  }
}
