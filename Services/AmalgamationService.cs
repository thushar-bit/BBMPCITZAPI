using BBMPCITZAPI.Controllers;
using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using NUPMS_BA;
using NUPMS_BO;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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

        public DataSet INS_NCL_MUTATION_AMALGAMATION_MAIN()
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.INS_NPM_AMALGAMATION_MAIN";
                OracleParameter[] prm = new OracleParameter[]
                {
               new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),

                };
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

        public DataSet INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(string[] propertyId)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT";
                OracleParameter[] prm = new OracleParameter[]
                {
                  new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,ParameterDirection.Input),

              new OracleParameter("C_MAIN",OracleDbType.RefCursor,ParameterDirection.Output),

                    };
                string commaSeparated = string.Join(",", propertyId);
                prm[0].Value = commaSeparated;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }


        public DataSet GetAmalgamationProperties(string[] propertyId)
        {
            try
            {
                string sp_name = "AMALGAMATION_REACT.GET_EKHATA_OWNERS";
                OracleParameter[] prm = new OracleParameter[]
                {
                  new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,ParameterDirection.Input),
          
              new OracleParameter("C_MAIN",OracleDbType.RefCursor,ParameterDirection.Output),

                    };
                string commaSeparated = string.Join(",", propertyId);
                prm[0].Value = commaSeparated;
           
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AMALGAMATION_REACT.GetAmalgamationProperty stored procedure.");
                throw;
            }
        }

    }
}
