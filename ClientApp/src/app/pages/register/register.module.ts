import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatInputModule, MatButtonModule, MatIconModule, MatCheckboxModule, MatCardModule, MatProgressSpinnerModule } from '@angular/material';
import { DndModule } from 'ng2-dnd';

import { RegisterRouting } from './register.routes';

import { RegisterComponent, DialogTermsComponent } from './register.component';
import { RegisterConfirmEmailComponent } from './components/register-confirm-email.component';

@NgModule({
  declarations: [
    RegisterComponent,
    DialogTermsComponent,
    RegisterConfirmEmailComponent
  ],
  imports: [
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatDialogModule,
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
  entryComponents: [
    DialogTermsComponent
  ],
  providers: []
})
export class RegisterModule { }
