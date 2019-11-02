import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root',
})
export class HttpErrorHandlerService {
  constructor(private router: Router) {}

  handleError(error: HttpErrorResponse) {
    switch (error.status) {
      case 209:
        window.location.href = window.location.origin + error.error.text;
        break;
      case 401:
        this.router.navigate(['unauthorized']);
        break;
      case 403:
        this.router.navigate(['forbidden']);
        break;
      case 404:
        this.router.navigate(['not-found']);
        break;
      case 500:
        this.router.navigate(['internal-server-error']);
        break;
    }
  }

  handleFormError(error: HttpErrorResponse, formControl: FormControl) {
    if (error.status === 422) {
      formControl.setErrors({wrongcode: true});
    } else {
      this.handleError(error);
    }
  }
}
