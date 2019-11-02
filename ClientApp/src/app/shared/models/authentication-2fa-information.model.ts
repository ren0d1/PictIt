export class Authentication2faInformation {
    constructor(
      public hasAuthenticator: boolean,
      public isEnabled: boolean,
      public IsMachineRemembered: boolean,
      public recoveryCodesCount: number) {}
}
