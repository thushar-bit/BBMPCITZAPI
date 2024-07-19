using BBMPCITZAPI.Models;
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
        int Insert_PROPERTY_ADDRESS_TEMP(Insert_PROPERTY_ADDRESS_TEMP insertCITZProperty);
        #endregion
        #region AreaDimension
        int UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP(UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP);
        int UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI(UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI);
        int UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA(UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA);
        #endregion
        #region Site Details Events
        public int UPD_NCL_PROPERTY_SITE_TEMP_USAGE(UPD_NCL_PROPERTY_SITE_TEMP_USAGE UPD_NCL_PROPERTY_SITE_TEMP_USAGE);
        public DataSet GetNPMMasterTable(int FeaturesHeadID);
        #endregion
        #region Building Details Events
        public int DEL_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG);
        public DataSet GET_NCL_FLOOR_AREA(NCLBuilding NCLBLDG);
        public DataSet GET_NCL_TEMP_FLOOR_PRE(NCLBuilding NCLBLDG);
        public int DEL_INS_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG);
        #endregion
        #region MultiStory Details Events
        public DataSet GET_NCL_MOB_TEMP_FLOOR_AREA(int PROPERTYCODE);
        public int INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ULBCODE, NCLAPARTMENT NCLAPT);
        #endregion
        #region Owner Details Events
        public DataSet COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(int propertyCode, int ownerNumber);
        public DataSet DEL_SEL_NCL_PROP_OWNER_TEMP(int propertyCode, int ownerNumber);
        public void INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ID_BASIC_PROPERTY, NCLPropOwner NCLOwner, NCLPropOwnerID NCLOwnerID, NCLPropOwnerIDDoc NCLOwnerIDDOC, NCLPropOwnerPhoto NCLOwnerPhoto, string digilockerid);
        #endregion
        #region Property Rights
        int NCL_PROPERTY_RIGHTS_TEMP_INS(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight);
        int NCL_PROPERTY_RIGHTS_TEMP_UPD(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight);
        int NCL_PROPERTY_RIGHTS_TEMP_DEL(int RIGHTSID, int ID_BASIC_PROPERTY, int ULBCODE, int PROPERTYCODE);
        #endregion
        #region Document Upload Events
         int NCL_PROPERTY_ID_TEMP_INS(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID);
         DataSet GetNCLDocView(int DOCUMENTID, int PROPERTYCODE);
         DataSet NCL_PROPERTY_ID_TEMP_DEL(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID);
         DataSet GetMasterDocByCategoryOrClaimType(int ULBCODE, int CATEGORYID, int ClaimTypeID);
        #endregion
        #region Classification Document Upload Events
         DataSet INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(NCLClassPropIdentification nCLClassPropIdentification);
         DataSet DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID);
         DataSet SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID);
         DataSet GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS(int CLASSIFICATIONID, int SUBCLASSIFICATIONID1, int SUBCLASSIFICATIONID2);
         DataSet GetPropertySubClassByULBAndCategory(int PropCatID, int ulbcode);
        #endregion
        #region Tax Events
         DataSet InsertBBMPPropertyTaxResponse(int UlbCode, string Json, string Response, string IpAddress, string Createdby, string oParamater);
        #endregion
        #region Objection Events
         DataSet InsertBBMPPropertyTaxResponse(int PROPERTYCODE, string OBJECTIONDETAILS, byte[] SCANNEDDOCUMENT, string DOCUMENTDETAILS, string CREATEDBY);
        #endregion
        #region eSignCode
        #endregion
        float Fn_CPlus_NameMatchJulyFinal2023(string n1_input, string n2_input);
        DataSet GetUserCitizen(string userId);
    }
}
