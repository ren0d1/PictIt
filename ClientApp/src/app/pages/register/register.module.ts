import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule, MatCardModule, MatProgressSpinnerModule } from '@angular/material';
import { DndModule } from 'ng2-dnd';

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
    MatCardModule,
    MatProgressSpinnerModule,
    DndModule.forRoot(),
    RegisterRouting
  ],
  exports: [
    RegisterComponent
  ],
  providers: []
})
export class RegisterModule { }
