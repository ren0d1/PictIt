import { Component, OnInit  } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from '../../../shared/services/http-error-handler.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrls: ['./delete-account.component.css']
})
export class DeleteAccountComponent implements OnInit {
  constructor(private http: HttpClient, private router: Router, private errorHandler: HttpErrorHandlerService) {}

  passwordFormControl = new FormControl('', [
    Validators.required,
    Validators.pattern(XRegExp('(?=.*\\pN)(?=.*\\p{Ll})(?=.*\\p{Lu})(?=.*(\\pP|\\pS)).{8,}', 'A')),
    Validators.minLength(8)
  ]);

  deleteAccountForm: FormGroup;

  ngOnInit() {
    this.deleteAccountForm = new FormGroup ({
      passwordFormControl: this.passwordFormControl
    });
  }

  sendForm() {
    if (this.deleteAccountForm.valid) {
      this.http.post('/api/user/personalData', null, { params: new HttpParams().set('password', this.passwordFormControl.value) }).subscribe(() => {
        this.router.navigate(['home']);
      }, (error: HttpErrorResponse) => {
        this.errorHandler.handleFormError(error, this.passwordFormControl);
      });
    }
  }
}
