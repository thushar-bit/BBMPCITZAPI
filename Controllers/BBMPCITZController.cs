using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Database;
using System.Data;
using BBMPCITZAPI.Services.Interfaces;
using Newtonsoft.Json;
using static BBMPCITZAPI.Services.BBMPBookModuleService;
using NUPMS_BA;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace BBMPCITZAPI.Controllers
{
    [ApiController]
    [Route("v1/BBMPCITZAPI")]
    public class BBMPCITZController : ControllerBase
    {
        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _IBBMPBOOKMODULE;
        private readonly PropertyDetails _PropertyDetails;

        public BBMPCITZController(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IOptions<PropertyDetails> PropertyDetails)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _PropertyDetails = PropertyDetails.Value;
        }
        NUPMS_BA.ObjectionModuleBA objModule = new NUPMS_BA.ObjectionModuleBA();
        NUPMS_BA.Objection_BA objBa = new NUPMS_BA.Objection_BA();
        //   NUPMS_BA.CitizenBA ciTz= new NUPMS_BA.CitizenBA();
        #region Initial
        [Authorize]

        [HttpGet("GET_PROPERTY_PENDING_CITZ_BBD_DRAFT")]
        public ActionResult<DataSet> GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(int UlbCode,int EID,int propertyid)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_BBD_DRAFT");
            try
            {
                var dataSet = objModule.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(UlbCode,EID, propertyid);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure GET_PROPERTY_PENDING_CITZ_BBD_DRAFT.");
                throw;
            }
        }

        [Authorize]
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
                _logger.LogError(ex, "Error occurred while executing stored procedure. GetMasterTablesData");
                throw;
            }
        }
        [Authorize]
        [HttpGet("GET_PROPERTY_PENDING_CITZ_NCLTEMP")]
        public ActionResult<DataSet> GET_PROPERTY_PENDING_CITZ_NCLTEMP(int ULBCODE, int EIDAPPNO, int Propertycode)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_PENDING_CITZ_NCLTEMP");
            try
            {
                DataSet dataSet = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP( ULBCODE,  EIDAPPNO,  Propertycode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_PROPERTY_PENDING_CITZ_NCLTEMP");
                throw;
            }
        }
        [HttpPost("GET_PROPERTY_CTZ_PROPERTY")]
        public ActionResult<int> Insert_PROPERTY_ADDRESS_TEMP(Insert_PROPERTY_ADDRESS_TEMP insertCITZProperty)
        {
            _logger.LogInformation("GET request received at GET_PROPERTY_CTZ_PROPERTY");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.Insert_PROPERTY_ADDRESS_TEMP(insertCITZProperty); //change parameter
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_CTZ_PROPERTY");
                throw;
            }
        }
       
        [HttpGet("Get_Ctz_ObjectionModPendingAppl")]
        public ActionResult<string> Get_Ctz_ObjectionModPendingAppl(string LoginId)
        {
            _logger.LogInformation("GET request received at Get_Ctz_ObjectionModPendingAppl");
            try
            {
                string propertycode = _PropertyDetails.PROPERTYCODE;
                string propertyid = _PropertyDetails.PROPERTYID;
                _logger.LogInformation(" Get_Ctz_ObjectionModPendingAppl" + "propertycode =" + propertycode + "propertyid = " + propertyid + "loginIDs= " + LoginId);
                DataSet dsProperties = objModule.Get_Ctz_ObjectionModPendingAppl("EID", LoginId, propertyid, "", 0, "");

                if ((dsProperties != null && dsProperties.Tables.Count > 0 && dsProperties.Tables[0].Rows.Count > 0))
                {
                    string EID = dsProperties.Tables[0].Rows[0]["BOOKS_PROP_APPNO"].ToString()!;
                    string PropertyId = dsProperties.Tables[0].Rows[0]["PROPERTYCODE"].ToString()!;
                    var data = new
                    {
                        EID = EID,
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
                _logger.LogError(ex, "Error occurred while executing stored procedure Get_Ctz_ObjectionModPendingAppl.");
                throw;
            }
        }
        [HttpGet("COPY_DATA_FROM_BBDDRAFT_NCLTEMP")]
        public ActionResult<string> COPY_DATA_FROM_BBDDRAFT_NCLTEMP(string LoginId)
        {
            _logger.LogInformation("GET request received at COPY_DATA_FROM_BBDDRAFT_NCLTEMP");
            try
            {
                string propertycode = _PropertyDetails.PROPERTYCODE;
                string propertyid = _PropertyDetails.PROPERTYID;
                _logger.LogInformation(" COPY_DATA_FROM_BBDDRAFT_NCLTEMP" + "propertycode =" + propertycode + "propertyid = " + propertyid + "loginIDs= " + LoginId);
                int rowsEffected = objModule.COPY_DATA_FROM_BBDDRAFT_NCLTEMP(Convert.ToInt64(propertycode), Convert.ToString(LoginId));
                string json = JsonConvert.SerializeObject(rowsEffected, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
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
                _logger.LogError(ex, "Error occurred while executing stored procedure. GetNPMMasterTable");
                throw;
            }
        }
        #endregion
        #region Building Details Events
        [HttpPost("DEL_SEL_NCL_PROP_BUILDING_TEMP")]
        public ActionResult<DataSet> DEL_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG)
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.DEL_SEL_NCL_PROP_BUILDING_TEMP");
                throw;
            }
        }
        [HttpPost("GET_NCL_FLOOR_AREA")]
        public ActionResult<DataSet> GET_NCL_FLOOR_AREA(NCLBuilding NCLBLDG)
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
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_NCL_FLOOR_AREA");
                throw;
            }
        }
        [HttpPost("GET_NCL_TEMP_FLOOR_PRE")]
        public ActionResult<DataSet> GET_NCL_TEMP_FLOOR_PRE(NCLBuilding NCLBLDG)
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
                _logger.LogError(ex, "Error occurred while executing stored procedure. GET_NCL_TEMP_FLOOR_PRE");
                throw;
            }
        }
        [HttpPost("DEL_INS_SEL_NCL_PROP_BUILDING_TEMP")]
        public ActionResult<int> DEL_INS_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG)
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NCL_MOB_TEMP_FLOOR_AREA");
                throw;
            }
        }
        [HttpPost("INS_UPD_NCL_PROPERTY_APARTMENT_TEMP1")]
        public ActionResult<int> INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ULBCODE, NCLAPARTMENT NCLAPT)
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
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_UPD_NCL_PROPERTY_APARTMENT_TEMP1");
                throw;
            }
        }
        #endregion
        #region Owner Details Events
        [HttpGet("COPY_OWNER_FROM_BBDDRAFT_NCLTEMP")]
        public ActionResult<DataSet> COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(int EIDAPPNO, int propertyCode, int ownerNumber)
        {
            _logger.LogInformation("GET request received at COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP( EIDAPPNO, propertyCode, ownerNumber);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
                throw;
            }
        }
        [HttpGet("DEL_SEL_NCL_PROP_OWNER_TEMP")]
        public ActionResult<DataSet> DEL_SEL_NCL_PROP_OWNER_TEMP(int EIDAPPNO, int propertyCode, int ownerNumber)
        {
            _logger.LogInformation("GET request received at GET_NCL_TEMP_FLOOR_PRE DEL_SEL_NCL_PROP_OWNER_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.DEL_SEL_NCL_PROP_OWNER_TEMP( EIDAPPNO, propertyCode,  ownerNumber);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.DEL_SEL_NCL_PROP_OWNER_TEMP");
                throw;
            }
        }
        [HttpGet("UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY")]
        public ActionResult<DataSet> UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY(int EIDAPPNO, int propertyCode, int ownerNumber, int IDENTIFIERTYPE, string IDENTIFIERNAME_EN, string MOBILENUMBER, string MOBILEVERIFY, string loginId)
        {
            _logger.LogInformation("GET request received at UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
            try
            {
                DataSet dataSet  = objModule.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY(EIDAPPNO, propertyCode,  ownerNumber,  IDENTIFIERTYPE,  IDENTIFIERNAME_EN,  MOBILENUMBER,  MOBILEVERIFY,  loginId);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
                throw;
            }
        }

        #endregion
        #region Property Rights
        [HttpPost("NCL_PROPERTY_RIGHTS_TEMP_INS")]
        public ActionResult<int> NCL_PROPERTY_RIGHTS_TEMP_INS(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_RIGHTS_TEMP_INS");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_RIGHTS_TEMP_INS( ID_BASIC_PROPERTY, NCLPropRight);
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
        public ActionResult<int> NCL_PROPERTY_RIGHTS_TEMP_UPD(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_RIGHTS_TEMP_UPD");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_RIGHTS_TEMP_UPD( ID_BASIC_PROPERTY, NCLPropRight);
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
        public ActionResult<int> NCL_PROPERTY_RIGHTS_TEMP_DEL(int EIDAPPNO, int RIGHTSID, int ID_BASIC_PROPERTY, int ULBCODE, int PROPERTYCODE)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_RIGHTS_TEMP_DEL");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_RIGHTS_TEMP_DEL( EIDAPPNO, RIGHTSID,  ID_BASIC_PROPERTY,  ULBCODE,  PROPERTYCODE);
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
        public ActionResult<int> NCL_PROPERTY_ID_TEMP_INS(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_ID_TEMP_INS");
            try
            {
                int dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_ID_TEMP_INS( ID_BASIC_PROPERTY, NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
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
                DataSet dataSet = _IBBMPBOOKMODULE.GetNCLDocView( DOCUMENTID,  PROPERTYCODE);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetNCLDocView");
                throw;
            }
        }
        [HttpPost("NCL_PROPERTY_ID_TEMP_DEL")]
        public ActionResult<DataSet> NCL_PROPERTY_ID_TEMP_DEL(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_ID_TEMP_DEL");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.NCL_PROPERTY_ID_TEMP_DEL( ID_BASIC_PROPERTY, NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_ID_TEMP_DEL");
                throw;
            }
        }
        [HttpGet("GetMasterDocByCategoryOrClaimType")]
        public ActionResult<DataSet> GetMasterDocByCategoryOrClaimType(int ULBCODE, int CATEGORYID, int ClaimTypeID)
        {
            _logger.LogInformation("GET request received at GetMasterDocByCategoryOrClaimType");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetMasterDocByCategoryOrClaimType( ULBCODE,  CATEGORYID,  ClaimTypeID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetMasterDocByCategoryOrClaimType");
                throw;
            }
        }
        #endregion
        #region Classification Document Upload Events
        [HttpPost("INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(NCLClassPropIdentification nCLClassPropIdentification)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(nCLClassPropIdentification);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
                throw;
            }
        }
        [HttpGet("DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP")]
        public ActionResult<DataSet> DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID, int EIDAPPNO)
        {
            _logger.LogInformation("GET request received at DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP( PROPERTYCODE,  DOCUMENTROWID, EIDAPPNO);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
                throw;
            }
        }
        [HttpGet("SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP")]
        public ActionResult<DataSet> SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID)
        {
            _logger.LogInformation("GET request received at SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP( PROPERTYCODE,  DOCUMENTROWID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP");
                throw;
            }
        }
        [HttpGet("GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS")]
        public ActionResult<DataSet> GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS(int CLASSIFICATIONID, int SUBCLASSIFICATIONID1, int SUBCLASSIFICATIONID2)
        {
            _logger.LogInformation("GET request received at GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS( CLASSIFICATIONID,  SUBCLASSIFICATIONID1,  SUBCLASSIFICATIONID2);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
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
                DataSet dataSet = _IBBMPBOOKMODULE.GetPropertySubClassByULBAndCategory( PropCatID,  ulbcode);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.GetPropertySubClassByULBAndCategory");
                throw;
            }
        }
        #endregion
        #region  Tax Events
        [HttpGet("GetTaxDetails")]
        public ActionResult<DataSet> GetTaxDetails(string applicationNo)
        {
            _logger.LogInformation("GET request received at GetTaxDetails");
            try
            {
                DataSet dataSet = _IBBMPBOOKMODULE.GetTaxDetails(applicationNo);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
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
                DataSet dataSet = _IBBMPBOOKMODULE.InsertBBMPPropertyTaxResponse( UlbCode,  Json,  Response,  IpAddress,  Createdby,  oParamater);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
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
                DataSet dataSet = _IBBMPBOOKMODULE.InsertBBMPPropertyTaxResponse( PROPERTYCODE,  OBJECTIONDETAILS,  SCANNEDDOCUMENT,  DOCUMENTDETAILS,  CREATEDBY);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.InsertBBMPPropertyTaxResponseObjectionEvents");
                throw;
            }
        }
        #endregion
        #region eSignCode
        #endregion
        [HttpGet("NameMatchScore2323")]
        public ActionResult<bool> GET_NameMatches(string ownerName1,string ownerName2)
        {
            _logger.LogInformation("GET request received at NameMatchScore2323");
            try
            {
                float NAMEMATCHSCORE = _IBBMPBOOKMODULE.Fn_CPlus_NameMatchJulyFinal2023(ownerName1, ownerName2);
              
                //int NAMEMATCHSCORE = 27;
                if (NAMEMATCHSCORE > 80)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing stored procedure.NameMatchScore2323");
                throw;
            }
        }
    }
}
