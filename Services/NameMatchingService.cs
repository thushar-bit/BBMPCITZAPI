
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Controllers;
using Microsoft.Extensions.Options;
using BBMPCITZAPI.Services.Interfaces;

namespace NUPMS_BA
{
    public class NameMatchingService : INameMatchingService
    {
        private readonly ILogger<BBMPCITZController> _logger;
        private readonly PropertyDetails _PropertyDetails;
        public NameMatchingService(ILogger<BBMPCITZController> logger, IConfiguration configuration, IOptions<PropertyDetails> propertyDetails)
        {
            _logger = logger;
            _PropertyDetails = propertyDetails.Value;
           
        }

        public List<NameMatchingResult> CompareDictionaries(Dictionary<int, string> srcDic, Dictionary<int, string> ekycDic)
        {
            List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
            List<int> alreadyUsedOwnersList = new List<int>();

            foreach (KeyValuePair<int, string> kvpSrcDic in srcDic)
            {
                List<NameMatchingResult> objListNameMatchingResult = new List<NameMatchingResult>();

                foreach (KeyValuePair<int, string> kvpEkycDic in ekycDic)
                {
                    if (!alreadyUsedOwnersList.Contains(kvpEkycDic.Key))//Escaping already macthed owner
                    {
                        NameMatchingResult objNameMatchingResult = new NameMatchingResult();
                        objNameMatchingResult.OwnerNo = kvpSrcDic.Key;
                        objNameMatchingResult.OwnerName = kvpSrcDic.Value;
                        objNameMatchingResult.EKYCOwnerNo = kvpEkycDic.Key;
                        objNameMatchingResult.EKYCOwnerName = kvpEkycDic.Value;
                        objNameMatchingResult.NameMatchScore = CallNameMatchAPI(kvpSrcDic.Value, kvpEkycDic.Value);
                        objListNameMatchingResult.Add(objNameMatchingResult);
                    }
                }

                if (objListNameMatchingResult.Count > 0)
                {
                    //Sorting based on macthed score to select the best matched owner
                    List<NameMatchingResult> sortedListNameMatchingResult = objListNameMatchingResult.OrderByDescending(p => p.NameMatchScore).ToList();

                    objFinalListNameMatchingResult.Add(sortedListNameMatchingResult[0]); //Adding best matched owner to final list
                    alreadyUsedOwnersList.Add(sortedListNameMatchingResult[0].EKYCOwnerNo);

                    objListNameMatchingResult.Clear();
                    sortedListNameMatchingResult.Clear();
                }
            }

            return objFinalListNameMatchingResult;
        }

        public  int CallNameMatchAPI(string name1, string name2)
        {
            int nameMatchScore = 0;
            string APIResponseStatus = "", APIResponse = "", jsonPayload = "";
            try
            {
               // string apiUrl = ConfigurationManager.AppSettings["NameMatchAPI"];
                string apiUrl = _PropertyDetails.NameMatchURL;
                //var parameters = new Dictionary<string, string> { { "name1", "" + name1 + "" }, { "name2", "" + name2 + "" } };
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                using (HttpClient client = new HttpClient())
                {
                    jsonPayload = "{\"name1\": \"" + name1 + "\", \"name2\": \"" + name2 + "\"}";
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

                    // Check if the response is successful (status code 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content as string
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        APIResponse = responseBody;

                        JObject Obj_Json = JObject.Parse(responseBody);
                        string requestStatus = (string)Obj_Json.SelectToken("Status");
                        if (requestStatus == "Success")
                        {
                            APIResponseStatus = "SUCCESS";
                            nameMatchScore = (int)Obj_Json.SelectToken("Message");
                        }
                        else
                        {
                            string errormessage = (string)Obj_Json.SelectToken("Message");
                            APIResponseStatus = "FAIL" + errormessage;
                        }
                    }
                    else
                    {
                        APIResponse = Convert.ToString(response);
                        APIResponseStatus = "FAIL" + response.StatusCode;
                    }
                }
                return nameMatchScore;
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

    }

    public class NameMatchingResult
    {
        public int OwnerNo { get; set; }

        public string OwnerName { get; set; }

        public int EKYCOwnerNo { get; set; }

        public string EKYCOwnerName { get; set; }

        public int NameMatchScore { get; set; }
    }

}
