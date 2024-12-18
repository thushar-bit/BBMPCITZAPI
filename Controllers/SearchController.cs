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
    [Route("v1/ObjectionAPI")]
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
        public ActionResult<DataSet> INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE, string loginId, string EMAIL,  EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at GET_WARD_BY_WARDNUMBER");
            try
            {

                var dataSet = _SearchService.INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA(IDENTIFIERTYPE, IdentifierName, MOBILENUMBER, MOBILEVERIFY, NAMEMATCHSCORE, EMAIL,  loginId, objEKYCDetailsBO);
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


    }
}