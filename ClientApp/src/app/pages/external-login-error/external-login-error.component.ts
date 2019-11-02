import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-external-login-error',
  templateUrl: 'external-login-error.component.html',
  styleUrls: ['external-login-error.component.css']
})

export class ExternalLoginErrorComponent implements OnInit {
  constructor(private router: Router) {}

  ngOnInit() {
    setTimeout((router: Router) => {
        this.router.navigate(['login']);
    }, 5000);  // 5s
  }
}
