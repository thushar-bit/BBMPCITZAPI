﻿using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.NETCore;
using NUPMS_BA;
using NUPMS_BO;
using System.Data;
using System.Security.Cryptography;
using System.Text;




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

        public ReportsController(ILogger<ReportsController> logger, IConfiguration configuration, IOptions<ESignSettings> eSignSettings, INameMatchingService NameMatchService)
        {
            _logger = logger;
            _Esign = eSignSettings.Value;
            _NameMatchService = NameMatchService;
        }

        NUPMS_BA.ObjectionModuleBA objModule = new NUPMS_BA.ObjectionModuleBA();

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

                if (dsNCLTablesData.Tables[19].Rows.Count == 0)
                {
                    return "Bescom Details not Saved";
                }


                return "SUCCESS";
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FinalSubmitValidations function reportcontroller");
                throw ex;
            }
        }

        private int OWNER_COUNT_NOTIN_BOOKS(DataTable dsNCLTable,DataTable dsBBDTable)
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
            }catch(Exception ex)
            {
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

                if (dsNCLTablesData != null && dsNCLTablesData.Tables.Count > 0 && dsNCLTablesData.Tables[0].Rows.Count > 0 && Convert.ToInt32(dsNCLTablesData.Tables[0].Rows[0][0]) > 0)
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
                        objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_KAVERIDOC = GetOWNERMATCHEDWITH_KAVERIDOC(dsNCLTablesData, objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_KAVERIDOC,LoginId);
                        objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_KAVERIEC = GetOWNERMATCHEDWITH_KAVERIEC(dsNCLTablesData, objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNER_COUNT_KAVERIEC,LoginId);
                        objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO.OWNERMATCHEDWITH_SASDATA = GetOWNERMATCHEDWITH_SASDATA(dsNCLTablesData.Tables[12], dsNCLTablesData,LoginId);
                          objModule.AssignRefferalCodeForeKhataApplication(objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO);
                        objModule.UPD_NCL_PROPERTY_COMPARE_MATRIX_TEMP(Convert.ToInt64(BOOKS_PROP_APPNO), Convert.ToInt64(propertycode), Convert.ToString(LoginId), objNCL_PROPERTY_COMPARE_MATRIX_TEMP_BO);
                        return "SUCCESS";
                    }
                    else
                    {

                        return result;
                    }
                }
                return "Please Save the Values";
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FinalSubmitValidation api  reportcontroller");
                throw ex;
            }
        }
        private string OWNERMATCHEDWITH_BOOKS(DataSet dsNCLTablesData,DataSet dsBBTablesData)
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
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error in OWNERMATCHEDWITH_BOOKS function  reportcontroller");
                throw ex;
            }
        }

        private string GetOWNERMATCHEDWITH_KAVERIDOC(DataSet dsNCLTablesData, int OWNER_COUNT_KAVERIDOC,string LoginId)
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
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetOWNERMATCHEDWITH_KAVERIDOC function  reportcontroller");
                throw ex;
            }
        }

        private string GetOWNERMATCHEDWITH_KAVERIEC(DataSet dsNCLTablesData, int OWNER_COUNT_KAVERIEC,string LoginId)
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetOWNERMATCHEDWITH_KAVERIEC function  reportcontroller");
                throw ex;
            }
        }

        private string GetOWNERMATCHEDWITH_SASDATA(DataTable dtSASData,DataSet dsNCLTablesData, string LoginId)
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
                objModule.INS_UPD_NCL_PROPERTY_SAS_APP_NAMEMATCH_TEMP(Convert.ToInt64(dsNCLTablesData.Tables[5].Rows[0]["BOOKS_PROP_APPNO"]), Convert.ToInt64(dsNCLTablesData.Tables[5].Rows[0]["PROPERTYCODE"]), objNameMatchingResult.OwnerNo, SASOwnerName, objNameMatchingResult.EKYCOwnerNo, objNameMatchingResult.EKYCOwnerName, objNameMatchingResult.NameMatchScore, Convert.ToString(LoginId));
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
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzeKhataAcknowledgement.rdlc")
                };


                report.DataSources.Clear();

                int rcount = 0;
                // Set up parameters
                DataSet dsReportData = objModule.SEL_CitzeKhataAcknowledgement(Convert.ToInt32(BOOKS_PROP_APPNO), Convert.ToInt32(propertycode), Convert.ToString(LoginId));
                ReportParameter[] param = new ReportParameter[16];

                param[0] = new ReportParameter("P_OPRNAME", "NA");
                param[1] = new ReportParameter("P_CENTERNAME", "NA");
                param[2] = new ReportParameter("P_PROPERTYCATEGORYID", dsNCLTablesData.Tables[1].Rows[0]["PROPERTYCATEGORYID"].ToString());
                param[3] = new ReportParameter("P_USERTYPE", "CITIZEN");
                param[4] = new ReportParameter("P_LANGUAGE", "0");
                param[5] = new ReportParameter("P_ISCOMPANY", dsNCLTablesData.Tables[5].Rows.Count > 0 && dsNCLTablesData.Tables[5].Rows[0]["ISCOMPANY"].ToString() == "N" ? "N" : "Y");
                param[6] = new ReportParameter("P_Hname", "Bruhat Bangalore Mahanagara Palike");
                param[7] = new ReportParameter("P_ZONENAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["ZONENAME"]));
                param[8] = new ReportParameter("SUB_DIVISION_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["SUB_DIVISION_NAME"]));
                param[9] = new ReportParameter("P_WARD_NAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["WARD_NAME"]));
                param[10] = new ReportParameter("P_DOORSITENO", Convert.ToString(dsReportData.Tables[0].Rows[0]["DOORNO"]));
                param[11] = new ReportParameter("P_BUIDINGNAME1",string.IsNullOrEmpty(Convert.ToString(dsReportData.Tables[0].Rows[0]["BUILDINGNAME"])) ? "NA" : Convert.ToString(dsReportData.Tables[0].Rows[0]["BUILDINGNAME"]));

                param[12] = new ReportParameter("P_STREETNAME1", Convert.ToString(dsReportData.Tables[0].Rows[0]["STREETNAME"]));
                param[13] = new ReportParameter("P_APPLICANTNAME", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTNAME"]));
                param[14] = new ReportParameter("P_BOOKS_PROP_APPNO", Convert.ToString(BOOKS_PROP_APPNO));
                param[15] = new ReportParameter("P_APPLICANTPOSTALADDRESS", Convert.ToString(dsReportData.Tables[0].Rows[0]["APPLICANTPOSTALADDRESS"]));
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
                _logger.LogError(ex, "Error GetFinalBBMPReport function the report.");
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
                param[7] = new ReportParameter("P_OWNERNAMEBBMP",  string.IsNullOrEmpty(Convert.ToString(dsReportData.Tables[0].Rows[0]["BBMP_REG_OWNERNAMES"])) ? "NA" : Convert.ToString(dsReportData.Tables[0].Rows[0]["BBMP_REG_OWNERNAMES"]));
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
                _logger.LogError(ex, "Error GetEndorsementReport the report.");
                throw;
            }
        }


        private byte[] GetPDF(int propertycode, int BOOKS_PROP_APPNO)
        {
            try
            {
                string Date = DateTime.Now.ToShortDateString();
                DataSet dsNCLTablesData = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMP(555, BOOKS_PROP_APPNO, propertycode); // Data from NCL temp tables  
                DataSet dsNCLTablesDataDummy = objModule.GET_PROPERTY_PENDING_CITZ_NCLTEMPDUMMY(propertycode);

                LocalReport report = new LocalReport
                {
                    EnableExternalImages = true,
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP.rdlc")
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
                _logger.LogError(ex, "Error GetMaskedMobileNumber the report.");
                Alert.Show(ex.Message);
                return "";
            }
        }
        [HttpGet("GetEsign")]
        public ESign GetEsign(int Propertycode, int booksAppNo)
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
                    ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP.rdlc")
                };
                report.DataSources.Clear();
                string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CitzRegistration_BBMP.rdlc");

                report.ReportPath = reportPath;
                report.Refresh();
                string Url = ConvertReportToPDF(Propertycode, booksAppNo);
                string hash = "";
                hash = GetPDFHash(Url, _Esign.TempFiles_ctz.ToString());
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
                Alert.Show(ex.Message);
                throw ex;
            }
        }

        protected string ConvertReportToPDF(int Propertycode, int booksAppNo)
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
                Alert.Show(ex.Message);
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
        private string GetPDFHash(string pdfFilePath, string tmpPath)
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
                //  Session["Stamper"] = Stamper;
                //  Session["OutputStream"] = OutputStream;
                //   Session["reader"] = reader;
                //    Session["sap"] = appearance;
            }
            catch (Exception ex)
            {
                // objLogHelper.LogError(ex, "GetPDFHash For Property:" + ((Convert.ToString(Session["BookModPropertyID"]) != "") ? Convert.ToString(Session["BookModPropertyID"]) : ((Convert.ToString(Session["BookModPropertyCode"]) != "") ? Convert.ToString(Session["BookModPropertyCode"]) : Convert.ToString(Session["LoginId"]))));
                Alert.Show(ex.Message);
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
                Alert.Show(ex.Message);
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
        //[HttpGet("E-signRediredirection")]
        //private void AttachSignature(string esignxml, string propertyCode, string BOOK_APP_NO, string LoginId)
        //{
        //    string signature = "", sig = "", outFile = "", Url = "", path = "";
        //    try
        //    {
        //        signature = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(esignxml));
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(signature);
        //        XmlNodeList elemList = xmlDoc.GetElementsByTagName("EsignResp");
        //        sig = elemList[0].ChildNodes[1].InnerText;
        //        //Start generating PDF Hash
        //        //PdfSignatureAppearance appearance = (PdfSignatureAppearance)Session["sap"];
        //        //FileStream OutputStream = (FileStream)Session["OutputStream"];
        //        //PdfReader reader = (PdfReader)Session["reader"]; //uploaded pdf is now open to attach signature
        //        PdfSignatureAppearance appearance = (PdfSignatureAppearance);
        //        FileStream OutputStream = (FileStream)Session["OutputStream"];
        //        PdfReader reader = (PdfReader)Session["reader"]; //uploaded pdf is now open to attach signature
        //        byte[] sigbytes = Convert.FromBase64String(sig); //string format of your signature is convert to bytes.
        //        byte[] paddedSig = new byte[8192]; //alignment of the pdf is done.
        //        Array.Copy(sigbytes, 0, paddedSig, 0, sigbytes.Length);

        //        PdfDictionary dic2 = new PdfDictionary();
        //        dic2.Put(PdfName.CONTENTS, new PdfString(paddedSig).SetHexWriting(true));
        //        appearance.Close(dic2);
        //        OutputStream.Close();
        //        reader.Close();
        //    }
        //    catch (Exception ex1)
        //    {
        //        // objLogHelper.LogError(ex1, "AttachSignature1");
        //        Alert.Show("AttachSignature1:" + ((ex1.InnerException != null) ? ex1.InnerException.Message.Replace('\n', ' ') : ex1.Message.Replace('\n', ' ')));
        //    }

        //    try
        //    {
        //        path = Server.MapPath("../TempFiles/");
        //        outFile = Session["SignedFileName"].ToString();
        //        Url = _Esign.TempURl_ctz.ToString() + outFile;

        //        byte[] bytes = File.ReadAllBytes(Path.Combine(path, outFile));
        //        insertCitzData(bytes, propertyCode, BOOK_APP_NO, LoginId);

        //        System.Web.UI.AttributeCollection col = pdfiframe.Attributes;
        //        col.Add("src", Url);
        //    }
        //    catch (Exception ex2)
        //    {
        //        //  objLogHelper.LogError(ex2, "AttachSignature2");
        //        Alert.Show("AttachSignature2:" + ((ex2.InnerException != null) ? ex2.InnerException.Message.Replace('\n', ' ') : ex2.Message.Replace('\n', ' ')));
        //    }
        //}

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
        //        // objLogHelper.LogError(ex, "Final Submit For Application:" + Convert.ToString(Session["BOOKS_PROP_APPNO"]));
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
        //      //  objLogHelper.LogError(ex, "SMS for Final Submit:" + Convert.ToString(Session["BOOKS_PROP_APPNO"]) + ((ex.InnerException != null) ? ex.InnerException.Message.Replace('\n', ' ') : ex.Message.Replace('\n', ' ')));
        //        Alert.Show("Application Submitted Successfully but SMS is failed. Reference No: " + Convert.ToString(Session["BOOKS_PROP_APPNO"]));
        //        return "";
        //    }
        //}
    }
}
