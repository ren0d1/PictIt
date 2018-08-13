import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';
import { HttpClient } from '@angular/common/http';
import { RegisterUser } from '../../shared/models/register-user.model';
import { EmailLookup } from '../../shared/models/email-lookup.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(private http: HttpClient) { }

  registrationForm: FormGroup;

  firstNameFormControl = new FormControl('', [
    Validators.pattern(XRegExp('^(([\\p{Lt}\\p{Lu}][\\p{Ll}]+)|(\\p{Lm})|(\\p{Lo}))+([ ]?(([\\p{Lt}\\p{Lu}][\\p{Ll}]+)|(\\p{Lm})|(\\p{Lo})))*$')),
    Validators.maxLength(50)
  ]);

  lastNameFormControl = new FormControl('', [
    Validators.pattern(XRegExp('^(([\\p{Lt}\\p{Lu}][\\p{Ll}]+)|(\\p{Lm})|(\\p{Lo}))+([ ]?(([\\p{Lt}\\p{Lu}][\\p{Ll}]+)|(\\p{Lm})|(\\p{Lo})))*$')),
    Validators.maxLength(50)
  ]);

  userNameFormControl = new FormControl('', [
    Validators.pattern(XRegExp('^(\\pL|\\pS|\\pN|\\pP)+( (\\pL|\\pS|\\pN|\\pP)+)*$', 'A')),
    Validators.maxLength(50)
  ]);

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

  termsAndConditionsFormControl = new FormControl('', [
    Validators.requiredTrue
  ]);

  sent = false;

  ngOnInit() {
    this.registrationForm = new FormGroup ({
      firstNameFormControl: this.firstNameFormControl,
      lastNameFormControl: this.lastNameFormControl,
      userNameFormControl: this.userNameFormControl,
      emailFormControl: this.emailFormControl,
      passwordFormControl: this.passwordFormControl,
      termsAndConditionsFormControl: this.termsAndConditionsFormControl
    }, this.userIdentityKnown);
  }

  onSubmit() {
    this.validateEmail(this.emailFormControl, () => {
      if (this.registrationForm.valid) {
        const userToRegister: RegisterUser = new RegisterUser(
          this.firstNameFormControl.value,
          this.lastNameFormControl.value,
          this.userNameFormControl.value,
          this.emailFormControl.value,
          this.passwordFormControl.value,
          this.termsAndConditionsFormControl.value
        );

        this.http.post('api/user/register', userToRegister).subscribe(result => {
          this.sent = true;
        });
      }
    });
  }

  // Custom Validators
  userIdentityKnown(group: FormGroup) {
    if ((group.get('firstNameFormControl').value.length > 0 && group.get('lastNameFormControl').value.length > 0) ||
        group.get('userNameFormControl').value.length > 0) {
      return null;
    }

    return { missingIdentity: true };
  }

  validateEmail(control: FormControl, callback: Function) {
    this.http.get('api/user/register/email?email=' + control.value).subscribe((result: EmailLookup) => {
      if (!result.email_sendable) {
        control.setErrors({ emailUnreachable: true});
      }

      callback();
    });
  }
}
