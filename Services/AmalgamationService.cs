using BBMPCITZAPI.Controllers;
using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using NUPMS_BA;
using NUPMS_BO;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static BBMPCITZAPI.Models.Amalgamation;

namespace BBMPCITZAPI.Services
{
    public class AmalgamationService : IAmalgamationService
    {
        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _auth;

        public AmalgamationService(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService Auth)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _auth = Auth;
        }

        public DataSet GET_NCL_MUTATION_AMALGAMATION_MAIN(string propertyid,int ulbcode)
        {
            try
            {
                string sp_name = "NMT.GET_PROP_DETAILS";
                OracleParameter[] prm = new OracleParameter[]
                {
              new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,256),// VARCHAR2(256);
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("C_RECORD",OracleDbType.RefCursor),// SYS_REFCURSOR

                };
                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = propertyid;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = ulbcode;
                prm[2].Direction = ParameterDirection.Output;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_MUTATION_AMALGAMATION_MAIN stored procedure.");
                throw;
            }
        }
        public int GET_NCL_MUTATION_AMALGAMATION_MAIN_COUNT(string propertyid, int ulbcode)
        {
            try
            {
                string sp_name = "NMT.GET_NMT_APPL_COUNT_PID";
                OracleParameter[] prm = new OracleParameter[]
                {
              new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,256),// VARCHAR2(256);
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("C_COUNT",OracleDbType.Int32),// SYS_REFCURSOR

                };
                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = propertyid;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = ulbcode;
                prm[2].Value = null;
                prm[2].Direction = ParameterDirection.Output;
               ;
                int res = _databaseService.ExecuteNonQuery(sp_name, prm);
                return int.Parse(prm[2].Value.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_MUTATION_AMALGAMATION_MAIN stored procedure.");
                throw;
            }
        }
        public DataSet GeneratateMuttaplid(Int64 propertycode, string propertyid,Int64 MutationApplID)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.INS_NPM_AMALGAMATION_MAIN";
                OracleParameter[] prm = new OracleParameter[]
                {
                     new OracleParameter("P_MUTAPPLID",OracleDbType.Int64),// NUMBER;
                     new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64),// NUMBER;
              new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,256),// VARCHAR2(256);
             
              new OracleParameter("C_MAIN",OracleDbType.RefCursor),// SYS_REFCURSOR

                };
                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = MutationApplID;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = propertycode;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = propertyid;
                prm[3].Direction = ParameterDirection.Output;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_MUTATION_AMALGAMATION_MAIN stored procedure.");
                throw;
            }
        }

        public DataSet INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string EMAIL, string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.INS_NPM_PROPERTY_AMALGAMATION_TEMP_WITH_EKYCDATA";
                OracleParameter[] prm = new OracleParameter[]
                {
                  new OracleParameter("P_TRANSACTIONNO",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_OWNERNAME",OracleDbType.NVarchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERNAME_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERADDRESS",OracleDbType.NVarchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERADDRESS_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERPHOTO",OracleDbType.Blob,ParameterDirection.Input),
              new OracleParameter("P_MaskedAadhaar",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_OwnerGender",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DATEOFBIRTH",OracleDbType.Varchar2,ParameterDirection.Input),

              new OracleParameter("P_MOBILENUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_MOBILEVERIFY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_VAULTREFNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_AADHAARHASH",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_LOGINID",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter("P_EMAIL",OracleDbType.Varchar2,ParameterDirection.Input),
                    new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("C_OWNERRECORD",OracleDbType.RefCursor,ParameterDirection.Output),

                    };

                prm[0].Value = objEKYCDetailsBO.TxnNo;
                prm[1].Value = objEKYCDetailsBO.OwnerNameKnd;
                prm[2].Value = objEKYCDetailsBO.OwnerNameEng;


                prm[3].Value = objEKYCDetailsBO.AddressKnd;
                prm[4].Value = objEKYCDetailsBO.AddressEng;
                prm[5].Value = objEKYCDetailsBO.photoBytes;
                prm[6].Value = objEKYCDetailsBO.maskedAadhaar;
                prm[7].Value = objEKYCDetailsBO.Gender;
                prm[8].Value = objEKYCDetailsBO.DateOfBirth;

                prm[9].Value = MOBILENUMBER;
                prm[10].Value = MOBILEVERIFY;

                prm[11].Value = objEKYCDetailsBO.VaultRefNumber;
                prm[12].Value = objEKYCDetailsBO.AadhaarHash;
                prm[13].Value = loginId;
                prm[14].Value = EMAIL;
                prm[15].Value = _auth.GetIPAddress();
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA stored procedure.");
                throw;
            }
        }
        public DataSet INS_NCL_AMAL_APPL_MAIN(int ulbcode, Int64 MutationApplID, string createdby)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.INS_NCL_AMAL_APPL_MAIN";
                OracleParameter[] prm = new OracleParameter[]
                {
                     new OracleParameter("P_ULBCODE",OracleDbType.Int16),// NUMBER;
                     new OracleParameter("P_MUTAPPLID",OracleDbType.Int64),// NUMBER;
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,256),// VARCHAR2(256);
             
         

                };
                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = ulbcode;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = MutationApplID;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = createdby;
              
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_MUTATION_AMALGAMATION_MAIN stored procedure.");
                throw;
            }
        }

        public DataSet INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT_APPL(AMalGamation_final AmalFinal)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT";
                OracleParameter[] prm = new OracleParameter[]
                {
                   
                     new OracleParameter("P_ULBCODE",OracleDbType.Int16),// NUMBER;
              new OracleParameter("P_MUTAPPLID",OracleDbType.Int64),// VARCHAR2(256);
             new OracleParameter("P_AMALORDERNUMBER",OracleDbType.Varchar2,256),// VARCHAR2(256);
             new OracleParameter("P_AMALORDERDATE",OracleDbType.Date),// VARCHAR2(256);
             new OracleParameter("P_VAULTREFNUMBER",OracleDbType.Varchar2,256),// VARCHAR2(256);
             new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,256),// VARCHAR2(256);
            
         

                };
                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = AmalFinal.UlbCode;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = AmalFinal.MUTAPPLID;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = AmalFinal.AmalOrderNumber;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = AmalFinal.AmalOrderDate;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = AmalFinal.VaultRefNumber;
                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = AmalFinal.LoginId;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT_APPL stored procedure.");
                throw;
            }
        }





        public DataSet INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(AMalGamation_final AmalFinal)
        {
            try
            {
                string sp_name = "NMT.NMT_TEMP_AMALG_UPD";
                OracleParameter[] prm = new OracleParameter[]
                {
                 new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_ULBCODE ",OracleDbType.Int32),// NUMBER
                new OracleParameter("P_MUTAPPLID",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_UGD",OracleDbType.Varchar2,1),// VARCHAR2(1);
                new OracleParameter("P_CORNERSITE",OracleDbType.Varchar2,1),// CHAR(1);
                new OracleParameter("P_PROPERTYSTATUS",OracleDbType.Varchar2,3),// CHAR(3);
                new OracleParameter("P_CHECKBANDI_NORTH",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CHECKBANDI_SOUTH",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CHECKBANDI_EAST",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CHECKBANDI_WEST",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
                new OracleParameter("P_EASTWEST",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_NORTHSOUTH",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_ODDSITE",OracleDbType.Varchar2,1),// CHAR(1);
                new OracleParameter("P_SITEAREA",OracleDbType.Decimal),// NUMBER;
                new OracleParameter("P_PROPERTYCATEGORYID ",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_PROPERTYPHOTO ",OracleDbType.Blob),// BLOB;
                new OracleParameter("P_SURVEYNO ",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_ASSESMENTNUMBER ",OracleDbType.Varchar2),// VARCHAR2(500);
                new OracleParameter("C_RECORD",OracleDbType.RefCursor),// SYS_REFCURSOR;
                new OracleParameter("C_RECORD2",OracleDbType.RefCursor),// SYS_REFCURSOR;

                    };
               
                prm[0].Value = AmalFinal.PROPERTYCODE;
                prm[1].Value = 555;
                prm[2].Value = AmalFinal.MUTAPPLID;
                prm[3].Value = AmalFinal.UGD;
                prm[4].Value = AmalFinal.CORNERSITE;
                prm[5].Value = "MUE";
                prm[6].Value = AmalFinal.CHECKBANDI_NORTH;
                prm[7].Value = AmalFinal.CHECKBANDI_SOUTH;
                prm[8].Value = AmalFinal.CHECKBANDI_EAST;
                prm[9].Value = AmalFinal.CHECKBANDI_WEST;
                prm[10].Value = AmalFinal.LoginId;
                prm[11].Value = AmalFinal.EASTWEST;
                prm[12].Value = AmalFinal.NORTHSOUTH;
                prm[13].Value = AmalFinal.ODDSITE;
                prm[14].Value = AmalFinal.SITEAREA;
                prm[15].Value = AmalFinal.PROPERTYCATEGORYID;
                prm[16].Value = AmalFinal.PROPERTYPHOTO;
                prm[17].Value = AmalFinal.SURVEYNO;
                prm[18].Value = AmalFinal.ASSESMENTNUMBER;
                prm[19].Direction = ParameterDirection.Output;
                prm[20].Direction = ParameterDirection.Output;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }


        public DataSet CopyNPMtoNMTMain(AMalGamation_final AmalFinal)
        {
            try
            {
                string sp_name = "NMT.NMT_TEMP_AMALG_INS";
                OracleParameter[] prm = new OracleParameter[]
                {
                    new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_ULBCODE ",OracleDbType.Int32),// NUMBER
                new OracleParameter("P_MUTAPPLID",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_UGD",OracleDbType.Varchar2,1),// VARCHAR2(1);
                new OracleParameter("P_CORNERSITE",OracleDbType.Varchar2,1),// CHAR(1);
                new OracleParameter("P_PROPERTYSTATUS",OracleDbType.Varchar2,3),// CHAR(3);
                new OracleParameter("P_CHECKBANDI_NORTH",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CHECKBANDI_SOUTH",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CHECKBANDI_EAST",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CHECKBANDI_WEST",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
                new OracleParameter("P_EASTWEST",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_NORTHSOUTH",OracleDbType.Varchar2,500),// VARCHAR2(500);
                new OracleParameter("P_ODDSITE",OracleDbType.Varchar2,1),// CHAR(1);
                new OracleParameter("P_SITEAREA",OracleDbType.Decimal),// NUMBER;
                new OracleParameter("P_PROPERTYCATEGORYID ",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_PROPERTYPHOTO ",OracleDbType.Blob),// BLOB;
                new OracleParameter("C_RECORD",OracleDbType.RefCursor),// SYS_REFCURSOR;
                new OracleParameter("C_RECORD2",OracleDbType.RefCursor),// SYS_REFCURSOR;

                    };
                prm[0].Value = AmalFinal.PROPERTYCODE;
                prm[1].Value = AmalFinal.UlbCode;
                prm[2].Value = AmalFinal.MUTAPPLID;
                prm[3].Value = AmalFinal.UGD;
                prm[4].Value = AmalFinal.CORNERSITE;
                prm[5].Value = "MUT";
                prm[6].Value = AmalFinal.CHECKBANDI_NORTH;
                prm[7].Value = AmalFinal.CHECKBANDI_SOUTH;
                prm[8].Value = AmalFinal.CHECKBANDI_EAST;
                prm[9].Value = AmalFinal.CHECKBANDI_WEST;
                prm[10].Value = AmalFinal.LoginId;
                prm[11].Value = AmalFinal.EASTWEST;
                prm[12].Value = AmalFinal.NORTHSOUTH;
                prm[13].Value = AmalFinal.ODDSITE;
                prm[14].Value = AmalFinal.SITEAREA;
                prm[15].Value = AmalFinal.PROPERTYCATEGORYID;
                prm[16].Value = AmalFinal.PROPERTYPHOTO;
                prm[17].Direction = ParameterDirection.Output;
                prm[18].Direction = ParameterDirection.Output;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.GetAmalgamationProperty stored procedure.");
                throw;
            }
        }
        public DataSet NCL_AMALGAMATION_DOCUMENT_TEMP_DEL(Int64 MutationApplicatioId, Int32 DocumentId)
        {
            string sp_name = "NMT.NMT_APPL_DOCUMENTS_DEL";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_MUTAPPLID",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTID",OracleDbType.Int64,ParameterDirection.Input),
               new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output),

                    };

            prm[0].Value = MutationApplicatioId;
            prm[1].Value = DocumentId;


            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet NCL_AMALGAMATION_DOCUMENTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID)
        {
            try
            {
                string sp_name = "NMT.NMT_APPL_DOCUMENTS_INS";
                OracleParameter[] prm = new OracleParameter[]
                {
                 new OracleParameter("P_ORDERNUMBER",OracleDbType.Varchar2,200),// VARCHAR2(200);
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
              new OracleParameter("P_DOCUMENTDETAILS",OracleDbType.Varchar2,2000),// VARCHAR2(2000);
              new OracleParameter("P_SCANNEDDOCUMENT",OracleDbType.Blob),// BLOB;
              new OracleParameter("P_MUTAPPLID",OracleDbType.Int64),// NUMBER;
              new OracleParameter("P_ORDERDATE",OracleDbType.Date),// DATE;
              new OracleParameter("P_DOCUMENTTYPEID",OracleDbType.Int32),// NUMBER;
              new OracleParameter("C_RECORD",OracleDbType.RefCursor),// SYS_REFCURSOR;
                };
                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = NCLPropID.ORDERNUMBER;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = NCLPropID.CREATEDBY;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropID.DOCUMENTDETAILS;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = NCLPropID.SCANNEDDOCUMENT;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = NCLPropID.MUTAPPLID;
                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = NCLPropID.ORDERDATE;
                prm[6].Direction = ParameterDirection.Input;
                prm[6].Value = NCLPropID.DOCUMENTTYPEID;
                prm[7].Direction = ParameterDirection.Output;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.NCL_AMALGAMATION_DOCUMENTS_TEMP_INS stored procedure.");
                throw;
            }
        }
        public int REC_MUTATION_APPL(Int64 PROPERTYCODE, int ULBCODE, Int64 MUTAPPLID, NWFTaskFlow objNWF, NMT_APPL_MAIN objMAIN)
        {
            string sp_name = "NMT.REC_MUTATION_APPL";
            OracleParameter[] prm = new OracleParameter[] {
                    new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64),// NUMBER;
                    new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER; DELETE_NMT_OWNER_TEMP
                    new OracleParameter("P_MUTAPPLID",OracleDbType.Int64),// NUMBER;
                    new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);                     
                    new OracleParameter("P_ULBTYPEID",OracleDbType.Int32),// NUMBER;                                                  
                    new OracleParameter("P_TRANTYPEID",OracleDbType.Int32),// NUMBER;
                    new OracleParameter("P_UPDATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
                    new OracleParameter("P_APPLICATIONSTATUS",OracleDbType.Varchar2,3),// VARCHAR2(3);
                    new OracleParameter("P_APPLICATIONNO",OracleDbType.Int32),// NUMBER;
                    };
            prm[0].Direction = ParameterDirection.Input;
            prm[0].Value = PROPERTYCODE;
            prm[1].Direction = ParameterDirection.Input;
            prm[1].Value = ULBCODE;
            prm[2].Direction = ParameterDirection.Input;
            prm[2].Value = MUTAPPLID;
            prm[3].Direction = ParameterDirection.Input;
            prm[3].Value = objNWF.CREATEDBY;
            prm[4].Direction = ParameterDirection.Input;
            prm[4].Value = objNWF.ULBCODETYPEID;
            prm[5].Direction = ParameterDirection.Input;
            prm[5].Value = objNWF.TRANTYPEID;
            prm[6].Direction = ParameterDirection.Input;
            prm[6].Value = objMAIN.UPDATEDBY;
            prm[7].Direction = ParameterDirection.Input;
            prm[7].Value = objMAIN.APPLICATIONSTATUS;
            prm[8].Direction = ParameterDirection.Input;
            prm[8].Value = objMAIN.APPLICATIONNO;
            return _databaseService.ExecuteNonQuery(sp_name, prm);
        }
        public DataSet SEL_CitzAmalgamationAck(int AmalgamationId)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.SEL_CitzAmalAcknowledgement";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_AMAL_MUT_REQ_ID", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("C_RECORD", OracleDbType.RefCursor, ParameterDirection.Output),

                };
                prm[0].Value = AmalgamationId;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.SEL_CitzAmalgamationAck stored procedure.");
                throw;
            }
        }


    }
}
