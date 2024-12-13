using BBMPCITZAPI.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NUPMS_BA;
using System.Data;
using static BBMPCITZAPI.Services.BBMPBookModuleService;

namespace BBMPCITZAPI.Services.Interfaces
{
    public interface IBBMPBookModuleService
    {
        #region InitialRegion
        DataSet GET_PROPERTY_PENDING_CITZ_NCLTEMP(int ULBCODE, int Propertycode);
      
        DataSet GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(int ULBCODE, int Propertycode);
        DataSet GetMasterTablesData(string ULBCODE);
        public DataSet GetMasterTablesData_React(string ULBCODE, string Page);
        public DataSet GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React(string ULBCODE, long Propertycode, string Page);
        public DataSet GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(int ULBCODE, long BOOKS_PROP_APPNO, long propertyCode, string Page);
     
       
        int INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP(INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP insertCITZProperty);
        int INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2(INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2 insertCITZProperty);
        #endregion
        #region AreaDimension
        int UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP(UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP);
        int UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI(UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI);
        int UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA(UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA);
        public int UPD_AREA_CHECKBANDI_KAVERI_DATA(UPD_AREA_CHECKBANDI_KAVERI_DATA UPD_AREA_CHECKBANDI_KAVERI_DATA);
        #endregion
        #region Site Details Events
        public int UPD_NCL_PROPERTY_SITE_TEMP_USAGE(UPD_NCL_PROPERTY_SITE_TEMP_USAGE UPD_NCL_PROPERTY_SITE_TEMP_USAGE);
        public DataSet GetNPMMasterTable(int FeaturesHeadID);
        #endregion
        #region Building Details Events
        public DataSet DEL_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG);
        public DataSet GET_NCL_FLOOR_AREA(NCLBuilding NCLBLDG);
        public DataSet GET_NCL_TEMP_FLOOR_PRE(NCLBuilding NCLBLDG);
        public int DEL_INS_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG);
        #endregion
        #region MultiStory Details Events
        public DataSet GET_NCL_MOB_TEMP_FLOOR_AREA(int PROPERTYCODE);
        public int INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ULBCODE, NCLAPARTMENT NCLAPT);
        #endregion
        #region Owner Details Events
        public DataSet COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(int P_BOOKS_PROP_APPNO, int propertyCode, int ownerNumber);
        public DataSet DEL_SEL_NCL_PROP_OWNER_TEMP(int P_BOOKS_PROP_APPNO, int propertyCode, int ownerNumber);
        public void INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ID_BASIC_PROPERTY, NCLPropOwner NCLOwner, NCLPropOwnerID NCLOwnerID, NCLPropOwnerIDDoc NCLOwnerIDDOC, NCLPropOwnerPhoto NCLOwnerPhoto, string digilockerid);
        #endregion
        #region Property Rights
        int NCL_PROPERTY_RIGHTS_TEMP_INS(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight);
        int NCL_PROPERTY_RIGHTS_TEMP_UPD(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight);
        int NCL_PROPERTY_RIGHTS_TEMP_DEL(int P_BOOKS_PROP_APPNO, int RIGHTSID, int ID_BASIC_PROPERTY, int ULBCODE, int PROPERTYCODE);
        #endregion
        #region Document Upload Events
         int NCL_PROPERTY_ID_TEMP_INS(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID);
         DataSet GetNCLDocView(int DOCUMENTID, int PROPERTYCODE);
         DataSet NCL_PROPERTY_ID_TEMP_DEL(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID);
         DataSet GetMasterDocByCategoryOrClaimType(int ULBCODE, int CATEGORYID, int ClaimTypeID);
        #endregion
        #region Classification Document Upload Events
         DataSet INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP(NCLClassPropIdentification nCLClassPropIdentification);
         DataSet DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int P_DOC_SCAN_ID, int P_BOOKS_PROP_APPNO);
         DataSet SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID);
         DataSet GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS(int PROPERTYCATEGORYID, int CLASSIFICATIONID, int SUBCLASSIFICATIONID);
         DataSet GetPropertySubClassByULBAndCategory(int PropCatID, int ulbcode);
        #endregion
        #region Tax Events
        DataSet GetTaxDetails(string applicationNo, long propertycode, long P_BOOKS_PROP_APPNO,string loginId);
         DataSet InsertBBMPPropertyTaxResponse(int UlbCode, string Json, string Response, string IpAddress, string Createdby, string oParamater);
        #endregion
        #region Objection Events
         DataSet InsertBBMPPropertyTaxResponse(int PROPERTYCODE, string OBJECTIONDETAILS, byte[] SCANNEDDOCUMENT, string DOCUMENTDETAILS, string CREATEDBY);

        #endregion
        #region eSignCode
        #endregion
       
        DataSet GetUserCitizen(string userId);
       public int Insert_React_UserFlow(int P_BOOKS_PROP_APPNOAPPNO, int propertyCode, int currentStep, int DraftWardId, int DraftZoneId, string LoginID);
        public DataSet Get_React_UserFlow( Int64 propertyCode,Int64 BOOKAPP_NO, string LoginID);
        public int Ins_EsignPDf(Int64 propertycode, Int64 bookspropappno, byte[] pdfFilePath, string appearance);
        public DataSet Get_ESignPdf(Int64 propertycode, Int64 bookspropappno);
        public int Ins_PDF_Draft_Exception_log(string propertycode, string Propertyid, string status, string output,string response);
        public DataSet GetEAASTHIStatus();
        public DataSet GetEAASTHIDailyReport();
        public DataSet GetEAASTHIDailyReportDetails(int wardNumber, string QueryName, int pageNo, int PageSize);
        public int INS_NPM_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP(Int64 propertyCode, string? ECNUMBER, string? REGISTRATIONNUMBER, string? IS_LATEST_REGISTRATIONNO, string? LATEST_REGISTRATIONNO, string? DISTRICTNAME, string? TALUKANAME, string? VILLAGENAME, string? HOBLINAME, string? ARTICLENAME, string? EXECUTIONDATE, Int64 KAVERIECDOC_RESPONSE_ROWID, string loginID);
    }
}
