import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';

import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ResetPassword } from '../../../shared/models/reset-password.model';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private http: HttpClient, private router: Router) {}

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

  resetPasswordForm: FormGroup;
  sent = false;
  success: boolean | undefined = undefined;
  code = '';
  email = '';

  ngOnInit() {
    this.resetPasswordForm = new FormGroup ({
      emailFormControl: this.emailFormControl,
      passwordFormControl: this.passwordFormControl
    });

    this.activeRoute.queryParams.subscribe(params => {
      this.code = params['code'];
      this.email = params['email'];
      this.emailFormControl.setValue(this.email);
    });
  }

  resetPassword() {
    this.sent = true;

    if (this.resetPasswordForm.valid) {
      const reset: ResetPassword = new ResetPassword(
        this.code,
        this.emailFormControl.value,
        this.passwordFormControl.value
      );

      this.http.post('api/user/resetpassword', reset).subscribe(result => {
        this.success = true;
        setTimeout(() => {
          this.router.navigate(['login']);
        }, 5000);  // 5s
      }, error => {
        this.success = false;
        setTimeout(() => {
          this.router.navigate(['home']);
        }, 5000);  // 5s
      });
    }
  }
}
