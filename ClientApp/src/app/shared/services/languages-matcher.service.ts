import { Injectable } from '@angular/core';
import { HashTable } from 'angular-hashtable';
import { Language } from '../models/language.model';

@Injectable({
    providedIn: 'root',
})
export class LanguagesMatcherService {
    private _languages: HashTable<string, Language>;

    constructor() {
        this._languages = new HashTable<string, Language>();
        this._languages.put('en', new Language('English', 'ENGLISH', 'us'));
        this._languages.put('fr', new Language('Français', 'FRANCAIS', 'fr'));
    }

    getLanguageName(languageCode: string): string {
        if (this._languages.has(languageCode)) {
            return this._languages.get(languageCode).languageName;
        }

        return '';
    }

    getLanguageUpperCaseName(languageCode: string): string {
        if (this._languages.has(languageCode)) {
            return this._languages.get(languageCode).languageUpperCaseName;
        }

        return '';
    }

    getCountryAlphaCode(languageCode: string): string {
        if (this._languages.has(languageCode)) {
            return this._languages.get(languageCode).countryAlphaCode;
        }

        return '';
    }

    getLanguageInformations(languageCode: string): Language {
        if (this._languages.has(languageCode)) {
            return this._languages.get(languageCode);
        }

        return undefined;
    }
}
