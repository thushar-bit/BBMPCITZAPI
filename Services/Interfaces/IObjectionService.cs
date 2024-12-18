using NUPMS_BO;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Services.Interfaces
{
    public interface IObjectionService
    {
        public DataSet GET_PROPERTY_OBJECTORS_CITZ_NCLTEMP(int ULBCODE, long Propertycode,long objectionid);
        public DataSet INS_NCL_OBJECTION_MAIN(int ULBCODE, long Propertycode,long PropertyEID);
        public DataSet INS_NCL_SEARCH_MAIN();
        public DataSet INS_NCL_PROPERTY_OBJECTOR_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE, string EMAIL, string PROPERTYID, string loginId, EKYCDetailsBO objEKYCDetailsBO);
        public int INS_NCL_OBJECTION_KAVERI_DOC_DETAILS_TEMP(Int64 BOOKS_PROP_APPNO, Int64 propertyCode, string REGISTRATIONNUMBER, string NATUREDEED, string APPLICATIONNUMBER, string REGISTRATIONDATETIME, string KAVERIDOC_RESPONSE_ROWID, string loginId);
        public int INS_NCL_OBJECTION_KAVERI_PARTIES_DETAILS_TEMP(Int64 BOOKS_PROP_APPNO, Int64 propertyCode, string REGISTRATIONNUMBER, string PARTYNAME, string PARTYADDRESS, string IDPROOFTYPE, string IDPROOFNUMBER, string PARTYTYPE, string ADMISSIONDATE, string KAVERIDOC_RESPONSE_ROWID, string loginId, int EKYC_OWNERNO, string EKYC_OWNERNAME, int NAMEMATCH_SCORE);
        public DataSet NCL_OBJECTION_OBJECTION_DOCUMENTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID);
        public DataSet INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT(INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT final);
        public int INS_NCL_OBJECTION_KAVERI_PROPERTY_DETAILS_TEMP(Int64 BOOKS_PROP_APPNO, Int64 propertyCode, string REGISTRATIONNUMBER, string PROPERTYID, string DOCUMENTID, string VILLAGENAME, string SRONAME, string HOBLINAME, string ZONENAME, double TotalArea, string KAVERIDOC_RESPONSE_ROWID, string loginId);

        public int NCL_PROPERTY_OBJECTION_DOCUMENT_TEMP_DEL(Models.NCLPropIdentification NCLPropID);
        public DataSet SEL_CitzeObjectionAcknowledgement(long ObjectionId, long propertyCode, string loginID,long WardId);

    }
}
