import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {

  // Url of the Identity Provider
  issuer: window.location.origin,

  requireHttps: true,

  // URL of the SPA to redirect the user to after login
  redirectUri: window.location.origin + '/auth-callback',

  responseType: 'id_token token',

  // The SPA's id. The SPA is registerd with this id at the auth-server
  clientId: 'pict_it_spa',

  // set the scope for the permissions the client should request
  // The first three are defined by OIDC. The 4th is a usecase-specific one
  scope: 'openid profile email pict-it-api.read',

  logoutUrl: window.location.origin + '/api/user/logout',

  postLogoutRedirectUri: window.location.origin + '/home'
};
