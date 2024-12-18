using BBMPCITZAPI.Controllers;
using BBMPCITZAPI.Database;
using BBMPCITZAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NUPMS_BO;
using NUPMS_DA;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Services
{
    public class ObjectionService : IObjectionService
    {

        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;

        public ObjectionService(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
        }
        public DataSet GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP(int ULBCODE, long Propertycode,long objectionid)
        {
            try
            {
                string sp_name = "OBJECTORSMODULE_REACT.GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_ULBCODE", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_OBJECTIONID", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("C_COUNT", OracleDbType.RefCursor, ParameterDirection.Output),
                  new OracleParameter("C_IDENTIFIERSMST",OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_REASONS", OracleDbType.RefCursor, ParameterDirection.Output),
                 new OracleParameter("C_OWNERRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_DOCUPL", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_REASONDOCUPL",OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("C_KAVERI_DOC_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_KAVERI_PROP_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_KAVERI_PARTIES_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output)
              
                };
                prm[0].Value = ULBCODE;
                prm[1].Value = Propertycode;
                prm[2].Value = objectionid;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP stored procedure.");
                throw;
            }
        }
        public DataSet INS_NCL_OBJECTION_MAIN(int ULBCODE, long Propertycode,long PropertyEID)
        {
            try
            {
                string sp_name = "OBJECTORSMODULE_REACT.INS_NCL_OBJECTION_MAIN";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_ULBCODE", OracleDbType.Int32, ParameterDirection.Input),
                 new OracleParameter("P_PropertyEID", OracleDbType.Int64, ParameterDirection.Input),
               new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),

                };
        
                prm[0].Value = Propertycode;
                prm[1].Value = ULBCODE;
                prm[2].Value = PropertyEID;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_OBJECTION_MAIN stored procedure.");
                throw;
            }
        }
        public DataSet INS_NCL_SEARCH_MAIN()
        {
            try
            {
                string sp_name = "SEARCHPROPERTYMODULE_REACT.INS_NCL_SEARCH_MAIN";
                OracleParameter[] prm = new OracleParameter[]
                {
               new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),

                };
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing SEARCHPROPERTYMODULE_REACT.INS_NCL_SEARCH_MAIN stored procedure.");
                throw;
            }
        }
        public DataSet INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE,string EMAIL,string PROPERTYID, string loginId, EKYCDetailsBO objEKYCDetailsBO)
        {
            try
            {
                string sp_name = "OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA";
                OracleParameter[] prm = new OracleParameter[]
                {
                  new OracleParameter("P_TRANSACTIONNO",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_OWNERNAME",OracleDbType.NVarchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERNAME_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_IDENTIFIERNAME",OracleDbType.NVarchar2,ParameterDirection.Input),
              new OracleParameter("P_IDENTIFIERNAME_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERADDRESS",OracleDbType.NVarchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERADDRESS_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_OWNERPHOTO",OracleDbType.Blob,ParameterDirection.Input),
              new OracleParameter("P_MaskedAadhaar",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_OwnerGender",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DATEOFBIRTH",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_IDENTIFIERTYPE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_MOBILENUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_MOBILEVERIFY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_NAMEMATCHSCORE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_VAULTREFNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_AADHAARHASH",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_LOGINID",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter("P_EMAIL",OracleDbType.Varchar2,ParameterDirection.Input),
                   new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("C_OWNERRECORD",OracleDbType.RefCursor,ParameterDirection.Output),
           
                    };

                prm[0].Value = objEKYCDetailsBO.TxnNo;
                prm[1].Value = objEKYCDetailsBO.OwnerNameKnd;
                prm[2].Value = objEKYCDetailsBO.OwnerNameEng;
                prm[3].Value = IdentifierName;
                prm[4].Value = objEKYCDetailsBO.IdentifierNameEng;
                prm[5].Value = objEKYCDetailsBO.AddressKnd;
                prm[6].Value = objEKYCDetailsBO.AddressEng;
                prm[7].Value = objEKYCDetailsBO.photoBytes;
                prm[8].Value = objEKYCDetailsBO.maskedAadhaar;
                prm[9].Value = objEKYCDetailsBO.Gender;
                prm[10].Value = objEKYCDetailsBO.DateOfBirth;
                prm[11].Value = IDENTIFIERTYPE;
                prm[12].Value = MOBILENUMBER;
                prm[13].Value = MOBILEVERIFY;
                prm[14].Value = NAMEMATCHSCORE;
                prm[15].Value = objEKYCDetailsBO.VaultRefNumber;
                prm[16].Value = objEKYCDetailsBO.AadhaarHash;
                prm[17].Value = loginId;
                prm[18].Value = EMAIL;
                prm[19].Value = PROPERTYID;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA stored procedure.");
                throw;
            }
        }

       
        public DataSet NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID)
        {
            try
            {
                string sp_name = "OBJECTORSMODULE_REACT.NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_ORDERNUMBER", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_DOCUMENTTYPEID", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_DOCUMENTEXTENSION", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_DOCUMENTDETAILS", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_SCANNEDDOCUMENT", OracleDbType.Blob, ParameterDirection.Input),
                new OracleParameter("P_ULBCODE", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("P_ORDERDATE", OracleDbType.Date, ParameterDirection.Input),
                new OracleParameter("P_OBJECTIONID", OracleDbType.Int64, ParameterDirection.Input),
                new OracleParameter("cur_user", OracleDbType.RefCursor, ParameterDirection.Output)
                };
                prm[0].Value = NCLPropID.ORDERNUMBER;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = NCLPropID.PROPERTYCODE;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropID.DOCUMENTTYPEID;
                prm[2].Direction = ParameterDirection.Input;
                //prm[3].Direction = ParameterDirection.Input;
                //prm[3].Value = ID_BASIC_PROPERTY;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = NCLPropID.CREATEDBY;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = NCLPropID.DOCUMENTEXTENSION;
                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = NCLPropID.DOCUMENTDETAILS;
                prm[6].Direction = ParameterDirection.Input;
                prm[6].Value = NCLPropID.SCANNEDDOCUMENT;
                prm[7].Direction = ParameterDirection.Input;
                prm[7].Value = NCLPropID.ULBCODE;
                prm[8].Direction = ParameterDirection.Input;
                prm[8].Value = NCLPropID.ORDERDATE;
                prm[9].Direction = ParameterDirection.Input;
                prm[9].Value = NCLPropID.ObjectionId;
                prm[10].Direction = ParameterDirection.Output;
                prm[10].Value = null;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS stored procedure.");
                throw;
            }
        }
        
        public DataSet INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT(INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT final)
        {
            try
            {
                string sp_name = "OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_OBJECTIONID", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_SCANNEDDOCUMENTOBJECTION", OracleDbType.Blob, ParameterDirection.Input),
                new OracleParameter("P_REASONDOCUMENT", OracleDbType.Blob, ParameterDirection.Input),
                new OracleParameter("P_ISCOMMUNICATIONADDRESS", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_REASONID", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_REASONDETAILS", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_ULBCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_NAMEDOCUMENTDETAILS", OracleDbType.Varchar2, ParameterDirection.Input),
                 new OracleParameter("P_REASONDOCUMENTDETAILS", OracleDbType.Varchar2, ParameterDirection.Input),
                 new OracleParameter("P_DOCUMENTEXTENSION", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_DOORNO", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_BUILDINGNAME", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_AREALOCATLITY", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("P_PINCODE", OracleDbType.Varchar2, ParameterDirection.Input),
               
                new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2, ParameterDirection.Input),
                 new OracleParameter("P_TYPEOFDOCUMENT", OracleDbType.Varchar2, ParameterDirection.Input),
                };
                prm[0].Value = final.OBJECTIONID;
                prm[1].Value = final.PROPERTYCODE;
                prm[2].Value = final.SCANNEDDOCUMENTOBJECTION;
                prm[3].Value = final.REASONDOCUMENT;

                prm[4].Value = final.ISCOMMUNICATIONADDRESS;
                prm[5].Value = final.REASONID;

                prm[6].Value = final.REASONDETAILS;
                prm[7].Value = final.ULBCODE;

                prm[8].Value = final.NAMEDOCUMENTDETAILS;

                prm[9].Value = final.REASONDOCUMENTDETAILS;
                prm[10].Value = final.DOCUMENTEXTENSION;
                prm[11].Value = final.DOORNO;

                prm[12].Value = final.BUILDINGNAME;
                prm[13].Value = final.AREALOCATLITY;

                prm[14].Value = final.PINCODE;
                prm[15].Value = final.CREATEDBY;
                prm[16].Value = final.TYPEOFDOCUMENT;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT stored procedure.");
                throw;
            }
        }
        public int INS_NCL_OBJECTION_KAVERI_DOC_DETAILS_TEMP(Int64 objectionid, Int64 propertyCode, string REGISTRATIONNUMBER, string NATUREDEED, string APPLICATIONNUMBER, string REGISTRATIONDATETIME, string KAVERIDOC_RESPONSE_ROWID, string loginId)
        {
            string sp_name = "OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTORS_KAVERI_DOC_DETAILS_TEMP";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_OBJECTIONID",OracleDbType.Int64,ParameterDirection.Input),
               new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_REGISTRATIONNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_NATUREDEED",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_APPLICATIONNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_REGISTRATIONDATETIME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_KAVERIDOC_RESPONSE_ROWID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
             
                    };

            prm[0].Value = objectionid;
            prm[1].Value = propertyCode;
            prm[2].Value = REGISTRATIONNUMBER;
            prm[3].Value = NATUREDEED;
            prm[4].Value = APPLICATIONNUMBER;
            prm[5].Value = REGISTRATIONDATETIME;
            prm[6].Value = KAVERIDOC_RESPONSE_ROWID;
            prm[7].Value = loginId;

            try
            {
                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public int INS_NCL_OBJECTION_KAVERI_PARTIES_DETAILS_TEMP(Int64 objectionid, Int64 propertyCode, string REGISTRATIONNUMBER, string PARTYNAME, string PARTYADDRESS, string IDPROOFTYPE, string IDPROOFNUMBER, string PARTYTYPE, string ADMISSIONDATE, string KAVERIDOC_RESPONSE_ROWID, string loginId, int EKYC_OWNERNO, string EKYC_OWNERNAME, int NAMEMATCH_SCORE)
        {
            try
            {
                string sp_name = "OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTORS_KAVERI_PARTIES_DETAILS_TEMP";
                OracleParameter[] prm = new OracleParameter[]
                {
                 new OracleParameter("P_OBJECTIONID",OracleDbType.Int64,ParameterDirection.Input),
                     new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_REGISTRATIONNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_PARTYNAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_PARTYADDRESS",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_IDPROOFTYPE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_IDPROOFNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_PARTYTYPE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_ADMISSIONDATE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_KAVERIDOC_RESPONSE_ROWID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_EKYC_OWNERNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_EKYC_OWNERNAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_NAMEMATCH_SCORE",OracleDbType.Int32,ParameterDirection.Input)
                    };

                prm[0].Value = objectionid;
                prm[1].Value = propertyCode;
                prm[2].Value = REGISTRATIONNUMBER;
                prm[3].Value = PARTYNAME;
                prm[4].Value = PARTYADDRESS;
                prm[5].Value = IDPROOFTYPE;
                prm[6].Value = IDPROOFNUMBER;
                prm[7].Value = PARTYTYPE;
                prm[8].Value = ADMISSIONDATE;
                prm[9].Value = KAVERIDOC_RESPONSE_ROWID;
                prm[10].Value = loginId;
                prm[11].Value = EKYC_OWNERNO;
                prm[12].Value = EKYC_OWNERNAME;
                prm[13].Value = NAMEMATCH_SCORE;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTORSMODULE_REACT.INS_NCL_PROPERTY_OBJECTORS_KAVERI_PARTIES_DETAILS_TEMP stored procedure.");
                throw;
            }
        }
        public int INS_NCL_OBJECTION_KAVERI_PROPERTY_DETAILS_TEMP(Int64 objectionId, Int64 propertyCode, string REGISTRATIONNUMBER, string PROPERTYID, string DOCUMENTID, string VILLAGENAME, string SRONAME, string HOBLINAME, string ZONENAME, double TotalArea, string KAVERIDOC_RESPONSE_ROWID, string loginId)
        {
            string sp_name = "OBJECTORSMODULE_REACT.INS_NCL_OBJECTION_KAVERI_PROPERTY_DETAILS_TEMP";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_OBJECTIONID",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_REGISTRATIONNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_VILLAGENAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_SRONAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_HOBLINAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_ZONENAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_TOTALAREA", OracleDbType.Double,ParameterDirection.Input),
              new OracleParameter("P_KAVERIDOC_RESPONSE_ROWID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

            prm[0].Value = propertyCode;
            prm[1].Value = objectionId;
            prm[2].Value = REGISTRATIONNUMBER;
            prm[3].Value = PROPERTYID;
            prm[4].Value = DOCUMENTID;
            prm[5].Value = VILLAGENAME;
            prm[6].Value = SRONAME;
            prm[7].Value = HOBLINAME;
            prm[8].Value = ZONENAME;
            prm[9].Value = TotalArea;
            prm[10].Value = KAVERIDOC_RESPONSE_ROWID;
            prm[11].Value = loginId;

            try
            {
                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public int NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL(Models.NCLPropIdentification NCLPropID)
        {
            string sp_name = "OBJECTORSMODULE_REACT.NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_OBJECTIONID",OracleDbType.Int64,ParameterDirection.Input),
            
                    };

            prm[0].Value = NCLPropID.PROPERTYCODE;
            prm[1].Value = NCLPropID.ObjectionId;
        

            try
            {
                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet SEL_CitzeObjectionAcknowledgement(long ObjectionId, long propertyCode, string loginID,long WardId)
        {
            string sp_name = "OBJECTORSMODULE_REACT.SEL_CitzeObjectionAcknowledgement";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_OBJECTIONID",OracleDbType.Int64,ParameterDirection.Input),
               new OracleParameter("P_LOGINID",OracleDbType.Varchar2,ParameterDirection.Input),
               new OracleParameter("V_WARDID",OracleDbType.Int64,ParameterDirection.Input),
                new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output),
                    };

            prm[0].Value = propertyCode;
            prm[1].Value = ObjectionId;
            prm[2].Value = loginID;
            prm[3].Value = WardId;

            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }



    }
    }
