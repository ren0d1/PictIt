import { Component, OnInit} from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { ConsentRequest } from '../../shared/models/consent-request.model';
import { IdentityResource } from '../../shared/models/identity-resource.model';

import { ActivatedRoute } from '@angular/router';
import { HttpErrorHandlerService } from '../../shared/services/http-error-handler.service';

@Component({
  selector: 'app-consent',
  templateUrl: './consent.component.html',
  styleUrls: ['./consent.component.css']
})
export class ConsentComponent implements OnInit {
  constructor(private http: HttpClient, private activeRoute: ActivatedRoute, private errorHandler: HttpErrorHandlerService) {}

  consentRequest: ConsentRequest;
  requestedIdentityScopes: IdentityResource[];
  consentForm: FormGroup;
  returnUrl = '';

  ngOnInit() {
    this.consentForm = new FormGroup({});

    this.activeRoute.queryParams.subscribe(params => {
      this.returnUrl = params['returnUrl'];
    });

    this.http.get('/api/user/consent', { params: new HttpParams().set('returnUrl', this.returnUrl) }).subscribe(result => {
        this.consentRequest = result['consentRequest'];

        this.requestedIdentityScopes = result['requestedIdentityScopes'];

        this.requestedIdentityScopes.forEach(scope => {
          this.consentForm.addControl(scope.name, new FormControl('', scope.required ? [Validators.requiredTrue] : []));
        });
      }, (error: HttpErrorResponse) => this.errorHandler.handleError(error));
  }
}
