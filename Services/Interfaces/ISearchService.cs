using NUPMS_BO;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;
namespace BBMPCITZAPI.Services.Interfaces
{
    public interface ISearchService 
    {
        public DataSet INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA(int IDENTIFIERTYPE, string IdentifierName, string MOBILENUMBER, string MOBILEVERIFY, int NAMEMATCHSCORE, string EMAIL,  string loginId, EKYCDetailsBO objEKYCDetailsBO);
        public DataSet INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT(INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT final);
    }
}
