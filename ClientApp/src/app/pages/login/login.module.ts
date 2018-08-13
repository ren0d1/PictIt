import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule } from '@angular/material';

import { LoginComponent } from './login.component';
import { LoginRouting } from './login.routes';

@NgModule({
  declarations: [
    LoginComponent
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
