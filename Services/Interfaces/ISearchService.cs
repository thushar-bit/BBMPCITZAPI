using NUPMS_BO;
using System.Data;
using static BBMPCITZAPI.Models.ObjectionModels;
namespace BBMPCITZAPI.Services.Interfaces
{
    public interface ISearchService 
    {
        public DataSet INS_NCL_PROPERTY_SEARCH_TEMP_WITH_EKYCDATA( string MOBILENUMBER, string MOBILEVERIFY, string EMAIL,  string loginId, EKYCDetailsBO objEKYCDetailsBO);
        public DataSet INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT(INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT final);
        public DataSet SEL_CitzeSearchAck(int searchReqId);
        public DataSet SEL_OFFLINE_PTAX_BY_APPLICATION_SEARCH(string ApplicationNo);
    }
}
