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
using NUPMS_BO;
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
        public ActionResult<DataSet> GET_BBD_NCL_OWNER_BYEKYCTRANSACTION(long transactionNumber, string OwnerType)
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
       
        [HttpGet("DEL_SEL_NCL_PROP_OWNER_TEMP")]
        public ActionResult<DataSet> DEL_SEL_NCL_PROP_OWNER_TEMP(int P_BOOKS_PROP_APPNO, int propertyCode, int ownerNumber)
        {
            _logger.LogInformation("GET request received at GET_NCL_TEMP_FLOOR_PRE DEL_SEL_NCL_PROP_OWNER_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.DEL_SEL_NCL_PROP_OWNER_TEMP(P_BOOKS_PROP_APPNO, propertyCode, ownerNumber);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
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
                DataSet dataSet = obj.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY(P_BOOKS_PROP_APPNO, propertyCode, ownerNumber, IDENTIFIERTYPE, IDENTIFIERNAME_EN, MOBILENUMBER, MOBILEVERIFY, loginId);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA")]
        public ActionResult<int> INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string MOBILENUMBER, string MOBILEVERIFY, string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
            try
            {
                int dataSet = obj.INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA(IDENTIFIERTYPE, MOBILENUMBER, MOBILEVERIFY, loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                throw;
            }
        }
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
                throw;
            }
        }
        [HttpGet("Insert_React_UserFlow")]
        public ActionResult<int> INS_USER_REACT_FLOW(int P_BOOKS_PROP_APPNO, int propertyCode, int currentStep, int DraftWardId, int DraftZoneId, string LoginID)
        {
            _logger.LogInformation("GET request received at Insert_React_UserFlow");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.Insert_React_UserFlow(P_BOOKS_PROP_APPNO, propertyCode, currentStep, DraftWardId, DraftZoneId, LoginID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.Insert_React_UserFlow");
                throw;
            }
        }
        [HttpGet("Get_React_UserFlow")]
        public ActionResult<ReactUserFlow> Get_React_UserFlow( Int64 propertyCode, Int64 BOOKAPP_NO, string LoginID)
        {
            _logger.LogInformation("GET request received at Get_React_UserFlow");
            try
            {
                ReactUserFlow react = new ReactUserFlow();

                DataSet dataSet = _IBBMPBOOKMODULE.Get_React_UserFlow( propertyCode, BOOKAPP_NO, LoginID);
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    react.AllowFlow = 0;
                }
                else 

                {
                    react.LoginId = Convert.ToString(dataSet.Tables[0].Rows[0]["LoginId"]);
                    react.PropertyCode = Convert.ToInt64(dataSet.Tables[0].Rows[0]["PropertyCode"]);
                    react.BOOK_APP_NO = Convert.ToInt64(dataSet.Tables[0].Rows[0]["BOOK_APP_NO"]);
                    react.DraftWardId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["DraftWardId"]);
                    react.DraftZoneId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["DraftZoneId"]);
                    react.AllowFlow =  Convert.ToInt32(dataSet.Tables[0].Rows[0]["CurrentStep"]);
                   
    }


                string json = JsonConvert.SerializeObject(react, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.Get_React_UserFlow");
                throw;
            }
        }
        public class ReactUserFlow
        {
            public string? LoginId { get; set; }
            public long? PropertyCode { get; set; }
            public long? BOOK_APP_NO { get; set; }
            public int? DraftWardId { get; set; }
            public int? DraftZoneId { get; set; }
           
            public int? AllowFlow { get; set; }

        }
    }
    }
