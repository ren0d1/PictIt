import { Component } from '@angular/core';
import { MatSidenav } from '@angular/material';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { LoggerService } from '../../services/logger.service';
import { LanguagesMatcherService } from '../../services/languages-matcher.service';
import { HashTable } from 'angular-hashtable';
import { Language } from '../../models/language.model';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
    isHandset$: Observable<boolean> = this._breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

    appName: string;
    availableLanguagesCode: string[];
    availableLanguagesInformations = new HashTable<string, Language>();

    loggedIn = false;

    constructor(private _breakpointObserver: BreakpointObserver, public translate: TranslateService, private _logger: LoggerService, _languagesMatcher: LanguagesMatcherService) {
        const userBrowserLang = translate.getBrowserLang();
        this.availableLanguagesCode = translate.getLangs();

        _logger.logDebug('browser language: ' + userBrowserLang);
        _logger.logDebug('languagesAvailable: ' + this.availableLanguagesCode.join());

        this.availableLanguagesCode.forEach(availableLanguage => {
            const informationRetrievalAttempt: Language | undefined = _languagesMatcher.getLanguageInformations(availableLanguage);

            if (informationRetrievalAttempt !== undefined) {
                this.availableLanguagesInformations.put(availableLanguage, informationRetrievalAttempt);
            }
        });

        translate.onLangChange.subscribe((event: LangChangeEvent) => {
            translate.get('NAV-MENU.NAME', {value: 'X'}).subscribe((res: string) => this.appName = res);
        });

        if (userBrowserLang !== undefined && translate.getLangs().includes(userBrowserLang)) {
            translate.setDefaultLang(userBrowserLang);
            _logger.logDebug('browser language matched');
        } else {
            translate.setDefaultLang('en');
            _logger.logDebug('browser language defaulted');
        }

        translate.get('NAV-MENU.NAME', {value: 'X'}).subscribe((res: string) => this.appName = res);
    }

    public setLanguage(lang) {
        this.translate.use(lang);
    }

    public login() {
        this._logger.logDebug('Do login logic');
        this.loggedIn = !this.loggedIn;
    }

    public register() {
        this._logger.logDebug('Do logout logic');
    }

    public closeMenu(menu: MatSidenav) {
        this.isHandset$.subscribe(isHandset => {
            if (isHandset) {
                menu.close();
            }
        });
    }
}
