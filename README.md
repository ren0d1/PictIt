# PictIt: bachelor thesis project
<img style="float: left;" src="https://i.ibb.co/nsRqFbZ/pictit.png" width="125">This project is a platform allowing a registered user to search for registered users by uploading a picture of the target. It will then provide to the searcher the information that the searched user was willing to share with members of the platform.
![License MIT](https://img.shields.io/github/license/ren0d1/PictIt)

## Points of interest
+ GDPR compliant
+ Fonctionalities
    * Specific regex (Front- and back-end) to allow more freedom to the users for legal characters in their password
    * Mail confirmation
    * Local account and external account support (Facebook, GitHub, Google, LinkedIn et Microsoft)
    * Account linking: allow for an account (local or external) to be linked with another authentification service. i.e. If my Google email and my Github email are the same, then I can connect to my account using either of them
    * History of requests
    * GDPR: ask for consent, consent is removable at any time, user can dowload user related data, user can delete account and related user data
+ Security
    * Azure Key Vault
    * SQL "Encrypted at rest"
    * Password requirement: minimum 8 character, 5 separate, with at least one lowercase letter, one uppercase letter, a digit, a special character (any Unicode character part of a standardised set which doesn't fit as a letter, a digit, an empty character or an escaping character. i.e. **most emojis are accepted as special characters**)
    * Password hashing algorithm: Argon2 (Winner of « Password Hashing Competition » in July 2015)
    * 2FA
    * Bruteforce protection: temporary block after 3 wrong password attempts.
    * HTTPS (HSTS enabled and TLS 1.2)
    *  Identity Server (OpenID Connect and OAuth 2.0)

## Technologies used
+ .NET Core 2.1
+ Kestrel and IIS
+ MS SQL Server
+ LINQ
+ Entity Framework Core (ORM)
+ Identity Server (OpenID Connect and OAuth 2.0)
+ Azure Key Vault
+ Azure hosting
+ Angular 6 and Material Design
+ XRegExp
+ SendGrid

## Visuals
### Registration
![Registration Picture](https://i.ibb.co/3rkVCPf/pictit-register.png)
### Login: consent and 2FA
![Registration Registration](https://i.ibb.co/pf8F388/pictit-login.png)
### Search result
<img style="float: left;margin-right: 25px;" src="https://i.ibb.co/ZhWDTVv/card-pictit.png">
**N.B.**: Guesses are picture based.
+ Color: blue for guessed male and pink for guessed female.
+ First line: username chosen for display by the user who has been searched.
+ Second line: guessed age and emotion.
## Demo
[![Click here to see the demo](https://i.ytimg.com/vi/9jgqyDDYFY8/hqdefault.jpg)](https://youtu.be/9jgqyDDYFY8 "Demo PictIt")