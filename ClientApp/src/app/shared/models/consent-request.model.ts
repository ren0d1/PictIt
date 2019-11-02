export class ConsentRequest {
  constructor(
    public clientId: string,
    public Nonce: string,
    public scopesRequested: string[],
    public subject: string) {}
}
