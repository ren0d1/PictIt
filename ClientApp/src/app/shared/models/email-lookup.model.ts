export class EmailLookup {
    constructor(
        public mx_set: boolean,
        public valid_mx: boolean,
        public email_sendable: boolean,
        public accepts_all_emails: boolean
    ) {}
}
