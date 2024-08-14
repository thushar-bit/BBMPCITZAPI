using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Reporting.NETCore;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using NUPMS_BA;

namespace BBMPCITZAPI.Controllers
{
    [ApiController]
   //Authorize]
    [Route("v1/Report")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ILogger<ReportsController> logger, IConfiguration configuration)
        {
            _logger = logger;
        }

        NUPMS_BA.ObjectionModuleBA objModule = new NUPMS_BA.ObjectionModuleBA();
       

        [HttpGet("GetFinalBBMPReport")]
        public IActionResult GetFinalReport(int propertycode, int BOOKS_PROP_APPNO)
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
                return File(bytes, mimeType, "FinalReport.pdf");


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating the report.");
                return StatusCode(500, "Internal server error");
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
                _logger.LogError(ex, "Error generating the report.");
                Alert.Show(ex.Message);
                return "";
            }
        }
    }
}
