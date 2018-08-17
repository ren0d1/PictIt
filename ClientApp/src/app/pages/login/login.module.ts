import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule } from '@angular/material';

import { LoginComponent } from './login.component';
import { ForgotPasswordComponent } from './components/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password.component';
import { Login2faComponent } from './components/login-2fa.component';
import { RecoveryCodeComponent } from './components/recovery-code.component';
import { LoginRouting } from './login.routes';

@NgModule({
  declarations: [
    LoginComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    Login2faComponent,
    RecoveryCodeComponent
  ],
  imports: [
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    LoginRouting
  ],
  exports: [
    LoginComponent
  ],
  providers: []
})
export class LoginModule { }
