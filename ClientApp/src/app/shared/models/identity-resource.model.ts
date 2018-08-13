export class IdentityResource {
  constructor(
    public name: string,
    public displayName: string,
    public description: string,
    public emphasize: boolean,
    public required: boolean) {}
}
