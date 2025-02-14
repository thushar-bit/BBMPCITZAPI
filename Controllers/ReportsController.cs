using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services;
using BBMPCITZAPI.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.NETCore;
using Newtonsoft.Json;
using NUPMS_BA;
using NUPMS_BO;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Utilities;
using Serilog;
using System.Data;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;




namespace BBMPCITZAPI.Controllers
{
    [ApiController]
    //Authorize]
    [Route("v1/Report")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly ESignSettings _Esign;
        private readonly INameMatchingService _NameMatchService;
        private readonly IBBMPBookModuleService _BBMPBookService;
        private readonly ISearchService _SearchService;
        private readonly IObjectionService _ObjectionService;
        private readonly IMutationObjectionService _MutationService;
        private readonly IErrorLogService _errorLogService;
        public ReportsController(ILogger<ReportsController> logger, IConfiguration configuration, IOptions<ESignSettings> eSignSettings, INameMatchingService NameMatchService, IErrorLogService errorLogService,
           IBBMPBookModuleService BBMPBookService, IObjectionService objectionService,ISearchService searchService, IMutationObjectionService mutationObjection)
        {
            _logger = logger;
            _Esign = eSignSettings.Value;
            _NameMatchService = NameMatchService;
            _errorLogService = errorLogService;
            _BBMPBookService = BBMPBookService;
            _ObjectionService = objectionService;
            _SearchService = searchService;
            _MutationService = mutationObjection;
        }

        NUPMS_BA.ObjectionModuleBA objModule = new NUPMS_BA.ObjectionModuleBA();
        NUPMS_BA.PROPERYID_BA propertyda = new NUPMS_BA.PROPERYID_BA();
        NUPMS_BA.BBD_BA objBbd = new NUPMS_BA.BBD_BA();
        NUPMS_BA.WFTask WF = new NUPMS_BA.WFTask();

        private string FinalSubmitValidations(DataSet dsNCLTablesData)
        {

            try
            {
                if (dsNCLTablesData.Tables[12].Rows.Count == 0)
                {
                    return "Tax Details not Saved"; //
                }
                if (dsNCLTablesData.Tables[1].Rows.Count == 0 && dsNCLTablesData.Tables[4].Rows.Count == 0 && dsNCLTablesData.Tables[11].Rows.Count == 0)
                {
                    return "Location Details not Saved"; //ADDRESS
                }

                if (Convert.ToString(dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]) == "1")
                {

                    if (dsNCLTablesData.Tables[2].Rows.Count == 0 && dsNCLTablesData.Tables[3].Rows.Count == 0)
                    {

                        return "Area Dimension Details not Saved";
                    }
                    if (dsNCLTablesData.Tables[2].Rows.Count == 0)
                    {
                        return "Site Details not Saved";
                    }

                    else if (Convert.ToString(dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]) == "2")
                    {

                        if (dsNCLTablesData.Tables[2].Rows.Count == 0 && dsNCLTablesData.Tables[3].Rows.Count == 0)
                        {

                            return "Area Dimension Details not Saved";
                        }
                        if (dsNCLTablesData.Tables[8].Rows.Count == 0)
                        {
                            return "Buidling Details not Saved";
                        }
                        if (dsNCLTablesData.Tables[19].Rows.Count == 0)
                        {
                            return "Bescom Details not Saved";
                        }
                    }
                    else if (Convert.ToString(dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]) == "3")
                    {
                        if (dsNCLTablesData.Tables[6].Rows.Count == 0)
                        {
                            return "Area Dimension AppartMentDetails Not Saved";
                        }
                        if (dsNCLTablesData.Tables[7].Rows.Count == 0)
                        {
                            return "Multi Storey Appartment Details not Saved";
                        }
                        if (dsNCLTablesData.Tables[19].Rows.Count == 0)
                        {
                            return "Bescom Details not Saved";
                        }
                    }
                }
                if (dsNCLTablesData.Tables[20].Rows.Count == 0)
                {
                    return "Matrix Details Not Saved";
                }
                if (Convert.ToString(dsNCLTablesData.Tables[20].Rows[0]["KAVERIDOC_AVAILABLE"]) == "1")
                {
                    if (dsNCLTablesData.Tables[14].Rows.Count == 0 && dsNCLTablesData.Tables[15].Rows.Count == 0 && dsNCLTablesData.Tables[16].Rows.Count == 0 && dsNCLTablesData.Tables[17].Rows.Count == 0 && dsNCLTablesData.Tables[18].Rows.Count == 0)
                    {
                        return "Kaveri Details not Saved";
                    }
                }
                else if (Convert.ToString(dsNCLTablesData.Tables[20].Rows[0]["KAVERIDOC_AVAILABLE"]) == "2")
                {
                    if (dsNCLTablesData.Tables[9].Rows.Count == 0)
                    {
                        return "Document not Uploaded";
                    }
                }
                if (dsNCLTablesData.Tables[5].Rows.Count == 0)
                {
                    return "Owner Details not Saved";
                }
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "FinalSubmitValidations");
                _logger.LogError(ex, "Error in FinalSubmitValidations function reportcontroller");
                throw ex;
            }
        }

        private int OWNER_COUNT_NOTIN_BOOKS(DataTable dsNCLTable, DataTable dsBBDTable)
        {
            try
            {
                int i = 0;

                foreach (DataRow objGridViewRow2 in dsNCLTable.Rows)
                {
                    bool isExitingOwner = false;
                    foreach (DataRow objGridViewRow1 in dsBBDTable.Rows)
                    {
                        if ((Convert.ToInt64(objGridViewRow1["OWNERNUMBER"]) == (Convert.ToInt64(objGridViewRow2["OWNERNUMBER"]))))
                        {
                            isExitingOwner = true;
                        }
                    }
                    if (!isExitingOwner)//new owner
                    {
                        i = i + 1;
                    }
                }
                return i;
            } catch (Exception ex)
            {
                _errorLogService.LogError(ex, "OWNER_COUNT_NOTIN_BOOKS");
                _logger.LogError(ex, "Error in OWNER_COUNT_NOTIN_BOOKS function reportcontroller");
                throw ex;
            }
        }

        [HttpGet("FinalSubmitValidation")]
        public string FinalSubmitValidation(int propertycode, int BOOKS_PROP_APPNO, string LoginId)
        {
            try
            {
                bool isValidationDone = false;
                NCL_PROPERTY_COMPARE_MATRIX_TEMP_BO objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO = new NCL_PROPERTY_COMPARE_MATRIX_TEMP_BO();

                DataSet dsNCLTablesData = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP(555, Convert.ToInt64(BOOKS_PROP_APPNO), Convert.ToInt64(propertycode));
                DataSet dsBBDTablesDatas = objModule.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(555, Convert.ToInt64(propertycode));//Data from NCL temp tables  

                if (dsNCLTablesData != null && dsNCLTablesData.Tables.Count > 1 && dsNCLTablesData.Tables[1].Rows.Count > 0 && Convert.ToInt32(dsNCLTablesData.Tables[0].Rows[0][0]) > 0)
                {
                    int firstValue = Convert.ToInt32(dsNCLTablesData.Tables[0].Rows[0][0]);
                    if (firstValue > 0)
                    {
                        string result = FinalSubmitValidations(dsNCLTablesData);
                        if (result == "SUCCESS")
                        {
                            isValidationDone = true;
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.KAVERIDOC_AVAILABLE = Convert.ToInt32(dsNCLTablesData.Tables[20].Rows[0]["KAVERIDOC_AVAILABLE"]);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.IS_LATEST_REGISTRATIONNO = Convert.ToString(dsNCLTablesData.Tables[20].Rows[0]["IS_LATEST_REGISTRATIONNO"]);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.CATEGORYCHANGE = (dsBBDTablesDatas.Tables[1].Rows[0]["PROPERTYCATEGORYID"] == dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]) ? "N" : "Y";
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_BOOKS = dsBBDTablesDatas.Tables[1].Rows.Count;
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_EKYC = dsNCLTablesData.Tables[5].Rows.Count;
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_NOTINBOOKS = OWNER_COUNT_NOTIN_BOOKS(dsNCLTablesData.Tables[5], dsBBDTablesDatas.Tables[5]);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_KAVERIDOC = KAVERIDOCOwnerCount(dsNCLTablesData.Tables[16]);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_KAVERIEC = KAVERIECOwnerCount(dsNCLTablesData.Tables[18]);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_SASDATA = dsNCLTablesData.Tables[12].Rows.Count;
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_BOOKS = (objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_NOTINBOOKS > 0) ? "N" : OWNERMATCHEDWITH_BOOKS(dsNCLTablesData, dsBBDTablesDatas);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_KAVERIDOC = GetOWNERMATCHEDWITH_KAVERIDOC(dsNCLTablesData, objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_KAVERIDOC, LoginId);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_KAVERIEC = GetOWNERMATCHEDWITH_KAVERIEC(dsNCLTablesData, objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_KAVERIEC, LoginId);
                            objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_SASDATA = GetOWNERMATCHEDWITH_SASDATA(dsNCLTablesData.Tables[12], dsNCLTablesData, LoginId);
                            objModule.AssignRefferalCodeForeKhataApplication(objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO);
                            objModule.UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Convert.ToInt64(BOOKS_PROP_APPNO), Convert.ToInt64(propertycode), Convert.ToString(LoginId), objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO);
                            return "SUCCESS";
                        }
                        else
                        {

                            return result;
                        }
                    }
                    else
                    {
                        return "Please Save All  Values";
                    }
                }
                else
                {
                    return "Please Save All Values";
                }
            }


            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "FinalSubmitValidation");
                _logger.LogError(ex, "Error in FinalSubmitValidation api  reportcontroller");
                throw ex;
            }
        }
        private string OWNERMATCHEDWITH_BOOKS(DataSet dsNCLTablesData, DataSet dsBBTablesData)
        {
            try
            {
                string matched = "N";
                bool isNameMatchFailed = false;

                Dictionary<Int64, string> dicBookOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsBBTablesData.Tables[5].Rows)
                {

                    dicBookOwners.Add(Convert.ToInt64(dr["OWNERNUMBER"]), Convert.ToString(dr["OWNERNAME_EN"]));

                }

                Dictionary<Int64, string> dicEkycOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsNCLTablesData.Tables[5].Rows)
                {

                    dicEkycOwners.Add(Convert.ToInt64(dr["OWNERNUMBER"]), Convert.ToString(dr["OWNERNAME_EN"]));

                }


                List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
                objFinalListNameMatchingResult = _NameMatchService.CompareDictionaries(dicBookOwners, dicEkycOwners);

                foreach (NameMatchingResult objNameMatchingResult in objFinalListNameMatchingResult)
                {
                    if (objNameMatchingResult.NameMatchScore < Convert.ToInt32(75))
                    {
                        isNameMatchFailed = true;
                    }
                    // objModule.UPD_NCL_PROPERTY_KAVERI_PARTIES_DETAILS_TEMP(objNameMatchingResult.OwnerNo, objNameMatchingResult.EKYCOwnerNo, objNameMatchingResult.EKYCOwnerName, objNameMatchingResult.NameMatchScore, Convert.ToString(LoginId));
                }



                if (isNameMatchFailed == true)
                {
                    matched = "N";
                }
                else
                {
                    matched = "Y";
                }
                return matched;
            } catch (Exception ex)
            {
                _errorLogService.LogError(ex, "OWNERMATCHEDWITH_BOOKS");
                _logger.LogError(ex, "Error in OWNERMATCHEDWITH_BOOKS function  reportcontroller");
                throw ex;
            }
        }

        private string GetOWNERMATCHEDWITH_KAVERIDOC(DataSet dsNCLTablesData, int OWNER_COUNT_KAVERIDOC, string LoginId)
        {
            try
            {
                string matched = "N";
                bool isNameMatchFailed = false;

                Dictionary<Int64, string> dicKaveriOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsNCLTablesData.Tables[16].Rows)
                {
                    if (Convert.ToString(dr["PARTYTYPE"]) == "Claimant")
                    {
                        dicKaveriOwners.Add(Convert.ToInt64(dr["ROW_ID"]), Convert.ToString(dr["PARTYNAME"]));
                    }
                }

                Dictionary<Int64, string> dicEkycOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsNCLTablesData.Tables[5].Rows)
                {

                    dicEkycOwners.Add(Convert.ToInt64(dr["OWNERNUMBER"]), Convert.ToString(dr["OWNERNAME_EN"]));

                }


                List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
                objFinalListNameMatchingResult = _NameMatchService.CompareDictionaries(dicKaveriOwners, dicEkycOwners);

                foreach (NameMatchingResult objNameMatchingResult in objFinalListNameMatchingResult)
                {
                    if (objNameMatchingResult.NameMatchScore < Convert.ToInt32(75))
                    {
                        isNameMatchFailed = true;
                    }
                    objModule.UPD_NCL_PROPERTY_KAVERI_PARTIES_DETAILS_TEMP(objNameMatchingResult.OwnerNo, objNameMatchingResult.EKYCOwnerNo, objNameMatchingResult.EKYCOwnerName, objNameMatchingResult.NameMatchScore, Convert.ToString(LoginId));
                }

                if (Convert.ToString(dsNCLTablesData.Tables[20].Rows[0]["KAVERIDOC_AVAILABLE"]) != "1")
                {
                    matched = "N";
                }
                else if (OWNER_COUNT_KAVERIDOC != dsNCLTablesData.Tables[5].Rows.Count)
                {
                    matched = "N";
                }
                else if (isNameMatchFailed == true)
                {
                    matched = "N";
                }
                else
                {
                    matched = "Y";
                }
                return matched;
            } catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetOWNERMATCHEDWITH_KAVERIDOC");
                _logger.LogError(ex, "Error in GetOWNERMATCHEDWITH_KAVERIDOC function  reportcontroller");
                throw ex;
            }
        }

        private string GetOWNERMATCHEDWITH_KAVERIEC(DataSet dsNCLTablesData, int OWNER_COUNT_KAVERIEC, string LoginId)
        {
            try
            {
                string matched = "N";
                bool isNameMatchFailed = false;

                Dictionary<Int64, string> dicKaveriECOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsNCLTablesData.Tables[18].Rows)
                {
                    if (Convert.ToString(dr["ISCLAIMANTOREXECUTANT"]) == "C")
                    {
                        dicKaveriECOwners.Add(Convert.ToInt32(dr["ROW_ID"]), Convert.ToString(dr["OWNERNAME"]));
                    }
                }
                Dictionary<Int64, string> dicEkycOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsNCLTablesData.Tables[5].Rows)
                {

                    dicEkycOwners.Add(Convert.ToInt64(dr["OWNERNUMBER"]), Convert.ToString(dr["OWNERNAME_EN"]));

                }

                List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
                objFinalListNameMatchingResult = _NameMatchService.CompareDictionaries(dicKaveriECOwners, dicEkycOwners);

                foreach (NameMatchingResult objNameMatchingResult in objFinalListNameMatchingResult)
                {
                    if (objNameMatchingResult.NameMatchScore < Convert.ToInt32(75))
                    {
                        isNameMatchFailed = true;
                    }
                    objModule.UPD_NCL_PROPERTY_KAVERIEC_OWNERS_DETAILS_TEMP(objNameMatchingResult.OwnerNo, objNameMatchingResult.EKYCOwnerNo, objNameMatchingResult.EKYCOwnerName, objNameMatchingResult.NameMatchScore, Convert.ToString(LoginId));
                }

                if (Convert.ToString(dsNCLTablesData.Tables[20].Rows[0]["KAVERIDOC_AVAILABLE"]) != "1")
                {
                    matched = "N";
                }
                else if (OWNER_COUNT_KAVERIEC != dsNCLTablesData.Tables[5].Rows.Count)
                {
                    matched = "N";
                }
                else if (isNameMatchFailed == true)
                {
                    matched = "N";
                }
                else
                {
                    matched = "Y";
                }
                return matched;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetOWNERMATCHEDWITH_KAVERIEC");
                _logger.LogError(ex, "Error in GetOWNERMATCHEDWITH_KAVERIEC function  reportcontroller");
                throw ex;
            }
        }

        private string GetOWNERMATCHEDWITH_SASDATA(DataTable dtSASData, DataSet dsNCLTablesData, string LoginId)
        {
            try
            {
                string matched = "N";
                bool isNameMatchFailed = false;

                Dictionary<Int64, string> dicTaxDataOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dtSASData.Rows)
                {
                    dicTaxDataOwners.Add(Convert.ToInt32(dr["ROW_ID"]), Convert.ToString(dr["OWNERNAME"]));
                }

                Dictionary<Int64, string> dicEkycOwners = new Dictionary<Int64, string>();
                foreach (DataRow dr in dsNCLTablesData.Tables[5].Rows)
                {

                    dicEkycOwners.Add(Convert.ToInt64(dr["OWNERNUMBER"]), Convert.ToString(dr["OWNERNAME_EN"]));

                }


                List<NameMatchingResult> objFinalListNameMatchingResult = new List<NameMatchingResult>();
                objFinalListNameMatchingResult = _NameMatchService.CompareDictionaries(dicTaxDataOwners, dicEkycOwners);

                foreach (NameMatchingResult objNameMatchingResult in objFinalListNameMatchingResult)
                {
                    if (objNameMatchingResult.NameMatchScore < Convert.ToInt32(75))
                    {
                        isNameMatchFailed = true;
                    }
                    string SASOwnerName = "thushar";
                    objModule.UPD_NCL_PROPERTY_SAS_APP_NAMEMATCH_TEMP(Convert.ToInt64(dsNCLTablesData.Tables[5].Rows[0]["BOOKS_PROP_APPNO"]), Convert.ToInt64(dsNCLTablesData.Tables[5].Rows[0]["PROPERTYCODE"]), objNameMatchingResult.OwnerNo, SASOwnerName, objNameMatchingResult.EKYCOwnerNo, objNameMatchingResult.EKYCOwnerName, objNameMatchingResult.NameMatchScore, Convert.ToString(LoginId));
                }

                if (Convert.ToString(dsNCLTablesData.Tables[20].Rows[0]["KAVERIDOC_AVAILABLE"]) != "1")
                {
                    matched = "N";
                }
                else if (dtSASData.Rows.Count != dsNCLTablesData.Tables.Count)
                {
                    matched = "N";
                }
                else if (isNameMatchFailed == true)
                {
                    matched = "N";
                }
                else
                {
                    matched = "Y";
                }
                return matched;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetOWNERMATCHEDWITH_SASDATA");
                _logger.LogError(ex, "Error GetOWNERMATCHEDWITH_SASDATA the report.");
                throw;
            }
        }
        private int KAVERIDOCOwnerCount(DataTable dt)
        {
            try
            {
                int i = 0;


                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToString(dr["PARTYTYPE"]) == "Claimant")
                    {
                        i = i + 1;
                    }
                }

                return i;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "KAVERIDOCOwnerCount");
                _logger.LogError(ex, "Error KAVERIDOCOwnerCount private function the report.");
                throw;
            }
        }

        private int KAVERIECOwnerCount(DataTable dt)
        {
            try
            {
                int i = 0;


                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToString(dr["ISCLAIMANTOREXECUTANT"]) == "C")
                    {
                        i = i + 1;
                    }
                }

                return i;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "KAVERIECOwnerCount");
                _logger.LogError(ex, "Error KAVERIECOwnerCount private function the report.");
                throw;
            }
        }

        [HttpGet("GetFinalBBMPReport")]
        public IActionResult GetFinalReport(int propertycode, int BOOKS_PROP_APPNO, string LoginId)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();
                DataSet dsNCLTablesData = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP(555, BOOKS_PROP_APPNO, propertycode); // Data from NCL temp tables  
                DataSet dsNCLTablesDataDummy = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(propertycode);

                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP2.rdlc")
                };


                report.DataSources.Clear();

                int rcount = 0;
                // Set up parameters
                DataSet dsReportData = objModule.SEL_CitzeKhataAcknowledgement(Convert.ToInt32(BOOKS_PROP_APPNO), Convert.ToInt32(propertycode), Convert.ToString(LoginId));
                ReportParameter[] param = new ReportParameter[16];

                param[7] = new ReportParameter("P_ZONENAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["ZONENAME"]));
                param[8] = new ReportParameter("SUB_DIVISION_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["SUB_DIVISION_NAME"]));
                param[9] = new ReportParameter("P_WARD_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["WARD_NAME"]));
                param[10] = new ReportParameter("P_DOORSITENO", Convert.ToString(dsReportData.Tables[0].Rows[0]["DOORNO"]));
                param[11] = new ReportParameter("P_BUIDINGNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["BUILDINGNAME"]));
                param[12] = new ReportParameter("P_STREETNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["STREETNAME"]));
                param[13] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTNAME"]));
                param[14] = new ReportParameter("P_APPLICANTPOSTALADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTPOSTALADDRESS"]));
                param[15] = new ReportParameter("P_BOOKS_PROP_APPNO", "K-" + Convert.ToString(BOOKS_PROP_APPNO));

                param[6] = new ReportParameter("P_Hname", Convert.ToString(dsReportData.Tables[0].Rows[0]["ULB_DISPLAYNAME"]));
                param[3] = new ReportParameter("P_USERTYPE", "CITIZEN");
                param[3] = new ReportParameter("P_USERTYPE", "NA");
                param[0] = new ReportParameter("P_OPRNAME", "NA");
                param[1] = new ReportParameter("P_CENTERNAME", "NA");
                param[2] = new ReportParameter("P_PROPERTYCATEGORYID", Convert.ToString(dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]));
                param[4] = new ReportParameter("P_LANGUAGE", "0"); //English
                                                                   //   param[4] = new ReportParameter("P_LANGUAGE", "1"); //Kannada
                foreach (DataRow row in dsNCLTablesData.Tables[5].Rows)
                {
                    if (row["OWNERIDENTITYSLNO"] != DBNull.Value)
                    {
                        string ownerMobileNo = MaskMobileNo(dsNCLTablesData.Tables[5].Rows[rcount]["MOBILENUMBER"].ToString());
                        row["MOBILENUMBER"] = ownerMobileNo;
                        //if (ownerMobileNo == "")
                        //{
                        //    return;
                        //}
                    }
                    rcount++;
                }

                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["APARTMENTLANDPID"] == DBNull.Value)
                    {
                        row["APARTMENTLANDPID"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["MUNICIPALOLDNUMBER"] == DBNull.Value)
                    {
                        row["MUNICIPALOLDNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["ASSESMENTNUMBER"] == DBNull.Value)
                    {
                        row["ASSESMENTNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesDataDummy.Tables[0].Rows)
                {
                    if (row["surveyno"] == DBNull.Value)
                    {
                        row["surveyno"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[4].Rows)
                {
                    if (row["longitude"] == DBNull.Value)
                    {
                        row["longitude"] = "N.A.";
                    }
                    if (row["latitude"] == DBNull.Value)
                    {
                        row["latitude"] = "N.A.";
                    }
                }

                foreach (DataRow row in dsNCLTablesData.Tables[5].Rows)
                {
                    if (row["ISCOMPANY"] != DBNull.Value)
                    {
                        if (row["ISCOMPANY"].ToString() == "N")
                        {
                            param[5] = new ReportParameter("P_ISCOMPANY", "N");
                        }
                        else
                        {
                            param[5] = new ReportParameter("P_ISCOMPANY", "Y");
                        }
                    }
                    else
                    {
                        param[5] = new ReportParameter("P_ISCOMPANY", "N");
                    }

                }

                foreach (DataRow row in dsNCLTablesData.Tables[9].Rows)
                {
                    if (row["DOCUMENTDETAILS"] == DBNull.Value)
                    {
                        row["DOCUMENTDETAILS"] = "N.A.";
                    }
                }
                report.DataSources.Add(new ReportDataSource("DataSet1", dsNCLTablesData.Tables[1]));
                report.DataSources.Add(new ReportDataSource("PropSite", dsNCLTablesData.Tables[2]));
                report.DataSources.Add(new ReportDataSource("PropPhoto", dsNCLTablesData.Tables[11]));
                report.DataSources.Add(new ReportDataSource("PropDimention", dsNCLTablesData.Tables[3]));
                report.DataSources.Add(new ReportDataSource("PropCoordinates", dsNCLTablesData.Tables[4]));
                report.DataSources.Add(new ReportDataSource("OwnerDet", dsNCLTablesData.Tables[5]));
                report.DataSources.Add(new ReportDataSource("Rights", dsNCLTablesData.Tables[10]));
                report.DataSources.Add(new ReportDataSource("DocsToUpl", dsNCLTablesData.Tables[9]));
                report.DataSources.Add(new ReportDataSource("Apartment", dsNCLTablesData.Tables[11]));
                report.DataSources.Add(new ReportDataSource("Kattada", dsNCLTablesData.Tables[8]));
                report.DataSources.Add(new ReportDataSource("PropSurvey", dsNCLTablesData.Tables[0]));
                report.DataSources.Add(new ReportDataSource("Liabilities", dsNCLTablesData.Tables[1]));
                report.DataSources.Add(new ReportDataSource("MOBuilding", dsNCLTablesData.Tables[2]));
                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>8.27in</PageWidth>" +
                   "  <PageHeight>11.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = Guid.NewGuid().ToString() + ".pdf";
                return File(bytes, mimeType, "FinalReport.pdf");
                // return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetFinalBBMPReport");
                _logger.LogError(ex, "Error GetFinalBBMPReport function the report.");
                throw;
            }
        }
        [HttpGet("GetFinalObjectionAcknowledgementReport")]
        public IActionResult GetFinalObjectionAcknowledgementReport(int propertycode, int OBJECTIONID, string LoginId,long WardId)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();
                DataSet dsNCLTablesData = _ObjectionService.GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP(555, propertycode, OBJECTIONID); // Data from NCL temp tables  
                DataSet dsNCLTablesDataDummy = _BBMPBookService.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React("555",propertycode,"ADDRESS");

                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP4.rdlc")
                };


                report.DataSources.Clear();

                int rcount = 0;
                string ReasonType = "";
               
                switch (Convert.ToString(dsNCLTablesData.Tables[3].Rows[0]["REASONID"]))
                {
                    case "1":   ReasonType = "It is GOVT/BBMP Land";
                        break;
                    case "2":
                        ReasonType = "Court Order";
                        break;
                    case "3":
                        ReasonType = "Latest Registration/Sale Deed in another person name";
                        break;
                    case "4":
                        ReasonType = "Inheritance /Succession Dispute";
                        break;
                    case "5":
                        ReasonType = "Others";
                        break;
                }

                // Set up parameters
                DataSet dsReportData = _ObjectionService.SEL_CitzeObjectionAcknowledgement(Convert.ToInt32(OBJECTIONID), Convert.ToInt32(propertycode), Convert.ToString(LoginId), WardId);
                ReportParameter[] param = new ReportParameter[20];

                param[7] = new ReportParameter("P_ZONENAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["ZONENAME"]).Trim());
                param[8] = new ReportParameter("SUB_DIVISION_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["SUB_DIVISION_NAME"]).Trim());
                param[9] = new ReportParameter("P_WARD_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["WARD_NAME"]).Trim());
                param[10] = new ReportParameter("P_DOORSITENO", Convert.ToString(dsReportData.Tables[0].Rows[0]["DOORNO"]).Trim());
                param[11] = new ReportParameter("P_BUIDINGNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["BUILDINGNAME"]).Trim());
               // param[12] = new ReportParameter("P_STREETNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["STREETNAME"]));
                param[13] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTNAME"]).Trim());
                param[14] = new ReportParameter("P_APPLICANTPOSTALADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTPOSTALADDRESS"]).Trim());
             
                param[19] = new ReportParameter("P_REASONTYPE", ReasonType == "Others" ? Convert.ToString(dsNCLTablesData.Tables[3].Rows[0]["REASONDETAILS"]).Trim() : ReasonType);
                
                param[12] = new ReportParameter("P_STREETNAME1", "STREETNAME");
             
                param[16] = new ReportParameter("P_OBJECTIONNAME", Convert.ToString(dsNCLTablesData.Tables[4].Rows[0]["OBJECTIONNAME_EN"]).Trim());
                param[17] = new ReportParameter("P_OBJECTIONADDRESS", Convert.ToString(dsNCLTablesData.Tables[4].Rows[0]["OBJECTIONADDRESS_EN"]).Trim());
                param[18] = new ReportParameter("P_OBJECTIONPHOTO", Convert.ToString(dsNCLTablesData.Tables[4].Rows[0]["OWNERPHOTO"]).Trim());
              
                param[15] = new ReportParameter("P_BOOKS_PROP_APPNO", "K-" + Convert.ToString(OBJECTIONID));
                param[6] = new ReportParameter("P_Hname", "Objection");
                param[3] = new ReportParameter("P_USERTYPE", "CITIZEN");
                param[3] = new ReportParameter("P_USERTYPE", "NA");
                param[0] = new ReportParameter("P_OPRNAME", "NA");
                param[1] = new ReportParameter("P_CENTERNAME", "NA");
                param[2] = new ReportParameter("P_PROPERTYCATEGORYID", "23");
                param[4] = new ReportParameter("P_LANGUAGE", "0"); //English
                param[5] = new ReportParameter("P_ISCOMPANY", "N");                              
                //   param[4] = new ReportParameter("P_LANGUAGE", "1"); //Kannada
                foreach (DataRow row in dsNCLTablesData.Tables[4].Rows)
                {
                    if (row["OWNERIDENTITYSLNO"] != DBNull.Value)
                    {
                        string ownerMobileNo = MaskMobileNo(dsNCLTablesData.Tables[4].Rows[rcount]["MOBILENUMBER"].ToString());
                        row["MOBILENUMBER"] = ownerMobileNo;
                        
                    }
                    rcount++;
                }

                
                DataTable ds = new DataTable();
                report.DataSources.Add(new ReportDataSource("DataSet1", dsNCLTablesDataDummy.Tables[1]));
                report.DataSources.Add(new ReportDataSource("PropSite", ds));
                report.DataSources.Add(new ReportDataSource("PropPhoto", ds));
                report.DataSources.Add(new ReportDataSource("PropDimention", ds));
                report.DataSources.Add(new ReportDataSource("PropCoordinates", ds));
                report.DataSources.Add(new ReportDataSource("OwnerDet", dsNCLTablesData.Tables[4]));
                report.DataSources.Add(new ReportDataSource("Rights", ds));
                report.DataSources.Add(new ReportDataSource("DocsToUpl", ds));
                report.DataSources.Add(new ReportDataSource("Apartment", ds));
               report.DataSources.Add(new ReportDataSource("Kattada", ds));
                report.DataSources.Add(new ReportDataSource("PropSurvey", ds));
                report.DataSources.Add(new ReportDataSource("Liabilities", ds));
                report.DataSources.Add(new ReportDataSource("MOBuilding", ds));
                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>8.90in</PageWidth>" +
                   "  <PageHeight>11.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = "Objectors Acknowledgement";
                return File(bytes, mimeType, "ObjectorsAcknowledgement.pdf");
                // return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "ObjectorsAcknowledgement");
                _logger.LogError(ex, "Error ObjectorsAcknowledgement function the report.");
                throw;
            }
        }

        [HttpGet("GetEndorsementReport")]
        public IActionResult GetEndorsementReport(int propertycode, int BOOKS_PROP_APPNO, string LoginId)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();
                DataSet dsNCLTablesData = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP(555, BOOKS_PROP_APPNO, propertycode); // Data from NCL temp tables  
                DataSet dsNCLTablesDataDummy = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(propertycode);

                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_Endorsement_BBMP.rdlc")
                };


                report.DataSources.Clear();

                int rcount = 0;
                // Set up parameters
                DataSet dsReportData = objModule.SEL_CitzRegistration_Endorsement_BBMP(Convert.ToInt32(BOOKS_PROP_APPNO), Convert.ToInt32(propertycode), Convert.ToString(LoginId));
                ReportParameter[] param = new ReportParameter[13];

                param[0] = new ReportParameter("P_Hname", Convert.ToString(dsReportData.Tables[0].Rows[0]["HName"]));
                param[1] = new ReportParameter("P_ZONENAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["ZONENAME"]));
                param[2] = new ReportParameter("SUB_DIVISION_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["SUB_DIVISION_NAME"]));
                param[3] = new ReportParameter("P_WARD_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["WARD_NAME"]));
                param[4] = new ReportParameter("ARO_ADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["ARO_ADDRESS"]));
                param[5] = new ReportParameter("P_REASON", Convert.ToString(dsReportData.Tables[0].Rows[0]["REASON"]));
                param[6] = new ReportParameter("P_TYPEOFPROPERTY", dsReportData.Tables[0].Rows[0]["TYPEOFPROPERTY"].ToString());
                param[7] = new ReportParameter("P_OWNERNAMEBBMP", string.IsNullOrEmpty(Convert.ToString(dsReportData.Tables[0].Rows[0]["BBMP_REG_OWNERNAMES"])) ? "NA" : Convert.ToString(dsReportData.Tables[0].Rows[0]["BBMP_REG_OWNERNAMES"]));
                param[8] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTNAME"]));
                param[9] = new ReportParameter("P_APPLICANTPOSTALADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTPOSTALADDRESS"]));
                param[10] = new ReportParameter("P_PROPERTYID", Convert.ToString(propertycode));
                param[11] = new ReportParameter("P_BOOKS_PROP_APPNO", Convert.ToString(BOOKS_PROP_APPNO));
                param[12] = new ReportParameter("P_LANGUAGE", "0");


                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["APARTMENTLANDPID"] == DBNull.Value)
                    {
                        row["APARTMENTLANDPID"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["MUNICIPALOLDNUMBER"] == DBNull.Value)
                    {
                        row["MUNICIPALOLDNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["ASSESMENTNUMBER"] == DBNull.Value)
                    {
                        row["ASSESMENTNUMBER"] = "N.A.";
                    }
                }

                report.DataSources.Add(new ReportDataSource("DataSet1", dsNCLTablesData.Tables[1]));

                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>8.27in</PageWidth>" +
                   "  <PageHeight>11.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = Guid.NewGuid().ToString() + ".pdf";
                return File(bytes, mimeType, "FinalReport.pdf");
                // return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetEndorsementReport");
                _logger.LogError(ex, "Error GetEndorsementReport the report.");
                throw;
            }
        }


        private byte[] GetPDF(Int64 propertycode, Int64 BOOKS_PROP_APPNO)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();
                DataSet dsNCLTablesData = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP(555, BOOKS_PROP_APPNO, propertycode); // Data from NCL temp tables  
                DataSet dsNCLTablesDataDummy = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(propertycode);

                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP2.rdlc")
                };


                report.DataSources.Clear();

                int rcount = 0;
                // Set up parameters

                ReportParameter[] param = new ReportParameter[7];
                param[0] = new ReportParameter("P_OPRNAME", "NA");
                param[1] = new ReportParameter("P_CENTERNAME", "NA");
                param[2] = new ReportParameter("P_PROPERTYCATEGORYID", dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"].ToString());
                param[3] = new ReportParameter("P_USERTYPE", "CITIZEN");
                param[4] = new ReportParameter("P_LANGUAGE", "0");
                param[5] = new ReportParameter("P_ISCOMPANY", dsNCLTablesData.Tables[5].Rows.Count > 0 && dsNCLTablesData.Tables[5].Rows[0]["ISCOMPANY"].ToString() == "N" ? "N" : "Y");
                param[6] = new ReportParameter("P_Hname", "Bruhat Bangalore Mahanagara Palike"); // Adjust this parameter as needed
                foreach (DataRow row in dsNCLTablesData.Tables[5].Rows)
                {
                    if (row["OWNERIDENTITYSLNO"] != DBNull.Value)
                    {
                        string ownerMobileNo = MaskMobileNo(dsNCLTablesData.Tables[5].Rows[rcount]["MOBILENUMBER"].ToString());
                        row["MOBILENUMBER"] = ownerMobileNo;
                        //if (ownerMobileNo == "")
                        //{
                        //    return;
                        //}
                    }
                    rcount++;
                }

                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["APARTMENTLANDPID"] == DBNull.Value)
                    {
                        row["APARTMENTLANDPID"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["MUNICIPALOLDNUMBER"] == DBNull.Value)
                    {
                        row["MUNICIPALOLDNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["ASSESMENTNUMBER"] == DBNull.Value)
                    {
                        row["ASSESMENTNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesDataDummy.Tables[0].Rows)
                {
                    if (row["surveyno"] == DBNull.Value)
                    {
                        row["surveyno"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[4].Rows)
                {
                    if (row["longitude"] == DBNull.Value)
                    {
                        row["longitude"] = "N.A.";
                    }
                    if (row["latitude"] == DBNull.Value)
                    {
                        row["latitude"] = "N.A.";
                    }
                }

                foreach (DataRow row in dsNCLTablesData.Tables[5].Rows)
                {
                    if (row["ISCOMPANY"] != DBNull.Value)
                    {
                        if (row["ISCOMPANY"].ToString() == "N")
                        {
                            param[5] = new ReportParameter("P_ISCOMPANY", "N");
                        }
                        else
                        {
                            param[5] = new ReportParameter("P_ISCOMPANY", "Y");
                        }
                    }
                    else
                    {
                        param[5] = new ReportParameter("P_ISCOMPANY", "N");
                    }

                }

                foreach (DataRow row in dsNCLTablesData.Tables[9].Rows)
                {
                    if (row["DOCUMENTDETAILS"] == DBNull.Value)
                    {
                        row["DOCUMENTDETAILS"] = "N.A.";
                    }
                }
                report.DataSources.Add(new ReportDataSource("DataSet1", dsNCLTablesData.Tables[1]));
                report.DataSources.Add(new ReportDataSource("PropSite", dsNCLTablesData.Tables[2]));
                report.DataSources.Add(new ReportDataSource("PropPhoto", dsNCLTablesData.Tables[11]));
                report.DataSources.Add(new ReportDataSource("PropDimention", dsNCLTablesData.Tables[3]));
                report.DataSources.Add(new ReportDataSource("PropCoordinates", dsNCLTablesData.Tables[4]));
                report.DataSources.Add(new ReportDataSource("OwnerDet", dsNCLTablesData.Tables[5]));
                report.DataSources.Add(new ReportDataSource("Rights", dsNCLTablesData.Tables[10]));
                report.DataSources.Add(new ReportDataSource("DocsToUpl", dsNCLTablesData.Tables[9]));
                report.DataSources.Add(new ReportDataSource("Apartment", dsNCLTablesData.Tables[11]));
                report.DataSources.Add(new ReportDataSource("Kattada", dsNCLTablesData.Tables[8]));
                report.DataSources.Add(new ReportDataSource("PropSurvey", dsNCLTablesData.Tables[0]));
                report.DataSources.Add(new ReportDataSource("Liabilities", dsNCLTablesData.Tables[1]));
                report.DataSources.Add(new ReportDataSource("MOBuilding", dsNCLTablesData.Tables[2]));
                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>8.27in</PageWidth>" +
                   "  <PageHeight>11.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = Guid.NewGuid().ToString() + ".pdf";
                //   return File(bytes, mimeType, "FinalReport.pdf");
                return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetPDF");
                _logger.LogError(ex, "Error GetPDF the report.");
                throw;
            }
        }
        [HttpGet("GetMaskedMobileNumber")]
        public string MaskMobileNo(string MobileNo)
        {
            try
            {
                if ((MobileNo != "") && (MobileNo != "0") && (MobileNo.Length == 10))
                {
                    string retVal = string.Concat("".PadLeft(6, 'X'), MobileNo.Substring(MobileNo.Length - 4));
                    return retVal;
                }
                else
                {

                    return "";
                }
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetMaskedMobileNumber");
                _logger.LogError(ex, "Error GetMaskedMobileNumber the report.");
                Alert.Show(ex.Message);
                return "";
            }
        }
        [HttpGet("GetEsign")]
        public ESign GetEsign(Int64 Propertycode, Int64 booksAppNo)
        {
            try
            {
                string userId = "kmdsesign";
                string password = "7RvzXg4gWS";
                int departmentCode = 24;
                int serviceId = 1;
                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP2.rdlc")
                };
                report.DataSources.Clear();
                string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP2.rdlc");

                report.ReportPath = reportPath;
                report.Refresh();
                string Url = ConvertReportToPDF(Propertycode, booksAppNo);
                string hash = "";
                hash = GetPDFHash(Propertycode, booksAppNo, Url, _Esign.TempFiles_ctz.ToString());
                string esignxml = "";

                string returnURL = _Esign.ReturnURL_ctzEID.ToString();
                string authMode = "1";
                string txnNo = Guid.NewGuid().ToString();
                esignxml = GeteSignHashXML(userId, password, departmentCode.ToString(), "", serviceId, "eaasthi", hash, returnURL, txnNo, authMode);
                ESign s = PostDataToServer(esignxml);
                return s;
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetEsign");
                throw ex;
            }
        }

        protected string ConvertReportToPDF(Int64 Propertycode, Int64 booksAppNo)
        {
            string localPath = "";
            try
            {
                var bytes = GetPDF(Propertycode, booksAppNo);
                localPath = _Esign.TempFiles_ctz.ToString(); //take form appsettings
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                localPath = localPath + fileName;
                System.IO.File.WriteAllBytes(localPath, bytes);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "ConvertReportToPDF"); ;
            }
            return localPath;
        }
        //public string GetPDFHash(string pdfFilePath, string tmpPath)
        //{
        //    string pdfHash = "";
        //    try
        //    {
        //        // Create a new GUID-based filename
        //        string outFile = Guid.NewGuid().ToString() + ".pdf";
        //        string outFilePath = Path.Combine(tmpPath, outFile);

        //        // Open the existing PDF
        //        PdfReader pdfReader = new PdfReader(pdfFilePath);
        //        PdfSigner pdfSigner = new PdfSigner(pdfReader, new FileStream(outFilePath, FileMode.Create), new StampingProperties());

        //        // Prepare the PDF signature appearance
        //        PdfSignatureAppearance appearance = pdfSigner.GetSignatureAppearance();
        //        appearance.SetReason("Digitally Signed");
        //        appearance.SetLocation("Location");
        //        appearance.SetPageRect(new iText.Kernel.Geom.Rectangle(100, 100, 200, 50));
        //        appearance.SetPageNumber(1);  // Sign on the first page
        //        appearance.SetLayer2Text("This document is digitally signed.");
        //        appearance.SetLayer2FontSize(10);

        //        // Create the signature dictionary (empty as we're focusing on hash)
        //        PdfSignature dic = new PdfSignature(PdfName.Adobe_PPKLite, PdfName.Adbe_pkcs7_detached);
        //        dic.Reason = appearance.GetReason();
        //        dic.Location = appearance.GetLocation();
        //        dic.Date = new PdfDate(pdfSigner.GetSignDate());

        //        // Reserve space for the signature and set it to appearance
        //        pdfSigner.SetFieldName("sig");
        //        appearance.SetSignatureCreator("Your App Name");

        //        // Pre-close the document, reserving space for the signature
        //        pdfSigner.PreClose(new System.Collections.Generic.Dictionary<PdfName, int>());

        //        // Get the data to be signed (i.e., for the hash)
        //        Stream data = pdfSigner.GetRangeStream();

        //        // Calculate the SHA-256 hash using the existing CreatePDFSha256Hash function
        //        pdfHash = CreatePDFSha256Hash(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error generating PDF hash: " + ex.Message);
        //    }
        //    return pdfHash;
        //}
        private string GetPDFHash(Int64 properrtycode, Int64 BooksAppNO, string pdfFilePath, string tmpPath)
        {
            //Start generating PDF Hash
            string pdfHash = "";
            try
            {
                string outFile = Guid.NewGuid().ToString() + ".pdf";
                // Session["SignedFileName"] = outFile;
                // Response.Cookies["SignedFileName"].Value = outFile;
                PdfReader reader = new PdfReader(pdfFilePath);
                string timeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                FileStream OutputStream = new FileStream(Path.Combine(tmpPath, outFile), FileMode.CreateNew);

                PdfStamper Stamper = PdfStamper.CreateSignature(reader, OutputStream, '\0');
                PdfSignatureAppearance appearance = Stamper.SignatureAppearance;
                appearance.Reason = "Digitally Signed";
                appearance.SignDate = Convert.ToDateTime(timeStamp).AddMinutes(1);
                appearance.Acro6Layers = false;

                appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(100, 100, 300, 200), reader.NumberOfPages, "sig");
                int contentEstimated = 8192;
                Dictionary<PdfName, int> exc = new Dictionary<PdfName, int>();
                exc[PdfName.CONTENTS] = contentEstimated * 2 + 2;

                PdfSignature dic = new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
                Font font = new Font();
                font.Size = 10;
                font.SetFamily("Helvetica");
                font.SetStyle("italic");
                appearance.Layer2Font = font;

                appearance.CertificationLevel = iTextSharp.text.pdf.PdfSignatureAppearance.NOT_CERTIFIED;
                appearance.Image = null;

                dic.Reason = appearance.Reason;
                dic.Location = appearance.Location;
                dic.Contact = appearance.Contact;
                dic.Date = new PdfDate(appearance.SignDate);
                appearance.CryptoDictionary = dic;
                appearance.PreClose(exc);
                pdfHash = CreatePDFSha256Hash(appearance.GetRangeStream());
                //  Session["Stamper"] = Stamper; //DONT NEED THIS

                //  Session["OutputStream"] = OutputStream;
                //  Session["reader"] = reader;
                //    Session["sap"] = appearance;
                StorePDFSessionValues(properrtycode, BooksAppNO, OutputStream, reader, appearance);
            }
            catch (Exception ex)
            {
                // _errorLogService.LogError(ex, "GetPDFHash For Property:" + ((Convert.ToString(Session["BookModPropertyID"]) != "") ? Convert.ToString(Session["BookModPropertyID"]) : ((Convert.ToString(Session["BookModPropertyCode"]) != "") ? Convert.ToString(Session["BookModPropertyCode"]) : Convert.ToString(Session["LoginId"]))));
                _errorLogService.LogError(ex, "GetPDFHash");
            }
            return pdfHash;
        }

        private string GeteSignHashXML(string userId, string password, string departmentCode, string eTokenNo, int serviceId, string aadharHolderName, string hash, string responseURL, string transactionNo, string authMode)
        {
            string strXML = "<esigndata><departmentcode>T_DEPARTMENTCODE</departmentcode><serviceid>T_SERVICEID</serviceid><userid>T_USERID</userid><password>T_PASSWORD</password><hash>T_HASH</hash><etokenno>T_ETOKEN</etokenno><aadharholdername>T_AADHARHOLDERNAME</aadharholdername><responseurl>T_RESPONSEURL</responseurl><transactionno>T_TRANSACTIONNO</transactionno><authmode>T_AUTHMODE</authmode></esigndata>";
            try
            {
                AESUtils objCrypt = new AESUtils();
                strXML = strXML.Replace("T_DEPARTMENTCODE", departmentCode);
                strXML = strXML.Replace("T_USERID", objCrypt.EncryptStringAES(userId, "0E7EDB4DD1D34450"));
                strXML = strXML.Replace("T_PASSWORD", objCrypt.EncryptStringAES(password, "0E7EDB4DD1D34450"));
                if (eTokenNo != "")
                    strXML = strXML.Replace("T_ETOKEN", objCrypt.EncryptStringAES(eTokenNo, "0E7EDB4DD1D34450"));
                else
                    strXML = strXML.Replace("T_ETOKEN", "0E7EDB4DD1D34450");
                strXML = strXML.Replace("T_TRANSACTIONNO", transactionNo);
                strXML = strXML.Replace("T_SERVICEID", serviceId.ToString());
                strXML = strXML.Replace("T_AADHARHOLDERNAME", aadharHolderName);
                strXML = strXML.Replace("T_HASH", hash);
                strXML = strXML.Replace("T_RESPONSEURL", responseURL);
                strXML = strXML.Replace("T_AUTHMODE", authMode);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "");
            }
            return Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(strXML));
        }

        private string CreatePDFSha256Hash(Stream StreamToHash)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Byte[] result = hash.ComputeHash(StreamToHash);

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }

        #region eSignCode

        private ESign PostDataToServer(string esignXML)
        {
            try
            {
                string url = _Esign.ESignServicesURL_ctz.ToString();
                Response.Clear();
                var sb = new System.Text.StringBuilder();
                ESign s = new ESign()
                {
                    url = url,
                    ESignXML = esignXML
                };
                return s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class ESign
        {
            public string url { get; set; }
            public string ESignXML { get; set; }
        }





        #endregion

        //  E-sign Redirection Logic
        [HttpGet("E-signRediredirection")]
        public void AttachSignature(string esignxml, Int64 propertyCode, Int64 BOOK_APP_NO, string LoginId)
        {
            string signature = "", sig = "", outFile = "", Url = "", path = "";
            try
            {
                signature = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(esignxml));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(signature);
                //  XmlNodeList elemList = xmlDoc.GetElementsByTagName("EsignResp");
                // sig = elemList[0].ChildNodes[1].InnerText;
                //Start generating PDF Hash
                //PdfSignatureAppearance appearance = (PdfSignatureAppearance)Session["sap"];
                //FileStream OutputStream = (FileStream)Session["OutputStream"];
                //PdfReader reader = (PdfReader)Session["reader"]; //uploaded pdf is now open to attach signature
                var Dataset = RetrievePDFSessionValues(propertyCode, BOOK_APP_NO);
                //    PdfSignatureAppearance appearance = (PdfSignatureAppearance)Session["sap"];
                //   FileStream OutputStream = (FileStream)Session["OutputStream"];
                //    PdfReader reader = (PdfReader)Session["reader"]; //uploaded pdf is now open to attach signature

                PdfSignatureAppearance appearance = (PdfSignatureAppearance)Dataset.Item1;
                FileStream OutputStream = (FileStream)Dataset.Item2;
                PdfReader reader = (PdfReader)Dataset.Item3; //uploaded pdf is now open to attach signature

                byte[] sigbytes = Convert.FromBase64String(sig); //string format of your signature is convert to bytes.
                byte[] paddedSig = new byte[8192]; //alignment of the pdf is done.
                Array.Copy(sigbytes, 0, paddedSig, 0, sigbytes.Length);

                PdfDictionary dic2 = new PdfDictionary();
                dic2.Put(PdfName.CONTENTS, new PdfString(paddedSig).SetHexWriting(true));
                appearance.Close(dic2);
                OutputStream.Close();
                reader.Close();
            }
            catch (Exception ex1)
            {
                // _errorLogService.LogError(ex1, "AttachSignature1");
                // Alert.Show("AttachSignature1:" + ((ex1.InnerException != null) ? ex1.InnerException.Message.Replace('\n', ' ') : ex1.Message.Replace('\n', ' ')));
            }

            //try
            //{
            //    path = Server.MapPath("../TempFiles/");
            //    outFile = Session["SignedFileName"].ToString();
            //    Url = _Esign.TempURl_ctz.ToString() + outFile;

            //    byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(path, outFile));
            //    insertCitzData(bytes, propertyCode, BOOK_APP_NO, LoginId);

            //    System.Web.UI.AttributeCollection col = pdfiframe.Attributes;
            //    col.Add("src", Url);
            //}
            //catch (Exception ex2)
            //{
            //    //  _errorLogService.LogError(ex2, "AttachSignature2");
            // //   Alert.Show("AttachSignature2:" + ((ex2.InnerException != null) ? ex2.InnerException.Message.Replace('\n', ' ') : ex2.Message.Replace('\n', ' ')));
            //}
        }

        //private void insertCitzData(byte[] bytes, string propertycode, string BookAPPNo, string LoginId)
        //{
        //    DataSet dsPropDetails = objModule.Get_Ctz_ObjectionModPendingAppl("EID", Convert.ToString(LoginId), Convert.ToString(BookAPPNo), "", 0, "");
        //    try
        //    {
        //        if (dsPropDetails != null && dsPropDetails.Tables.Count > 0)
        //        {
        //            int res = objModule.INS_SUBMIT_CTZ_APPLICATION_OBJMOD(Convert.ToInt32(BookAPPNo), Convert.ToInt32(BookAPPNo), Convert.ToBase64String(bytes), Convert.ToString(LoginId));
        //            //   string mobilenumber = SMSGeneration(BookAPPNo);
        //            //    Alert.Show("Application Submitted Successfully and SMS is sent to " + mobilenumber + ". Reference No: " + Convert.ToString(BookAPPNo));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // _errorLogService.LogError(ex, "Final Submit For Application:" + Convert.ToString(Session["BOOKS_PROP_APPNO"]));
        //        Alert.Show("Application not submitted, Please try again:" + ((ex.InnerException != null) ? ex.InnerException.Message.Replace('\n', ' ') : ex.Message.Replace('\n', ' ')));
        //    }
        //}
        //private string SMSGeneration(string BookAPPNo)
        //{
        //    try
        //    {
        //        NUPMS_BA.Report_BA RBA = new NUPMS_BA.Report_BA();

        //        string TEMPLAETID = "23";
        //        Session["TEMPLAETID"] = TEMPLAETID;
        //        Session["SENTFROM"] = "EAASTHI";

        //        DataSet dsPropDetails = RBA.MutationnOTICEDetails(TEMPLAETID);
        //        string TEMPLATETEXT = dsPropDetails.Tables[0].Rows[0]["TEMPLATETEXT"].ToString();
        //        string SMSTEMPLATEID = dsPropDetails.Tables[0].Rows[0]["SMSTEMPLATEID"].ToString();
        //        Session["SMSTEMPLATEID"] = SMSTEMPLATEID;

        //        DataSet dtMobileNumber = objModule.GETPropertyMobileNumberForSMS(Convert.ToInt32(BookAPPNo));
        //        if (dtMobileNumber != null && dtMobileNumber.Tables.Count > 0 && dtMobileNumber.Tables[0].Rows.Count > 0)
        //        {
        //            Session["MOBILENUMBER"] = dtMobileNumber.Tables[0].Rows[0]["MOBILENUMBER"].ToString().Trim();
        //            Session["ULBNAME"] = dtMobileNumber.Tables[1].Rows[0]["ULBNAME"].ToString();
        //            TEMPLATETEXT = TEMPLATETEXT.Replace("{#ULBNane#}", Session["ULBNAME"].ToString());
        //            TEMPLATETEXT = TEMPLATETEXT.Replace("{#Date#}", DateTime.Now.ToString());
        //            TEMPLATETEXT = TEMPLATETEXT.Replace("{#refno#}", Session["BOOKS_PROP_APPNO"].ToString());

        //            string MOBILENO = Session["MOBILENUMBER"].ToString();
        //            string SECRET_KEY = ConfigurationManager.AppSettings["BBMP_SECRET_KEY_ctz"].ToString();
        //            string SENDER_ADDRESS = ConfigurationManager.AppSettings["BBMP_SENDER_ADDRESS_ctz"].ToString();

        //            EAS_BA objEAS = new EAS_BA();
        //            objEAS.SendSMS(SECRET_KEY, SENDER_ADDRESS, MOBILENO, TEMPLATETEXT, Session["ULB"].ToString());
        //        }
        //        return Convert.ToString(Session["MOBILENUMBER"]);
        //    }
        //    catch (Exception ex)
        //    {
        //      //  _errorLogService.LogError(ex, "SMS for Final Submit:" + Convert.ToString(Session["BOOKS_PROP_APPNO"]) + ((ex.InnerException != null) ? ex.InnerException.Message.Replace('\n', ' ') : ex.Message.Replace('\n', ' ')));
        //        Alert.Show("Application Submitted Successfully but SMS is failed. Reference No: " + Convert.ToString(Session["BOOKS_PROP_APPNO"]));
        //        return "";
        //    }
        //}



        private int StorePDFSessionValues(Int64 propertycode, Int64 bookspropappno, FileStream sessionId, PdfReader pdfFilePath, PdfSignatureAppearance appearance)
        {
            // Convert FileStream (sessionId) to byte array
            byte[] fileContent;
            using (MemoryStream ms = new MemoryStream())
            {
                sessionId.CopyTo(ms);
                fileContent = ms.ToArray();
            }

            // Serialize PdfSignatureAppearance object to JSON
            var signatureData = new
            {
                Reason = appearance.Reason,
                SignDate = appearance.SignDate,
                CertificationLevel = appearance.CertificationLevel,
                Layer2Font = new
                {
                    appearance.Layer2Font.Familyname,
                    appearance.Layer2Font.Size
                }
            };
            string serializedAppearance = JsonConvert.SerializeObject(signatureData);
            _BBMPBookService.Ins_EsignPDf(propertycode, bookspropappno, fileContent, serializedAppearance);


            // Bind parameters
            //    command.Parameters.Add(new OracleParameter("session_id", Guid.NewGuid().ToString())); // Create a unique ID
            //   command.Parameters.Add(new OracleParameter("pdf_file_content", fileContent)); // Store file content as BLOB
            //   command.Parameters.Add(new OracleParameter("pdf_signature_data", serializedAppearance)); // Store appearance data as JSON in CLOB

            return 1;
        }
        public class PdfSignatureAppearanceDto
        {
            public string Reason { get; set; }
            public DateTime SignDate { get; set; }
            public string FontFamily { get; set; }
            public float FontSize { get; set; }
            public string CertificationLevel { get; set; }
            // Add any other fields from PdfSignatureAppearance you want to store
        }


        private (PdfSignatureAppearance, FileStream, PdfReader) RetrievePDFSessionValues(Int64 propertyCode, Int64 BOOK_APP_NO)
        {
            try
            {
                // Call the service method to get the DataSet containing the stored PDF and signature data
                DataSet dataSet = _BBMPBookService.Get_ESignPdf(propertyCode, BOOK_APP_NO);

                // Check if the DataSet contains data
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    // Retrieve PDF content from the "BLOB" in the database
                    byte[] pdfFileContent = (byte[])dataSet.Tables[0].Rows[0]["PDF_FILE_CONTENT"];
                    var localPath = _Esign.TempFiles_ctz.ToString(); //take form appsettings
                    string fileName = Guid.NewGuid().ToString() + ".pdf";
                    localPath = localPath + fileName;
                    System.IO.File.WriteAllBytes(localPath, pdfFileContent);

                    // Retrieve signature data (stored as JSON or another serialized format)
                    string signatureData = dataSet.Tables[0].Rows[0]["PDF_SIGNATURE_DATA"].ToString(); // Assuming it's stored as JSON

                    // Reconstruct PdfReader from the file content (in-memory)
                    PdfReader reader;
                    using (MemoryStream pdfStream = new MemoryStream(pdfFileContent))
                    {
                        reader = new PdfReader(pdfStream); // Open PDF for signature
                    }

                    // Create a temporary file for output (this would be used for signing the PDF later)
                    string tempFilePath = Path.GetTempFileName(); // Generate a temp file path
                    FileStream outputStream = new FileStream(tempFilePath, FileMode.OpenOrCreate);

                    // Create a PdfStamper to apply the signature
                    PdfStamper stamper = PdfStamper.CreateSignature(reader, outputStream, '\0');

                    // Access the PdfSignatureAppearance object from the stamper
                    PdfSignatureAppearance signatureAppearance = stamper.SignatureAppearance;

                    // Deserialize PdfSignatureAppearance from the stored JSON data (into a DTO)
                    var appearanceDto = JsonConvert.DeserializeObject<PdfSignatureAppearanceDto>(signatureData);

                    // Manually recreate the PdfSignatureAppearance object from the DTO
                    signatureAppearance.Reason = appearanceDto.Reason;
                    signatureAppearance.SignDate = appearanceDto.SignDate;

                    signatureAppearance.Layer2Font = new iTextSharp.text.Font();
                    signatureAppearance.Layer2Font.SetFamily(appearanceDto.FontFamily);
                    signatureAppearance.Layer2Font.Size = appearanceDto.FontSize;
                    signatureAppearance.CertificationLevel = int.Parse(appearanceDto.CertificationLevel);
                    // Add any other properties that need to be set based on the DTO

                    // Return the tuple containing PdfSignatureAppearance, FileStream, and PdfReader
                    return (signatureAppearance, outputStream, reader);
                }

                // If no data found, return null values
                return (null, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving PDF session values.");
                throw;
            }
        }
        public class PropertyData
        {

            public string? PropertyCode { get; set; }
            public string? ProperytyId { get; set; }
            public string? BookNumber { get; set; }
            public string? BookId { get; set; }

        }

        private async Task<string> GetDraftKhataDownloadURL(PropertyData propertyData)
        {
         //   string url = "http://10.10.133.197/dataapi/api/eaasthidata/GetDraft";
            string url = _Esign.DraftURL;
            _logger.LogError(url, "DraftURL");
            // Create an HTTP client to send the request
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Prepare the JSON request body
                    var requestBody = new
                    {
                        PropertyCode = propertyData.PropertyCode,
                        ProperytyId = propertyData.ProperytyId,
                        BookNumber = propertyData.BookNumber,
                        BookId = propertyData.BookId
                    };

                    // Serialize the request body to JSON
                    var jsonRequest = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    // Send the POST request with the JSON body
                    var response = await httpClient.PostAsync(url, content);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
                        _BBMPBookService.Ins_PDF_Draft_Exception_log(propertyData.PropertyCode, propertyData.ProperytyId, responseObject?.status, responseObject?.outputDocument,"SUCCESS");
                        // Check the status and return the outputDocument URL
                        if (responseObject?.status == "success")
                        {

                            return responseObject.outputDocument;
                        }
                        else
                        {
                            return "Failed to retrieve the document.";
                        }
                    }
                    else
                    {
                        return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    
                    _logger.LogError(ex, "Error occurred while retrieving GetDraftKhataDownloadURL");
                    _errorLogService.LogError(ex, "GetDraftKhataDownloadURL");
                    return $"Internal server error: {ex.Message}";
                }
            }
        }
        public class ResponseModel
        {
            public string status { get; set; }
            public string outputDocument { get; set; }
            public string response { get; set; }
        }

        [HttpPost("DownloadDraftPDF")]
        public async Task<IActionResult> GetDraftDownload(PropertyData propertyData)
        {
            //url will be sent as a parameter.
            _logger.LogInformation("url coming from GetDraftKhataDownloadURL" + "bookid:" + propertyData.BookId, "booknumber:" + propertyData.BookNumber, "bookpropertycode:" + propertyData.PropertyCode, "propertyid:" + propertyData.ProperytyId);
            // string url = "http://10.10.133.197/eaasthirestapi/TempFiles/thushar.pdf";


            // string url = "http://10.10.133.197/eaasthirestapi/api/eaasthidata/GetDraft";

            string url = await GetDraftKhataDownloadURL(propertyData);
            _logger.LogError($"url coming from GetDraftKhataDownloadURL: {url}");




            using (var httpClient = new HttpClient())
            {
                try
                {  

                    var response = await httpClient.GetAsync(url);


                    if (response.IsSuccessStatusCode)
                    {

                        byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();


                        string mimeType = "application/pdf";


                        return File(pdfBytes, mimeType, "DraftReport.pdf");
                    }
                    else
                    {
                        return BadRequest("Failed to download the PDF.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    _logger.LogError(ex, "Error occurred while retrieving DownloadDraftPDF");
                    _errorLogService.LogError(ex, "DownloadDraftPDF");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
        [HttpPost("DownloadPagePDF")]
        public async Task<IActionResult> GetPageDocumentDownload(string BookNo, string pageno)
        {
           
            _logger.LogInformation("url coming from GetPageDocumentDownload" + "bookno:" + BookNo, "pageno:" + pageno );
            DataSet dsPageData = objBbd.GetPdf(int.Parse(BookNo.ToString()), int.Parse(pageno.Trim()));
            try
            {
                if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                {
                    byte[] pdfBytes;
                    pdfBytes = await System.IO.File.ReadAllBytesAsync(dsPageData.Tables[0].Rows[0]["FILEPATH"].ToString());
                    string mimeType = "application/pdf";
                    string base64String4 = Convert.ToBase64String(pdfBytes, 0, pdfBytes.Length);
                 
                   return Ok(base64String4);
                }
                else
                {
                    return BadRequest("Failed to download the PDF.");
                }
            }

            catch (Exception ex)
            {
                // Log the exception (optional)
               
                _logger.LogError(ex, "Error occurred while retrieving DownloadDraftPDF");
                _errorLogService.LogError(ex, "DownloadDraftPDF");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetEAASTHIStatus")]
        public  ActionResult<DataSet> GetEAASTHIStatus()
        {
            try
            {
                DataSet dataSet =  _BBMPBookService.GetEAASTHIStatus();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving Get_EAASTHI_Status");
                _errorLogService.LogError(ex, "Get_EAASTHI_Status");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetEAASTHIDailyReport")]
        public ActionResult<DataSet> GetEAASTHIDailyReport()
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GetEAASTHIDailyReport();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving Get_EAASTHI_Status");
                _errorLogService.LogError(ex, "Get_EAASTHI_Status");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetEAASTHIDailyReportDetails")]
        public ActionResult<DataSet> GetEAASTHIDailyReportDetails(int wardNumber, string QueryName, int pageNo, int PageSize)
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GetEAASTHIDailyReportDetails(wardNumber,  QueryName,  pageNo,  PageSize);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving Get_EAASTHI_Status");
                _errorLogService.LogError(ex, "Get_EAASTHI_Status");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GET_ARO_BY_ZONE")]
        public ActionResult<DataSet> GET_ARO_BY_ZONE(int zoneid)
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GET_ARO_BY_ZONE(zoneid);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving Get_EAASTHI_Status");
                _errorLogService.LogError(ex, "Get_EAASTHI_Status");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GET_WARD_BY_ARO")]
        public ActionResult<DataSet> GET_WARD_BY_ARO(int AROID)
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GET_WARD_BY_ARO(AROID);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving Get_EAASTHI_Status");
                _errorLogService.LogError(ex, "Get_EAASTHI_Status");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GET_PENDENCE_REPORT")]
        public ActionResult<DataSet> GET_PENDENCE_REPORT(int ZoneId,int AROId,int WARDID,string SEARCHTYPE)
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GET_PENDENCE_REPORT(ZoneId, AROId, WARDID, SEARCHTYPE);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GET_PENDENCE_REPORT");
                _errorLogService.LogError(ex, "GET_PENDENCE_REPORT");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GET_PENDENCE_REPORT_DETAILS")]
        public ActionResult<DataSet> GET_PENDENCE_REPORT_DETAILS(int WARDID, string PROPERTYID, string TYPEOFROLE, int PAGENO, int PAGECOUNT)
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GET_PENDENCE_REPORT_DETAILS(WARDID, PROPERTYID,  TYPEOFROLE, PAGENO, PAGECOUNT);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GET_PENDENCE_REPORT_DETAILS");
                _errorLogService.LogError(ex, "GET_PENDENCE_REPORT_DETAILS");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetECDailyReport")]
        public ActionResult<DataSet> GetECDailyReport()
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GetECDailyReport();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GET_PENDENCE_REPORT_DETAILS");
                _errorLogService.LogError(ex, "GET_PENDENCE_REPORT_DETAILS");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetMutationDailyReport")]
        public ActionResult<DataSet> GetMutationDailyReport()
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GetMutationDailyReport();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GET_PENDENCE_REPORT_DETAILS");
                _errorLogService.LogError(ex, "GET_PENDENCE_REPORT_DETAILS");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetPublicNoticesReport")]
        public ActionResult<DataSet> GetPublicNoticesReport(int PAGENO,int PAGECOUNT)
        {
            try
            {
                DataSet dataSet = _BBMPBookService.MUTATION_NOTICES(555, PAGENO, PAGECOUNT);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GET_PENDENCE_REPORT_DETAILS");
                _errorLogService.LogError(ex, "GET_PENDENCE_REPORT_DETAILS");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetDraftDownloaded")]
        public ActionResult<DataSet> GetDraftDownloaded()
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GetDraftDownloaded();
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GetDraftDownloaded");
                _errorLogService.LogError(ex, "GetDraftDownloaded");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("DownloadNoticePDF")]
        public async Task<IActionResult> DownloadNoticePDF(int MutApplId, int Propcode)
        {

            _logger.LogInformation("url coming from GetPageDocumentDownload" + "bookno:" + MutApplId, "pageno:" + Propcode);
           
            DataSet dsPageData = WF.GetNoticeImage(MutApplId, Propcode);
            try
            {
                if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                {
                    byte[] pdfBytes = (byte[])dsPageData.Tables[0].Rows[0]["PDFNOTICE"];
                    string filename = MutApplId + "SavedPropNotice" + Propcode + ".pdf";
                    return File(pdfBytes, "application/pdf", filename);
                }
                else
                {
                    return BadRequest("Failed to download the PDF.");
                }
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving DownloadDraftPDF");
                _errorLogService.LogError(ex, "DownloadDraftPDF");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetFinalSearchAcknowledgementReport")]
        public IActionResult GetFinalSearchAcknowledgementReport( int SearchReqID)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();

                DataSet dsReportData = _SearchService.SEL_CitzeSearchAck(Convert.ToInt32(SearchReqID));
                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzSearchPropertyAck.rdlc")
                };


                report.DataSources.Clear();

                string SASNO = "";
                if (Convert.ToString(dsReportData.Tables[0].Rows[0]["IS_SAS_APPLICATIONO"]) == "Y"){
                    SASNO = "No SAS Number Available";
                }
                else
                {
                    SASNO = Convert.ToString(dsReportData.Tables[0].Rows[0]["SASAPPLICATIONNO"]);
                }
                // Set up parameters
               
                ReportParameter[] param = new ReportParameter[7];
           
                param[0] = new ReportParameter("P_BOOKS_PROP_APPNO", "S-" + Convert.ToString(SearchReqID));
                param[1] = new ReportParameter("P_Hname", "Property Search Request");
                param[2] = new ReportParameter("P_LANGUAGE", "0"); 
                param[3] = new ReportParameter("P_ZONENAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["ZONENAME"])); 
                param[4] = new ReportParameter("P_WARD_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["WARD_NAME"])); 
                param[5] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["SEARCHNAME"])); 
                param[6] = new ReportParameter("P_SASAPPLICATIONNUMBER", SASNO); 
              


                DataTable ds = new DataTable();
                report.DataSources.Add(new ReportDataSource("DataSet1", ds));
                report.DataSources.Add(new ReportDataSource("PropSite", ds));
                report.DataSources.Add(new ReportDataSource("PropPhoto", ds));
                report.DataSources.Add(new ReportDataSource("PropDimention", ds));
                report.DataSources.Add(new ReportDataSource("PropCoordinates", ds));
                report.DataSources.Add(new ReportDataSource("OwnerDet", ds));
                report.DataSources.Add(new ReportDataSource("Rights", ds));
                report.DataSources.Add(new ReportDataSource("DocsToUpl", ds));
                report.DataSources.Add(new ReportDataSource("Apartment", ds));
                report.DataSources.Add(new ReportDataSource("Kattada", ds));
                report.DataSources.Add(new ReportDataSource("PropSurvey", ds));
                report.DataSources.Add(new ReportDataSource("Liabilities", ds));
                report.DataSources.Add(new ReportDataSource("MOBuilding", ds));
                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>8.90in</PageWidth>" +
                   "  <PageHeight>10.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = "Search Request Acknowledgement";
                return File(bytes, mimeType, "Search Request Acknowledgement.pdf");
                // return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "ObjectorsAcknowledgement");
                _logger.LogError(ex, "Error ObjectorsAcknowledgement function the report.");
                throw;
            }
        }
        [HttpGet("GetFinalMutationAcknowledgementReport")]
        public IActionResult GetFinalMutationAcknowledgementReport(int mutationRequestId)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();

                DataSet dsReportData = _MutationService.SEL_Citz_Mutation_Objection_Acknowledgement(Convert.ToInt32(mutationRequestId));
                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzMutationObjectionPropertyAck.rdlc")
                };


                report.DataSources.Clear();

                string SASNO = "";
              
                // Set up parameters

                ReportParameter[] param = new ReportParameter[7];

                param[0] = new ReportParameter("P_BOOKS_PROP_APPNO", "M-" + Convert.ToString(mutationRequestId));
                param[1] = new ReportParameter("P_Hname", "Mutation Objection Request");
                param[2] = new ReportParameter("P_LANGUAGE", "0");
                param[3] = new ReportParameter("P_ZONENAME", "thushar");
                param[4] = new ReportParameter("P_WARD_NAME", "the great");
                param[5] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["MUT_OBJECTOR_NAME_EN"]));
                param[6] = new ReportParameter("P_REASONDETAILS", Convert.ToString(dsReportData.Tables[0].Rows[0]["REASONDETAILS"]));



                DataTable ds = new DataTable();
                report.DataSources.Add(new ReportDataSource("DataSet1", ds));
                report.DataSources.Add(new ReportDataSource("PropSite", ds));
                report.DataSources.Add(new ReportDataSource("PropPhoto", ds));
                report.DataSources.Add(new ReportDataSource("PropDimention", ds));
                report.DataSources.Add(new ReportDataSource("PropCoordinates", ds));
                report.DataSources.Add(new ReportDataSource("OwnerDet", ds));
                report.DataSources.Add(new ReportDataSource("Rights", ds));
                report.DataSources.Add(new ReportDataSource("DocsToUpl", ds));
                report.DataSources.Add(new ReportDataSource("Apartment", ds));
                report.DataSources.Add(new ReportDataSource("Kattada", ds));
                report.DataSources.Add(new ReportDataSource("PropSurvey", ds));
                report.DataSources.Add(new ReportDataSource("Liabilities", ds));
                report.DataSources.Add(new ReportDataSource("MOBuilding", ds));
                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>8.90in</PageWidth>" +
                   "  <PageHeight>10.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = "Mutation Objection Acknowledgement";
                return File(bytes, mimeType, "Mutation Objection Acknowledgement.pdf");
                // return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "Mutation Objection");
                _logger.LogError(ex, "Error Mutation Objection function the report.");
                throw;
            }
        }
        [HttpGet("GET_Final_eKhatha_Status_Based_on_ePID")]
        public ActionResult<DataSet> GET_Final_eKhatha_Status_Based_on_ePID(string PropertyEpid )
        {
            try
            {
                DataSet dataSet = _BBMPBookService.GET_Final_eKhatha_Status_Based_on_ePID(PropertyEpid);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);
                return Ok(json);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)

                _logger.LogError(ex, "Error occurred while retrieving GET_Final_eKhatha_Status_Based_on_ePID");
                _errorLogService.LogError(ex, "GET_Final_eKhatha_Status_Based_on_ePID");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetNEwFinalBBMPReport")]
        public IActionResult GetNEwFinalBBMPReport(int propertycode, int BOOKS_PROP_APPNO, string LoginId)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();
                DataSet dsNCLTablesData = objModule.GET_PROPERTY_PENDING_NEW_CITZ_NCLTEMP(555, BOOKS_PROP_APPNO, propertycode); // Data from NCL temp tables  
                DataSet dsNCLTablesDataDummy = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(propertycode);

                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP3.rdlc")
                };


                report.DataSources.Clear();

                int rcount = 0;
                // Set up parameters
              //  DataSet dsReportData = objModule.SEL_NEWCitzeKhataAcknowledgement(Convert.ToInt32(BOOKS_PROP_APPNO), Convert.ToInt32(propertycode), Convert.ToString(LoginId));
                ReportParameter[] param = new ReportParameter[20];

                //param[7] = new ReportParameter("P_ZONENAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["ZONENAME"]));
                //param[8] = new ReportParameter("SUB_DIVISION_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["SUB_DIVISION_NAME"]));
                //param[9] = new ReportParameter("P_WARD_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["WARD_NAME"]));
                //param[10] = new ReportParameter("P_DOORSITENO", Convert.ToString(dsReportData.Tables[0].Rows[0]["DOORNO"]));
                //param[11] = new ReportParameter("P_BUIDINGNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["BUILDINGNAME"]));
                //param[12] = new ReportParameter("P_STREETNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["STREETNAME"]));
                //param[13] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTNAME"]));
                //param[14] = new ReportParameter("P_APPLICANTPOSTALADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTPOSTALADDRESS"]));
                //param[15] = new ReportParameter("P_BOOKS_PROP_APPNO", "K-" + Convert.ToString(BOOKS_PROP_APPNO));

                //param[6] = new ReportParameter("P_Hname", Convert.ToString(dsReportData.Tables[0].Rows[0]["ULB_DISPLAYNAME"]));
                //param[3] = new ReportParameter("P_USERTYPE", "CITIZEN");
                //param[3] = new ReportParameter("P_USERTYPE", "NA");
                //param[0] = new ReportParameter("P_OPRNAME", "NA");
                //param[1] = new ReportParameter("P_CENTERNAME", "NA");
                //param[2] = new ReportParameter("P_PROPERTYCATEGORYID", Convert.ToString(dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]));
                //param[4] = new ReportParameter("P_LANGUAGE", "0"); //English

                /////////////------------------------------------------------///////////
                param[7] = new ReportParameter("P_ZONENAME", Convert.ToString("bangalore"));
                param[8] = new ReportParameter("SUB_DIVISION_NAME", Convert.ToString("bangalore"));
                param[9] = new ReportParameter("P_WARD_NAME", Convert.ToString("bangalore"));
                param[10] = new ReportParameter("P_DOORSITENO", Convert.ToString("bangalore"));
                param[11] = new ReportParameter("P_BUIDINGNAME1", Convert.ToString("bangalore"));
                param[12] = new ReportParameter("P_STREETNAME1", Convert.ToString("bangalore"));
                param[13] = new ReportParameter("P_APPLICANTNAME", Convert.ToString("bangalore"));
                param[14] = new ReportParameter("P_APPLICANTPOSTALADDRESS", Convert.ToString("bangalore"));
                param[15] = new ReportParameter("P_BOOKS_PROP_APPNO", "K-" + Convert.ToString(BOOKS_PROP_APPNO));

                param[6] = new ReportParameter("P_Hname", Convert.ToString("BBMP"));
                param[3] = new ReportParameter("P_USERTYPE", "CITIZEN");
                param[3] = new ReportParameter("P_USERTYPE", "NA");
                param[0] = new ReportParameter("P_OPRNAME", "NA");
                param[1] = new ReportParameter("P_CENTERNAME", "NA");
                param[2] = new ReportParameter("P_PROPERTYCATEGORYID", Convert.ToString(dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"]));
                param[4] = new ReportParameter("P_LANGUAGE", "0"); //English


                //   param[4] = new ReportParameter("P_LANGUAGE", "1"); //Kannada
                //  if (mode == "final")
                // {
                param[16] = new ReportParameter("P_ACK_DRAFT", "N");//Final
                //}
                //else
                //{
                //    param[16] = new ReportParameter("P_ACK_DRAFT", "Y");//Draft       
                //}

                param[17] = new ReportParameter("ARO_ADDRESS", Convert.ToString("bangalore"));
                param[18] = new ReportParameter("P_REASON", Convert.ToString("bangalore"));
                param[19] = new ReportParameter("P_ARO_MOBILE", Convert.ToString("bangalore"));

                //param[17] = new ReportParameter("ARO_ADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["ARO_ADDRESS"]));
                //param[18] = new ReportParameter("P_REASON", Convert.ToString(dsReportData.Tables[0].Rows[0]["REASON"]));
                //param[19] = new ReportParameter("P_ARO_MOBILE", Convert.ToString(dsReportData.Tables[0].Rows[0]["ARO_MOBILE"]));



                foreach (DataRow row in dsNCLTablesData.Tables[5].Rows)
                {
                    if (row["OWNERIDENTITYSLNO"] != DBNull.Value)
                    {
                        string ownerMobileNo = MaskMobileNo(dsNCLTablesData.Tables[5].Rows[rcount]["MOBILENUMBER"].ToString());
                        row["MOBILENUMBER"] = ownerMobileNo;
                        //if (ownerMobileNo == "")
                        //{
                        //    return;
                        //}
                    }
                    rcount++;
                }

                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["APARTMENTLANDPID"] == DBNull.Value)
                    {
                        row["APARTMENTLANDPID"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["MUNICIPALOLDNUMBER"] == DBNull.Value)
                    {
                        row["MUNICIPALOLDNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[1].Rows)
                {
                    if (row["ASSESMENTNUMBER"] == DBNull.Value)
                    {
                        row["ASSESMENTNUMBER"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesDataDummy.Tables[0].Rows)
                {
                    if (row["surveyno"] == DBNull.Value)
                    {
                        row["surveyno"] = "N.A.";
                    }
                }
                foreach (DataRow row in dsNCLTablesData.Tables[4].Rows)
                {
                    if (row["longitude"] == DBNull.Value)
                    {
                        row["longitude"] = "N.A.";
                    }
                    if (row["latitude"] == DBNull.Value)
                    {
                        row["latitude"] = "N.A.";
                    }
                }

                foreach (DataRow row in dsNCLTablesData.Tables[5].Rows)
                {
                    if (row["ISCOMPANY"] != DBNull.Value)
                    {
                        if (row["ISCOMPANY"].ToString() == "N")
                        {
                            param[5] = new ReportParameter("P_ISCOMPANY", "N");
                        }
                        else
                        {
                            param[5] = new ReportParameter("P_ISCOMPANY", "Y");
                        }
                    }
                    else
                    {
                        param[5] = new ReportParameter("P_ISCOMPANY", "N");
                    }

                }

                foreach (DataRow row in dsNCLTablesData.Tables[9].Rows)
                {
                    if (row["DOCUMENTDETAILS"] == DBNull.Value)
                    {
                        row["DOCUMENTDETAILS"] = "N.A.";
                    }
                }


                DataTable ds = new DataTable();

                report.DataSources.Add(new ReportDataSource("DataSet1", dsNCLTablesData.Tables[1]));
                report.DataSources.Add(new ReportDataSource("PropSite", dsNCLTablesData.Tables[2]));
                report.DataSources.Add(new ReportDataSource("PropPhoto", dsNCLTablesData.Tables[11]));
                report.DataSources.Add(new ReportDataSource("PropDimention", dsNCLTablesData.Tables[3]));
                report.DataSources.Add(new ReportDataSource("PropCoordinates", dsNCLTablesData.Tables[4]));
                report.DataSources.Add(new ReportDataSource("OwnerDet", dsNCLTablesData.Tables[5]));
                //report.DataSources.Add(new ReportDataSource("Rights", dsNCLTablesData.Tables[10]));
                report.DataSources.Add(new ReportDataSource("DocsToUpl", dsNCLTablesData.Tables[8]));
                report.DataSources.Add(new ReportDataSource("Apartment", dsNCLTablesData.Tables[6]));
                report.DataSources.Add(new ReportDataSource("Kattada", ds));

                report.DataSources.Add(new ReportDataSource("PropSurvey", ds));
                //report.DataSources.Add(new ReportDataSource("Liabilities", dsNCLTablesData.Tables[1]));
                //report.DataSources.Add(new ReportDataSource("MOBuilding", dsNCLTablesData.Tables[2]));

                report.DataSources.Add(new ReportDataSource("ClassificationDocs", ds));
                report.DataSources.Add(new ReportDataSource("AddressDet", dsNCLTablesData.Tables[10]));
                report.DataSources.Add(new ReportDataSource("SAS", dsNCLTablesData.Tables[11]));
                report.DataSources.Add(new ReportDataSource("KaveriDoc", dsNCLTablesData.Tables[12]));
                report.DataSources.Add(new ReportDataSource("KaveriPropDet", dsNCLTablesData.Tables[13]));
                report.DataSources.Add(new ReportDataSource("KaveriPartyDet", dsNCLTablesData.Tables[14]));
                report.DataSources.Add(new ReportDataSource("BESCOM", dsNCLTablesData.Tables[17]));
                report.DataSources.Add(new ReportDataSource("CompareMatrix", dsNCLTablesData.Tables[18]));
                report.DataSources.Add(new ReportDataSource("BookData", ds));
                report.DataSources.Add(new ReportDataSource("BookOwnerDet", ds));
                report.DataSources.Add(new ReportDataSource("BookDimension", ds));
                report.DataSources.Add(new ReportDataSource("BookArea", ds));
                report.DataSources.Add(new ReportDataSource("BookPropCategory", ds));
                report.DataSources.Add(new ReportDataSource("OddSiteSide", dsNCLTablesData.Tables[21]));
                report.DataSources.Add(new ReportDataSource("BWSSB", dsNCLTablesData.Tables[22]));
                report.DataSources.Add(new ReportDataSource("KaveriDet", dsNCLTablesData.Tables[23]));
                report.DataSources.Add(new ReportDataSource("KaveriECDet", dsNCLTablesData.Tables[15]));

                report.SetParameters(param);
                report.Refresh();
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string deviceInfo = "<DeviceInfo>" +
                   "  <OutputFormat>PDF</OutputFormat>" +
                   "  <PageWidth>10in</PageWidth>" +
                   "  <PageHeight>11.69in</PageHeight>" +
                   "  <MarginTop>0.2in</MarginTop>" +
                   "  <MarginLeft>0.2in</MarginLeft>" +
                   "  <MarginRight>0.2in</MarginRight>" +
                   "  <MarginBottom>0.2in</MarginBottom>" +
                   "</DeviceInfo>";

                Warning[] warnings;
                string[] streamIds;
                string extension = string.Empty;
                byte[] bytes = report.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string fileName = String.Empty;
                fileName = Guid.NewGuid().ToString() + ".pdf";
                return File(bytes, mimeType, "FinalReport.pdf");
                // return bytes;

            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GetFinalBBMPReport");
                _logger.LogError(ex, "Error GetFinalBBMPReport function the report.");
                throw;
            }
        }
    }
}



