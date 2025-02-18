using NUPMS_BO;
using System.Data;

namespace BBMPCITZAPI.Services.Interfaces
{
    public interface IAmalgamationService
    {
        public DataSet INS_NCL_MUTATION_AMALGAMATION_MAIN();
        public DataSet GetAmalgamationProperties(string[] propertyId);
        public DataSet INS_NCL_PROPERTY_AMAL_TEMP_WITH_EKYCDATA(string MOBILENUMBER, string MOBILEVERIFY, string EMAIL, string loginId, EKYCDetailsBO objEKYCDetailsBO);
        public DataSet INS_NCL_PROPERTY_AMAL_FINAL_SUBMIT(string[] propertyId);
    }
}
