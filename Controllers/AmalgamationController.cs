
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Newtonsoft.Json;
using NUPMS_BA;
using NUPMS_BO;
using System.Data;
using static BBMPCITZAPI.Models.Amalgamation;

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
        [HttpGet("GET_NCL_MUTATION_AMALGAMATION_MAIN")]   //existing 
        public async Task<ActionResult> GET_NCL_MUTATION_AMALGAMATION_MAIN(string PropertyId,int ulbcode,Int64 MutatioAppl,bool IsAdd,string LoginId)
        {
            _logger.LogInformation("GET request received at GET_NCL_MUTATION_AMALGAMATION_MAIN");
            try
            {
                NMT_BA objBA = new NMT_BA();
                int count = _AmalgamationService.GET_NCL_MUTATION_AMALGAMATION_MAIN_COUNT(PropertyId, ulbcode);
                decimal totalArea = 0 ;
               
                if (count == 0)
                {
                    var NpmPropDetail = _AmalgamationService.GET_NCL_MUTATION_AMALGAMATION_MAIN(PropertyId, ulbcode);
                    if (NpmPropDetail.Tables[0].Rows[0]["PROPERTYCATEGORYID"].ToString() == "1" || (NpmPropDetail.Tables[0].Rows[0]["PROPERTYCATEGORYID"].ToString() == "2" && ulbcode.ToString() == "555"))
                    {
                        if (NpmPropDetail.Tables[0].Rows[0]["PROPERTYCLASSIFICATIONID"].ToString() == "1" ) 
                        {
                            string json = JsonConvert.SerializeObject(NpmPropDetail, Formatting.Indented);
                            if (IsAdd == true)
                            {
                              
                                var dataSet = _AmalgamationService.GeneratateMuttaplid(Convert.ToInt64(NpmPropDetail.Tables[0].Rows[0]["PROPERTYCODE"]),
                                    Convert.ToString(NpmPropDetail.Tables[0].Rows[0]["PROPERTYID"]), MutatioAppl);
                                if (Convert.ToInt64(dataSet.Tables[0].Rows[0]["NEW_MUTAAPPLID"]) == 0 && Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]) == 0)
                                {
                                    return Ok(new { success = false, message = $"Entry Already Exist for PropertyId {PropertyId}" });
                                }
                                
                                AMalGamation_final AmalFinal = new AMalGamation_final();
                                if (Convert.ToInt64(dataSet.Tables[0].Rows[0]["NEW_MUTAAPPLID"]) == 0)
                                {
                                    _AmalgamationService.INS_NCL_AMAL_APPL_MAIN(ulbcode, Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]), "crc");
                                    AmalFinal.MUTAPPLID = Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]);
                                    AmalFinal.LoginId = LoginId;
                                    AmalFinal.PROPERTYCODE = Convert.ToInt64(NpmPropDetail.Tables[0].Rows[0]["PROPERTYCODE"]);
                                    AmalFinal.UlbCode = ulbcode;
                                    _AmalgamationService.CopyNPMtoNMTMain(AmalFinal);
                                    DataSet NmpPropDetail = new DataSet();
                                    NmpPropDetail = objBA.GetNmpAmalgPropDetails(Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]),555);
                                    if (NmpPropDetail.Tables.Count > 0)
                                    {
                                        if (NmpPropDetail.Tables[0].Rows.Count > 0)
                                        {

                                            DataTable table;
                                            table = NmpPropDetail.Tables[0];
                                            object sumSiteObject;
                                            object sumBuildingObject;
                                            sumSiteObject = table.Compute("Sum(SITEAREA)", "");
                                           // sumBuildingObject = table.Compute("Sum(AREA)", "0");
                                            totalArea = Convert.ToDecimal(sumSiteObject);
                                        }
                                    }
                                            return Ok(new { success = true, MutationApplicationId = Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]), Details = json,TotalArea = totalArea });
                                }
                                else
                                {
                                    _AmalgamationService.INS_NCL_AMAL_APPL_MAIN(ulbcode, Convert.ToInt64(dataSet.Tables[0].Rows[0]["NEW_MUTAAPPLID"]), "crc");
                                    AmalFinal.MUTAPPLID = Convert.ToInt64(dataSet.Tables[0].Rows[0]["NEW_MUTAAPPLID"]);
                                    AmalFinal.LoginId = LoginId;
                                    AmalFinal.PROPERTYCODE = Convert.ToInt64(NpmPropDetail.Tables[0].Rows[0]["PROPERTYCODE"]);
                                    AmalFinal.UlbCode = ulbcode;
                                    _AmalgamationService.CopyNPMtoNMTMain(AmalFinal);
                                    DataSet NmpPropDetail = new DataSet();
                                    NmpPropDetail = objBA.GetNmpAmalgPropDetails(Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]), 555);
                                    if (NmpPropDetail.Tables.Count > 0)
                                    {
                                        if (NmpPropDetail.Tables[0].Rows.Count > 0)
                                        {

                                            DataTable table;
                                            table = NmpPropDetail.Tables[0];
                                            object sumSiteObject;
                                            object sumBuildingObject;
                                            sumSiteObject = table.Compute("Sum(SITEAREA)", "");
                                            // sumBuildingObject = table.Compute("Sum(AREA)", "0");
                                            totalArea = Convert.ToDecimal(sumSiteObject);
                                        }
                                    }
                                    NmpPropDetail = objBA.GetNmpAmalgPropDetails(Convert.ToInt64(dataSet.Tables[0].Rows[0]["MUTTAPPLIID"]), 555);
                                    return Ok(new { success = true, MutationApplicationId = Convert.ToInt64(dataSet.Tables[0].Rows[0]["NEW_MUTAAPPLID"]), Details = json,TotalArea = totalArea });
                                }
                            }
                            else
                            {
                                return Ok(new { success = true, MutationApplicationId =0, Details = json, TotalArea = totalArea });
                            }

                        }
                        else
                        {
                            return Ok(new { success = false, message = $"ಈ ಆಸ್ತಿ ಸಂಖ್ಯೆ ಅನಧಿಕೃತ / ಅಕ್ರಮ ಆಸ್ತಿಗೆ ಒಳಪಟ್ಟಿದೆ PropertyId{PropertyId}" });
                        }
                    }
                    else
                    {
                        return Ok(new { success = false, message = $"ಈ ಆಸ್ತಿ ಕಟ್ಟಡ / ಅಪಾರ್ಟ್ಮೆಂಟ್ ಗೆ ಒಳಪಟ್ಟಿದೆ PropertyId{PropertyId}" });
                    }
                }
                else if (count == 1)
                {
                    return Ok(new { success = false, message = $"ಈ ಆಸ್ತಿ ಸಂಖ್ಯಾಗೆ ಮ್ಯುಟೇಶನ್ ಅಥವಾ ಆಸ್ತಿ ತಿದ್ದುಪಡಿ ಪರ್ಕ್ರಿಯೆ ಚಾಲನೆಯಲ್ಲಿದು, ಈ ಆಸ್ತಿಗೆ ಮ್ಯುಟೇಶನ್ ಅರ್ಜಿ ಸಲ್ಲಿಸಲು ಸಾದ್ಯವಿಲ್ಲ PropertyId{PropertyId}" });
                }
                else if (count == 111)
                {
                    return Ok(new { success = false, message = $"ಪ್ರಸ್ತುತ ಹಣಕಾಸು ವರ್ಷದಲ್ಲಿ ತೆರಿಗೆ ಬಾಕಿ ಇರುವ ಕಾರಣ, ಈ ಆಸ್ತಿಗೆ ಮ್ಯುಟೇಶನ್ ಅರ್ಜಿ ಸಲ್ಲಿಸಲು ಸಾದ್ಯವಿಲ್ಲ PropertyId{PropertyId}" });
                }
                else
                {
                    return Ok(new { success = false, message = $"ಈ ಆಸ್ತಿ ಸಂಖ್ಯೆಗೆ ನಮೂನೆ-3/ನಮೂನೆ-2 ಲಭ್ಯವಿಲ್ಲ / ಅಸ್ತಿತ್ವದಲ್ಲಿಲ್ಲ ಅಥವಾ ಆಸ್ತಿ ಸಂಖ್ಯೆ ತಪ್ಪು PropertyId{PropertyId}" });
                }


                
                }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_NCL_MUTATION_AMALGAMATION_MAIN");
                _logger.LogError(ex, "Error occurred while executing stored procedure GET_NCL_MUTATION_AMALGAMATION_MAIN.");
                throw;
            }
        }
      

        //[HttpPost("CopyNPMtoNMTMain")]   //existing 
        //public ActionResult<DataSet> CopyNPMtoNMTMain(string[] PropertyId)
        //{
        //    _logger.LogInformation("GET request received at GetAmalgamationProperty");
        //    try
        //    {

        //        var dataSet = _AmalgamationService.CopyNPMtoNMTMain(PropertyId);
        //        string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

        //        return Ok(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.LogError(ex, "CopyNPMtoNMTMain");
        //        _logger.LogError(ex, "Error occurred while executing stored procedure CopyNPMtoNMTMain.");
        //        throw;
        //    }
        //}


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
        [HttpPost("NCL_AMALGAMATION_DOCUMENTS_TEMP_INS")]
        public ActionResult<DataSet> NCL_AMALGAMATION_DOCUMENTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID)
        {
            _logger.LogInformation("GET request received at NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS");
            try
            {
                if (NCLPropID.ORDERDATE != null)
                {
                    DateTime? documentDate = NCLPropID.ORDERDATE;

                    DateTime? d2 = documentDate?.AddDays(1);
                    NCLPropID.ORDERDATE = d2;
                }
                var dataSet = _AmalgamationService.NCL_AMALGAMATION_DOCUMENTS_TEMP_INS(ID_BASIC_PROPERTY, NCLPropID);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS");
                _logger.LogError(ex, "Error occurred while executing stored procedure NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS.");
                throw;
            }
        }
        [HttpPost("NCL_AMALGAMATION_DOCUMENT_TEMP_DEL")]
        public ActionResult<DataSet> NCL_AMALGAMATION_DOCUMENT_TEMP_DEL(Int64 MutationApplicatioId,Int32 DocumentId)
        {
            _logger.LogInformation("GET request received at NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL");
            try
            {
                DataSet dataSet = _AmalgamationService.NCL_AMALGAMATION_DOCUMENT_TEMP_DEL(MutationApplicatioId, DocumentId);
                string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL");
                _logger.LogError(ex, "Error occurred while executing stored procedure.NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL");
                throw;
            }
        }







        [HttpPost("INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT")]
        public ActionResult<string> INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(AMalGamation_final AmalFinal)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT");
            try
            {
                
                var amal = _AmalgamationService.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT_APPL(AmalFinal);
                var dataSet = _AmalgamationService.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(AmalFinal);
                int ApplNO;
                NMT_BA objBA = new NMT_BA();
                DataSet ds = new DataSet();
                NMT_APPL_MAIN objMAIN = new NMT_APPL_MAIN();
                NWFTaskFlow objNWF = new NWFTaskFlow();
                ds = objBA.GET_NMT_APPL_PROPERTY(AmalFinal.MUTAPPLID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) //workflow generation
                {
                    ApplNO = Convert.ToInt32(ds.Tables[0].Rows[0]["APPLICATIONNO"].ToString());
                    objMAIN.ULBCODE = AmalFinal.UlbCode;
                    objMAIN.MUTAPPLID = Convert.ToInt32(AmalFinal.MUTAPPLID);
                    objMAIN.APPLICATIONNO = ApplNO;
                    objMAIN.UPDATEDBY = AmalFinal.LoginId;
                    objMAIN.APPLICATIONSTATUS = "REC";
                    objNWF.ULBCODE = AmalFinal.UlbCode;
                    objNWF.ULBCODETYPEID = 7;
                    objNWF.TRANTYPEID = 5;
                    objNWF.PROPERTYCODE = int.Parse(ds.Tables[0].Rows[0]["PROPERTYCODE"].ToString());
                    int propertyCode = int.Parse(ds.Tables[0].Rows[0]["PROPERTYCODE"].ToString());
                    try
                    {
                        var REQ = objBA.REC_MUTATION_APPL(propertyCode, AmalFinal.UlbCode, Convert.ToInt64(AmalFinal.MUTAPPLID), objNWF, objMAIN);
                    }catch(Exception ex)
                    {
                        if (ex.Message.Contains("TASK ALREADY GENERATED", StringComparison.OrdinalIgnoreCase))
                        {
                          return "Application Already Submitted";
                        }
                    }
                }


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
        public class NWFTaskFlow2
        {
            public string CREATEDBY { get; set; }// VARCHAR2(20);
            public int ULBCODE { get; set; }// NUMBER;
            public int WORKFLOWID { get; set; }// NUMBER;
            public int PROPERTYCODE { get; set; }// NUMBER;
            public string LOGINID { get; set; }// VARCHAR2(20);
            public int ULBCODETYPEID { get; set; }// NUMBER; TRANTYPEID
            public int TRANTYPEID { get; set; }// NUMBER; TRANTYPEID
            public int TASKID { get; set; }// NUMBER; TRANTYPEID

        }
        public class NMT_APPL_MAIN2
        {
            public string APPLICATIONSTATUS { get; set; }// VARCHAR2(3);
            public int APPLICATIONNO { get; set; }// NUMBER;
            public string CREATEDBY { get; set; }// VARCHAR2(20);P_UPDATEDBY
            public decimal REGISTEREDAMOUND { get; set; }// NUMBER;
            public DateTime APPLICATIONDATE { get; set; }// DATE;
            public DateTime COURTORDERDATE { get; set; }// DATE;
            public string COURTORDERNO { get; set; }// VARCHAR2(500);
            public int MUTATIONTYPEID { get; set; }// NUMBER;
            public int MUTAPPLID { get; set; }// NUMBER;
            public int ULBCODE { get; set; }// NUMBER;
            public string REGISTERED { get; set; }// VARCHAR2(1);
            public string DOCUMENTREGNO { get; set; }// VARCHAR2(500);
            public DateTime DOCUMENTREGDATE { get; set; }// DATE;
            public string UPDATEDBY { get; set; }// VARCHAR2(20);P_UPDATEDBY
            public int COURTORDERSID { get; set; }// NUMBER;
        }

    }
}
