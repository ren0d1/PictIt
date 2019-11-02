import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-show-codes',
  templateUrl: './show-codes.component.html',
  styleUrls: ['./show-codes.component.css']
})
export class ShowCodesComponent implements OnInit {
  constructor(private activeRoute: ActivatedRoute, private snackBar: MatSnackBar) {}

  recoveryCodes: string[];
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  ngOnInit() {
    this.activeRoute.queryParams.subscribe(params => {
      this.recoveryCodes = params['recoveryCodes'];
    });
  }

  remove(code: string): void {
    const index = this.recoveryCodes.indexOf(code);

    if (index >= 0) {
      this.recoveryCodes.splice(index, 1);
    }
  }

  saveToClipboard(code: string) {
    const copyBox = document.createElement('textarea');

    // Prevent it from showing up
    copyBox.style.position = 'fixed';
    copyBox.style.left = '0';
    copyBox.style.top = '0';
    copyBox.style.opacity = '0';
    copyBox.value = code;
    document.body.appendChild(copyBox);
    copyBox.focus();
    copyBox.select();
    document.execCommand('copy');
    document.body.removeChild(copyBox);

    this.snackBar.open('The following code has been copied to your clipboard:', code, {
      duration: 2000,
    });
  }
}
