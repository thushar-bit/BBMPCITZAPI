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
        public string? EKYCApplnCode { get; set; }
    }
    public class BBMPSMSSETTINGS
    {
        public string? BBMP_SECRET_KEY_ctz { get; set; }
        public string? BBMP_SENDER_ADDRESS_ctz { get; set; }
        public string? BBMP_SMS_USERNAME_ctz { get; set; }
        public string? BBMP_SMS_PASSWORD_ctz { get; set; }

    }
    public class OTPResponse
    {
        public string OTPResponseMessage { get; set; }
        public string OTP { get; set; }
    }
    public class PropertyDetails
    {
        public string PROPERTYCODE { get; set; }
        public string PROPERTYID { get; set; }
        public string NameMatchURL { get; set; }    
    }
    public class ESignSettings
    {
        public string TempFiles_ctz { get; set; }
        public string ReturnURL_ctzEID { get; set; }
        public string ESignServicesURL_ctz { get; set; }
        public string TempURl_ctz { get; set; }
    }
    public class BescomSettings
    {
        public string BESCOMAPIURL { get; set; }
        public string BESCOMAPIUsername { get; set; }
        public string BESCOMAPIPassword { get; set; }
    }
    public class KaveriSettings
    {
        public string KaveriAPIKey { get; set; }
        public string KaveriUsername { get; set; }
        public string KaveriPassword { get; set; }
        public string KaveriPublicKey { get; set; }
        public string KaveriECAPI { get; set; }
        public string KaveriDocDetailsAPI { get; set; }
        public string KaveriECDocAPI { get; set; }
        public string ServerIP { get; set; }    

    }
}
