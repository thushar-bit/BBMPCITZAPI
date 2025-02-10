using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUPMS_BA;
using NUPMS_BO;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using static BBMPCITZAPI.Models.KaveriData;
using System.Xml;


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
        private readonly IErrorLogService _errorLogService;
        public NameMatchController(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService IBBMPBOOKMODULE, IOptions<PropertyDetails> PropertyDetails,
             INameMatchingService NameMatching, IErrorLogService errorLogService)
        // ICacheService cacheService)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _IBBMPBOOKMODULE = IBBMPBOOKMODULE;
            _PropertyDetails = PropertyDetails.Value;
            _nameMatchingService = NameMatching;
            _errorLogService = errorLogService;
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
                _errorLogService.LogError(ex, "GET_BBD_NCL_OWNER_BYEKYCTRANSACTION");
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
                DataSet dataSet = obj.UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY(P_BOOKS_PROP_APPNO, propertyCode, ownerNumber, IDENTIFIERTYPE, IDENTIFIERNAME_EN, MOBILENUMBER, MOBILEVERIFY, loginId);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "UPD_NCL_PROPERTY_OWNER_TEMP_MOBILEVERIFY");
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
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

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
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React");
                _logger.LogError(ex, "Error occurred while executing stored procedure.GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
                throw;
            }
        }
        [HttpPost("INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA")]
        public ActionResult<DataSet> INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE, string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            _logger.LogInformation("GET request received at INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
            try
            {
                DataSet dataSet = obj.INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA(IDENTIFIERTYPE, IdentifierName, MOBILENUMBER, MOBILEVERIFY, NAMEMATCHSCORE, loginId, objEKYCDetailsBO);
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "INS_NCL_PROPERTY_OWNER_TEMP_WITH_EKYCDATA");
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
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "GET_PROPERTY_PENDING_CITZ_NCLTEMP_React");
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
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "COPY_OWNER_FROM_BBDDRAFT_NCLTEMP");
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
                string json = JsonConvert.SerializeObject(dataSet, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "Insert_React_UserFlow");
                _logger.LogError(ex, "Error occurred while executing stored procedure.Insert_React_UserFlow");
                throw;
            }
        }
        [HttpGet("Get_React_UserFlow")]
        public ActionResult<ReactUserFlow> Get_React_UserFlow(Int64 propertyCode, Int64 BOOKAPP_NO, string LoginID)
        {
            _logger.LogInformation("GET request received at Get_React_UserFlow");
            try
            {
                ReactUserFlow react = new ReactUserFlow();

                DataSet dataSet = _IBBMPBOOKMODULE.Get_React_UserFlow(propertyCode, BOOKAPP_NO, LoginID);
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
                    react.AllowFlow = Convert.ToInt32(dataSet.Tables[0].Rows[0]["CurrentStep"]);

                }


                string json = JsonConvert.SerializeObject(react, Newtonsoft.Json.Formatting.Indented);

                return Ok(json);
            }
            catch (Exception ex)
            {
                _errorLogService.LogError(ex, "Get_React_UserFlow");
                _logger.LogError(ex, "Error occurred while executing stored procedure.Get_React_UserFlow");
                throw;
            }
        }
        [HttpGet("NEW_KHATA_DETAILS")]
        public void GetTESTNEWKHATA()
        {

            PropertyMatrixCalculation propertyMatrix = new PropertyMatrixCalculation();
            var folderPath = @"E:\NewKhathaFiles\Inbox";

            var jsonFiles = Directory.GetFiles(folderPath, "*.json");
            if (jsonFiles.Length == 0)
            {
                Console.WriteLine("No json files");
                _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS("No EPID", "Failed", "N", "N", "No JSON FILES", "Not an exception", "Not an exception", "NO FILE NAME");
                return;
            }
            foreach (var filePath in jsonFiles)
            {
                try
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS("No EPID", "Failed", "N", "N", "JSON file not found at the specified path.", "Not an exception", "Not an exception", "NO FILE NAME");


                    }
                    string fileName = Path.GetFileName(filePath);
                    string jsonString = System.IO.File.ReadAllText(filePath);
                    newkhatadetails newkhatajson = new newkhatadetails();
                    try
                    {
                        newkhatajson = JsonConvert.DeserializeObject<newkhatadetails>(jsonString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File Processed failed", fileName);
                        _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS("No EPID", "Failed", "Y", "N", $"Something Went Wrong when Deserializing + {filePath}", ex.Message, ex.StackTrace, fileName);
                        _errorLogService.LogError(ex, "GetTESTNEWKHATA");
                        _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NEW_KHATA_DETAILS_REACT_API");
                        var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";

                        if (!Directory.Exists(faileddestinationFolder))
                        {
                            Directory.CreateDirectory(faileddestinationFolder);
                        }
                        var faileddestinationPath = Path.Combine(faileddestinationFolder, filePath);
                        System.IO.File.Move(filePath, faileddestinationPath);
                        continue;
                    }
                    _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Recieved", "N", "N", $"{filePath}", "No exception", "Not an exception", fileName);
                    List<KaveriData.EcData> ECdocumentDetails = new List<KaveriData.EcData>();
                    KaveriData.EcData Dosc = new KaveriData.EcData();
                    KaveriData.DocumentDetails documentDetails = new KaveriData.DocumentDetails();
                    KaveriData.KaveriDocBase64 kaveriDocBase64 = new KaveriData.KaveriDocBase64();
                    string KAVERIDOC_RESPONSE_ROWID = "";
                    string fromdate = "";
                    string toDate = "";
                    byte[] ECbase64String1 = [];
                    try
                    {
                        
                        if (newkhatajson.KaveriDeedInformation.IsDeedBefore01042004 == false)
                        {


                            JObject Obj_Json = JObject.Parse(newkhatajson.KaveriECInformation.KaveriECInfoRawAPIResponseJSON.Replace("],,", "],").Replace(",}", "}"));
                            string responseCode = (string)Obj_Json.SelectToken("responseCode")!;



                            string responseMessage = (string)Obj_Json.SelectToken("responseMessage")!;

                            if (responseMessage == "Sucess")
                            {
                                string base64String = (string)Obj_Json.SelectToken("json")!;
                                 ECbase64String1 = (byte[])Obj_Json.SelectToken("base64")!;
                                ECdocumentDetails = JsonConvert.DeserializeObject<List<KaveriData.EcData>>(base64String)!;
                                Dosc = ECdocumentDetails.OrderByDescending(x => x.ExecutionDate).FirstOrDefault();
                                string responseRawContent = newkhatajson.KaveriECInformation.KaveriECInfoRawAPIResponseJSON.ToString();
                                fromdate = responseRawContent.Substring(responseRawContent.IndexOf("fromDate", 0) + 11, 19);
                                toDate = responseRawContent.Substring(responseRawContent.IndexOf("toDate", 0) + 9, 19);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File Processed failed", fileName);
                        _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Failed", "Y", "N", $"Unable to Deserialize Kaveri EC json .Please send correct json + {filePath}", ex.Message, ex.StackTrace, fileName);
                        _errorLogService.LogError(ex, "GetTESTNEWKHATA");
                        _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NEW_KHATA_DETAILS_REACT_API");
                        var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";


                        if (!Directory.Exists(faileddestinationFolder))
                        {
                            Directory.CreateDirectory(faileddestinationFolder);
                        }


                        var faileddestinationPath = Path.Combine(faileddestinationFolder, fileName);


                        System.IO.File.Move(filePath, faileddestinationPath);
                        continue;
                    }
                    if (newkhatajson.KaveriDeedInformation.IsDeedBefore01042004 == false)
                    {
                        try
                        {

                            var response = JsonConvert.DeserializeObject<KaveriData.KAVERI_API_DOC_DETAILS_RESPONSE>(newkhatajson.KaveriDeedInformation.KaveriDeedInfoRawAPIResponseJSON);
                            documentDetails = JsonConvert.DeserializeObject<KaveriData.DocumentDetails>(response.json);
                            kaveriDocBase64 = JsonConvert.DeserializeObject<KaveriData.KaveriDocBase64>(newkhatajson.KaveriDeedInformation.KaveriDeedDocumentbase64);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("File Processed failed", fileName);
                            _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Failed", "Y", "N", $"Unable to Deserialize Kaveri Deed json .Please send correct json + {filePath}", ex.Message, ex.StackTrace, fileName);
                            _errorLogService.LogError(ex, "GetTESTNEWKHATA");
                            _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NEW_KHATA_DETAILS_REACT_API");
                            var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";


                            if (!Directory.Exists(faileddestinationFolder))
                            {
                                Directory.CreateDirectory(faileddestinationFolder);
                            }


                            var faileddestinationPath = Path.Combine(faileddestinationFolder, fileName);


                            System.IO.File.Move(filePath, faileddestinationPath);
                            continue;
                        }
                    }

                    var mainParameters = new List<EKYCDetailsBO>();
                    try
                    {
                        foreach (var kycdetails in newkhatajson.OwnerData)
                        {
                            EKYCDetailsBO objEKYCDetailsBO = obj.ParseEKYCResponse(kycdetails.EKYCJSON);
                            mainParameters.Add(objEKYCDetailsBO);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File Processed failed", fileName);
                        _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Failed", "Y", "N", $"Unable to Deserialize EKYC json .Please send correct json + {filePath}", ex.Message, ex.StackTrace, fileName);
                        _errorLogService.LogError(ex, "GetTESTNEWKHATA");
                        _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NEW_KHATA_DETAILS_REACT_API");
                        var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";


                        if (!Directory.Exists(faileddestinationFolder))
                        {
                            Directory.CreateDirectory(faileddestinationFolder);
                        }


                        var faileddestinationPath = Path.Combine(faileddestinationFolder, fileName);


                        System.IO.File.Move(filePath, faileddestinationPath);
                        continue;
                    }
                    DataSet ids = _IBBMPBOOKMODULE.GENERATE_NEW_KHATA_PROPERTYCODE();
                    if (ids != null && ids.Tables.Count > 0 && ids.Tables[0].Rows.Count > 0)
                    {
                        Int64 pr = Convert.ToInt64(ids.Tables[0].Rows[0]["PROPERTYCODE"]);
                        Int64 bk = Convert.ToInt64(ids.Tables[0].Rows[0]["BOOKS_PROP_APPNO"]);






                        int dataSet = _IBBMPBOOKMODULE.Insert_New_khata_details(newkhatajson, pr, bk, mainParameters, documentDetails, Dosc, fromdate, toDate,
                            kaveriDocBase64.base64, ECbase64String1);




                            
                        if (dataSet == 1)
                        {
                            Console.WriteLine("File Processed Sucessfully", fileName);
                            if (newkhatajson.OverallRecommendation.StatusId == 8)
                            {
                                DataSet workFlowId = _IBBMPBOOKMODULE.GenarateWORKFLOWID(pr, newkhatajson.LoginInformation.UserMobileNumber, "N");
                            }

                            if (newkhatajson.SASNo != "" && newkhatajson.OverallRecommendation.StatusId == 10)
                            {
                                Console.WriteLine("Auto Approve Initiatied");
                                DataSet workFlowId = _IBBMPBOOKMODULE.GenarateWORKFLOWID(pr, newkhatajson.LoginInformation.UserMobileNumber, "Y");
                                bool istrue = obj.callAutoApproval(pr, newkhatajson.SASNo, Convert.ToInt64(workFlowId.Tables[0].Rows[0]["WORKFLOWID"])); //taskid
                                string CertPath = @"E:\File\jcrevenue.pfx";
                                string Password = "10031970";
                                string signatureString = "";
                                string subject = "";
                                string CertExp = "";

                                PropertyDataXML prop = new PropertyDataXML();
                                string proXML = prop.GetForm3XML(Convert.ToInt32(pr));
                                DigitalSign(proXML, CertPath, Password, out signatureString, out subject, out CertExp);
                                if (signatureString != "" && subject != "")
                                {
                                    var bytes = Encoding.UTF8.GetBytes(proXML);
                                    var base64 = Convert.ToBase64String(bytes);
                                    string xml = Convert.ToString(base64);


                                    // New Property Creation
                                    int apx = prop.ApproveProperty(pr.ToString(), "555", Convert.ToString(xml), signatureString, "", subject, "", Convert.ToInt64(workFlowId.Tables[0].Rows[0]["WORKFLOWID"]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");

                                    GetBbmpTax(Convert.ToInt32(pr), CertPath, Password, "AutoApprooval", newkhatajson.EPID);
                                    Console.WriteLine("Auto Approve Successfull", fileName);
                                }
                                //      if (isExecutedWithOutError == true)
                                //      {
                                //    propertyMatrix.MatrixCalculation("MAINTABLE", pr, bk);
                                //    }
                            }
                            var successdestinationFolder = @"E:\NewKhathaFiles\Successful";
                            if (!Directory.Exists(successdestinationFolder))
                            {
                                Directory.CreateDirectory(successdestinationFolder);
                            }



                            var successdestinationPath = Path.Combine(successdestinationFolder, fileName);


                            System.IO.File.Move(filePath, successdestinationPath);
                            _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Success", "N", "N", $"Data Saved Successfully + {filePath}", "No Exception", "Not an exception", fileName);
                            continue;
                            //     return "Data Saved Successfully";
                        }
                        else if (dataSet == 0)
                        {
                            Console.WriteLine("File Processed failed", fileName);
                            //  var faileddestinationFolder = @"E:\New_Khata\Failed";
                            var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";
                            // Ensure the destination folder exists
                            if (!Directory.Exists(faileddestinationFolder))
                            {
                                Directory.CreateDirectory(faileddestinationFolder);
                            }

                            var faileddestinationPath = Path.Combine(faileddestinationFolder, fileName);

                            // Move the file to the destination folder
                            System.IO.File.Move(filePath, faileddestinationPath);
                            _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Failed", "N", "Y", $"Something Went Wrong while Saving in SP.Please check the Table + {filePath}", "No Exception", "Not an exception", fileName);
                            continue;
                            //   return "Something Went Wrong while Saving in SP.Please check the Table";
                        }
                        else
                        {
                            Console.WriteLine("File Processed failed", fileName);
                            //var faileddestinationFolder = @"E:\New_Khata\Failed";
                            var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";

                            if (!Directory.Exists(faileddestinationFolder))
                            {
                                Directory.CreateDirectory(faileddestinationFolder);
                            }


                            var faileddestinationPath = Path.Combine(faileddestinationFolder, fileName);


                            System.IO.File.Move(filePath, faileddestinationPath);
                            _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Failed", "N", "Y", $"Something Went Wrong while Saving in SP.Please check the Logs + {filePath}", "No Exception", "Not an exception", fileName);
                            continue;
                            //    return "Something Went Wrong while Saving in SP.Please check the Logs";
                        }

                    }
                    else
                    {
                        Console.WriteLine("Something Went Wrong While Generating PropertyCode and Book App NO ", fileName);
                        _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(newkhatajson.EPID, "Failed", "N", "Y", $"Something Went Wrong While Generating PropertyCode and Book App NO + {filePath}", "Something Went Wrong While Generating PropertyCode and Book App NO", "Not an exception", fileName);
                        return;
                    }

                }
                catch (Exception ex)
                {

                    _errorLogService.LogError(ex, "GetTESTNEWKHATA");
                    _logger.LogError(ex, "Error occurred while executing stored procedure.GET_NEW_KHATA_DETAILS_REACT_API");
                    string fileName = Path.GetFileName(filePath);
                    var faileddestinationFolder = @"E:\NewKhathaFiles\Failed";
                    Console.WriteLine("File Processed failed", fileName);
                    if (!Directory.Exists(faileddestinationFolder))
                    {
                        Directory.CreateDirectory(faileddestinationFolder);
                    }


                    var faileddestinationPath = Path.Combine(faileddestinationFolder, fileName);


                    System.IO.File.Move(filePath, faileddestinationPath);
                    _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS("NO EPID", "Failed", "Y", "N", $"Error occurred while executing stored procedure.GET_NEW_KHATA_DETAILS_REACT_API  + {filePath}", ex.Message, ex.StackTrace, fileName);
                    continue;

                }

            }
            Console.WriteLine("All FILES PROCESSED!!");
            //  return "All Files Processed .No More Files";


        }



        [HttpGet("NEW_TO_APPROVE_DETAILS")]
        public string GetTOAPPROVEDETAILS(string fileName, Int64 pr, string MobileNumber, string sasNo, string EPID,Int64 WorkFlowId)
        {
            try
            {

              //  DataSet workFlowId = _IBBMPBOOKMODULE.GenarateWORKFLOWID(pr, MobileNumber, "Y");
                bool istrue = obj.callAutoApproval(pr, sasNo, WorkFlowId); //taskid
                string CertPath = @"E:\File\jcrevenue.pfx";
                string Password = "10031970";
                string signatureString = "";
                string subject = "";
                string CertExp = "";

                PropertyDataXML prop = new PropertyDataXML();
                string proXML = prop.GetForm3XML(Convert.ToInt32(pr));
                DigitalSign(proXML, CertPath, Password, out signatureString, out subject, out CertExp);
                if (signatureString != "" && subject != "")
                {
                    var bytes = Encoding.UTF8.GetBytes(proXML);
                    var base64 = Convert.ToBase64String(bytes);
                    string xml = Convert.ToString(base64);


                    // New Property Creation
                    int apx = prop.ApproveProperty(pr.ToString(), "555", Convert.ToString(xml), signatureString, "", subject, "", WorkFlowId, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");

                    GetBbmpTax(Convert.ToInt32(pr), CertPath, Password, "AutoApprooval", EPID);

                }
                _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(EPID, "Success", "N", "N", $"Data Approved Successfully ", "No Exception", "Not an exception", fileName);
                return "File Approved Successfully";
            }
            catch (Exception ex)
            {
                _IBBMPBOOKMODULE.INS_NEW_KHATA_PROPERTY_API_STATUS(EPID, "Failed", "Y", "N", $"Error occurred while executing new khata react Approved stored procedure", ex.Message, ex.StackTrace, fileName);
                _errorLogService.LogError(ex, "NEW_TO_APPROVE_DETAILS");
                _logger.LogError(ex, "Error occurred while executing stored procedure.NEW_TO_APPROVE_DETAILS");
                throw ex;
                //    return "Something Went Wrong while Saving in SP.Please check the Logs";
            }
        }


    
        
        


        

        public static void DigitalSign(string dataToSign, string certificatePath, string certificatePassword, out string signatureString, out string subject, out string CertExp)
        {

            byte[] signature;
            X509Certificate2 certPFX = new X509Certificate2(certificatePath, certificatePassword);
            DateTime dtAfter = certPFX.NotAfter;
            if (DateTime.Now < dtAfter)
            {
                try
                {
                    byte[] data = StringToByte(dataToSign.Trim());
                    using (RSA rsa = certPFX.GetRSAPrivateKey())
                    {
                        if (rsa == null)
                        {
                            throw new InvalidOperationException("The certificate does not have a private key.");
                        }
                        signature = rsa.SignData(data, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                    }
                     //   RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certPFX.PrivateKey;
             
                    signatureString = Convert.ToBase64String(signature);
                    subject = certPFX.Subject;
                    CertExp = "N";
                }
                catch (Exception ex)
                {
                    signatureString = "";
                    subject = "";
                    CertExp = "N";
                    //BbdExceptionLog(0, 0, ex.ToString(), "PortBbdTODraft");
                    throw ex;
                }
            }
            else
            {
                signatureString = "";
                subject = "";
                CertExp = "Y";
            }
        }
        private static byte[] StringToByte(string strData)
        {
            System.Text.UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(strData);
        }


        private void GetBbmpTax(int PropertyCode, string CertPath, string Password, string TaxCall,string EPID)
        {
            //string CertPath = System.Configuration.ConfigurationManager.ConnectionStrings["CertPath"].ToString();
            //string Password = System.Configuration.ConfigurationManager.ConnectionStrings["Password"].ToString();
            string[] formats = { "MM-dd-yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd" };
            //  Report_BA RBA = new Report_BA();
            NMT_BA objNmtBA = new NMT_BA();
             NCL_BA objBA = new NCL_BA();

            DataTable dt = objNmtBA.GetBbmpPropTax(PropertyCode);
            string PropertyId = "";
            string signatureString = "";
            string subject = "";
            string CertExp = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    BbmpTaxData obj = new BbmpTaxData();
                    obj.PROPERTYCODE = int.Parse(item["PROPERTYCODE"].ToString());
                    obj.PROPERTYID = item["PROPERTYID"].ToString();
                    PropertyId = item["PROPERTYID"].ToString();
                    obj.ULBCODE = int.Parse(item["ULBCODE"].ToString());
                    obj.FINYEARID = int.Parse(item["FINYEARID"].ToString());// NUMBER;
                    obj.BANKID = int.Parse(item["BANKID"].ToString());// NUMBER;
                    obj.APPLICATIONNO = Convert.ToInt64(item["APPLICATIONNO"].ToString());
                    obj.NEWPROPERTYID = item["NEWPROPERTYID"].ToString();
                    obj.TAX_PAYMENT_DATE = DateTime.ParseExact(item["TAX_PAYMENT_DATE"].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None);
                    //obj.TAX_PAYMENT_DATE = Convert.ToDateTime(item["TAX_PAYMENT_DATE"].ToString());// DATE;  
                    //obj.TAX_PAYMENT_DATE = Convert.ToDateTime(item["28-04-2020"].ToString());// DATE;
                    obj.RECIEPTNO = Convert.ToInt64(item["RECIEPTNO"].ToString());//VARCHAR2(100);
                    obj.PAYMENTMODE = item["PAYMENTMODE"].ToString();// NUMBER;
                    obj.PAYMENTDETAILS = decimal.Parse(item["PAYMENTDETAILS"].ToString());// NUMBER;
                    obj.PROPERTYTAX = decimal.Parse(item["PROPERTYTAX"].ToString());// NUMBER;
                    obj.CESS = decimal.Parse(item["CESS"].ToString());// NUMBER;
                    obj.TOTALTAX = decimal.Parse(item["TOTALTAX"].ToString());// NUMBER;
                    obj.REBATE = decimal.Parse(item["REBATE"].ToString());// NUMBER;
                    obj.PENALTY = decimal.Parse(item["PENALTY"].ToString());// NUMBER;
                    obj.INTEREST = decimal.Parse(item["INTEREST"].ToString());// NUMBER;
                    obj.SWMCESS = decimal.Parse(item["SWMCESS"].ToString());// NUMBER;
                    obj.NETTAX = decimal.Parse(item["NETTAX"].ToString());// NUMBER;
                    obj.ADVANCE = decimal.Parse(item["ADVANCE"].ToString());// NUMBER;
                    obj.BALANCETAXPAID = decimal.Parse(item["BALANCETAXPAID"].ToString());// NUMBER;
                    obj.EXCESSAMOUNT = decimal.Parse(item["EXCESSAMOUNT"].ToString());// NUMBER;
                    obj.CREATEDBY = "";
                    obj.WSRESPONSEID = int.Parse(item["RESPONSEID"].ToString());
                    obj.OWNERADDRESS = item["OWNERADDRESS"].ToString();
                    obj.OWNERNAME = item["OWNERNAME"].ToString();


                    MemoryStream ms = new MemoryStream();
                    XmlTextWriter xw = new XmlTextWriter(ms, System.Text.Encoding.UTF8);

                    xw.Formatting = System.Xml.Formatting.None;
                    xw.WriteStartDocument(true);

                    xw.WriteStartElement("khatha");
                    xw.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    xw.WriteAttributeString("xsi:noNamespaceSchemaLocation", "Khatha.xsd");

                    xw.WriteStartElement("documentversion");
                    xw.WriteElementString("versionnumber", "version:1.0.0.0");
                    xw.WriteEndElement();

                    #region PropertyTaxDetails
                    //start of Property Tax Details element
                    xw.WriteStartElement("taxdetails");

                    xw.WriteStartElement("tax");
                    //xw.WriteElementString("sasfilingnumber", item["CHALLANNUMBER"].ToString());
                    //xw.WriteElementString("sasfilingdate", item["COLLECTIONDATE"].ToString());
                    //xw.WriteElementString("bankname", item["BANK"].ToString());
                    //xw.WriteElementString("taxpaiddate", item["COLLECTIONDATE"].ToString());
                    //xw.WriteElementString("totaltax", item["TOTALAMOUNT"].ToString());
                    //xw.WriteElementString("cess", item["TAXCESS"].ToString());
                    //xw.WriteElementString("taxtobepaid", item["TAXAMOUNT"].ToString());
                    //xw.WriteElementString("FYEAR", item["FYEAR"].ToString());

                    xw.WriteElementString("sasfilingnumber", item["APPLICATIONNO"].ToString());
                    xw.WriteElementString("sasfilingdate", DateTime.ParseExact(item["TAX_PAYMENT_DATE"].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None).ToString());
                    xw.WriteElementString("bankname", item["BANK"].ToString());
                    xw.WriteElementString("taxpaiddate", DateTime.ParseExact(item["TAX_PAYMENT_DATE"].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None).ToString());
                    xw.WriteElementString("totaltax", (decimal.Parse(item["TOTALTAX"].ToString()) + decimal.Parse(item["CESS"].ToString())).ToString());
                    xw.WriteElementString("cess", item["CESS"].ToString());
                    xw.WriteElementString("taxtobepaid", item["PROPERTYTAX"].ToString());
                    xw.WriteElementString("FYEAR", item["FINYEAR"].ToString());

                    xw.WriteEndElement();

                    //end of Property Tax Details element
                    xw.WriteEndElement();
                    #endregion PropertyTaxDetails
                    xw.WriteEndElement();
                    xw.WriteEndDocument();
                    xw.Close();
                    string xmldoc = Encoding.UTF8.GetString(ms.ToArray());
                 
                            DigitalSign(xmldoc, CertPath, Password, out  signatureString, out  subject, out CertExp);
                    if (signatureString != "" && subject != "")
                    {
                        var bytes1 = Encoding.UTF8.GetBytes(xmldoc);
                        var base641 = Convert.ToBase64String(bytes1);
                        string xml1 = Convert.ToString(base641);

                        obj.SIGNEDDATA = xml1;
                        obj.SIGNATURE = signatureString;
                        obj.SIGNEDBY = subject;

                        objNmtBA.InsertNmtTax(obj);

                    }
                    else
                    {
                        if (CertExp == "Y")
                        {
                            objBA.WS_CAL_INS(EPID, "555", "AutoApprooval", "certificate expired");
                        }
                        else
                        {
                            objBA.WS_CAL_INS(EPID, "555", "AutoApprooval", "signature not generated");
                        }
                    }
                }
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
