import { Component, OnInit } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpErrorHandlerService } from '../../shared/services/http-error-handler.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private http: HttpClient, private router: Router, private errorHandler: HttpErrorHandlerService) {}

  returnUrl = '';

  ngOnInit() {
    this.activeRoute.queryParams.subscribe(params => {
      this.returnUrl = params['ReturnUrl'];
    });

    this.http.get('/api/user/logout').subscribe(() => {
      if(this.returnUrl === undefined || this.returnUrl.length > 0)
      {
        this.router.navigateByUrl(this.returnUrl);
      }else{
        this.router.navigateByUrl('home');
      }
    }, (error: HttpErrorResponse) => this.errorHandler.handleError(error))
  }
}
