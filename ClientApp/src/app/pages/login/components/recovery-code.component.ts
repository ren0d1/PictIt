import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-recovery-code',
  templateUrl: './recovery-code.component.html',
  styleUrls: ['./recovery-code.component.css']
})
export class RecoveryCodeComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private http: HttpClient) {}

  recoveryCodeFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(6),
    Validators.maxLength(8)
  ]);

  authenticatorUsingRecoveryCodeForm: FormGroup;
  returnUrl = '';

  ngOnInit() {
    this.authenticatorUsingRecoveryCodeForm = new FormGroup ({
        recoveryCodeFormControl: this.recoveryCodeFormControl
    });

    this.activeRoute.queryParams.subscribe(params => {
        this.returnUrl = params['returnUrl'];
    });
  }

  sendForm() {
    if (this.authenticatorUsingRecoveryCodeForm.valid) {
      this.http.post('/api/user/Login2fa/RecoveryCode', {'recoveryCode': this.recoveryCodeFormControl.value, 'returnUrl': this.returnUrl}).subscribe(result => {

      }, (error: HttpErrorResponse) => {
        if (error.status === 422) {
          this.recoveryCodeFormControl.setErrors({wrongcode: true});
        }
      });
    }
  }
}
