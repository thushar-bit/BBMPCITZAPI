using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUPMS_BA;
using System.Data;
using System.Text;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/Bescom")]
    [ApiController]
    public class BescomController : ControllerBase
    {

        private readonly ILogger<BBMPCITZController> _logger;
       
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly INameMatchingService _nameMatchingService;
        private readonly BescomSettings _BescomSettings;
        //   private readonly ICacheService _cacheService;

        public BescomController(ILogger<BBMPCITZController> logger,DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IOptions<BescomSettings> BescomSettings)
        {
            _logger = logger;
           
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _BescomSettings = BescomSettings.Value;
        
          
        }
        NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();


        [HttpPost("GetBescomData")]
        public async Task<IActionResult> GetBescomData(long BOOKS_PROP_APPNO, long propertycode,long BescomAccountNumber,string LoginId,int propertytype
            ,string FloorNumber)
        {
            string jsonPayload = "";
            string APIResponseStatus = "", APIResponse = "";
            Int64 transactionNo = 0;
            string json = "No Bescom Details Found";
            bool isResponseStored = false;
            try
            {


                jsonPayload = "{\"escomName\": \"1\", \"accountId\": \"" + BescomAccountNumber.ToString().Trim() + "\"}";

                string apiUrl = _BescomSettings.BESCOMAPIURL;
                string username = _BescomSettings.BESCOMAPIUsername;
                string password = _BescomSettings.BESCOMAPIPassword;
                string authInfo = "" + username + ":" + password + "";
                string encodedAuthInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

                transactionNo = obj.INSERT_BESCOM_API_REQUEST_DATA(Convert.ToInt64(BOOKS_PROP_APPNO), Convert.ToInt64(propertycode), BescomAccountNumber.ToString().Trim(), jsonPayload, Convert.ToString(LoginId));
                using (HttpClient client = new HttpClient())
                {
                    // Set the Authorization header for basic authentication
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedAuthInfo);

                    // Prepare the HTTP request content (JSON payload)
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request to the API endpoint with JSON payload
                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    // Check if the response is successful (status code 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content as string
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        APIResponse = responseBody;
                        APIResponseStatus = "SUCCESS";

                        string BESCOM_RESPONSE_ROWID = obj.INSERT_BESCOM_API_RESPONSE_DATA(transactionNo, APIResponseStatus, APIResponse);
                        isResponseStored = true;

                        JObject Obj_Json = JObject.Parse(responseBody);
                        string requestStatus = (string)Obj_Json.SelectToken("requestStatus");
                        if (requestStatus == "000")
                        {
                            string consumerName = (string)Obj_Json.SelectToken("consumerName");
                            string accountId = (string)Obj_Json.SelectToken("accountId");
                            string escomName = (string)Obj_Json.SelectToken("escomName");
                            string rrNo = (string)Obj_Json.SelectToken("rrNo");
                            string natureBusiness = (string)Obj_Json.SelectToken("natureBusiness");
                            string mobileNo = (string)Obj_Json.SelectToken("mobileNo");
                            string email = (string)Obj_Json.SelectToken("email");
                            string address = (string)Obj_Json.SelectToken("address");
                            DataSet responseNCL = obj.INS_NCL_PROPERTY_BESCOM_TEMP(Convert.ToInt64(BOOKS_PROP_APPNO), Convert.ToInt64(propertycode), Convert.ToInt32(propertytype), Convert.ToInt32(FloorNumber) , Convert.ToString(BescomAccountNumber), consumerName, escomName, rrNo, natureBusiness, mobileNo, email, address, BESCOM_RESPONSE_ROWID, Convert.ToString(LoginId));
                             json = JsonConvert.SerializeObject(responseNCL, Formatting.Indented);
                          
                            return Ok(json);
                        }
                        return Ok(json);
                    }
                    return Ok(json);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
    }
}
