using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUPMS_BA;
using System.Data;
using static BBMPCITZAPI.Models.KaveriData;

namespace BBMPCITZAPI.Controllers
{
    [Route("v1/Name_Match")]
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
        public ActionResult<DataSet> GET_BBD_NCL_OWNER_BYEKYCTRANSACTION(long transactionNumber,string OwnerType)
        {
            try
            {
              
                DataSet dsOwnerData = obj.GET_NCL_PROP_OWNER_TEMP_BYEKYCTRANSACTION(transactionNumber);
                if (dsOwnerData != null && dsOwnerData.Tables.Count > 0 && dsOwnerData.Tables[1].Rows.Count > 0)
                    {
                        string json1 = JsonConvert.SerializeObject(dsOwnerData.Tables[1], Newtonsoft.Json.Formatting.Indented);
                    List<EKYCResponse> ekycResponse = JsonConvert.DeserializeObject<List<EKYCResponse>>(json1);

                   
                    string apiDataInput = ekycResponse[0].APIDATAINPUT;
                    var s = obj.ParseEKYCResponse(apiDataInput);
                        return Ok(s);
                    }
               
           
                string json = JsonConvert.SerializeObject(dsOwnerData, Newtonsoft.Json.Formatting.Indented);
               
                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_BBD_NCL_OWNER_BYEKYCTRANSACTION");
                throw;
            }
        }
        [HttpPost("SASNameMatch")]
        public ActionResult<int> Get_SAS_Name_match(long propertyCode, long BOOK_APP_NO, string LoginId, List<KaveriData.ekycdata> ekycdatas)
        {
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(555, BOOK_APP_NO, propertyCode, "ADDRESS");

                Dictionary<int, string> dicEkycOwners = new Dictionary<int, string>();  
                foreach (var ekyc in ekycdatas)
                {
                    dicEkycOwners.Add(ekyc.OwnerNumber, ekyc.OwnerName);
                }
                int ownerNumber = 1;
                Dictionary<int, string> dicSASOwners = new Dictionary<int, string>();

                string sasOwnerName = dataSet.Tables[0].Rows[0]["OWNERNAME"].ToString();
                string SasApplicationNumber = dataSet.Tables[0].Rows[0]["APPLICATIONNUMBER"].ToString(); 
                dicSASOwners.Add(ownerNumber, sasOwnerName);
                List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
                objFinalListNameMatchingResult = _nameMatchingService.CompareDictionaries(dicSASOwners, dicEkycOwners);
                foreach (var i in objFinalListNameMatchingResult)
                {
                    obj.INS_NCL_PROPERTY_SAS_APP_NAMEMATCH_TEMP(SasApplicationNumber, BOOK_APP_NO, propertyCode, sasOwnerName, i.OwnerNo, i.OwnerName, i.NameMatchScore, LoginId);
                }
                return 09;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("KaveriNameMatch")]
        public ActionResult<int> Get_Kaveri_Name_match(long propertyCode, long BOOK_APP_NO, string LoginId, List<KaveriData.ekycdata> ekycdatas)
        {
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(555, BOOK_APP_NO, propertyCode, "KAVERI_DETAILS");

                Dictionary<int, string> dicEkycOwners = new Dictionary<int, string>();
                foreach (var ekyc in ekycdatas)
                {
                    dicEkycOwners.Add(ekyc.OwnerNumber, ekyc.OwnerName);
                }
                int ownerNumber = 1;
                Dictionary<int, string> dicSASOwners = new Dictionary<int, string>();

                string sasOwnerName = dataSet.Tables[0].Rows[0]["OWNERNAME"].ToString();
                string SasApplicationNumber = dataSet.Tables[0].Rows[0]["APPLICATIONNUMBER"].ToString();
                dicSASOwners.Add(ownerNumber, sasOwnerName);
                List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
                objFinalListNameMatchingResult = _nameMatchingService.CompareDictionaries(dicSASOwners, dicEkycOwners);
                foreach (var i in objFinalListNameMatchingResult)
                {
                    obj.INS_NCL_PROPERTY_SAS_APP_NAMEMATCH_TEMP(SasApplicationNumber, BOOK_APP_NO, propertyCode, sasOwnerName, i.OwnerNo, i.OwnerName, i.NameMatchScore, LoginId);
                }
                return 09;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
