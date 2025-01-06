using BBMPCITZAPI.Controllers;
using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using NUPMS_BO;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Services
{
    public class MutationObjectionService : IMutationObjectionService
    {

        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IBBMPBookModuleService _auth;

        public MutationObjectionService(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IBBMPBookModuleService Auth)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _auth = Auth;
        }
        public DataSet INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string EMAIL, string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            try
            {
                string sp_name = "MUTATIONOBJECTIONMODULE_REACT.INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA";
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
                _logger.LogError(ex, "Error occurred while executing MUTATIONOBJECTIONMODULE_REACT.INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA stored procedure.");
                throw;
            }
        }
        public DataSet INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT(INS_NCL_PROPERTY_MUTATION_OBJECTION_FINAL_SUBMIT final)
        {
            try
            {
                string sp_name = "MUTATIONOBJECTIONMODULE_REACT.INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_MUTATION_OBJECTION_REQ_ID", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_IDDocument", OracleDbType.Blob, ParameterDirection.Input),
                new OracleParameter("P_IDCardNumber", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_MobileNumber", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_REASONDETAILS", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_Mobiverify", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_Email", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_LOGINID", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_CREATEDIP", OracleDbType.Varchar2, ParameterDirection.Input),
               
                         new OracleParameter("C_RECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                };
                prm[0].Value = final.Mutatation_Req_Id;
                prm[1].Value = final.ObjectionDocument;
                prm[2].Value = final.ObjectionDocumentName;
                prm[3].Value = final.MobileNumber;
                prm[4].Value = final.REASONDETAILS;
                prm[5].Value = final.Mobiverify;
                prm[6].Value = final.Email;
                prm[7].Value = final.LoginId;
                prm[8].Value = _auth.GetIPAddress();

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing MUTATIONOBJECTIONMODULE_REACT.INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }
        public DataSet INS_NCL_MUTATION_OBJECTION_MAIN()
        {
            try
            {
                string sp_name = "MUTATIONOBJECTIONMODULE_REACT.INS_NCL_MUTATION_OBJECTION_MAIN";
                OracleParameter[] prm = new OracleParameter[]
                {
               new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),

                };
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing MUTATIONOBJECTIONMODULE_REACT.INS_NCL_MUTATION_OBJECTION_MAIN stored procedure.");
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
                _logger.LogError(ex, "Error occurred while executing MUTATIONOBJECTIONMODULE_REACT.INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }
       

    }
}
