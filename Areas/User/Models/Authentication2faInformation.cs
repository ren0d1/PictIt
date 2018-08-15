namespace PictIt.Areas.User.Models
{
    public class Authentication2faInformation
    {
        public bool HasAuthenticator { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsMachineRemembered { get; set; }

        public int RecoveryCodesCount { get; set; }
    }
}
