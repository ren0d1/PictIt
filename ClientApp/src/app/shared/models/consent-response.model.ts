export class ConsentResponse {
  constructor(
    public granted: boolean,
    public scopesConsented: string[],
    public rememberConsent: boolean) {}
}
