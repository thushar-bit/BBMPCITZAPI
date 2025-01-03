using BBMPCITZAPI.Controllers;
using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using NUPMS_BO;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Services
{
    public class SearchService : ISearchService
    {

        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _auth;

        public SearchService(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService , IBBMPBookModuleService Auth)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _auth = Auth;
        }
        public DataSet INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA( string MOBILENUMBER, string MOBILEVERIFY,  string EMAIL,  string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            try
            {
                string sp_name = "SEARCHPROPERTYMODULE_REACT.INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA";
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
        public DataSet INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT(INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT final)
        {
            try
            {
                string sp_name = "SEARCHPROPERTYMODULE_REACT.INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_Search_Req_Id", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_IsHaveAAdhaarNumber", OracleDbType.Char, ParameterDirection.Input),
                new OracleParameter("P_IsHaveSASNumber", OracleDbType.Char, ParameterDirection.Input),
                new OracleParameter("P_DoorNo", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_BuildingName", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_AreaOrLocality", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_Pincode", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_IDDocument", OracleDbType.Blob, ParameterDirection.Input),
                new OracleParameter("P_IDCardType", OracleDbType.Varchar2, ParameterDirection.Input),
                 new OracleParameter("P_IDCardNumber", OracleDbType.Varchar2, ParameterDirection.Input),
                 new OracleParameter("P_MobileNumber", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_Mobiverify", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_Email", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_ZoneId", OracleDbType.Int16, ParameterDirection.Input),
                new OracleParameter("P_WardId", OracleDbType.Int16, ParameterDirection.Input),
                new OracleParameter("P_SEARCHNAME", OracleDbType.Varchar2, ParameterDirection.Input),
                 new OracleParameter("P_SASAPPLICATIONNUMBER", OracleDbType.Varchar2, ParameterDirection.Input),
                    new OracleParameter("P_LOGINID", OracleDbType.Varchar2, ParameterDirection.Input),
                      new OracleParameter("P_ISHAVEOLDEKHATA", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("P_OLDEKHATADOCUMENT", OracleDbType.Blob, ParameterDirection.Input),
                           new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,ParameterDirection.Input),
                         new OracleParameter("C_RECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                };
                prm[0].Value = final.Search_Req_Id;
             
                prm[1].Value = final.IsHaveAAdhaarNumber;
                prm[2].Value = final.IsHaveSASNumber;

                prm[3].Value = final.DoorNo;
                prm[4].Value = final.BuildingName;

                prm[5].Value = final.AreaOrLocality;
                prm[6].Value = final.Pincode;

                prm[7].Value = final.IDDocument;

                prm[8].Value = final.IDCardType;
                prm[9].Value = final.IDCardNumber;
                prm[10].Value = final.MobileNumber;

                prm[11].Value = final.Mobiverify;
                prm[12].Value = final.Email;

                prm[13].Value = final.ZoneId;
                prm[14].Value = final.WardId;
                prm[15].Value = final.SearchName;
                prm[16].Value = final.SASApplicationNumber;
                prm[17].Value = final.LoginId;
                prm[18].Value = final.IsHaveOldEkhata;
                prm[19].Value = final.OldEkhataDocument;
                prm[20].Value = _auth.GetIPAddress();

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }
        public DataSet SEL_CitzeSearchAck(int searchReqId)
        {
            try
            {
                string sp_name = "SEARCHPROPERTYMODULE_REACT.SEL_CitzSearchAcknowledgement";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_Search_Req_Id", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("C_RECORD", OracleDbType.RefCursor, ParameterDirection.Output),

                };
                prm[0].Value = searchReqId;

                


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }
        public DataSet SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH(string ApplicationNo)
        {
            try
            {
                string sp_name = "SEARCHPROPERTYMODULE_REACT.SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_APPLICATIONNO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("CUR_TAXDATA", OracleDbType.RefCursor, ParameterDirection.Output),

                };
                prm[0].Value = ApplicationNo;




                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }

    }
}
