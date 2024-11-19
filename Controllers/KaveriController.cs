using BBMPCITZAPI.Models;
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


namespace BBMPCITZAPI.Controllers
{
    [Route("v1/KaveriAPI")]
    [ApiController]
   
    public class KaveriController : ControllerBase
    {

        private readonly ILogger<EKYCController> _logger;
        private readonly KaveriSettings _kaveriSettings;
        private readonly INameMatchingService _nameMatchingService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly IErrorLogService _errorLogService;
        public KaveriController(ILogger<EKYCController> logger, IOptions<KaveriSettings> kaveriSettings,
            INameMatchingService NameMatching, IBBMPBookModuleService IBBMPBOOKMODULE, IErrorLogService errorLogService)
        {
            _logger = logger;
            _kaveriSettings = kaveriSettings.Value;
            _nameMatchingService = NameMatching;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
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
            public HttpResponseMessage httpResponseMessage {  get; set; }   
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
                    Json = "{\r\n \"apikey\":\""+ Convert.ToString(_kaveriSettings.KaveriAPIKey) + "\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"finalRegNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
                    transactionNo = obj.INS_KAVERI_API_DOCUMENT_REQUEST(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoECNumber, Json, Convert.ToString(LoginId));
                }
                else if (urlKeyWord == "KaveriECDocAPI")
                {
                    _logger.LogInformation("GET request received at  KaveriAPIRequest KaveriECDocAPI");
                    requestUri = new Uri(_kaveriSettings.KaveriECDocAPI);
                    Json = "{\r\n \"apikey\":\""+ Convert.ToString(_kaveriSettings.KaveriAPIKey) + "\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"certificateNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
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

        private async Task<TransactionDetails> KaveriAPIRequestEC(string urlKeyWord, string RegistrationNoECNumber, string PropertyCode, string LoginId)
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

                if (urlKeyWord == "KaveriECDocAPI")
                {
                    _logger.LogInformation("GET request received at  KaveriAPIRequest KaveriECDocAPI");
                    requestUri = new Uri(_kaveriSettings.KaveriECDocAPI);
                    Json = "{\r\n \"apikey\":\"" + Convert.ToString(_kaveriSettings.KaveriAPIKey) + "\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"certificateNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
                    //   Json = "{\"apikey\": \"1\",\"username\": \"StazL1fAkoRt+o7I01iekrPbHaTQ32wBkAtrULKQ1otSv3DcbI0DLMBI63xevCyYSp3zLNonRI+bE5Q0W7k2unQvfCl0EpK1SmEF33El1ACe44nQbwfiIc5L2CTL8zgeQR0rc1CyTkirEVGlVlr8nrSGd8W5ACVNS12aj4vsdrc=\",\"password\": \"kzpJ98Kio4FNocARzdqSLu7lQhEBQ1fcf4AHYTC2I5UC+/e0VJPEVv+pnV17DWBAJXIMJY7ybPvRJ7Z+Eggm2uSL2/aWN+K9Jo19YiWq8pTzOpg7vFygPdYgIVPc9qdhHoBovpzQp6GvjI3n85BmqxlIc8peBtKyNjYd4HMk6+Y=\",\"certificateNumber\": \"d+BB+O9L/4lW0de9+t4LAZ42/3CtPpHKSyZMA5k0OkEjFciQhCnwAO0NHNC6dJWD3jGzXlWmYbdVJnbNfdZ5QM4PbMR50CudjelEATRTvD9eB2A0tphnX1x5k4J+RmBJxUmsfNTCKzRVpWTaOAYWozbeqf2sSbDMJXMK543LfEo=\"}";
                    transactionNo = obj.INS_WS_KAVERI_API_ECDOC_REQUEST( Convert.ToInt64(PropertyCode), RegistrationNoECNumber, Json, Convert.ToString(LoginId));
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

        [Authorize]
        [HttpPost("GetKaveriDocData")]
        public async Task<IActionResult> GetKaveriDocData( string RegistrationNoNumber, string BOOKS_APP_NO, string PropertyCode, string LoginId)
        {
            string APIResponse = "", APIResponseStatus = "";
            bool isResponseStored = false;
            _logger.LogInformation("GET request received at GetKaveriDocData");
            try
            {

                //RegistrationNumber = "NMG-1-00224-2023-24";

                TransactionDetails httpResponse = await KaveriAPIRequest("KaveriDocDetailsAPI", RegistrationNoNumber, BOOKS_APP_NO, PropertyCode, LoginId);
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
                                obj.INS_NCL_PROPERTY_KAVERI_DOC_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoNumber,
                                    documentDetails.naturedeed, documentDetails.applicationnumber, documentDetails.registrationdatetime,KAVERIDOC_RESPONSE_ROWID, LoginId);

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
                                        P_BOOKS_PROP_APPNO = Convert.ToInt64(BOOKS_APP_NO),
                                        loginId = LoginId,
                                    };
                                    _IBBMPBOOKMODULE.UPD_AREA_CHECKBANDI_KAVERI_DATA(s);
                                    obj.UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), "KAVERIDOC_RESPONSE_ROWID", KAVERIDOC_RESPONSE_ROWID, Convert.ToString(LoginId));
                                }
                                foreach (var propertyinfo in documentDetails.propertyinfo)
                                {
                                    obj.INS_NCL_PROPERTY_KAVERI_PROPERTY_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                                    RegistrationNoNumber, propertyinfo.propertyid, propertyinfo.documentid, propertyinfo.villagenamee, propertyinfo.sroname, propertyinfo.hobli, propertyinfo.zonenamee, totalKavAreaMT, KAVERIDOC_RESPONSE_ROWID, LoginId);
                                }
                            }
                                if (documentDetails.partyinfo != null)
                                {
                                    int ownerNumber = 1;
                                  
                                            foreach (var party in documentDetails.partyinfo)
                                    {
                                      
                                        obj.INS_NCL_PROPERTY_KAVERI_PARTIES_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoNumber, party.partyname, party.address, party.idprooftypedesc, party.idproofnumber, party.partytypename,
                                            party.admissiondate, KAVERIDOC_RESPONSE_ROWID, Convert.ToString(LoginId), ownerNumber, "", 0);
                                    }
                                }
                                
                            }
                        } else
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
       [Authorize]
        [HttpPost("GetKaveriECData")]
        public  async Task<IActionResult> GetKaveriECData(string ECNumber,string RegistrationNoNumber, string BOOKS_APP_NO, string PropertyCode, string LoginId,DateTime RegsiteredDateTime)
        {
            string APIResponse = "", APIResponseStatus = "";
            bool isResponseStored = false;

            try

            {

                //RegistrationNumber = "NMG-1-00224-2023-24";

                TransactionDetails httpResponse =await KaveriAPIRequest("KaveriECDocAPI", ECNumber, BOOKS_APP_NO, PropertyCode, LoginId);
                var respornseContent = httpResponse.httpResponseMessage.Content.ReadAsStringAsync().Result;
                APIResponse = respornseContent;
                string respStat = httpResponse.httpResponseMessage.StatusCode.ToString();
                if (respStat == "OK")
                {
                    JObject Obj_Json = JObject.Parse(respornseContent.Replace("],,", "],").Replace(",}", "}"));
                    string responseCode = (string)Obj_Json.SelectToken("responseCode");
                    APIResponseStatus = "SUCCESS";
                    string KAVERIDOC_RESPONSE_ROWID = "";
                    
                        isResponseStored = true;
                    string responseMessage = (string)Obj_Json.SelectToken("responseMessage");
                    if (responseMessage == "Sucess")
                    {
                        string base64String = (string)Obj_Json.SelectToken("json");
                        byte[] base64String1 = (byte[])Obj_Json.SelectToken("base64");
                        //    string fromDate = (string)Obj_Json.SelectToken("fromDate");
                        //    string toDate = (string)Obj_Json.SelectToken("toDate");
                        //    DateTime fromDate1 = DateTime.Parse(fromDate);
                        //    string format = "MM/dd/yyyy HH:mm:ss";
                        //    DateTime toDate1 = DateTime.ParseExact(toDate, format, System.Globalization.CultureInfo.InvariantCulture);

                        ////   DateTime minimumFromDate = DateTime.ParseExact(RegsiteredDateTime, format, System.Globalization.CultureInfo.InvariantCulture);

                        //    DateTime currentDate = DateTime.Now;
                        //    DateTime sevenDaysAgo = currentDate.AddDays(-8);
                        //    if (fromDate1 >= RegsiteredDateTime && toDate1 > sevenDaysAgo && toDate1 < currentDate)
                        //    {
                        string responseRawContent = respornseContent.ToString();
                        if (responseRawContent.IndexOf("fromDate", 0) < 0 || responseRawContent.IndexOf("toDate", 0) < 0)
                        {
                            
                            return Ok(new { success = false, message = "EC Data doesn't have valid from date and to date" });
                        }
                        string fromDate = responseRawContent.Substring(responseRawContent.IndexOf("fromDate", 0) + 11, 19);
                        string toDate = responseRawContent.Substring(responseRawContent.IndexOf("toDate", 0) + 9, 19);

                        if (fromDate != "" && toDate != "" && DateTime.Parse(fromDate) < DateTime.Parse(Convert.ToString(RegsiteredDateTime)) && DateTime.Parse(toDate) > DateTime.Now.AddDays(-8))
                        {
                            List<KaveriData.EcData> ECdocumentDetails = JsonConvert.DeserializeObject<List<KaveriData.EcData>>(base64String);
                    
                        var documentSummaries = new HashSet<string>(ECdocumentDetails.Select(doc => doc.DocSummary));
                        bool DoesExist = documentSummaries.Contains(RegistrationNoNumber);
                        var registrationNumberPosition = 0;
                        if (DoesExist)
                        {
                            registrationNumberPosition =  ECdocumentDetails.FindIndex(doc => doc.DocSummary == RegistrationNoNumber);
                        }
                        else
                        {
                            registrationNumberPosition = -1;
                        }
                        DataSet KAVERIDOC_RESPONSE = obj.INS_KAVERI_API_ECDOC_RESPONSE(Convert.ToInt64(httpResponse.transactionId), APIResponseStatus,
                               APIResponse, registrationNumberPosition, ECdocumentDetails.Count(), base64String1, Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode));
                        DataTable dataTableByName = KAVERIDOC_RESPONSE.Tables["Table"];
                        if (dataTableByName.Rows.Count > 0)
                        {
                            // Access the first row and the first column value
                            DataRow row = dataTableByName.Rows[0];
                            KAVERIDOC_RESPONSE_ROWID = row[0].ToString();
                        }
                        if (DoesExist)
                        {

                              var Dosc = ECdocumentDetails.OrderByDescending(x => x.ExecutionDate).FirstOrDefault();
                            //var Dosc = ECdocumentDetails.First(x => x.DocSummary == "NMG-1-00071-2023-24");
                            var parsedData = ParseDescription(Dosc.Description);
                            


                            if (Dosc.DocSummary == RegistrationNoNumber)
                            {
                                obj.INS_NCL_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                                ECNumber, RegistrationNoNumber, "Y", Dosc.DocSummary, parsedData.District, parsedData.Taluka, parsedData.Village, parsedData.HobliOrTown, "article", Dosc.ExecutionDate, Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), LoginId);
                                if (Dosc.Executants.Count() > 0)
                                {
                                    foreach (var i in Dosc.Executants)
                                    {
                                        obj.INS_NCL_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                                 RegistrationNoNumber, "Y", Dosc.DocSummary, i, "E", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 0, "", 0, LoginId);
                                    }
                                }
                                if (Dosc.Claimants.Count() > 0)
                                {
                                    int ownerNumber = 1;
                                   
                                    foreach (var i in Dosc.Claimants)
                                    {

                                                obj.INS_NCL_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                              RegistrationNoNumber, "Y", Dosc.DocSummary, i, "C", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 1, "", 0, LoginId);
                                          
                                    }
                                }
                                obj.UPD_ECDATA_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), "Y", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), Convert.ToString(LoginId));
                                return Ok(new { success = true, data = Dosc, ECDataExists = DoesExist });
                            }
                            else
                            {
                                obj.INS_NCL_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                                ECNumber, RegistrationNoNumber, "N", Dosc.DocSummary, parsedData.District, parsedData.Taluka, parsedData.Village, parsedData.HobliOrTown, "article", Dosc.ExecutionDate, Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), LoginId);
                                
                                if(Dosc.Executants.Count() > 0)
                                {
                                    foreach (var i in Dosc.Executants)
                                    {
                                        obj.INS_NCL_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                                 RegistrationNoNumber, "N", Dosc.DocSummary, i, "E", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 0, "", 0, LoginId);
                                    }
                                }
                                if(Dosc.Claimants.Count()>0)
                                {
                                    foreach (var i in Dosc.Claimants)
                                    {
                                        obj.INS_NCL_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode),
                                          RegistrationNoNumber, "N", Dosc.DocSummary, i, "C", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 0, "", 0, LoginId);
                                    }
                                }
                                obj.UPD_ECDATA_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), "N", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), Convert.ToString(LoginId));
                                await GetKaveriDocData(Dosc.DocSummary, BOOKS_APP_NO, PropertyCode, LoginId);
                                return Ok(new { success = true, data = Dosc, ECDataExists = DoesExist });
                            }
                        }
                        else
                        {
                            return Ok(new { success = true ,ECDataExists = DoesExist });
                        }
                    }

                    else
                    {
                            if (fromDate != "" && toDate != "")
                            {
                                return Ok(new { success = false, message = "The EC number has expired. Please provide an EC number issued within 7 days prior to the registration date.Submitted EC Dates are "+ fromDate.Substring(0, 10) +","+ toDate.Substring(0, 10) });

                            }
                            else
                            {
                                return Ok(new { success = false, message = "The EC number has expired. Please provide an EC number issued within 7 days prior to the registration date.Submitted EC Dates are.Submitted EC Dates are "+ fromDate +","+ toDate });

                               
                            }
                          
                    
                    }
                    }
                    else
                    {
                        return Ok(new { success = false, message = $"Kaveri EC Details API returned bad response: {responseMessage}" });

                    }

                }
                return Ok(new { success = false, message = $"Kaveri EC Details API returned bad response: {respStat}" });
                
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetKaveriECData");
                throw (ex);
                
            }
        }

        public class ECRequest
        {
          public  string ECNumber { get; set; }
            public string RegistrationNoNumber { get; set; }
            public Int64 PropertyCode{ get; set; }
            public string LoginId{ get; set; }
            public DateTime RegsiteredDateTime { get; set; }
        }
        [HttpPost("GetKaveriEC")]
        public async Task<IActionResult> GetKaveriECUploadData(ECRequest ECREQUEST)
        {
            string APIResponse = "", APIResponseStatus = "";
            bool isResponseStored = false;

            try

            {

                //RegistrationNumber = "NMG-1-00224-2023-24";

                TransactionDetails httpResponse = await KaveriAPIRequestEC("KaveriECDocAPI", ECREQUEST.ECNumber, Convert.ToString(ECREQUEST.PropertyCode), ECREQUEST.LoginId);
                var respornseContent = httpResponse.httpResponseMessage.Content.ReadAsStringAsync().Result;
                APIResponse = respornseContent;
                string respStat = httpResponse.httpResponseMessage.StatusCode.ToString();
                if (respStat == "OK")
                {
                    JObject Obj_Json = JObject.Parse(respornseContent.Replace("],,", "],").Replace(",}", "}"));
                    string responseCode = (string)Obj_Json.SelectToken("responseCode");
                    APIResponseStatus = "SUCCESS";
                    string KAVERIDOC_RESPONSE_ROWID = "";

                    isResponseStored = true;
                    string responseMessage = (string)Obj_Json.SelectToken("responseMessage");
                    if (responseMessage == "Sucess")
                    {
                        string base64String = (string)Obj_Json.SelectToken("json");
                        byte[] base64String1 = (byte[])Obj_Json.SelectToken("base64");
                       
                        string responseRawContent = respornseContent.ToString();
                        if (responseRawContent.IndexOf("fromDate", 0) < 0 || responseRawContent.IndexOf("toDate", 0) < 0)
                        {

                            return Ok(new { success = false, message = "EC Data doesn't have valid from date and to date" });
                        }
                        string fromDate = responseRawContent.Substring(responseRawContent.IndexOf("fromDate", 0) + 11, 19);
                        string toDate = responseRawContent.Substring(responseRawContent.IndexOf("toDate", 0) + 9, 19);
                        DateTime dtECEndDateForValidate = new DateTime(2024, 10, 31);
                        List<KaveriData.EcData> ECdocumentDetails = JsonConvert.DeserializeObject<List<KaveriData.EcData>>(base64String);

                        var documentSummaries = new HashSet<string>(ECdocumentDetails.Select(doc => doc.DocSummary));
                        bool DoesExist = documentSummaries.Contains(ECREQUEST.RegistrationNoNumber);
                        var registrationNumberPosition = 0;
                        if (DoesExist)
                        {
                            registrationNumberPosition = ECdocumentDetails.FindIndex(doc => doc.DocSummary == ECREQUEST.RegistrationNoNumber);
                        }
                        else
                        {
                            registrationNumberPosition = -1;
                        }
                        DataSet KAVERIDOC_RESPONSE = obj.INS_WS_KAVERI_API_ECDOC_RESPONSE(Convert.ToInt64(httpResponse.transactionId), APIResponseStatus,
                               APIResponse, registrationNumberPosition, ECdocumentDetails.Count(), base64String1, Convert.ToInt64(ECREQUEST.PropertyCode), ECREQUEST.LoginId);
                        DataTable dataTableByName = KAVERIDOC_RESPONSE.Tables["Table"];
                        if (fromDate != "" && toDate != "" && DateTime.Parse(fromDate) < DateTime.Parse(Convert.ToString(ECREQUEST.RegsiteredDateTime)) && DateTime.Parse(toDate) >= dtECEndDateForValidate)
                        {
                          
                            if (dataTableByName.Rows.Count > 0)
                            {
                                // Access the first row and the first column value
                                DataRow row = dataTableByName.Rows[0];
                                KAVERIDOC_RESPONSE_ROWID = row[0].ToString();
                            }
                            if (DoesExist)
                            {

                                var Dosc = ECdocumentDetails.OrderByDescending(x => x.ExecutionDate).FirstOrDefault();
                                //var Dosc = ECdocumentDetails.First(x => x.DocSummary == "NMG-1-00071-2023-24");
                                var parsedData = ParseDescription(Dosc.Description);



                                if (Dosc.DocSummary == ECREQUEST.RegistrationNoNumber)
                                {
                                Int64 ReqId =    obj.INS_NPM_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP(Convert.ToInt64(ECREQUEST.PropertyCode),
                                    ECREQUEST.ECNumber, ECREQUEST.RegistrationNoNumber, "Y", Dosc.DocSummary, parsedData.District, parsedData.Taluka, parsedData.Village, parsedData.HobliOrTown, "article", Dosc.ExecutionDate, Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), ECREQUEST.LoginId);
                                    if (Dosc.Executants.Count() > 0)
                                    {
                                        foreach (var i in Dosc.Executants)
                                        {
                                            obj.INS_NPM_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(ReqId), Convert.ToInt64(ECREQUEST.PropertyCode),
                                     ECREQUEST.RegistrationNoNumber, "Y", Dosc.DocSummary, i, "E", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 0, "", 0, ECREQUEST.LoginId);
                                        }
                                    }
                                    if (Dosc.Claimants.Count() > 0)
                                    {
                                        int ownerNumber = 1;

                                        foreach (var i in Dosc.Claimants)
                                        {

                                            obj.INS_NPM_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(ReqId), Convert.ToInt64(ECREQUEST.PropertyCode),
                         ECREQUEST.RegistrationNoNumber, "Y", Dosc.DocSummary, i, "C", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 1, "", 0, ECREQUEST.LoginId);

                                        }
                                    }
                                   
                                    return Ok(new { success = true, data = Dosc, ECDataExists = DoesExist, RequestId = ReqId });
                                }
                                else
                                {
                                    Int64 ReqId = obj.INS_NPM_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP( Convert.ToInt64(ECREQUEST.PropertyCode),
                                    ECREQUEST.ECNumber, ECREQUEST.RegistrationNoNumber, "N", Dosc.DocSummary, parsedData.District, parsedData.Taluka, parsedData.Village, parsedData.HobliOrTown, "article", Dosc.ExecutionDate, Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), ECREQUEST.LoginId);

                                    if (Dosc.Executants.Count() > 0)
                                    {
                                        foreach (var i in Dosc.Executants)
                                        {
                                            obj.INS_NPM_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(ReqId), Convert.ToInt64(ECREQUEST.PropertyCode),
                                     ECREQUEST.RegistrationNoNumber, "N", Dosc.DocSummary, i, "E", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 0, "", 0, ECREQUEST.LoginId);
                                        }
                                    }
                                    if (Dosc.Claimants.Count() > 0)
                                    {
                                        foreach (var i in Dosc.Claimants)
                                        {
                                            obj.INS_NPM_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(Convert.ToInt64(ReqId), Convert.ToInt64(ECREQUEST.PropertyCode),
                                              ECREQUEST.RegistrationNoNumber, "N", Dosc.DocSummary, i, "C", Convert.ToInt64(KAVERIDOC_RESPONSE_ROWID), 0, "", 0, ECREQUEST.LoginId);
                                        }
                                    }
                                    return Ok(new { success = true, data = Dosc, ECDataExists = DoesExist,RequestId = ReqId });
                                }
                            }
                            else
                            {
                                return Ok(new { success = true, ECDataExists = DoesExist });
                            }
                        }

                        else
                        {
                            if (fromDate != "" && toDate != "")
                            {
                                return Ok(new { success = false, message = "The EC number has expired. EC should be obtained from 01.04.2004 onwards only and EC should not be older than 7 days .Submitted EC Dates are from date-" + fromDate.Substring(0, 10) + ", to date-" + toDate.Substring(0, 10) });

                            }
                            else
                            {
                                return Ok(new { success = false, message = "The EC number has expired. EC should be obtained from 01.04.2004 onwards only and EC should not be older than 7 days.Submitted EC Dates are.Submitted EC Dates are from date-" + fromDate + ",to date-" + toDate });


                            }


                        }
                    }
                    else
                    {
                        return Ok(new { success = false, message = $"Kaveri EC Details API returned bad response: {responseMessage}" });

                    }

                }
                return Ok(new { success = false, message = $"Kaveri EC Details API returned bad response: {respStat}" });

            }
            catch (Exception ex)
            {

                _errorLogService.LogError(ex, "GetKaveriECData");
                
                throw (ex);

            }
        }
        private ECDataDescription ParseDescription(List<string> descriptions)
        {
            var parsedDescription = new ECDataDescription();

            foreach (var description in descriptions)
            {
                var lines = description.Split('\n');

                foreach (var line in lines)
                {
                    var parts = line.Split(':');

                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();

                        switch (key)
                        {
                            case "DISTRICT":
                                parsedDescription.District = value;
                                break;
                            case "TALUKA":
                                parsedDescription.Taluka = value;
                                break;
                            case "HOBLI/TOWN":
                                parsedDescription.HobliOrTown = value;
                                break;
                            case "INDEX II: VILLAGE":
                                parsedDescription.Village = value;
                                break;
                            case "Survey No":
                                parsedDescription.SurveyNo = value;
                                break;
                        }
                    }
                }
            }

            return parsedDescription;
        }
        [HttpGet("GET_KAVERI_UPLOAD_DETAILS")]
        public ActionResult<DataSet> GET_KAVERI_UPLOAD_DETAILS(long ReqId, long Propertycode, long BOOKS_APP_NO)
        {
            _logger.LogInformation("GET request received at GET_KAVERI_UPLOAD_DETAILS");
            try
            {
                var dataSet = obj.GET_KAVERI_UPLOAD_DETAILS(Propertycode,ReqId,  BOOKS_APP_NO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_KAVERI_UPLOAD_DETAILS");
                _logger.LogError(ex, "Error occurred while executing stored procedure GET_KAVERI_UPLOAD_DETAILS.");
                throw;
            }
        }
       
        [HttpPost("INS_KAVERI_API_ECDOC_SUBMIT")]
        public ActionResult<DataSet> INS_KAVERI_API_ECDOC_SUBMIT(ECDocumentSave ecDcoument)
        {
            _logger.LogInformation("GET request received at INS_KAVERI_API_ECDOC_SUBMIT");
            try
            {
                var dataSet = obj.INS_KAVERI_API_ECDOC_SUBMIT(ecDcoument.Propertycode, ecDcoument.ReqId, ecDcoument.KaveriECDoc , ecDcoument.LoginId, ecDcoument.KaveriDocName);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_KAVERI_API_ECDOC_SUBMIT");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_KAVERI_API_ECDOC_SUBMIT.");
                throw;
            }
        }
        public class ECDocumentSave
        {
            public long Propertycode { get; set; }
            public long ReqId { get; set; }
            public byte[] KaveriECDoc { get; set; }
            public string KaveriDocName { get; set; }
            public string LoginId { get; set; }
        }

    }
}
