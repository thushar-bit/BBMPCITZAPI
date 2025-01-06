using NUPMS_BO;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;

namespace BBMPCITZAPI.Services.Interfaces
{
    public interface IMutationObjectionService
    {
        public DataSet INS_NCL_MUTATION_OBJECTION_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string EMAIL, string loginId, EKYCDetailsBO objEKYCDetailsBO);
        public DataSet INS_NCL_MUTATION_OBJECTION_FINAL_SUBMIT(INS_NCL_PROPERTY_MUTATION_OBJECTION_FINAL_SUBMIT final);
        public DataSet SEL_CitzeSearchAck(int searchReqId);
        public DataSet INS_NCL_MUTATION_OBJECTION_MAIN();

    }
}
