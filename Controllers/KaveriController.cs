using BBMPCITZAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/KaveriAPI")]
    [ApiController]
   // [Authorize]
    public class KaveriController : ControllerBase
    {

        private readonly ILogger<EKYCController> _logger;
        private readonly KaveriSettings _kaveriSettings;

        public KaveriController(ILogger<EKYCController> logger, IOptions<KaveriSettings> kaveriSettings
             )
        {
            _logger = logger;
            _kaveriSettings = kaveriSettings.Value;

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


        private HttpResponseMessage KaveriAPIRequest(string urlKeyWord, string RegistrationNoECNumber, string BOOKS_APP_NO, string PropertyCode, string LoginId)
        {
            Int64 transactionNo = 0;
            //   ViewState["Kaveri_TransactionNo"] = transactionNo;
            string Json = "";
            string rsaKeyDetails = "<RSAKeyValue><Modulus>" + Convert.ToString(_kaveriSettings.KaveriPublicKey) + " </Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            Uri requestUri = new Uri(_kaveriSettings.KaveriECAPI);

            HttpClient client1 = new HttpClient();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) => { return true; };

            if (urlKeyWord == "KaveriDocDetailsAPI")
            {
                requestUri = new Uri(_kaveriSettings.KaveriDocDetailsAPI);
                //Json = "{\r\n  \"username\": \"" + username + "\",\r\n  \"password\": \"" + password + "\",\r\n  \"finalRegNumber\": \"" + RegistrationNo + "\"\r\n}";
                Json = "{\r\n \"apikey\":\"1\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"finalRegNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
                transactionNo = obj.INS_KAVERI_API_DOCUMENT_REQUEST(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoECNumber, Json, Convert.ToString(LoginId));
            }
            else if (urlKeyWord == "KaveriECDocAPI")
            {
                requestUri = new Uri(_kaveriSettings.KaveriECDocAPI);
                Json = "{\r\n \"apikey\":\"1\",\r\n  \"username\": \"" + Encrypt(_kaveriSettings.KaveriUsername.ToString(), rsaKeyDetails) + "\",\r\n  \"password\": \"" + Encrypt(_kaveriSettings.KaveriPassword.ToString(), rsaKeyDetails) + "\",\r\n  \"certificateNumber\": \"" + Encrypt(RegistrationNoECNumber, rsaKeyDetails) + "\"}";
             //   Json = "{\"apikey\": \"1\",\"username\": \"StazL1fAkoRt+o7I01iekrPbHaTQ32wBkAtrULKQ1otSv3DcbI0DLMBI63xevCyYSp3zLNonRI+bE5Q0W7k2unQvfCl0EpK1SmEF33El1ACe44nQbwfiIc5L2CTL8zgeQR0rc1CyTkirEVGlVlr8nrSGd8W5ACVNS12aj4vsdrc=\",\"password\": \"kzpJ98Kio4FNocARzdqSLu7lQhEBQ1fcf4AHYTC2I5UC+/e0VJPEVv+pnV17DWBAJXIMJY7ybPvRJ7Z+Eggm2uSL2/aWN+K9Jo19YiWq8pTzOpg7vFygPdYgIVPc9qdhHoBovpzQp6GvjI3n85BmqxlIc8peBtKyNjYd4HMk6+Y=\",\"certificateNumber\": \"d+BB+O9L/4lW0de9+t4LAZ42/3CtPpHKSyZMA5k0OkEjFciQhCnwAO0NHNC6dJWD3jGzXlWmYbdVJnbNfdZ5QM4PbMR50CudjelEATRTvD9eB2A0tphnX1x5k4J+RmBJxUmsfNTCKzRVpWTaOAYWozbeqf2sSbDMJXMK543LfEo=\"}";
                transactionNo = obj.INS_KAVERI_API_ECDOC_REQUEST(Convert.ToInt64(BOOKS_APP_NO), Convert.ToInt64(PropertyCode), RegistrationNoECNumber, Json, Convert.ToString(LoginId));
            }
            // ViewState["Kaveri_TransactionNo"] = transactionNo;

            var content2 = new StringContent(Json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = client1.PostAsync(requestUri, content2).Result; //Request for Deed download

            return httpResponse;
        }
        [HttpGet("GetKaveriDocData")]
        public IActionResult GetKaveriDocData( string RegistrationNoNumber, string BOOKS_APP_NO, string PropertyCode, string LoginId)
        {
            string APIResponse = "", APIResponseStatus = "";
            bool isResponseStored = false;
            try
            {

                //RegistrationNumber = "NMG-1-00224-2023-24";

                HttpResponseMessage httpResponse = KaveriAPIRequest("KaveriDocDetailsAPI", RegistrationNoNumber, BOOKS_APP_NO, PropertyCode, LoginId);
                var respornseContent = httpResponse.Content.ReadAsStringAsync().Result;
                APIResponse = respornseContent;
                string respStat = httpResponse.StatusCode.ToString();

                if (respStat == "OK")
                {
                    APIResponseStatus = "SUCCESS";

                    string KAVERIDOC_RESPONSE_ROWID = obj.INS_KAVERI_API_DOCUMENT_RESPONSE(Convert.ToInt64(1), APIResponseStatus, APIResponse);
                    isResponseStored = true;
                    var response = JsonConvert.DeserializeObject<List<KaveriData.KAVERI_API_DOC_DETAILS_RESPONSE>>(respornseContent);
                    var documentDetailsList = new List<KaveriData.DocumentDetails>();
                    foreach (var responsevar in response)
                    {
                        if (responsevar.responseCode == "1000")
                        {
                            if (!string.IsNullOrEmpty(responsevar.json))
                            {
                                var documentDetails = JsonConvert.DeserializeObject<KaveriData.DocumentDetails>(responsevar.json);
                                documentDetailsList.Add(documentDetails);
                            }
                        } else
                        {
                            return Ok(new { success = false, message = $"Kaveri Doc Details API returned bad response: {responsevar.responseCode}, {responsevar.responseMessage}" });
                        }
                    }
                    return Ok(new { success = true, data = documentDetailsList });
                }
                return Ok(new { success = false, message = $"Kaveri Doc Details API returned bad response: {respStat}" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetKaveriECData")]
        public IActionResult GetKaveriECData( string ECNumber, string BOOKS_APP_NO, string PropertyCode, string LoginId)
        {
            string APIResponse = "", APIResponseStatus = "";
            bool isResponseStored = false;
            try
            {

                //RegistrationNumber = "NMG-1-00224-2023-24";

                HttpResponseMessage httpResponse = KaveriAPIRequest("KaveriECDocAPI", ECNumber, BOOKS_APP_NO, PropertyCode, LoginId);
                var respornseContent = httpResponse.Content.ReadAsStringAsync().Result;
                APIResponse = respornseContent;
                string respStat = httpResponse.StatusCode.ToString();

                if (respStat == "OK")
                {
                    JObject Obj_Json = JObject.Parse(respornseContent.Replace("[", "").Replace("]", ""));
                    string responseCode = (string)Obj_Json.SelectToken("responseCode");
                    APIResponseStatus = "SUCCESS";

                    string KAVERIDOC_RESPONSE_ROWID = obj.INS_KAVERI_API_ECDOC_RESPONSE(Convert.ToInt64(1), APIResponseStatus, APIResponse);
                    isResponseStored = true;
                    string responseMessage = (string)Obj_Json.SelectToken("responseMessage");
                    if (responseMessage == "Sucess")
                    {
                        string base64String = (string)Obj_Json.SelectToken("base64");
                        return Ok(base64String);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
