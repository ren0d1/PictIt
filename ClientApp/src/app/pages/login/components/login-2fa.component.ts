import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';

import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login-2fa',
  templateUrl: './login-2fa.component.html',
  styleUrls: ['./login-2fa.component.css']
})
export class Login2faComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute) {}

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
    form.submit();
  }
}
