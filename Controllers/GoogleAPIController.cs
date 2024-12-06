
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Google.Apis.Auth.OAuth2;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/GoogleAPI")]
    [ApiController]
    public class GoogleAPIController : ControllerBase
    {
        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IErrorLogService _errorLogService;
        public GoogleAPIController(ILogger<BBMPCITZController> logger,  IOptions<BescomSettings> BescomSettings, IErrorLogService errorLogService)
        {
            _logger = logger;
            _errorLogService = errorLogService;

        }
        private static readonly String TRANSLATE_API_URL =  "https://translation.googleapis.com/v3/projects/812417225443:translateText";
        private async Task<string> GetAccessTokenAsync()
        {
            try
            {
                // Load the service account JSON key
                using var stream = new FileStream("translation.json", FileMode.Open, FileAccess.Read);
                var credential = GoogleCredential.FromStream(stream)
                    .CreateScoped("https://www.googleapis.com/auth/cloud-platform");

                var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
                return token;
            }
            catch(Exception ex)
            {
                _errorLogService.LogError(ex, "GoogleAPITransalation GetAccessTokenAsync");
                throw;
            }
        }
        [HttpPost("GetGoogleApiData")]
        public async Task<string> GoogleAPITransalation(string sourceLanguage, string targetLanguage, IEnumerable<string> contents)
        {
            try
            {
                using var httpClient = new HttpClient();

                // Build the request payload
                var payload = new
                {
                    sourceLanguageCode = sourceLanguage,
                    targetLanguageCode = targetLanguage,
                    contents = contents
                };

                var requestContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                // Set the authorization header
                var accessToken = await GetAccessTokenAsync();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                // Make the POST request
                var response = await httpClient.PostAsync(TRANSLATE_API_URL, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Translation API call failed: {response.ReasonPhrase}");
                }

                // Parse the response
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GoogleAPITransalation");
                throw;
            }
        }
    }
}
