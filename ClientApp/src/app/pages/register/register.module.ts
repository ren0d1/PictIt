import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule } from '@angular/material';

import { RegisterRouting } from './register.routes';

import { RegisterComponent } from './register.component';
import { RegisterConfirmEmailComponent } from './components/register-confirm-email.component';

@NgModule({
  declarations: [
    RegisterComponent,
    RegisterConfirmEmailComponent
  ],
  imports: [
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    RegisterRouting
  ],
  exports: [
    RegisterComponent
  ],
  providers: []
})
export class RegisterModule { }
