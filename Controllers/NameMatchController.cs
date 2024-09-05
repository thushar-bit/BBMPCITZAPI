using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;

namespace BBMPCITZAPI.Controllers
{
    [Route("Name_Match")]
    [ApiController]
    public class NameMatchController : ControllerBase
    {

        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly INameMatchingService _nameMatchingService;
        private readonly PropertyDetails _PropertyDetails;
        //   private readonly ICacheService _cacheService;

        public NameMatchController(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IOptions<PropertyDetails> PropertyDetails,
             INameMatchingService NameMatching)
        // ICacheService cacheService)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _PropertyDetails = PropertyDetails.Value;
            _nameMatchingService = NameMatching;
            //  _cacheService = cacheService;
        }
        NUPMS_BA.ObjectionModuleBA obj = new NUPMS_BA.ObjectionModuleBA();
    

        [HttpGet("GET_BBD_NCL_OWNER_BYEKYCTRANSACTION")]
        public ActionResult<int> GET_BBD_NCL_OWNER_BYEKYCTRANSACTION(long transactionNumber,string OwnerType)
        {
            try
            {
                int NameMatchScore = 123;
                if (OwnerType != "NEWOWNER")
                {
                    DataSet dsOwnerDetails = obj.GET_BBD_NCL_OWNER_BYEKYCTRANSACTION(transactionNumber);
                    if (dsOwnerDetails != null && dsOwnerDetails.Tables.Count > 0 && dsOwnerDetails.Tables[0].Rows.Count > 0)
                    {

                        NameMatchScore = _nameMatchingService.CallNameMatchAPI(Convert.ToString(dsOwnerDetails.Tables[0].Rows[0]["BBDOWNERNAME"]), Convert.ToString(dsOwnerDetails.Tables[0].Rows[0]["OWNERNAME_EN"]));
                        obj.GET_NCL_PROP_OWNER_TEMP_BYEKYCTRANSACTION(transactionNumber, NameMatchScore);
                        string json1 = JsonConvert.SerializeObject(NameMatchScore, Newtonsoft.Json.Formatting.Indented);

                        return Ok(json1);
                    }
                }
                obj.GET_NCL_PROP_OWNER_TEMP_BYEKYCTRANSACTION(transactionNumber, NameMatchScore);
                string json = JsonConvert.SerializeObject(NameMatchScore, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.");
                throw;
            }
        }
    }
}
