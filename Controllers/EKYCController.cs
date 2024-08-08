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

namespace BBMPCITZAPI.Controllers
{
    [ApiController]
    [Route("v1/E-KYCAPI")]
    public class EKYCController : ControllerBase
    {
        private readonly ILogger<EKYCController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly EKYCSettings _ekycSettings;
        private readonly BBMPSMSSETTINGS _BBMPSMSSETTINGS;

        public EKYCController(ILogger<EKYCController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IOptions<EKYCSettings> ekycSettings,
            IOptions<BBMPSMSSETTINGS> BBMPSMSSETTINGS)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _ekycSettings = ekycSettings.Value;
            _BBMPSMSSETTINGS = BBMPSMSSETTINGS.Value;
        }
        [HttpPost("RequestEKYC")]
        public string GetEKYCRequest(int OwnerNumber,int BOOK_APP_NO,int PROPERTY_CODE)
        {
            try
            {
                string EKYCTokenURL = _ekycSettings.EKYCTokenURL!;
                string EKYCDeptCode = _ekycSettings.EKYCDeptCode!;
                string EKYCIntegrationKey = _ekycSettings.EKYCIntegrationKey!;
                string EKYCIntegrationPassword = _ekycSettings.EKYCIntegrationPassword!;
                string EKYCServiceCode = _ekycSettings.EKYCServiceCode!;
                string EKYCResponseRedirectURL = _ekycSettings.EKYCResponseRedirectURL!;
                string EKYCRequestURL = _ekycSettings.EKYCRequestURL!;
              
                string transacDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                int transactionNo = 0;

                string EKYCTokenRequest = "{deptCode: " + EKYCDeptCode + ",integrationKey: \"" + EKYCIntegrationKey + "\",integrationPassword: \"" + EKYCIntegrationPassword + "\",txnNo:transactionNo,txnDateTime: " + transacDateTime + ",serviceCode: " + EKYCServiceCode + ",responseRedirectURL: \"" + EKYCResponseRedirectURL + "\"}";
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
                return EKYCRequestURL;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
        [HttpGet("EditOwnerDetailsFromEKYCData")]
        public ActionResult<DataSet> GET_NCL_PROP_OWNER_TEMP_BYEKYCTRANSACTION(int transactionNumber)
        {
            try
            {
                NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();
                var dataSet = obj.GET_NCL_PROP_OWNER_TEMP_BYEKYCTRANSACTION(transactionNumber);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
        [HttpPost]
        [Route("api/ekyc")]
        [ActionName("SaveEKYCResponseDetails")]
        public HttpResponseMessage SaveEKYCResponseDetails(JObject jObject)
        {
            string response = string.Empty, jsonInput = string.Empty, VaultRefNumber = string.Empty, TxnNo = string.Empty;
            string AddressEng = string.Empty, AddressKnd = string.Empty;
            string OwnerNameEng = string.Empty, OwnerNameKnd = string.Empty;
            string IdentifierNameEng = string.Empty, IdentifierNameKnd = string.Empty, maskedAadhaar = string.Empty;
            string Gender = string.Empty, DateOfBirth = string.Empty;
            bool isResponseStored = false;
            byte[] photoBytes = null;
            ObjectionModuleBA objObjectionModuleBA = new ObjectionModuleBA();

            try
            {
                jsonInput = JsonConvert.SerializeObject(jObject);
                XmlDocument xmlDocEKYC = new XmlDocument();
                string jsonStringEKYC = "{ \"EKYCNode\": {" + jsonInput.Trim().TrimStart('{').TrimEnd('}') + "} }";
                xmlDocEKYC = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonStringEKYC);
                DataSet dsEKYCData = new DataSet();
                dsEKYCData.ReadXml(new XmlNodeReader(xmlDocEKYC));

                if (dsEKYCData != null && dsEKYCData.Tables.Count > 0 && dsEKYCData.Tables["EKYCNode"].Rows.Count > 0)
                {
                    VaultRefNumber = Convert.ToString(dsEKYCData.Tables["EKYCNode"].Rows[0]["VaultRefNumber"]);
                    TxnNo = Convert.ToString(dsEKYCData.Tables["EKYCNode"].Rows[0]["TxnNo"]);

                    objObjectionModuleBA.UPDATE_EKYC_OWNER_API_RESPONSE(Convert.ToInt32(TxnNo), VaultRefNumber, jsonInput);
                    isResponseStored = true;

                    if (Convert.ToString(dsEKYCData.Tables["EKYCNode"].Rows[0]["FinalStatus"]) == "S")
                    {
                        if (dsEKYCData.Tables["eKYCData"].Rows.Count > 0)
                        {
                            AddressEng += Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["house"]);
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["street"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["street"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["loc"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["loc"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["vtc"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["vtc"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["subdist"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["subdist"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["dist"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["dist"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["state"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["state"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["country"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["country"])) : "";
                            AddressEng += (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["pc"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["pc"])) : "";

                            OwnerNameEng = (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["name"]) != "null") ? Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["name"]) : "";
                            IdentifierNameEng = (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["co"]) != "null") ? Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["co"]) : "";
                            Gender = (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["gender"]) != "null") ? Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["gender"]) : "";
                            DateOfBirth = (Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["dob"]) != "null") ? Convert.ToString(dsEKYCData.Tables["eKYCData"].Rows[0]["dob"]) : "";

                            if (dsEKYCData.Tables["localKYCData"].Rows.Count > 0)
                            {
                                AddressKnd += Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["house"]);
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["street"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["street"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["loc"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["loc"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["vtc"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["vtc"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["subdist"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["subdist"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["dist"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["dist"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["state"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["state"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["country"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["country"])) : "";
                                AddressKnd += (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["pc"]) != "null") ? ("," + Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["pc"])) : "";

                                OwnerNameKnd = (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["name"]) != "null") ? Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["name"]) : "";
                                IdentifierNameKnd = (Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["co"]) != "null") ? Convert.ToString(dsEKYCData.Tables["localKYCData"].Rows[0]["co"]) : "";
                            }

                            response = "{Status:Success}";
                        }
                        else
                        {
                            response = "{Status:Failure,Message:NoeKYCData}";
                        }
                        maskedAadhaar = Convert.ToString(dsEKYCData.Tables["EKYCNode"].Rows[0]["maskedAadhaar"]);
                        string photo = Convert.ToString(dsEKYCData.Tables["EKYCNode"].Rows[0]["Photo"]);
                        if (photo != "")
                        {
                            try
                            {
                                photoBytes = Convert.FromBase64String(photo);
                            }
                            catch (Exception ex1)
                            {

                            }
                        }
                    }
                    else
                    {
                        response = "{Status:Failure,Message:FinalStatusNotS}";
                    }
                }
                else
                {
                    response = "{Status:Failure,Message:NoEKYCNodeData}";
                }
            }
            catch (Exception ex)
            {
                response = "{Status:Failure,Message:" + ex.Message + "}";
            }
            finally
            {
                if (!isResponseStored)
                    objObjectionModuleBA.UPDATE_EKYC_OWNER_API_RESPONSE(Convert.ToInt32(TxnNo), VaultRefNumber, jsonInput);
                objObjectionModuleBA.UPDATE_EKYC_OWNER_API_RESPONSE_OWNERUPDATE(Convert.ToInt32(TxnNo), OwnerNameKnd, OwnerNameEng, IdentifierNameKnd, IdentifierNameEng, AddressKnd, AddressEng, photoBytes, response, maskedAadhaar, Gender, DateOfBirth);
            }
            return new HttpResponseMessage
            {
                Content = new StringContent(response, Encoding.UTF8, "application/xml")
            };
        }
        [HttpGet("UPDATE_EKYC_OWNER_VAULT_RESPONSE")]
        public int UPDATE_EKYC_OWNER_VAULT_RESPONSE(int txnNo,string success,string vaultrefno,string loginid)
        {
            try
            {
                NUPMS_BA.ObjectionModuleBA objba = new ObjectionModuleBA();
               int response = objba.UPDATE_EKYC_OWNER_VAULT_RESPONSE(txnNo, success, vaultrefno, loginid);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
    }
}

