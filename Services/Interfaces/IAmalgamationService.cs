using NUPMS_BO;
using System.Data;
using static BBMPCITZAPI.Models.Amalgamation;

namespace BBMPCITZAPI.Services.Interfaces
{
    public interface IAmalgamationService
    {
        public DataSet GET_NCL_MUTATION_AMALGAMATION_MAIN(string propertyid, int ulbcode);
        public DataSet GeneratateMuttaplid(Int64 propertycode, string propertyid,Int64 MutationApplID);
        public DataSet INS_NCL_AMAL_APPL_MAIN(int ulbcode, Int64 MutationApplID, string createdby);

        public int GET_NCL_MUTATION_AMALGAMATION_MAIN_COUNT(string propertyid, int ulbcode);


        public DataSet INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string EMAIL, string loginId, EKYCDetailsBO objEKYCDetailsBO);
        public DataSet INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT_APPL(AMalGamation_final AmalFinal);
        public DataSet INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(AMalGamation_final AmalFinal);
        public DataSet CopyNPMtoNMTMain(AMalGamation_final AmalFinal);
        public DataSet NCL_AMALGAMATION_DOCUMENT_TEMP_DEL(Int64 MutationApplicatioId, Int32 DocumentId);
        public DataSet NCL_AMALGAMATION_DOCUMENTS_TEMP_INS(int ID_BASIC_PROPERTY, Models.NCLPropIdentification NCLPropID);

        public int REC_MUTATION_APPL(Int64 PROPERTYCODE, int ULBCODE, Int64 MUTAPPLID, NWFTaskFlow objNWF, NMT_APPL_MAIN objMAIN);
    }
}
