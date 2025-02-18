using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUPMS_BO;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/AmalgamationAPI")]
    [ApiController]
  //  [Authorize]
    public class AmalgamationController : ControllerBase
    {
        private readonly ILogger<EKYCController> _logger;
        private readonly KaveriSettings _kaveriSettings;
        private readonly INameMatchingService _nameMatchingService;
        private readonly IAmalgamationService _AmalgamationService;
        private readonly IErrorLogService _errorLogService;

        public AmalgamationController(ILogger<EKYCController> logger, IOptions<KaveriSettings> kaveriSettings,
            INameMatchingService NameMatching, IAmalgamationService AmalgamationService, IErrorLogService errorLogService)
        {
            _logger = logger;
            _kaveriSettings = kaveriSettings.Value;
            _nameMatchingService = NameMatching;
            _AmalgamationService = AmalgamationService;
            _errorLogService = errorLogService;

        }
        [HttpPost("GetAmalgamationProperty")]   
        public ActionResult<DataSet> GetAmalgamationProperties(string[] PropertyId)
        {
            _logger.LogInformation("GET request received at GetAmalgamationProperty");
            try
            {

                var dataSet = _AmalgamationService.GetAmalgamationProperties(PropertyId);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetAmalgamationProperty");
                _logger.LogError(ex, "Error occurred while executing stored procedure GetAmalgamationProperty.");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string loginId, string EMAIL, EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA");
            try
            {

                var dataSet = _AmalgamationService.INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA(MOBILENUMBER, MOBILEVERIFY, EMAIL, loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA.");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(string[] propertyId)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT");
            try
            {

                var dataSet = _AmalgamationService.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(propertyId);
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
