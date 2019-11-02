export class ResetPassword {
  constructor(
    public code: string,
    public email: string,
    public password: string) {}
}
