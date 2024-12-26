using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Newtonsoft.Json;
using NUPMS_BA;
using NUPMS_BO;
using Org.BouncyCastle.Tls;
using System.Data;
using System.Data.Common;
using static BBMPCITZAPI.Models.KaveriData;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/SearchAPI")]
    [ApiController]

    public class SearchController : ControllerBase
    {

        private readonly ILogger<EKYCController> _logger;
        private readonly KaveriSettings _kaveriSettings;
        private readonly INameMatchingService _nameMatchingService;
        private readonly ISearchService _SearchService;
        private readonly IErrorLogService _errorLogService;

        public SearchController(ILogger<EKYCController> logger, IOptions<KaveriSettings> kaveriSettings,
            INameMatchingService NameMatching, ISearchService SearchService, IErrorLogService errorLogService)
        {
            _logger = logger;
            _kaveriSettings = kaveriSettings.Value;
            _nameMatchingService = NameMatching;
            _SearchService = SearchService;
            _errorLogService = errorLogService;

        }
        NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();


        [HttpPost("INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA( string MOBILENUMBER, string MOBILEVERIFY,  string loginId, string EMAIL,  EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at GET_WARD_BY_WARDNUMBER");
            try
            {

                var dataSet = _SearchService.INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA( MOBILENUMBER, MOBILEVERIFY,  EMAIL,  loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA.");
                throw;
            }
        }

        [HttpPost("INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT(INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT final)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT");
            try
            {

                var dataSet = _SearchService.INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT(final);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT.");
                throw;
            }
        }

        [HttpGet("SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH")]
        public ActionResult<DataSet> SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH(string ApplicationNo)
        {
            _logger.LogInformation("GET request received at SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH");
            try
            {

                var dataSet = _SearchService.SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH(ApplicationNo);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH");
                _logger.LogError(ex, "Error occurred while executing stored procedure SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH.");
                throw;
            }
        }
    }
}