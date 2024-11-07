using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUPMS_BA;
using NUPMS_BO;
using System;
using System.Data;
using System.IdentityModel.Claims;
using System.IO;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using static BBMPCITZAPI.Models.KaveriData;
using static BBMPCITZAPI.Models.ObjectionModels;


namespace BBMPCITZAPI.Controllers
{
    [Route("v1/ObjectionAPI")]
    [ApiController]

    public class ObjectionController : ControllerBase
    {

        private readonly ILogger<EKYCController> _logger;
        private readonly KaveriSettings _kaveriSettings;
        private readonly INameMatchingService _nameMatchingService;
        private readonly IObjectionService _ObjectionService;
        private readonly IErrorLogService _errorLogService;
       
        public ObjectionController(ILogger<EKYCController> logger, IOptions<KaveriSettings> kaveriSettings,
            INameMatchingService NameMatching, IObjectionService ObjectionService, IErrorLogService errorLogService)
        {
            _logger = logger;
            _kaveriSettings = kaveriSettings.Value;
            _nameMatchingService = NameMatching;
            _ObjectionService = ObjectionService;
            _errorLogService = errorLogService;
          
        }
        NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();


        private Dictionary<string, string> ExtractJsonParameters(JObject Obj_Json, List<string> lstParameters)
        {
            Dictionary<string, string> dictPayParameters = new Dictionary<string, string>();
            foreach (string parameter in lstParameters)
            {
                dictPayParameters.Add(parameter, (string)Obj_Json.SelectToken(parameter));
            }
            return dictPayParameters;
        }
        private static string Encrypt(string data, string rsaParameters)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.FromXmlString(rsaParameters);
                var byteData = Encoding.UTF8.GetBytes(data);
                var encryptedData = rsaCryptoServiceProvider.Encrypt(byteData, false);
                return Convert.ToBase64String(encryptedData);
            }
        }
        private class TransactionDetails
        {
            public HttpResponseMessage httpResponseMessage { get; set; }
            public Int64 transactionId { get; set; }
        }

        private async Task<TransactionDetails> KaveriAPIRequest(string urlKeyWord, string RegistrationNoECNumber, string BOOKS_APP_NO, string PropertyCode, string LoginId)
        {
            _logger.LogInformation("GET request received at KaveriAPIRequest");
            try
            {
                Int64 transactionNo = 0;
                //   ViewState["Kaveri_TransactionNo"] = transactionNo;
                string Json = "";
                string rsaKeyDetails = "<RSAKeyValue><Modulus>" + Convert.ToString(_kaveriSettings.KaveriPublicKey) + " </Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                Uri requestUri = new Uri(_kaveriSettings.KaveriDocDetailsAPI);

                HttpClient client1 = new HttpClient();
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) => { return true; };

                if (urlKeyWord == "KaveriDocDetailsAPI")
                {
                    _logger.LogInformation("GET request received at  KaveriAPIRequest KaveriDocDetailsAPI");
                    requestUri = new Uri(_kaveriSettings.KaveriDocDetailsAPI);
                    //Json = "{\r\n  \"username\": \"" + username + "\",\r\n  \"password\": \"" + password + "\",\r\n  \"finalRegNumber\": \"" + RegistrationNo + "\"\r\n}";
                    Json = "{\r\n \"apikey\":\"" + Convert.ToString(_kaveriSettings.KaveriAPIKey) + "\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"finalRegNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
                    transactionNo = obj.INS_KAVERI_API_DOCUMENT_REQUEST(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoECNumber, Json, Convert.ToString(LoginId));
                }
                else if (urlKeyWord == "KaveriECDocAPI")
                {
                    _logger.LogInformation("GET request received at  KaveriAPIRequest KaveriECDocAPI");
                    requestUri = new Uri(_kaveriSettings.KaveriECDocAPI);
                    Json = "{\r\n \"apikey\":\"" + Convert.ToString(_kaveriSettings.KaveriAPIKey) + "\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"certificateNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
                    //   Json = "{\"apikey\": \"1\",\"username\": \"StazL1fAkoRt+o7I01iekrPbHaTQ32wBkAtrULKQ1otSv3DcbI0DLMBI63xevCyYSp3zLNonRI+bE5Q0W7k2unQvfCl0EpK1SmEF33El1ACe44nQbwfiIc5L2CTL8zgeQR0rc1CyTkirEVGlVlr8nrSGd8W5ACVNS12aj4vsdrc=\",\"password\": \"kzpJ98Kio4FNocARzdqSLu7lQhEBQ1fcf4AHYTC2I5UC+/e0VJPEVv+pnV17DWBAJXIMJY7ybPvRJ7Z+Eggm2uSL2/aWN+K9Jo19YiWq8pTzOpg7vFygPdYgIVPc9qdhHoBovpzQp6GvjI3n85BmqxlIc8peBtKyNjYd4HMk6+Y=\",\"certificateNumber\": \"d+BB+O9L/4lW0de9+t4LAZ42/3CtPpHKSyZMA5k0OkEjFciQhCnwAO0NHNC6dJWD3jGzXlWmYbdVJnbNfdZ5QM4PbMR50CudjelEATRTvD9eB2A0tphnX1x5k4J+RmBJxUmsfNTCKzRVpWTaOAYWozbeqf2sSbDMJXMK543LfEo=\"}";
                    transactionNo = obj.INS_KAVERI_API_ECDOC_REQUEST(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoECNumber, Json, Convert.ToString(LoginId));
                }
                // ViewState["Kaveri_TransactionNo"] = transactionNo;

                var content2 = new StringContent(Json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await client1.PostAsync(requestUri, content2); //Request for Deed download
                TransactionDetails trc = new()
                {
                    httpResponseMessage = httpResponse,
                    transactionId = transactionNo,

                };
                return trc;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "KaveriAPIRequest");
                _logger.LogError(ex, "Error occurred while executing stored procedure.KaveriAPIRequest");
                throw ex;
            }
        }

       // [Authorize]
        [HttpPost("GetObjectionKaveriDocData")]
        public async Task<IActionResult> GetKaveriDocData(string RegistrationNoNumber, string objectionid, string PropertyCode, string LoginId)
        {
            string APIResponse = "", APIResponseStatus = "";
            bool isResponseStored = false;
            _logger.LogInformation("GET request received at GetKaveriDocData");
            try
            {

                //RegistrationNumber = "NMG-1-00224-2023-24";

                TransactionDetails httpResponse = await KaveriAPIRequest("KaveriDocDetailsAPI", RegistrationNoNumber, objectionid, PropertyCode, LoginId);
                var respornseContent = httpResponse.httpResponseMessage.Content.ReadAsStringAsync().Result;
                APIResponse = respornseContent;
                string respStat = httpResponse.httpResponseMessage.StatusCode.ToString();

                if (respStat == "OK")
                {
                    APIResponseStatus = "SUCCESS";

                    string KAVERIDOC_RESPONSE_ROWID = obj.INS_KAVERI_API_DOCUMENT_RESPONSE(httpResponse.transactionId, APIResponseStatus, APIResponse);
                    isResponseStored = true;
                    var response = JsonConvert.DeserializeObject<KaveriData.KAVERI_API_DOC_DETAILS_RESPONSE>(respornseContent);
                    var documentDetailsList = new List<KaveriData.DocumentDetails>();
                    int NameMatchScore = 0;
                    int EKYC_OWNERNO = 0;
                    string EKYC_OWNERNAME = "";

                    //foreach (var responsevar in response)
                    //   {
                    if (response.responseCode == "1000")
                    {
                        if (!string.IsNullOrEmpty(response.json))
                        {
                            var documentDetails = JsonConvert.DeserializeObject<KaveriData.DocumentDetails>(response.json);
                            documentDetailsList.Add(documentDetails);
                            _ObjectionService.INS_NCL_OBJECTION_KAVERI_DOC_DETAILS_TEMP(Convert.ToInt64(objectionid), Convert.ToInt64(PropertyCode), RegistrationNoNumber,
                                documentDetails.naturedeed, documentDetails.applicationnumber, documentDetails.registrationdatetime, KAVERIDOC_RESPONSE_ROWID, LoginId);

                            if (documentDetails.propertyinfo != null)
                            {

                                double totalKavAreaMT = 0;
                                double totalKavAreaFT = 0;

                                if (documentDetails.propertyinfo[0].propertyschedules != null)
                                {



                                    totalKavAreaMT += Convert.ToDouble(documentDetails.propertyinfo[0].propertyschedules[0].totalarea);

                                    totalKavAreaFT += Convert.ToInt64(totalKavAreaMT * 10.7639);

                                    //update total area sp here 
                                    UPD_AREA_CHECKBANDI_KAVERI_DATA s = new UPD_AREA_CHECKBANDI_KAVERI_DATA()
                                    {
                                        SITEAREA_KAVERI_FT = totalKavAreaFT,
                                        SITEAREA_KAVERI_MT = totalKavAreaMT,
                                        CHECKBANDI_EAST = documentDetails?.propertyinfo[0].eastboundary,
                                        CHECKBANDI_NORTH = documentDetails?.propertyinfo[0].northboundary,
                                        CHECKBANDI_SOUTH = documentDetails?.propertyinfo[0].southboundary,
                                        CHECKBANDI_WEST = documentDetails?.propertyinfo[0].westboundary,
                                        propertyCode = Convert.ToInt64(PropertyCode),
                                        P_BOOKS_PROP_APPNO = Convert.ToInt64(objectionid),
                                        loginId = LoginId,
                                    };
                                   // _ObjectionService.UPD_AREA_CHECKBANDI_KAVERI_DATA(s);
                                //    obj.UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), "KAVERIDOC_RESPONSE_ROWID", KAVERIDOC_RESPONSE_ROWID, Convert.ToString(LoginId));
                                }
                                foreach (var propertyinfo in documentDetails.propertyinfo)
                                {
                                    _ObjectionService.INS_NCL_OBJECTION_KAVERI_PROPERTY_DETAILS_TEMP(Convert.ToInt64(objectionid), Convert.ToInt64(PropertyCode),
                                    RegistrationNoNumber, propertyinfo.propertyid, propertyinfo.documentid, propertyinfo.villagenamee, propertyinfo.sroname, propertyinfo.hobli, propertyinfo.zonenamee, totalKavAreaMT, KAVERIDOC_RESPONSE_ROWID, LoginId);
                                }
                            }
                            if (documentDetails.partyinfo != null)
                            {
                                int ownerNumber = 1;

                                foreach (var party in documentDetails.partyinfo)
                                {

                                    _ObjectionService.INS_NCL_OBJECTION_KAVERI_PARTIES_DETAILS_TEMP(Convert.ToInt64(objectionid), Convert.ToInt64(PropertyCode), RegistrationNoNumber, party.partyname, party.address, party.idprooftypedesc, party.idproofnumber, party.partytypename,
                                        party.admissiondate, KAVERIDOC_RESPONSE_ROWID, Convert.ToString(LoginId), ownerNumber, "", 0);
                                }
                            }

                        }
                    }
                    else
                    {
                        return Ok(new { success = false, message = $"Kaveri Doc Details API returned bad response: {response.responseCode}, {response.responseMessage}" });
                    }
                    //}
                    return Ok(new { success = true, data = documentDetailsList });
                }
                return Ok(new { success = false, message = $"Kaveri Doc Details API returned bad response: {respStat}" });
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetKaveriDocData");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetKaveriDocData");
                throw;
            }
        }

        [HttpGet("GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP")]
        public ActionResult<DataSet> GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP(int ULBCODE, long Propertycode, long objectionid)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP");
            try
            {

                var dataSet = _ObjectionService.GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP(ULBCODE,Propertycode,objectionid);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP.");
                throw;
            }
        }
       
        [HttpPost("INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE, string loginId, string EMAIL, string PROPERTYID, EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at GET_WARD_BY_WARDNUMBER");
            try
            {

                var dataSet = _ObjectionService.INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA(IDENTIFIERTYPE, IdentifierName, MOBILENUMBER, MOBILEVERIFY, NAMEMATCHSCORE, EMAIL,PROPERTYID, loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA.");
                throw;
            }
        }
      
        [HttpPost("NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS")]
        public ActionResult<DataSet> NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS");
            try
            {
                if (NCLPropID.ORDERDATE != null)
                {
                    DateTime? documentDate = NCLPropID.ORDERDATE;

                    DateTime? d2 = documentDate?.AddDays(1);
                    NCLPropID.ORDERDATE = d2;
                }
                var dataSet = _ObjectionService.NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS(ID_BASIC_PROPERTY, NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS");
                _logger.LogError(ex, "Error occurred while executing stored procedure NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS.");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT(INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT final)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT");
            try
            {

                var dataSet = _ObjectionService.INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT(final);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT.");
                throw;
            }
        }
        [HttpPost("NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL")]
        public ActionResult<int> NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL(Models.NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL");
            try
            {
                int dataSet = _ObjectionService.NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL(NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL");
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL");
                throw;
            }
        }




    }

}
