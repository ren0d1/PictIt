export class RegisterUser {
  constructor(
    public firstName: string | null,
    public lastName: string | null,
    public userName: string | null,
    public email: string,
    public password: string,
    public acceptedTerms: boolean) {}
}
