using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Database;
using System.Data;
using BBMPCITZAPI.Services.Interfaces;
using Newtonsoft.Json;
using static BBMPCITZAPI.Services.BBMPBookModuleService;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;
using NUPMS_BA;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using BBMPCITZAPI.Services;

namespace BBMPCITZAPI.Controllers
{

    [ApiController]
    
    [Route("v1/E-KYCAPI")]
    public class EKYCController : ControllerBase
    {
        private readonly ILogger<EKYCController> _logger;
        private readonly EKYCSettings _ekycSettings;
        private readonly BBMPSMSSETTINGS _BBMPSMSSETTINGS;
        private readonly IErrorLogService _errorLogService;
        private readonly IObjectionService _ObjectionService;
        private readonly IMutationObjectionService _MutationService;
        private readonly IAmalgamationService _amalgamationService;

        public EKYCController(ILogger<EKYCController> logger,  IOptions<EKYCSettings> ekycSettings, IErrorLogService errorLogService,
            IOptions<BBMPSMSSETTINGS> BBMPSMSSETTINGS, IObjectionService ObjectionService,IMutationObjectionService mutationObjection,IAmalgamationService amalgamationService)
        {
            _logger = logger;
            _ekycSettings = ekycSettings.Value;
            _BBMPSMSSETTINGS = BBMPSMSSETTINGS.Value;
            _errorLogService = errorLogService;
            _ObjectionService = ObjectionService;
            _MutationService = mutationObjection;
            _amalgamationService = amalgamationService;
        }

        //[Authorize]
        [HttpPost("INS_NCL_OBJECTION_MAIN")]
        public async Task<string> INS_NCL_OBJECTION_MAIN(int ULBCODE, long Propertycode, long PropertyEID)
        {
            _logger.LogInformation("GET request received at INS_NCL_OBJECTION_MAIN");
            try
            {

                var dataSet = _ObjectionService.INS_NCL_OBJECTION_MAIN(ULBCODE, Propertycode, PropertyEID);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                long objid = 0;

                if (dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Rows.Count > 0 &&
                    dataSet.Tables[0].Columns.Contains("OBJID") &&
                    dataSet.Tables[0].Rows[0]["OBJID"] != DBNull.Value)
                {
                    objid = Convert.ToInt64(dataSet.Tables[0].Rows[0]["OBJID"]);
                }

           string json1 = await GetEKYCRequest(1, objid, Propertycode,"Objection");
                return json1;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_OBJECTION_MAIN");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_OBJECTION_MAIN.");
                throw;
            }
        }
        [HttpPost("INS_NCL_SEARCH_MAIN")]
        public async Task<string> INS_NCL_SEARCH_MAIN()
        {
            _logger.LogInformation("GET request received at INS_NCL_SEARCH_MAIN");
            try
            {

                var dataSet = _ObjectionService.INS_NCL_SEARCH_MAIN();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                long searchId = 0;

                if (dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Rows.Count > 0 &&
                    dataSet.Tables[0].Columns.Contains("SEARCH_REQ_ID") &&
                    dataSet.Tables[0].Rows[0]["SEARCH_REQ_ID"] != DBNull.Value)
                {
                    searchId = Convert.ToInt64(dataSet.Tables[0].Rows[0]["SEARCH_REQ_ID"]);
                }

                string json1 = await GetEKYCRequest(1, searchId, 10,"Search");
                return json1;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_SEARCH_MAIN");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_SEARCH_MAIN.");
                throw;
            }
        }
        [HttpPost("INS_NCL_MUTATION_OBJECTION_MAIN")]
        public async Task<string> INS_NCL_MUTATION_OBJECTION_MAIN()
        {
            _logger.LogInformation("GET request received at INS_NCL_MUTATION_OBJECTION_MAIN");
            try
            {

                var dataSet = _MutationService.INS_NCL_MUTATION_OBJECTION_MAIN();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                long ObjectionMutationId = 0;

                if (dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Rows.Count > 0 &&
                    dataSet.Tables[0].Columns.Contains("MUTATION_OBJECTION_REQ_ID") &&
                    dataSet.Tables[0].Rows[0]["MUTATION_OBJECTION_REQ_ID"] != DBNull.Value)
                {
                    ObjectionMutationId = Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTATION_OBJECTION_REQ_ID"]);
                }

                string json1 = await GetEKYCRequest(1, ObjectionMutationId, 10, "Objection_Mutation");
                return json1;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_MUTATION_OBJECTION_MAIN");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_MUTATION_OBJECTION_MAIN.");
                throw;
            }
        }
        [HttpPost("INS_NCL_MUTATION_AMALGAMATION_MAIN")]
        public async Task<string> INS_NCL_MUTATION_AMALGAMATION_MAIN()
        {
            _logger.LogInformation("GET request received at INS_NCL_MUTATION_AMALGAMATION_MAIN");
            try
            {

              
                string json1 = await GetEKYCRequest(1, 1, 10, "Amalgamation");
                return json1;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_MUTATION_AMALGAMATION_MAIN");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_MUTATION_AMALGAMATION_MAIN.");
                throw;
            }
        }

        [HttpPost("RequestEKYC")]
        public async Task<string> GetEKYCRequest(int OwnerNumber,long BOOK_APP_NO,long PROPERTY_CODE,string Page)
        {
            try
            {
                _logger.LogInformation( "EKYC Request Started");
                string EKYCTokenURL = _ekycSettings.EKYCTokenURL!;
                string EKYCDeptCode = _ekycSettings.EKYCDeptCode!;
                string EKYCIntegrationKey = _ekycSettings.EKYCIntegrationKey!;
                string EKYCIntegrationPassword = _ekycSettings.EKYCIntegrationPassword!;
                string EKYCServiceCode = _ekycSettings.EKYCServiceCode!;
                string EKYCResponseRedirectURL = "";
                string EKYCRequestURL = _ekycSettings.EKYCRequestURL!;
                string EKYCApplnCode = _ekycSettings.EKYCApplnCode!;
                string EKYCTokenRequest = "";

                string transacDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                long transactionNo = 0;
                if (Page == "Search")
                {
                    EKYCResponseRedirectURL = _ekycSettings.EKYCResponseSearchRedirectURL!;
                }
                else if(Page == "Objection")
                {
                    EKYCResponseRedirectURL = _ekycSettings.EKYCResponseObjectionRedirectURL!;
                }
                else if(Page == "Objection_Mutation")
                {
                    EKYCResponseRedirectURL = _ekycSettings.EKYCResponseMutationObjectionRedirectURL!;
                }
                else if(Page == "Amalgamation")
                {
                    EKYCResponseRedirectURL = _ekycSettings.EKYCResponseAmalgamationRedirectURL!;
                }
                if (EKYCApplnCode == "81")

                {
                    //live site
                    EKYCTokenRequest = "{deptCode: " + EKYCDeptCode + ",ApplnCode:" + EKYCApplnCode + ",integrationKey: \"" + EKYCIntegrationKey + "\",integrationPassword: \"" + EKYCIntegrationPassword + "\",txnNo:transactionNo,txnDateTime: " + transacDateTime + ",serviceCode: " + EKYCServiceCode + ",responseRedirectURL: \"" + EKYCResponseRedirectURL + "\"}";
                    //test site
                }
                else 
                {
                    EKYCTokenRequest = "{deptCode: " + EKYCDeptCode + ",integrationKey: \"" + EKYCIntegrationKey + "\",integrationPassword: \"" + EKYCIntegrationPassword + "\",txnNo:transactionNo,txnDateTime: " + transacDateTime + ",serviceCode: " + EKYCServiceCode + ",responseRedirectURL: \"" + EKYCResponseRedirectURL + "\"}";
                }
               
                  
                
                EKYCTokenRequest = EKYCTokenRequest.Replace("\"", "'");
                NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();
                transactionNo = obj.INSERT_EKYC_REQUEST_OWNER(BOOK_APP_NO, PROPERTY_CODE, OwnerNumber, transacDateTime, EKYCTokenRequest, "crc");
                EKYCTokenRequest = EKYCTokenRequest.Replace("transactionNo", transactionNo.ToString());

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                var content2 = new StringContent(EKYCTokenRequest, Encoding.UTF8, "application/json");
                HttpClient client1 = new HttpClient();
                var httpResponse = client1.PostAsync(EKYCTokenURL, content2).Result; //Request for Deed download
                var respornseContent = httpResponse.Content.ReadAsStringAsync().Result;
                string respStat = httpResponse.StatusCode.ToString();
                JObject Obj_Json = JObject.Parse(respornseContent);
                string TokenValue = (string)Obj_Json.SelectToken("Token")!;

                EKYCTokenRequest = EKYCTokenRequest.Replace("transactionNo", transactionNo.ToString());
                EKYCRequestURL = EKYCRequestURL + "?key=" + EKYCIntegrationKey + "&token=" + TokenValue;
                obj.INSERT_EKYC_OWNER_TOKEN_RESPONSE(transactionNo, TokenValue, EKYCRequestURL, "crc");

                return EKYCRequestURL;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "RequestEKYC");
                _logger.LogError(ex, "Error occurred while executing EKYC Request");
                throw;
            }
        }
      
        private string GenerateRandomNumber()
        {
            string numbers = "1234567890";
            string otp = string.Empty;
            int length = 6;
            string characters = numbers;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;
        }
        private bool SmsOtpFunc(string OTP, string Message, string smstemplateid, string MOBILENO)
        {
            try
            {
                EAS_BA objEAS = new EAS_BA();
                string TEMPLATETEXT = Message;
                string SECRET_KEY = "";
                string SENDER_ADDRESS = "";
                string SMS_USERNAME = "";
                string SMS_PASSWORD = "";

                SECRET_KEY = _BBMPSMSSETTINGS.BBMP_SECRET_KEY_ctz!;
                SENDER_ADDRESS = _BBMPSMSSETTINGS.BBMP_SENDER_ADDRESS_ctz!;
                SMS_USERNAME = _BBMPSMSSETTINGS.BBMP_SMS_USERNAME_ctz!;
                SMS_PASSWORD = _BBMPSMSSETTINGS.BBMP_SMS_PASSWORD_ctz!;



                string ret = objEAS.sendOTPMSG(SMS_USERNAME, SMS_PASSWORD, SENDER_ADDRESS, MOBILENO, TEMPLATETEXT, SECRET_KEY, smstemplateid);

                string[] resp = ret.Split(',');
                if (resp.Length >= 2)
                {
                    string sucRespCode = resp[0];
                    string Resp = resp[1];
                    if (sucRespCode == "402")
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex,"");
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }

        [HttpGet("SendOTP")]
        public OTPResponse SendOtp(string OwnerMobileNo)
        {
            try
            {
                var hdnOTP = GenerateRandomNumber();
                var smstemplate = 29;
                EAS_BA SMSTEMP = new EAS_BA();
                OTPResponse opt = new OTPResponse();
                DataSet dsSMS = SMSTEMP.GetSMSTemplete(smstemplate);
                if (dsSMS != null && dsSMS.Tables.Count > 0 && dsSMS.Tables[0].Rows.Count > 0)
                {
                    string msg = dsSMS.Tables[0].Rows[0]["TEMPLATETEXT"].ToString()!;
                    string templateid = dsSMS.Tables[0].Rows[0]["SMSTEMPLATEID"].ToString()!;
                    msg = msg.Replace("{#var#}", hdnOTP);
                    if (SmsOtpFunc(hdnOTP, msg, templateid, OwnerMobileNo.Trim()))
                    {
                        opt.OTPResponseMessage = "OTP SENT SUCCESSFULLY";
                        opt.OTP = hdnOTP;
                    }
                    else
                    {
                        opt.OTPResponseMessage = "Request for OTP failed. Please check Mobile Number and try again";
                        opt.OTP = hdnOTP;
                    }
                    return opt;
                }
                else
                {
                    opt.OTPResponseMessage = "Request for OTP failed. Please check Mobile Number and try again";
                    opt.OTP = hdnOTP;
                }
                return opt;
                //  return hdnOTP;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "SmsOtpFunc");
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
     //   [Authorize]
        [HttpGet("UPDATE_EKYC_OWNER_VAULT_RESPONSE")]
        public int UPDATE_EKYC_OWNER_VAULT_RESPONSE(int txnNo,string success,string vaultrefno,string loginid)
        {
            try
            {
                _logger.LogInformation("GET request received at UPDATE_EKYC_OWNER_VAULT_RESPONSE");
                NUPMS_BA.ObjectionModuleBA objba = new ObjectionModuleBA();
               int response = objba.UPDATE_EKYC_OWNER_VAULT_RESPONSE(txnNo, success, vaultrefno, loginid);
                _logger.LogInformation("GET  UPDATE_EKYC_OWNER_VAULT_RESPONSE response");
                return response;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPDATE_EKYC_OWNER_VAULT_RESPONSE");
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
        [HttpGet("EKYCTEST")]
        public string EKYCTEST(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    return "React Page is CONNECTED TO API";
                }
                else
                {
                    return "React page is Not connected To API";
                }
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex,"");
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
    }
}

