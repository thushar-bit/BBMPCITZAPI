namespace BBMPCITZAPI.Models
{
    public class EKYCSettings
    {
        public string? EKYCTokenURL { get; set; }
        public string? EKYCDeptCode { get; set; }
        public string? EKYCIntegrationKey { get; set; }
        public string? EKYCIntegrationPassword { get; set; }
        public string? EKYCServiceCode { get; set; }
        public string? EKYCResponseRedirectURL { get; set; }
        public string? EKYCRequestURL { get; set; }
    }
    public class BBMPSMSSETTINGS
    {
        public string? BBMP_SECRET_KEY_ctz { get; set; }
        public string? BBMP_SENDER_ADDRESS_ctz { get; set; }
        public string? BBMP_SMS_USERNAME_ctz { get; set; }
        public string? BBMP_SMS_PASSWORD_ctz { get; set; }
     
    }
}
