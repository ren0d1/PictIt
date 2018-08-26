import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import * as XRegExp from 'xregexp';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { RegisterUser } from '../../shared/models/register-user.model';
import { EmailLookup } from '../../shared/models/email-lookup.model';
import { Picture } from '../../shared/models/picture.model';
import { HttpErrorHandlerService } from '../../shared/services/http-error-handler.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, AfterViewInit {
  constructor(private http: HttpClient, private errorHandler: HttpErrorHandlerService, private router: Router) { }

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

  picturesFormControl = new FormControl('', []);

  sent = false;
  registered = false;
  uploadingPicture = false;
  improperFace = false;

  pictures: Picture[] = [];

  ngOnInit() {
    this.registrationForm = new FormGroup ({
      firstNameFormControl: this.firstNameFormControl,
      lastNameFormControl: this.lastNameFormControl,
      userNameFormControl: this.userNameFormControl,
      emailFormControl: this.emailFormControl,
      passwordFormControl: this.passwordFormControl,
      termsAndConditionsFormControl: this.termsAndConditionsFormControl,
      picturesFormControl: this.picturesFormControl
    }, this.userIdentityKnown);
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.picturesFormControl.setErrors({ notEnough: true });
    });
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

        this.sent = true;

        this.http.post('api/user/register', userToRegister).subscribe((userId: string) => {
          this.registered = true;

          const formData = new FormData();
          formData.append('userId', userId);
          for (let i = 0; i < this.pictures.length; i++) {
            formData.append('files', this.pictures[i].file);
          }

          this.http.post('api/user/picture', formData).subscribe(() => {
            this.router.navigate(['confirm-email']);
          }, (error: HttpErrorResponse) => {
            this.errorHandler.handleError(error);
            this.sent = false;
          });
        }, (error: HttpErrorResponse) => {
          this.errorHandler.handleError(error);
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
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }

  fileSelected(files: FileList) {
    this.uploadingPicture = true;
    for (let i = 0; i < files.length; i++) {
      this.checkIfFaceIsPresent(files[i], () => {
        this.pictures.push(new Picture(files[i], '', true));
        this.getImageSrc(files[i], this.pictures[this.pictures.length - 1]);
        this.picturesFormControl.setErrors(this.pictures.length < 5 ? { notEnough: true } : null);
      });
    }
  }

  transferDataSuccess(param: any) {
    const dataTransfer: DataTransfer = param.mouseEvent.dataTransfer;
    if (dataTransfer && dataTransfer.files) {
      const files: FileList = dataTransfer.files;
      this.uploadingPicture = true;
      for (let i = 0; i < files.length; i++) {
        this.checkIfFaceIsPresent(files[i], () => {
          this.pictures.push(new Picture(files[i], '', true));
          this.getImageSrc(files[i], this.pictures[this.pictures.length - 1]);
          this.picturesFormControl.setErrors(this.pictures.length < 5 ? { notEnough: true } : null);
        });
      }
    }
  }

  checkIfFaceIsPresent(file: File, callback: Function) {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post('api/picture/isAFace', formData).subscribe((isAFace: boolean) => {
      if (isAFace) {
        callback();
      } else {
        this.improperFace = true;

        let countSrc = 0;
        for (let i = 0; i < this.pictures.length; i++) {
          if (this.pictures[i].src.length > 0) {
            countSrc++;
          }
        }

        if (countSrc === this.pictures.length) {
          this.uploadingPicture = false;
        }
      }
    });
  }

  getImageSrc(file: File, picture: Picture) {
    const fr = new FileReader();

    fr.onload = (event) => {
      picture.src = event.target.result;

      let countSrc = 0;
      for (let i = 0; i < this.pictures.length; i++) {
        if (this.pictures[i].src.length > 0) {
          countSrc++;
        }
      }

      if (countSrc === this.pictures.length) {
        this.uploadingPicture = false;
      }
    };

    fr.readAsDataURL(file);
  }

  deletePicture(picture: Picture) {
    this.pictures.splice(this.pictures.indexOf(picture), 1);
    this.picturesFormControl.setErrors(this.pictures.length < 5 ? { notEnough: true } : null);
    this.improperFace = false;
  }
}
