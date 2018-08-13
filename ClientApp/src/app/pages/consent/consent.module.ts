import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { ConsentComponent } from './consent.component';
import { ConsentRouting } from './consent.routes';

@NgModule({
  declarations: [
    ConsentComponent
  ],
  imports: [
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatCheckboxModule,
    ConsentRouting
  ],
  exports: [
    ConsentComponent
  ],
  providers: []
})
export class ConsentModule { }
