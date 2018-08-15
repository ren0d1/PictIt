import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
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

  sendForm(form: HTMLFormElement) {
    this.sent = true;
    form.submit();
  }
}
