import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule, MatCardModule, MatChipsModule, MatSnackBarModule } from '@angular/material';
import { NgxQRCodeModule } from 'ngx-qrcode2';

import { ProfileComponent } from './profile.component';
import { Authentication2faComponent } from './components/authentication-2fa.component';
import { AuthenticatorComponent } from './components/authenticator.component';
import { ShowCodesComponent } from './components/show-codes.component';
import { DeleteAccountComponent } from './components/delete-account.component';
import { ProfileRouting } from './profile.routes';

@NgModule({
  declarations: [
    ProfileComponent,
    Authentication2faComponent,
    AuthenticatorComponent,
    ShowCodesComponent,
    DeleteAccountComponent
  ],
  imports: [
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatCardModule,
    MatChipsModule,
    MatSnackBarModule,
    NgxQRCodeModule,
    ProfileRouting
  ],
  exports: [
    ProfileComponent
  ],
  providers: []
})
export class ProfileModule { }
