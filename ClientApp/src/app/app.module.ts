import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatToolbarModule, MatCardModule, MatSidenavModule, MatListModule, MatMenuModule, MatButtonModule, MatIconModule, MatSlideToggleModule, MatCheckboxModule } from '@angular/material';

import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { Routing } from './app.routes';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { InternalServerErrorComponent } from './pages/internal-server-error/internal-server-error.component';
import { ExternalLoginErrorComponent } from './pages/external-login-error/external-login-error.component';
import { HomeComponent } from './pages/home/home.component';
import { RegisterModule } from './pages/register/register.module';
import { LoginModule } from './pages/login/login.module';
import { AuthCallbackComponent } from './pages/auth-callback/auth-callback.component';
import { ConsentModule } from './pages/consent/consent.module';
import { ProfileModule } from './pages/profile/profile.module';
import { GalleryComponent } from './pages/gallery/gallery.component';

import { OAuthModule } from 'angular-oauth2-oidc';

import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';

// AoT requires an exported function for factories
export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    UnauthorizedComponent,
    ForbiddenComponent,
    NotFoundComponent,
    InternalServerErrorComponent,
    ExternalLoginErrorComponent,
    HomeComponent,
    AuthCallbackComponent,
    GalleryComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'PictIt'}),
    BrowserAnimationsModule,
    HttpClientModule,
    MatToolbarModule,
    MatCardModule,
    MatSidenavModule,
    MatListModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule,
    MatCheckboxModule,
    RegisterModule,
    LoginModule,
    ConsentModule,
    ProfileModule,
    OAuthModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: createTranslateLoader,
          deps: [HttpClient]
      }
    }),
    ServiceWorkerModule.register('/ngsw-worker.js', { enabled: environment.production }),
    Routing
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(translate: TranslateService) {
    translate.addLangs(['en', 'fr']);
  }
}
