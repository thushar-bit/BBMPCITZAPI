using Microsoft.AspNetCore.Mvc;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Database;
using System.Data;
using BBMPCITZAPI.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NUPMS_BO;
using NUPMS_BA;

namespace BBMPCITZAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("v1/BBMPCITZAPI")]
    public class BBMPCITZController : ControllerBase
    {
        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly IErrorLogService _errorLogService;
        private readonly PropertyDetails _PropertyDetails;
        //   private readonly ICacheService _cacheService;

        public BBMPCITZController(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IOptions<PropertyDetails> PropertyDetails,
IErrorLogService errorLogService
         )
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _PropertyDetails = PropertyDetails.Value;
            _errorLogService = errorLogService;
            //  _cacheService = cacheService;
        }
        NUPMS_BA.ObjectionModuleBA objModule = new NUPMS_BA.ObjectionModuleBA();
        NUPMS_BA.Objection_BA objBa = new NUPMS_BA.Objection_BA();
        NUPMS_BA.GetReportData NUMPSdata = new NUPMS_BA.GetReportData();
        NUPMS_BA.BBD_BA BBDBA = new NUPMS_BA.BBD_BA();
       
        //   NUPMS_BA.CitizenBA ciTz= new NUPMS_BA.CitizenBA();
        #region Initial


        [HttpGet("GetMasterZone")]
        public ActionResult<DataSet> GetMasterZone()
        {
            _logger.LogInformation("GET request received at GetMasterZone");
            try
            {
                var dataSet = BBDBA.GetZone();
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetMasterZone");
                _logger.LogError(ex, "Error occurred while executing stored procedure GetMasterZone.");
                throw;
            }
        }
        [HttpGet("GetMasterWard")]
        public ActionResult<DataSet> GetMasterWard(int ZoneId)
        {
            _logger.LogInformation("GET request received at GetMasterWard");
            try
            {
                var dataSet = BBDBA.GetWardByZone(ZoneId);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetMasterWard");
                _logger.LogError(ex, "Error occurred while executing stored procedure GetMasterWard.");
                throw;
            }
        }
        [HttpGet("GetWardCordinates")]
        public ActionResult<DataSet> GetWardCordinates(int wardNumber)
        {
            _logger.LogInformation("GET request received at GetWardCordinates");
            try
            {

                var dataSet = objModule.GetWardCoordinates(wardNumber);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetWardCordinates");
                _logger.LogError(ex, "Error occurred while executing stored procedure GetWardCordinates.");
                throw;
            }
        }

        [HttpGet("LOAD_BBD_RECORDS")]
        public ActionResult<DataSet> LOAD_BBD_RECORDS(int ZoneId, int WardId, int SerachType, string Search)
        {
            _logger.LogInformation("GET request received at LOAD_BBD_RECORDS_BY_WARD");
            try
            {

                var dataSet = objModule.GET_EKHATA_BBD_RECORDS(ZoneId, WardId, SerachType, Search);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "LOAD_BBD_RECORDS");
                _logger.LogError(ex, "Error occurred while executing stored procedure LOAD_BBD_RECORDS.");
                throw;
            }
        }
        [HttpGet("GET_PROPERTY_PENDING_CITZ_BBD_DRAFT")]
        public IActionResult GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(int UlbCode, int propertyid)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_BBD_DRAFT");
            try
            {
                //  string cacheKey = "BBD_DRAFT_KEY" + propertyid;
                var dataSet = objModule.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(UlbCode, propertyid);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                //  await _cacheService.SetCacheDataAsync(cacheKey, json);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_BBD_DRAFT");
                _logger.LogError(ex, "Error occurred while executing stored procedure GET_PROPERTY_PENDING_CITZ_BBD_DRAFT.");
                throw;
            }
        }
        [HttpGet("GET_PROPERTY_BBD_Draft_Generated_Wards")]
        public IActionResult GET_PROPERTY_BBD_Draft_Generated_Wards()
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_BBD_Draft_Generated_Wards");
            try
            {
                //  string cacheKey = "BBD_DRAFT_KEY" + propertyid;
                var dataSet = BBDBA.GetDraftGeneratedWard();
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                //  await _cacheService.SetCacheDataAsync(cacheKey, json);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_BBD_Draft_Generated_Wards");
                _logger.LogError(ex, "Error occurred while executing stored procedure GET_PROPERTY_BBD_Draft_Generated_Wards.");
                throw;
            }
        }

        //[HttpGet("GetBBDRedisData")]
        //public async Task<IActionResult> GetBBDRedisDeta(int propertyid)
        //{
        //    _logger.LogInformation("GET request received at GetBBDRedisData");
        //    try
        //    {
        //        string cacheKey = "BBD_DRAFT_KEY" + propertyid;
        //        var cachedData = await _cacheService.GetCachedDataAsync<string>(cacheKey);
        //        return Ok(cachedData);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while executing stored procedure GetBBDRedisData.");
        //        throw;
        //    }
        //}


        [HttpGet("GetMasterTablesData")]
        public ActionResult<DataSet> GetMasterTablesData(string UlbCode)
        {
            _logger.LogInformation("GET request received at GetMasterTablesData");
            try
            {
                var dataSet = objModule.GetMasterTablesData(UlbCode);

                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetMasterTablesData");
                _logger.LogError(ex, "Error occurred while executing stored procedure. GetMasterTablesData");
                throw;
            }
        }
        [HttpGet("GetMasterTablesData_React")]
        public ActionResult<DataSet> GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(string UlbCode, string Page)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetMasterTablesData_React(UlbCode, Page);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetMasterTablesData_React");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                throw;
            }
        }
        [HttpGet("GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React")]
        public ActionResult<DataSet> GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React(string UlbCode, long propertyCode, string Page)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React(UlbCode, propertyCode, Page);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                throw;
            }
        }
        [HttpGet("GET_PROPERTY_PENDING_CITZ_NCLTEMP_React")]
        public ActionResult<DataSet> GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(int ULBCODE, long P_BOOKS_PROP_APPNO, long propertyCode, string Page)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(ULBCODE, P_BOOKS_PROP_APPNO, propertyCode, Page);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                throw;
            }
        }


        [HttpGet("GET_PROPERTY_PENDING_CITZ_NCLTEMP")]
        public async Task<IActionResult> GET_PROPERTY_PENDING_CITZ_NCLTEMP(int ULBCODE, int P_BOOKS_PROP_APPNO, int Propertycode)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_NCLTEMP");
            try
            {
                //  string cacheKey = "NCL_TEMP_KEY" + Propertycode + P_BOOKS_PROP_APPNO;
                DataSet dataSet = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP(ULBCODE, P_BOOKS_PROP_APPNO, Propertycode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                //    await _cacheService.SetCacheDataAsync(cacheKey, json);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_NCLTEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_PROPERTY_PENDING_CITZ_NCLTEMP");
                throw;
            }
        }
        //[HttpGet("GetNCLRedisData")]
        //public async Task<IActionResult> GetNCLRedisData( int P_BOOKS_PROP_APPNO, int Propertycode)
        //{
        //    _logger.LogInformation("GET request received at GetNCLRedisData");
        //    try
        //    {
        //        string cacheKey = "NCL_TEMP_KEY" + Propertycode + P_BOOKS_PROP_APPNO;
        //        var cachedData = await _cacheService.GetCachedDataAsync<string>(cacheKey);
        //        return Ok(cachedData);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while executing stored procedure GetBBDRedisData.");
        //        throw;
        //    }
        //}
        [HttpPost("INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP")]
        public ActionResult<int> INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP(INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP insertCITZProperty)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_CTZ_PROPERTY");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP(insertCITZProperty); //change parameter
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP");
                throw;
            }
        }
        [HttpPost("INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2")]
        public ActionResult<int> INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2(INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2 insertCITZProperty)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_CTZ_PROPERTY");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2(insertCITZProperty); //change parameter
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_CTZ_PROPERTY");
                throw;
            }
        }

        [HttpGet("Get_Ctz_ObjectionModPendingAppl")]
        public ActionResult<string> Get_Ctz_ObjectionModPendingAppl(string LoginId, string propertycode, string propertyid)
        {
            _logger.LogInformation("GET request received at Get_Ctz_ObjectionModPendingAppl");
            try
            {
                //  string propertycode = _PropertyDetails.PROPERTYCODE;
                // string propertyid = _PropertyDetails.PROPERTYID;
                _logger.LogInformation(" Get_Ctz_ObjectionModPendingAppl" + "propertycode =" + propertycode + "propertyid = " + propertyid + "loginIDs= " + LoginId);
                DataSet dsProperties = objModule.Get_Ctz_ObjectionModPendingAppl("EID", LoginId, propertyid, "", 0, "");

                if ((dsProperties != null && dsProperties.Tables.Count > 0 && dsProperties.Tables[0].Rows.Count > 0))
                {
                    string P_BOOKS_PROP_APPNO = dsProperties.Tables[0].Rows[0]["BOOKS_PROP_APPNO"].ToString()!;
                    string PropertyId = dsProperties.Tables[0].Rows[0]["PROPERTYCODE"].ToString()!;
                    var data = new
                    {
                        P_BOOKS_PROP_APPNO = P_BOOKS_PROP_APPNO,
                        PropertyId = PropertyId
                    };
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                    return Ok(json);
                }
                else
                {
                    return "There is a issue while copying the data from Book Module.No Data Found";
                }

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "Get_Ctz_ObjectionModPendingAppl");
                _logger.LogError(ex, "Error occurred while executing stored procedure Get_Ctz_ObjectionModPendingAppl.");
                throw;
            }
        }
        [HttpGet("COPY_DATA_FROM_BBDDRAFT_NCLTEMP")]
        public ActionResult<string> COPY_DATA_FROM_BBDDRAFT_NCLTEMP(string LoginId, string propertycode, string propertyid)
        {
            _logger.LogInformation("GET request received at COPY_DATA_FROM_BBDDRAFT_NCLTEMP");
            try
            {
                //      string propertycode = _PropertyDetails.PROPERTYCODE;
                //    string propertyid = _PropertyDetails.PROPERTYID;
                _logger.LogInformation(" COPY_DATA_FROM_BBDDRAFT_NCLTEMP" + "propertycode =" + propertycode + "propertyid = " + propertyid + "loginIDs= " + LoginId);
                DataSet rowsEffected = objModule.COPY_DATA_FROM_BBDDRAFT_NCLTEMP(Convert.ToInt64(propertycode), Convert.ToString(LoginId));
                string json = JsonConvert.SerializeObject(rowsEffected, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "COPY_DATA_FROM_BBDDRAFT_NCLTEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure COPY_DATA_FROM_BBDDRAFT_NCLTEMP.");
                throw;
            }
        }
        #endregion
        #region AreaDimension
        [HttpPost("UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP")]
        public ActionResult<int> UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP(UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP)
        {
            _logger.LogInformation("post request received at UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP(UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP); //change parameter
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure. UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP");
                throw;
            }
        }
        [HttpPost("UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI")]
        public ActionResult<int> UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI(UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI)
        {
            _logger.LogInformation("post request received at UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI(UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI");
                _logger.LogError(ex, "Error occurred while executing stored procedure. UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI");
                throw;
            }
        }
        [HttpPost("UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA")]
        public ActionResult<int> UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA(UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA)
        {
            _logger.LogInformation("post request received at UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA(UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA); //change parmeter
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA");
                _logger.LogError(ex, "Error occurred while executing stored procedure. UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA");
                throw;
            }
        }

        #endregion
        #region Site Details Events
        [HttpPost("UPD_NCL_PROPERTY_SITE_TEMP_USAGE")]
        public ActionResult<int> UPD_NCL_PROPERTY_SITE_TEMP_USAGE(UPD_NCL_PROPERTY_SITE_TEMP_USAGE UPD_NCL_PROPERTY_SITE_TEMP_USAGE)
        {
            _logger.LogInformation("post request received at UPD_NCL_PROPERTY_SITE_TEMP_USAGE");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.UPD_NCL_PROPERTY_SITE_TEMP_USAGE(UPD_NCL_PROPERTY_SITE_TEMP_USAGE);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_NCL_PROPERTY_SITE_TEMP_USAGE");
                _logger.LogError(ex, "Error occurred while executing stored procedure. UPD_NCL_PROPERTY_SITE_TEMP_USAGE");
                throw;
            }
        }
        [HttpGet("GetNPMMasterTable")]
        public ActionResult<DataSet> GetNPMMasterTable(int FeaturesHeadID)
        {
            _logger.LogInformation("GET request received at GetNPMMasterTable");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetNPMMasterTable(FeaturesHeadID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetNPMMasterTable");
                _logger.LogError(ex, "Error occurred while executing stored procedure. GetNPMMasterTable");
                throw;
            }
        }
        #endregion
        #region Building Details Events
        [HttpPost("DEL_SEL_NCL_PROP_BUILDING_TEMP")]
        public ActionResult<DataSet> DEL_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, Models.NCLBuilding NCLBLDG)
        {
            _logger.LogInformation("post request received at DEL_SEL_NCL_PROP_BUILDING_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.DEL_SEL_NCL_PROP_BUILDING_TEMP(ULBCODE, NCLBLDG);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "DEL_SEL_NCL_PROP_BUILDING_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.DEL_SEL_NCL_PROP_BUILDING_TEMP");
                throw;
            }
        }
        [HttpPost("GET_NCL_FLOOR_AREA")]
        public ActionResult<DataSet> GET_NCL_FLOOR_AREA(Models.NCLBuilding NCLBLDG)
        {
            _logger.LogInformation("post request received at GET_NCL_FLOOR_AREA");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_NCL_FLOOR_AREA(NCLBLDG);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_NCL_FLOOR_AREA");
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_NCL_FLOOR_AREA");
                throw;
            }
        }
        [HttpPost("GET_NCL_TEMP_FLOOR_PRE")]
        public ActionResult<DataSet> GET_NCL_TEMP_FLOOR_PRE(Models.NCLBuilding NCLBLDG)
        {
            _logger.LogInformation("post request received at GET_NCL_TEMP_FLOOR_PRE");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_NCL_TEMP_FLOOR_PRE(NCLBLDG);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_NCL_TEMP_FLOOR_PRE");
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_NCL_TEMP_FLOOR_PRE");
                throw;
            }
        }
        [HttpPost("DEL_INS_SEL_NCL_PROP_BUILDING_TEMP")]
        public ActionResult<int> DEL_INS_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, Models.NCLBuilding NCLBLDG)
        {
            _logger.LogInformation("post request received at DEL_INS_SEL_NCL_PROP_BUILDING_TEMP");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.DEL_INS_SEL_NCL_PROP_BUILDING_TEMP(ULBCODE, NCLBLDG); //change parameter
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "DEL_INS_SEL_NCL_PROP_BUILDING_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure. DEL_INS_SEL_NCL_PROP_BUILDING_TEMP");
                throw;
            }
        }
        #endregion
        #region MultiStory Details Events
        [HttpGet("GET_NCL_MOB_TEMP_FLOOR_AREA")]
        public ActionResult<DataSet> GET_NCL_MOB_TEMP_FLOOR_AREA(int PROPERTYCODE)
        {
            _logger.LogInformation("GET request received at GET_NCL_MOB_TEMP_FLOOR_AREA");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_NCL_MOB_TEMP_FLOOR_AREA(PROPERTYCODE);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_NCL_MOB_TEMP_FLOOR_AREA");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NCL_MOB_TEMP_FLOOR_AREA");
                throw;
            }
        }
        [HttpPost("INS_UPD_NCL_PROPERTY_APARTMENT_TEMP1")]
        public ActionResult<int> INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ULBCODE, Models.NCLAPARTMENT NCLAPT)
        {
            _logger.LogInformation("post request received at INS_UPD_NCL_PROPERTY_APARTMENT_TEMP1");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(ULBCODE, NCLAPT);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_UPD_NCL_PROPERTY_APARTMENT_TEMP1");
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_UPD_NCL_PROPERTY_APARTMENT_TEMP1");
                throw;
            }
        }
        #endregion
        #region Owner Details Events
        [HttpGet("COPY_OWNER_FROM_BBDDRAFT_NCLTEMP")]
        public ActionResult<DataSet> COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(int P_BOOKS_PROP_APPNOAPPNO, int propertyCode, int ownerNumber)
        {
            _logger.LogInformation("GET request received at COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(P_BOOKS_PROP_APPNOAPPNO, propertyCode, ownerNumber);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
                throw;
            }
        }
        [HttpGet("DEL_SEL_NCL_PROP_OWNER_TEMP")]
        public ActionResult<DataSet> DEL_SEL_NCL_PROP_OWNER_TEMP(int P_BOOKS_PROP_APPNO, int propertyCode, int ownerNumber)
        {
            _logger.LogInformation("GET request received at GET_NCL_TEMP_FLOOR_PRE DEL_SEL_NCL_PROP_OWNER_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.DEL_SEL_NCL_PROP_OWNER_TEMP(P_BOOKS_PROP_APPNO, propertyCode, ownerNumber);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "DEL_SEL_NCL_PROP_OWNER_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.DEL_SEL_NCL_PROP_OWNER_TEMP");
                throw;
            }
        }
        [HttpGet("UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY")]
        public ActionResult<DataSet> UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY(int P_BOOKS_PROP_APPNO, int propertyCode, int ownerNumber, int IDENTIFIERTYPE, string IDENTIFIERNAME_EN, string MOBILENUMBER, string MOBILEVERIFY, string loginId)
        {
            _logger.LogInformation("GET request received at UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
            try
            {
                DataSet dataSet = objModule.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY(P_BOOKS_PROP_APPNO, propertyCode, ownerNumber, IDENTIFIERTYPE, IDENTIFIERNAME_EN, MOBILENUMBER, MOBILEVERIFY, loginId);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
                _logger.LogError(ex, "Error occurred while executing stored procedure.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE, string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
            try
            {
                DataSet dataSet = objModule.INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA(IDENTIFIERTYPE, IdentifierName, MOBILENUMBER, MOBILEVERIFY, NAMEMATCHSCORE, loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
                throw;
            }
        }

        #endregion
        #region Property Rights
        [HttpPost("NCL_PROPERTY_RIGHTS_TEMP_INS")]
        public ActionResult<int> NCL_PROPERTY_RIGHTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropRights NCLPropRight)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_RIGHTS_TEMP_INS");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_RIGHTS_TEMP_INS(ID_BASIC_PROPERTY, NCLPropRight);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_RIGHTS_TEMP_INS");
                throw;
            }
        }
        [HttpPost("NCL_PROPERTY_RIGHTS_TEMP_UPD")]
        public ActionResult<int> NCL_PROPERTY_RIGHTS_TEMP_UPD(int ID_BASIC_PROPERTY, Models.NCLPropRights NCLPropRight)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_RIGHTS_TEMP_UPD");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_RIGHTS_TEMP_UPD(ID_BASIC_PROPERTY, NCLPropRight);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_RIGHTS_TEMP_UPD");
                throw;
            }
        }
        [HttpGet("NCL_PROPERTY_RIGHTS_TEMP_DEL")]
        public ActionResult<int> NCL_PROPERTY_RIGHTS_TEMP_DEL(int P_BOOKS_PROP_APPNO, int RIGHTSID, int ID_BASIC_PROPERTY, int ULBCODE, int PROPERTYCODE)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_RIGHTS_TEMP_DEL");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_RIGHTS_TEMP_DEL(P_BOOKS_PROP_APPNO, RIGHTSID, ID_BASIC_PROPERTY, ULBCODE, PROPERTYCODE);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_RIGHTS_TEMP_DEL");
                throw;
            }
        }
        #endregion
        #region Document Upload Events
        [HttpPost("NCL_PROPERTY_ID_TEMP_INS")]
        public ActionResult<int> NCL_PROPERTY_ID_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_ID_TEMP_INS");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_ID_TEMP_INS(ID_BASIC_PROPERTY, NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "NCL_PROPERTY_ID_TEMP_INS");
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_ID_TEMP_INS");
                throw;
            }
        }
        [HttpGet("GetNCLDocView")]
        public ActionResult<DataSet> GetNCLDocView(int DOCUMENTID, int PROPERTYCODE)
        {
            _logger.LogInformation("GET request received at GetNCLDocView");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetNCLDocView(DOCUMENTID, PROPERTYCODE);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetNCLDocView");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetNCLDocView");
                throw;
            }
        }
        [HttpPost("NCL_PROPERTY_ID_TEMP_DEL")]
        public ActionResult<DataSet> NCL_PROPERTY_ID_TEMP_DEL(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_ID_TEMP_DEL");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_ID_TEMP_DEL(ID_BASIC_PROPERTY, NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "NCL_PROPERTY_ID_TEMP_DEL");
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_ID_TEMP_DEL");
                throw;
            }
        }
        [HttpGet("GetMasterDocByCategoryOrClaimType")]
        public ActionResult<DataSet> GetMasterDocByCategoryOrClaimType(int ULBCODE, int CATEGORYID, int ClaimTypP_BOOKS_PROP_APPNO)
        {
            _logger.LogInformation("GET request received at GetMasterDocByCategoryOrClaimType");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetMasterDocByCategoryOrClaimType(ULBCODE, CATEGORYID, ClaimTypP_BOOKS_PROP_APPNO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetMasterDocByCategoryOrClaimType");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetMasterDocByCategoryOrClaimType");
                throw;
            }
        }
        #endregion
        #region Classification Document Upload Events
        [HttpPost("INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP(NCLClassPropIdentification nCLClassPropIdentification)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP(nCLClassPropIdentification);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP");
                throw;
            }
        }
        [HttpGet("DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP")]
        public ActionResult<DataSet> DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP(int PROPERTYCODE, int P_DOC_SCAN_ID, int P_BOOKS_PROP_APPNOAPPNO)
        {
            _logger.LogInformation("GET request received at DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(PROPERTYCODE, P_DOC_SCAN_ID, P_BOOKS_PROP_APPNOAPPNO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP");
                throw;
            }
        }
        [HttpGet("SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP")]
        public ActionResult<DataSet> SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID)
        {
            _logger.LogInformation("GET request received at SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(PROPERTYCODE, DOCUMENTROWID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
                throw;
            }
        }
        [HttpGet("GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS")]
        public ActionResult<DataSet> GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS(int PROPERTYCATEGORYID, int CLASSIFICATIONID, int SUBCLASSIFICATIONID)
        {
            _logger.LogInformation("GET request received at GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS(PROPERTYCATEGORYID, CLASSIFICATIONID, SUBCLASSIFICATIONID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS");
                throw;
            }
        }
        [HttpGet("GetPropertySubClassByULBAndCategory")]
        public ActionResult<DataSet> GetPropertySubClassByULBAndCategory(int PropCatID, int ulbcode)
        {
            _logger.LogInformation("GET request received at GetPropertySubClassByULBAndCategory");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetPropertySubClassByULBAndCategory(PropCatID, ulbcode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetPropertySubClassByULBAndCategory");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetPropertySubClassByULBAndCategory");
                throw;
            }
        }
        #endregion
        #region  Tax Events
        [HttpGet("GetTaxDetails")]
        public ActionResult<DataSet> GetTaxDetails(string applicationNo, long propertycode, long P_BOOKS_PROP_APPNO, string loginId)
        {
            _logger.LogInformation("GET request received at GetTaxDetails");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetTaxDetails(applicationNo, propertycode, P_BOOKS_PROP_APPNO, loginId);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetTaxDetails");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetTaxDetails");
                throw;
            }
        }
        [HttpPost("InsertBBMPPropertyTaxResponse")]
        public ActionResult<DataSet> InsertBBMPPropertyTaxResponse(int UlbCode, string Json, string Response, string IpAddress, string Createdby, string oParamater)
        {
            _logger.LogInformation("GET request received at InsertBBMPPropertyTaxResponse");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.InsertBBMPPropertyTaxResponse(UlbCode, Json, Response, IpAddress, Createdby, oParamater);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "InsertBBMPPropertyTaxResponse");
                _logger.LogError(ex, "Error occurred while executing stored procedure InsertBBMPPropertyTaxResponse.");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_SUBCLASS")]
        public ActionResult<int> INS_NCL_PROPERTY_SUBCLASS(Int64 BOOKS_PROP_APPNO, Int64 propertyCode, int CLASSIFICATIONID, int SUBCLASSIFICATIONID, string CREATEDBY, string? SUBCLASSIFICATION)
        {
            _logger.LogInformation("GET request received at InsertBBMPPropertyTaxResponse");
            try
            {
                int dataSet = objModule.INS_NCL_PROPERTY_SUBCLASS(BOOKS_PROP_APPNO, propertyCode, CLASSIFICATIONID, SUBCLASSIFICATIONID, CREATEDBY, SUBCLASSIFICATION);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_SUBCLASS");
                _logger.LogError(ex, "Error occurred while executing stored procedure InsertBBMPPropertyTaxResponse.");
                throw;
            }
        }
        #endregion
        #region Objection Events
        [HttpPost("InsertBBMPPropertyTaxResponseObjectionEvents")]
        public ActionResult<DataSet> InsertBBMPPropertyTaxResponse(int PROPERTYCODE, string OBJECTIONDETAILS, byte[] SCANNEDDOCUMENT, string DOCUMENTDETAILS, string CREATEDBY)
        {
            _logger.LogInformation("GET request received at InsertBBMPPropertyTaxResponseObjectionEvents");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.InsertBBMPPropertyTaxResponse(PROPERTYCODE, OBJECTIONDETAILS, SCANNEDDOCUMENT, DOCUMENTDETAILS, CREATEDBY);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "InsertBBMPPropertyTaxResponseObjectionEvents");
                _logger.LogError(ex, "Error occurred while executing stored procedure.InsertBBMPPropertyTaxResponseObjectionEvents");
                throw;
            }
        }
        #endregion
        #region eSignCode
        #endregion

        [HttpGet("GET_PROPERTY_AREA_DIMENSION_DATA")]
        public ActionResult<DataSet> GET_PROPERTY_AREA_DIMENSION_DATA(int BOOKS_PROP_APPNO, int Propertycode)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_AREA_DIMENSION_DATA");
            try
            {
                DataSet dataSet = objModule.GET_PROPERTY_AREA_DIMENSION_DATA(BOOKS_PROP_APPNO, Propertycode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_AREA_DIMENSION_DATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure.InsertBBMPPropertyTaxResponseObjectionEvents");
                throw;
            }
        }
        [HttpGet("GET_MST_FEATURE_BY_FEATUREHEADID")]
        public ActionResult<DataSet> GET_MST_FEATURE_BY_FEATUREHEADID(int FEATUREHEADID)
        {
            _logger.LogInformation("GET request received at GET_MST_FEATURE_BY_FEATUREHEADID");
            try
            {
                DataSet dataSet = objModule.GET_MST_FEATURE_BY_FEATUREHEADID(FEATUREHEADID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_MST_FEATURE_BY_FEATUREHEADID");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_MST_FEATURE_BY_FEATUREHEADID");
                throw;
            }
        }
        [HttpGet("GET_PROPERTY_CATEGORYWISE_DATA")]
        public ActionResult<DataSet> GET_PROPERTY_CATEGORYWISE_DATA(int BOOKS_PROP_APPNO, int Propertycode)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_CATEGORYWISE_DATA");
            try
            {
                DataSet dataSet = objModule.GET_PROPERTY_AREA_DIMENSION_DATA(BOOKS_PROP_APPNO, Propertycode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_CATEGORYWISE_DATA");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_CATEGORYWISE_DATA");
                throw;
            }
        }
        [HttpGet("VALIDATE_CATEGORY_DOC_UPLOAD")]
        public ActionResult<DataSet> VALIDATE_CATEGORY_DOC_UPLOAD(int BOOKS_PROP_APPNO, int Propertycode)
        {
            _logger.LogInformation("GET request received at VALIDATE_CATEGORY_DOC_UPLOAD");
            try
            {
                DataSet dataSet = objModule.GET_PROPERTY_AREA_DIMENSION_DATA(BOOKS_PROP_APPNO, Propertycode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "VALIDATE_CATEGORY_DOC_UPLOAD");
                _logger.LogError(ex, "Error occurred while executing stored procedure.VALIDATE_CATEGORY_DOC_UPLOAD");
                throw;
            }
        }
        [HttpGet("GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY")]
        public ActionResult<DataSet> GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(int Propertycode)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY");
            try
            {
                DataSet dataSet = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(Propertycode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY");
                throw;
            }
        }
        #region Matrix
        [HttpPost("UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP")]
        public ActionResult<DataSet> UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Int64 BOOKS_PROP_APPNO, Int64 propertyCode, string COLUMN_NAME, string COLUMN_VALUE, string loginID)
        {
            _logger.LogInformation("GET request received at UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP");
            try
             {
                DataSet dataSet = objModule.UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP(BOOKS_PROP_APPNO, propertyCode, COLUMN_NAME, COLUMN_VALUE, loginID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
               
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP");
                _logger.LogError(ex, "Error occurred while executing stored procedure.UPD_COL_NCL_PROPERTY_COMPARE_MATRIX_TEMP");
                throw;
            }
        }
        [HttpPost("UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP")]
        public ActionResult<DataSet> UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Int64 BOOKS_PROP_APPNO, Int64 propertyCode, string loginID, NCL_PROPERTY_COMPARE_MATRIX_TEMP_BO objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO)
        {
            _logger.LogInformation("GET request received at UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP");
            try
            {
                DataSet dataSet = objModule.UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP(BOOKS_PROP_APPNO, propertyCode, loginID, objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex,"");
                _logger.LogError(ex, "Error occurred while executing stored procedure.UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP");
                throw;
            }
        }
        #endregion
    }
}
