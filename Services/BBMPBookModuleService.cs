using Oracle.ManagedDataAccess.Client;
using System.Data;
using Microsoft.Extensions.Configuration;
using BBMPCITZAPI.Services.Interfaces;
using BBMPCITZAPI.Controllers;
using BBMPCITZAPI.Database;
using BBMPCITZAPI.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using NUPMS_DA;
using iTextSharp.text.pdf;
using NUPMS_BA;

namespace BBMPCITZAPI.Services
{
    public class BBMPBookModuleService : IBBMPBookModuleService
    {

        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BBMPBookModuleService(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Initial
        public DataSet GET_PROPERTY_PENDING_CITZ_BBD_DRAFT(int ULBCODE, int Propertycode)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_ULBCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("C_COUNT", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_SITE", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_DIMENSION", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_COORDINATES", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_OWNERRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_BUILDINGTYPE", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_APARTDET", OracleDbType.RefCursor, ParameterDirection.Output)
                };
                prm[0].Value = ULBCODE;
                prm[1].Value = Propertycode;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT stored procedure.");
                throw;
            }
        }
        public DataSet GetMasterTablesData(string ULBCODE)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_MST_DET_ULB";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_USERPROPDET",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_BLOCKMST",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_STREETMST",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_VILLAGEMST",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_WARDMST",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_ApartDet",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_PROPCategory",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_MULTIOWNER",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_IDENTIFIERTYPE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_IDENTITYDOC",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_LIAB",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_FLOORTYPE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_ROOFTYPE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_WOODTYPE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_BUILDTYPE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_FLOORNUM",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_FEATUREHEAD",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_BUILDINGUSAGE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_DOCUMENTTYPEUPL",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_PROPERTYCLASS",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = ULBCODE;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT stored procedure.");
                throw;
            }
        }
        public DataSet GetMasterTablesData_React(string ULBCODE, string Page)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.GET_MST_DATA_ULB_REACT";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),
               new OracleParameter("Page_Render", OracleDbType.Varchar2, ParameterDirection.Input),
                  new OracleParameter("C_PROPCategory", OracleDbType.RefCursor, ParameterDirection.Output),
              new OracleParameter("C_WARDMST",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_STREETMST",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_IDENTIFIERTYPE",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_FLOORNUM",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_FEATUREHEAD",OracleDbType.RefCursor,ParameterDirection.Output),
                 new OracleParameter("C_BUILDINGUSAGE",OracleDbType.RefCursor,ParameterDirection.Output),
                    new OracleParameter("C_PROPERTYCLASS",OracleDbType.RefCursor,ParameterDirection.Output),
                  new OracleParameter("C_PROPERTYSUBCLASS",OracleDbType.RefCursor,ParameterDirection.Output),
                   new OracleParameter("C_MSTYEAR",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = ULBCODE;
                prm[1].Value = Page;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT stored procedure.");
                throw;
            }
        }
        public DataSet GET_PROPERTY_PENDING_CITZ_BBD_DRAFT_React(string ULBCODE,long Propertycode, string Page)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_ULBCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int32, ParameterDirection.Input),
                 new OracleParameter("Page_Render", OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("C_COUNT", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_SITE", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_DIMENSION", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_COORDINATES", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_OWNERRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_BUILDINGTYPE", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_APARTDET", OracleDbType.RefCursor, ParameterDirection.Output)
                };
                prm[0].Value = ULBCODE;
                prm[1].Value = Propertycode;
                prm[2].Value = Page;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_BBD_DRAFT stored procedure.");
                throw;
            }
        }
        public DataSet GET_PROPERTY_PENDING_CITZ_NCLTEMP_React(int ULBCODE, long BOOKS_PROP_APPNO, long propertyCode,string Page)
        {
            string commandText = "OBJECTIONMODULE_REACT.GET_PROPERTY_PENDING_CITZ_NCLTEMP";
            OracleParameter[] array = new OracleParameter[24]
            {
            new OracleParameter("P_ULBCODE", OracleDbType.Int32, ParameterDirection.Input),
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64, ParameterDirection.Input),
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64, ParameterDirection.Input),
             new OracleParameter("Page_Render", OracleDbType.Varchar2, ParameterDirection.Input),
            new OracleParameter("C_COUNT", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_SITE", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_DIMENSION", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_COORDINATES", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_OWNERRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_APARTDET", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_BUILDINGTYPE", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_DOCUPL", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_CLASSDOCS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_PROPADDRESS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_TAXDATA", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_BBDOWNERRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_KAVERI_DOC_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_KAVERI_PROP_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_KAVERI_PARTIES_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_KAVERIEC_PROP_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_KAVERIEC_PARTIES_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_PROP_BESCOM_DETAILS", OracleDbType.RefCursor, ParameterDirection.Output),
            new OracleParameter("C_MATRIX",OracleDbType.RefCursor,ParameterDirection.Output)
            };
            array[0].Value = ULBCODE;
            array[1].Value = propertyCode;
            array[2].Value = BOOKS_PROP_APPNO;
            array[3].Value = Page;
            try
            {
                return _databaseService.ExecuteDataset(commandText, array);
            }
            catch (OracleException ex)
            {
                throw ex;
            }
        }

        public DataSet GET_PROPERTY_PENDING_CITZ_NCLTEMP(int ULBCODE, int Propertycode)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_NCLTEMP";
                OracleParameter[] prm = new OracleParameter[]
                {
                new OracleParameter("P_ULBCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("P_PROPERTYCODE", OracleDbType.Int32, ParameterDirection.Input),
                new OracleParameter("C_COUNT", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_MAIN", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_SITE", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_SURVEYNOS", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_PHOTO", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_DIMENSION", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_SUBCLASS", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_COORDINATES", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_CLASSDOCS", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_OWNERRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_LIABRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_RIGHTSRECORD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_MOBUILD", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_APARTDET", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_BUILDINGTYPE", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_DOCUPL", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_TENANTDET", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("C_PROPADDRESS", OracleDbType.RefCursor, ParameterDirection.Output)
                };
                prm[0].Value = ULBCODE;
                prm[1].Value = Propertycode;

                return _databaseService.ExecuteDataset(sp_name, prm);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GET_PROPERTY_PENDING_CITZ_NCLTEMP stored procedure.");
                throw;
            }
        }
        public  int INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP(INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP insertCITZProperty) 
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INS_UPD_NCL_PROPERTY_CATEGORY_SASDATA_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
    new OracleParameter("P_PROPERTYCODE", OracleDbType.Int32, ParameterDirection.Input),
   new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
    new OracleParameter("P_PROPERTYCATEGORYID", OracleDbType.Int32, ParameterDirection.Input),
    new OracleParameter("P_PUID", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2, ParameterDirection.Input),
    
  
};

                // Assign values to parameters
                prm[0].Value = insertCITZProperty.propertyCode;
                prm[1].Value = insertCITZProperty.P_BOOKS_PROP_APPNO;
                prm[2].Value = insertCITZProperty.categoryId;  // Ensure this matches the type expected
                prm[3].Value = insertCITZProperty.PUIDNo;
                prm[4].Value = insertCITZProperty.loginId;
             
            


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2(INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2 insertCITZProperty)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INS_UPD_NCL_PROPERTY_ADDRESS_TEMP2";
                OracleParameter[] prm = new OracleParameter[] {
    new OracleParameter("P_PROPERTYCODE", OracleDbType.Int32, ParameterDirection.Input),
      new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
    new OracleParameter("P_STREETID", OracleDbType.Int32, ParameterDirection.Input),
    new OracleParameter("P_DOORNO", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_BUILDINGNAME", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_AREAORLOCALITY", OracleDbType.Varchar2, ParameterDirection.Input),
     new OracleParameter("P_STREETNAME", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_LANDMARK", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_PINCODE", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_PROPERTYPHOTO", OracleDbType.Blob, ParameterDirection.Input),
    new OracleParameter("P_LATITUDE", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_LONGITUDE", OracleDbType.Varchar2, ParameterDirection.Input),
    new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2, ParameterDirection.Input),
   
  
};

                // Assign values to parameters
                prm[0].Value = insertCITZProperty.propertyCode;
                prm[1].Value = insertCITZProperty.P_BOOKS_PROP_APPNO;
                prm[2].Value = insertCITZProperty.STREETID;
                prm[3].Value = insertCITZProperty.DOORNO;
                prm[4].Value = insertCITZProperty.BUILDINGNAME;
                prm[5].Value = insertCITZProperty.AREAORLOCALITY;
                prm[6].Value = insertCITZProperty.STREETNAME;
                prm[7].Value = insertCITZProperty.LANDMARK;
                prm[8].Value = insertCITZProperty.PINCODE;
                prm[9].Value = insertCITZProperty.PROPERTYPHOTO;
                prm[10].Value = insertCITZProperty.Latitude;
                prm[11].Value = insertCITZProperty.Longitude;
                prm[12].Value = insertCITZProperty.loginId;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        //
        #region AreaDimension
        public int UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP(UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_SITEORBUILDING",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_EVENORODDSITE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_SITEAREA",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_SITEAREAFT",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_BUILDINGAREA",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_BUILDINGAREAFT",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_EASTWEST",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_NORTHSOUTH",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_EWODDSITE1FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_EWODDSITE2FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_EWODDSITE3FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_EWODDSITE4FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_NSODDSITE1FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_NSODDSITE2FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_NSODDSITE3FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_NSODDSITE4FT",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_SIDE9",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_SIDE10",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_OddSiteSides",OracleDbType.Int32,ParameterDirection.Input),
               new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),  
                    new OracleParameter("P_SITEAREA_KAVERI_MT",OracleDbType.Decimal,ParameterDirection.Input),
                    new OracleParameter("P_SITEAREA_KAVERI_FT",OracleDbType.Decimal,ParameterDirection.Input),
                    new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

                prm[0].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SITEORBUILDING;
                prm[2].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.EVENORODDSITE;
                prm[3].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SITEAREA;
                prm[4].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SITEAREAFT;
                prm[5].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.BUILDINGAREA;
                prm[6].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.BUILDINGAREAFT;
                prm[7].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.EASTWEST;
                prm[8].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.NORTHSOUTH;
                prm[9].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.EWODDSITE1FT;
                prm[10].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.EWODDSITE2FT;
                prm[11].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.EWODDSITE3FT;
                prm[12].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.EWODDSITE4FT;
                prm[13].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.NSODDSITE1FT;
                prm[14].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.NSODDSITE2FT;
                prm[15].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.NSODDSITE3FT;
                prm[16].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.NSODDSITE4FT;
                prm[17].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SIDE9;
                prm[18].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SIDE10;
                prm[19].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.OddSiteSides;
               
                prm[20].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.P_BOOKS_PROP_APPNO;
                prm[21].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SITEAREA_KAVERI_MT;
                prm[22].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.SITEAREA_KAVERI_FT;
                prm[23].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.loginId;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI(UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_NORTH_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_SOUTH_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_EAST_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_WEST_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
                     new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Varchar2,ParameterDirection.Input),
                    };

                prm[0].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_NORTH;
                prm[2].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_SOUTH;
                prm[3].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_EAST;
                prm[4].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_WEST;
                prm[5].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.loginId;
                prm[6].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.P_BOOKS_PROP_APPNO;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int UPD_AREA_CHECKBANDI_KAVERI_DATA(UPD_AREA_CHECKBANDI_KAVERI_DATA UPD_AREA_CHECKBANDI_KAVERI_DATA)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.UPD_AREA_CHECKBANDI_KAVERI_DATA";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_SITEAREA_KAVERI_MT",OracleDbType.Double,ParameterDirection.Input),
              new OracleParameter("P_SITEAREA_KAVERI_FT",OracleDbType.Double,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_NORTH_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_SOUTH_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_EAST_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CHECKBANDI_WEST_EN",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
                   
                    };

                prm[0].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.propertyCode;
                prm[1].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.P_BOOKS_PROP_APPNO;
                prm[2].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.SITEAREA_KAVERI_MT;
                prm[3].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.SITEAREA_KAVERI_FT;
                prm[4].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.CHECKBANDI_NORTH;
                prm[5].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.CHECKBANDI_SOUTH;
                prm[6].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.CHECKBANDI_EAST;
                prm[7].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.CHECKBANDI_WEST;
                prm[8].Value = UPD_AREA_CHECKBANDI_KAVERI_DATA.loginId;
              

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA(UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_CARPETAREA",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_ADDITIONALAREA",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_SUPERBUILTUPAREA",OracleDbType.Decimal,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
                  new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input)
                    };

                prm[0].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.CARPETAREA;
                prm[2].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.ADDITIONALAREA;
                prm[3].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.SUPERBUILTUPAREA;
                prm[4].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.loginId;
                prm[5].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.P_BOOKS_PROP_APPNO;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        #region Site Details Events
        public int UPD_NCL_PROPERTY_SITE_TEMP_USAGE(UPD_NCL_PROPERTY_SITE_TEMP_USAGE UPD_NCL_PROPERTY_SITE_TEMP_USAGE)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.UPD_NCL_PROPERTY_SITE_TEMP_USAGE";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_FEATUREHEADID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_FEATUREID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_BUILTYEAR",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
                   new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input)
                    };

                prm[0].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.FEATUREHEADID;
                prm[2].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.FEATUREID;
                prm[3].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.BUILTYEAR;
                prm[4].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.loginId;
                prm[5].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.P_BOOKS_PROP_APPNO;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GetNPMMasterTable(int FeaturesHeadID)
        {
            try
            {
                string sp_name = "NMP_MASTER.SELECT_MST_FEATURE";
                OracleParameter[] prm = new OracleParameter[] {
                new OracleParameter("P_FEATUREHEADID",OracleDbType.Int32),
                new OracleParameter("C_RECORD",OracleDbType.RefCursor)
                };

                prm[0].Value = FeaturesHeadID;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = null;
                prm[1].Direction = ParameterDirection.Output;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        #region Buidling Details Events
        public DataSet DEL_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_SEL_NCL_PROP_BUILDING_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FLOORTYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
             new OracleParameter("CUR_USER",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = NCLBLDG.PROPERTYCODE;
                prm[1].Value = ULBCODE;
                prm[2].Value = NCLBLDG.FLOORTYPEID;
                prm[3].Value = NCLBLDG.P_BOOKS_PROP_APPNO;
                prm[4].Value = null;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GET_NCL_FLOOR_AREA(NCLBuilding NCLBLDG)
        {
            try
            {
                string sp_name = "NCL.GET_OBJECTIONMODULE_FLOOR_AREA";
                OracleParameter[] prm = new OracleParameter[] {
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;             
             new OracleParameter("P_FLOORNUMBERID",OracleDbType.Int32),// NUMBER;
             new OracleParameter("CUR_USER",OracleDbType.RefCursor),// SYS_REFCURSOR;
                    };


                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = NCLBLDG.PROPERTYCODE;

                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = NCLBLDG.FLOORNUMBERID;

                prm[2].Direction = ParameterDirection.Output;
                prm[2].Value = null;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GET_NCL_TEMP_FLOOR_PRE(NCLBuilding NCLBLDG)
        {
            try
            {
                string sp_name = "VALIDATION_FLOOR.GET_NCL_TEMP_FLOOR_PRE";
                OracleParameter[] prm = new OracleParameter[] {  
          //   new OracleParameter("P_MUTAPPID",OracleDbType.Int32),// NUMBER;  
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
             new OracleParameter("P_FLOORNUMBERID",OracleDbType.Int32),// NUMBER;   
             new OracleParameter("CUR_USER",OracleDbType.RefCursor),// SYS_REFCURSOR;
                    };

                //prm[0].Value = MUTAPPLID;
                //prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = NCLBLDG.PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = NCLBLDG.FLOORNUMBERID;
                prm[2].Direction = ParameterDirection.Output;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }

        public int DEL_INS_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_INS_SEL_NCL_PROP_BUILDING_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BULDINGBLOCKID",OracleDbType.Int32,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_BULDINGBLOCKNAME",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_FLOORID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FLOORNUMBERID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FEATUREHEADID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FEATUREID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;    
             new OracleParameter("P_BUILTYEAR",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BUILDINGUSAGETYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_OWNUSAGEAREA",OracleDbType.Decimal,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_RENTEDAREA",OracleDbType.Decimal,ParameterDirection.Input),
             new OracleParameter("P_ACCOUNTID",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_ISWATERMETERNOAVAILABLE",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);             
             new OracleParameter("P_WATERMETERNO",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,ParameterDirection.Input),
             new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_FLOORTYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
             new OracleParameter("CUR_USER",OracleDbType.RefCursor,ParameterDirection.Output)// NUPMS.OBJECTIONMODULE.t_cursor;
                    };
                prm[0].Value = NCLBLDG.PROPERTYCODE;
                prm[1].Value = NCLBLDG.ULBCODE;
                prm[2].Value = 0;
                prm[3].Value = NCLBLDG.BUILDINGNUMBERID;
                prm[4].Value = NCLBLDG.BUILDINGBLOCKNAME;
                prm[5].Value = NCLBLDG.FLOORID;
                prm[6].Value = NCLBLDG.FLOORNUMBERID;
                prm[7].Value = NCLBLDG.FEATUREHEADID;
                prm[8].Value = NCLBLDG.FEATUREID;
                prm[9].Value = NCLBLDG.BUILTYEAR;
                prm[10].Value = NCLBLDG.BUILDINGUSAGETYPEID;
                prm[11].Value = NCLBLDG.ownUseArea;
                prm[12].Value = NCLBLDG.rentedArea;
                prm[13].Value = NCLBLDG.RRNO;
                prm[14].Value = NCLBLDG.ISWATERMETERNOAVAILABLE;
                prm[15].Value = NCLBLDG.WATERMETERNO;
                prm[16].Value = NCLBLDG.CREATEDIP;
                prm[17].Value = NCLBLDG.CREATEDBY;
                prm[18].Value = NCLBLDG.FLOORTYPEID;
                prm[19].Value = NCLBLDG.P_BOOKS_PROP_APPNO;
                prm[20].Value = null;


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        #region MultiStorey Details Events
        public DataSet GET_NCL_MOB_TEMP_FLOOR_AREA(int PROPERTYCODE)
        {
            try
            {
                string sp_name = "NCL.GET_NCL_MOB_TEMP_FLOOR_AREA";

                OracleParameter[] prm = new OracleParameter[] {
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;  
             new OracleParameter("CUR_USER",OracleDbType.RefCursor),// SYS_REFCURSOR;
                    };


                prm[0].Direction = ParameterDirection.Input;
                prm[0].Value = PROPERTYCODE;

                prm[1].Direction = ParameterDirection.Output;
                prm[1].Value = null;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ULBCODE, NCLAPARTMENT NCLAPT)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INS_UPD_NCL_PROPERTY_APARTMENT_TEMP";

                OracleParameter[] prm = new OracleParameter[] {
                new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
                new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),
                new OracleParameter("P_BLOCKNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
                new OracleParameter("P_FLOORNUMBERID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER      
                new OracleParameter("P_FLATNO",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
                      new OracleParameter("P_PLOTAREAOWNERSHARE_AREA",OracleDbType.Decimal,ParameterDirection.Input),// NUMBER;
                      new OracleParameter("P_PLOTAREAOWNERSHARE_NOS",OracleDbType.Decimal,ParameterDirection.Input),// NUMBER;                      
                      new OracleParameter("P_PLOTAREAOWNERSHARE_FRACTION",OracleDbType.Decimal,ParameterDirection.Input),// NUMBER;
                      new OracleParameter("P_PARKINGAVAILABLE",OracleDbType.Varchar2,ParameterDirection.Input),// CHAR(1);
                      new OracleParameter("P_PARKINGUNITS",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
                      new OracleParameter("P_PARKINGAREA",OracleDbType.Decimal,ParameterDirection.Input),// NUMBER;
                      new OracleParameter("P_RRNO",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);                       
                       new OracleParameter("P_BUILDINGUSAGETYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER
                       new OracleParameter("P_FEATUREHEADID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER
                       new OracleParameter("P_FEATUREID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER
                       new OracleParameter("P_BUILTYEAR",OracleDbType.Int32,ParameterDirection.Input),// NUMBER
                       new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,ParameterDirection.Input),
                      new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
                        new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input)
                    };

                prm[0].Value = NCLAPT.PROPERTYCODE;
                prm[1].Value = ULBCODE;
                prm[2].Value = NCLAPT.BLOCKNUMBER;
                prm[3].Value = NCLAPT.FLOORNUMBERID;
                prm[4].Value = NCLAPT.FLATNO;
                prm[5].Value = NCLAPT.PLOTAREAOWNERSHARE_AREA;
                prm[6].Value = NCLAPT.PLOTAREAOWNERSHARE_NOS;
                prm[7].Value = NCLAPT.PLOTAREAOWNERSHARE_FRACTION;
                prm[8].Value = NCLAPT.PARKINGAVAILABLE;
                prm[9].Value = NCLAPT.PARKINGUNITS;
                prm[10].Value = NCLAPT.PARKINGAREA;
                prm[11].Value = NCLAPT.RRNO;
                prm[12].Value = NCLAPT.BUILDINGUSAGETYPEID;
                prm[13].Value = NCLAPT.FEATUREHEADID;
                prm[14].Value = NCLAPT.FEATUREID;
                prm[15].Value = NCLAPT.YEAROFCONSTRUCTION;
                prm[16].Value = NCLAPT.CREATEDIP;
                prm[17].Value = NCLAPT.CREATEDBY;
                prm[18].Value = NCLAPT.P_BOOKS_PROP_APPNO;

                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        #region Owner Details Events
        public DataSet COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(int P_BOOKS_PROP_APPNO, int propertyCode, int ownerNumber)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32,ParameterDirection.Input),
                      new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("C_OWNERRECORD1",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_OWNERRECORD2",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = propertyCode;
                prm[1].Value = ownerNumber;
                prm[2].Value = P_BOOKS_PROP_APPNO;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet DEL_SEL_NCL_PROP_OWNER_TEMP(int P_BOOKS_PROP_APPNO,int propertyCode, int ownerNumber)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_SEL_NCL_PROP_OWNER_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32,ParameterDirection.Input),
               new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("C_OWNERRECORD1",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_OWNERRECORD2",OracleDbType.RefCursor,ParameterDirection.Output),
          
                    };

                prm[0].Value = propertyCode;
                prm[1].Value = ownerNumber;
                prm[2].Value = P_BOOKS_PROP_APPNO;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        //have to btnAddOwner_Click this is EKYC thing will do later.
        public void INS_UPD_NCL_PROPERTY_APARTMENT_TEMP(int ID_BASIC_PROPERTY, NCLPropOwner NCLOwner, NCLPropOwnerID NCLOwnerID, NCLPropOwnerIDDoc NCLOwnerIDDOC, NCLPropOwnerPhoto NCLOwnerPhoto, string digilockerid)
        {
            try
            {
                OracleParameter[] prm = new OracleParameter[] {
                  new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                  new OracleParameter("P_OWNERNAME",OracleDbType.Varchar2,500),// VARCHAR2(500);
                  new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
                  new OracleParameter("P_IDENTIFIERTYPE",OracleDbType.Int32),// NUMBER;
                  new OracleParameter("P_IDENTIFIERNAME",OracleDbType.Varchar2,500),// VARCHAR2(500);
                  new OracleParameter("P_MOBILENUMBER",OracleDbType.Varchar2,10),// VARCHAR2(10);
                  new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
                  new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
                  new OracleParameter("P_OWNERADDRESS",OracleDbType.Varchar2,2000),// VARCHAR2(2000);
                  new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32),// NUPMS.OBJECTIONMODULE.T_CURSOR;
                  new OracleParameter("P_CITIZENID",OracleDbType.Int32),// NUMBER;     
                  new OracleParameter("P_COMPNAME",OracleDbType.Varchar2,500),// VARCHAR2(2000);
                  new OracleParameter("P_ISCOMPANY",OracleDbType.Char),// Char(1);
                  new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,25)
            };
                prm[0].Value = NCLOwner.PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;

                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = NCLOwner.OWNERNAME;

                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = ID_BASIC_PROPERTY;

                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = NCLOwner.IDENTIFIERTYPE;

                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = NCLOwner.IDENTIFIERNAME;

                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = NCLOwner.MOBILENUMBER;

                prm[6].Direction = ParameterDirection.Input;
                prm[6].Value = NCLOwner.CREATEDBY;

                prm[7].Direction = ParameterDirection.Input;
                prm[7].Value = NCLOwner.ULBCODE;

                prm[8].Direction = ParameterDirection.Input;
                prm[8].Value = NCLOwner.OWNERADDRESS;

                prm[9].Direction = ParameterDirection.Output;
                prm[9].Value = null;

                prm[10].Direction = ParameterDirection.Input;
                prm[10].Value = NCLOwner.CITIZENID;

                prm[11].Direction = ParameterDirection.Input;
                prm[11].Value = NCLOwner.COMPANYNAME;

                prm[12].Direction = ParameterDirection.Input;
                prm[12].Value = NCLOwner.ISCOMPANY;

                prm[13].Direction = ParameterDirection.Input;
                prm[13].Value = NCLOwner.CREATEDIP;

                OracleParameter[] prm1 = new OracleParameter[] {
              new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);  
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;  
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;              
              new OracleParameter("P_OWNERIDENTITYSLNO",OracleDbType.Varchar2,100),// VARCHAR2(100);
              new OracleParameter("P_IDENTITYTYPEID",OracleDbType.Int32),// NUMBER;  
              new OracleParameter("P_AADHAARNO",OracleDbType.Varchar2,50),// NUMBER; 
              new OracleParameter("P_DIGILOCKERID",OracleDbType.Varchar2,500),
              new OracleParameter("C_OWNERIDENTITYID",OracleDbType.Int32),
              new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,25)

            };
                prm1[0].Value = NCLOwnerID.OWNERNUMBER;
                prm1[0].Direction = ParameterDirection.Input;

                prm1[1].Value = NCLOwnerID.CREATEDBY;
                prm1[1].Direction = ParameterDirection.Input;

                prm1[2].Direction = ParameterDirection.Input;
                prm1[2].Value = NCLOwnerID.PROPERTYCODE;

                prm1[3].Direction = ParameterDirection.Input;
                prm1[3].Value = NCLOwnerID.ULBCODE;

                prm1[4].Direction = ParameterDirection.Input;
                prm1[4].Value = ID_BASIC_PROPERTY;

                prm1[5].Direction = ParameterDirection.Input;
                prm1[5].Value = NCLOwnerID.OWNERIDENTITYSLNO;

                prm1[6].Direction = ParameterDirection.Input;
                prm1[6].Value = NCLOwnerID.IDENTITYTYPEID;

                prm1[7].Direction = ParameterDirection.Input;
                prm1[7].Value = NCLOwnerID.AADHAARNO;

                prm1[8].Direction = ParameterDirection.Input;
                prm1[8].Value = digilockerid;

                prm1[9].Direction = ParameterDirection.Output;
                prm1[9].Value = null;

                prm1[10].Direction = ParameterDirection.Input;
                prm1[10].Value = NCLOwner.CREATEDIP;

                OracleParameter[] prm2 = new OracleParameter[] {
                new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
                new OracleParameter("P_OWNERIDENTITYID",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_SCANNEDIDENEITY",OracleDbType.Blob),// BLOB;
                new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
                new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32)// NUMBER;         
                          };

                prm2[0].Value = NCLOwnerIDDOC.OWNERNUMBER;
                prm2[0].Direction = ParameterDirection.Input;
                prm2[1].Value = NCLOwnerIDDOC.CREATEDBY;
                prm2[1].Direction = ParameterDirection.Input;
                prm2[2].Direction = ParameterDirection.Input;
                prm2[2].Value = NCLOwnerIDDOC.OWNERIDENTITYID;
                prm2[3].Direction = ParameterDirection.Input;
                prm2[3].Value = NCLOwnerIDDOC.PROPERTYCODE;
                prm2[4].Direction = ParameterDirection.Input;
                prm2[4].Value = NCLOwnerIDDOC.SCANNEDIDENEITY;
                prm2[5].Direction = ParameterDirection.Input;
                prm2[5].Value = NCLOwnerIDDOC.ULBCODE;
                prm2[6].Direction = ParameterDirection.Input;
                prm2[6].Value = ID_BASIC_PROPERTY;

                OracleParameter[] prm3 = new OracleParameter[] {
              new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_OWNERPHOTO",OracleDbType.Blob),// Blob;
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
            };


                prm3[0].Value = NCLOwnerPhoto.OWNERNUMBER;
                prm3[0].Direction = ParameterDirection.Input;
                prm3[1].Value = NCLOwnerPhoto.CREATEDBY;
                prm3[1].Direction = ParameterDirection.Input;
                prm3[2].Direction = ParameterDirection.Input;
                prm3[2].Value = NCLOwnerPhoto.PROPERTYCODE;
                prm3[3].Direction = ParameterDirection.Input;
                prm3[3].Value = NCLOwnerPhoto.OWNERPHOTO;
                prm3[4].Direction = ParameterDirection.Input;
                prm3[4].Value = NCLOwnerPhoto.ULBCODE;
                prm3[5].Direction = ParameterDirection.Input;
                prm3[5].Value = ID_BASIC_PROPERTY;

                OracleParameter[] prm4 = new OracleParameter[] {

                   new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;                   
                   new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
                   new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
                   new OracleParameter("C_RECORD",OracleDbType.RefCursor)// Cursor;
                 
            };

                prm4[0].Direction = ParameterDirection.Input;
                prm4[0].Value = NCLOwner.PROPERTYCODE;

                prm4[1].Direction = ParameterDirection.Input;
                prm4[1].Value = ID_BASIC_PROPERTY;

                prm4[2].Direction = ParameterDirection.Input;
                prm4[2].Value = NCLOwner.ULBCODE;

                prm4[3].Direction = ParameterDirection.Output;
                prm4[3].Value = null;


             //   return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        } //check later btnOwnerSave_Click
          //make GET_NCL_PROP_OWNER_TEMP_BYEKYCTRANSACTION in EKYC EditOwnerDetailsFromEKYCData(transactionNo)
        #endregion
        #region Property Rights
        public int NCL_PROPERTY_RIGHTS_TEMP_INS( int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.NCL_PROPERTY_RIGHTS_TEMP_INS";

                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
              new OracleParameter("P_RIGHTS",OracleDbType.Varchar2,2000),// VARCHAR2(2000);
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_USER",OracleDbType.RefCursor)
              

            };

                prm[0].Value = NCLPropRight.CREATEDBY;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = NCLPropRight.RIGHTS;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropRight.PROPERTYCODE;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = NCLPropRight.ULBCODE;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = ID_BASIC_PROPERTY;
                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = NCLPropRight.P_BOOKS_PROP_APPNO; 
                prm[6].Direction = ParameterDirection.Output;
                prm[6].Value = null;
              


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int NCL_PROPERTY_RIGHTS_TEMP_UPD(int ID_BASIC_PROPERTY, NCLPropRights NCLPropRight)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.NCL_PROPERTY_RIGHTS_TEMP_UPD";

                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
              new OracleParameter("P_RIGHTS",OracleDbType.Varchar2,2000),// VARCHAR2(2000);
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_PROPERTYRIGHTSID",OracleDbType.Int32),
               new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_USER",OracleDbType.RefCursor),// DATE;
              
            };
                prm[0].Value = NCLPropRight.CREATEDBY;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Direction = ParameterDirection.Input;
                prm[1].Value = NCLPropRight.RIGHTS;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropRight.PROPERTYCODE;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = NCLPropRight.ULBCODE;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = ID_BASIC_PROPERTY;
                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = NCLPropRight.PROPERTYRIGHTSID;
                prm[6].Direction = ParameterDirection.Input;
                prm[6].Value = NCLPropRight.P_BOOKS_PROP_APPNO;
                prm[7].Direction = ParameterDirection.Output;
                prm[7].Value = null;
              


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int NCL_PROPERTY_RIGHTS_TEMP_DEL(int P_BOOKS_PROP_APPNO ,int RIGHTSID, int ID_BASIC_PROPERTY, int ULBCODE, int PROPERTYCODE)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.NCL_PROPERTY_RIGHTS_TEMP_DEL";

                OracleParameter[] prm = new OracleParameter[] {
            new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;              
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;              
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;              
              new OracleParameter("P_PROPERTYRIGHTSID",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_USER",OracleDbType.RefCursor,ParameterDirection.Output)// T_CURSOR;
           
            };

                prm[0].Value = PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = ULBCODE;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = ID_BASIC_PROPERTY;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = RIGHTSID;
                prm[4].Value = P_BOOKS_PROP_APPNO;




                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        #region Document Upload Events
        public int NCL_PROPERTY_ID_TEMP_INS( int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.NCL_PROPERTY_ID_TEMP_INS";

                OracleParameter[] prm = new OracleParameter[] {
                        new OracleParameter("P_ORDERNUMBER",OracleDbType.Varchar2,200),// VARCHAR2(200);
                        new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_DOCUMENTTYPEID",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,200),// VARCHAR2(20);
                        new OracleParameter("P_DOCUMENTEXTENSION",OracleDbType.Varchar2,200),// VARCHAR2(100);
                        new OracleParameter("P_DOCUMENTDETAILS",OracleDbType.Varchar2,200),// VARCHAR2(2000);
                        new OracleParameter("P_SCANNEDDOCUMENT",OracleDbType.Blob),// BLOB;
                        new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_ORDERDATE",OracleDbType.TimeStamp),// DATE;
                          new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
                         new OracleParameter("CUR_USER",OracleDbType.RefCursor)
                         
                         // SYS_REFCURSOR;
                    };
                prm[0].Value = NCLPropID.ORDERNUMBER;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = NCLPropID.PROPERTYCODE;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropID.DOCUMENTTYPEID;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Direction = ParameterDirection.Input;
                prm[3].Value = ID_BASIC_PROPERTY;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = NCLPropID.CREATEDBY;
                prm[5].Direction = ParameterDirection.Input;
                prm[5].Value = NCLPropID.DOCUMENTEXTENSION;
                prm[6].Direction = ParameterDirection.Input;
                prm[6].Value = NCLPropID.DOCUMENTDETAILS;
                prm[7].Direction = ParameterDirection.Input;
                prm[7].Value = NCLPropID.SCANNEDDOCUMENT;
                prm[8].Direction = ParameterDirection.Input;
                prm[8].Value = NCLPropID.ULBCODE;
                prm[9].Direction = ParameterDirection.Input;
                prm[9].Value = NCLPropID.ORDERDATE;
                prm[10].Direction = ParameterDirection.Input;
                prm[10].Value = NCLPropID.P_BOOKS_PROP_APPNO;
                prm[11].Direction = ParameterDirection.Output;
                prm[11].Value = null;


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GetNCLDocView(int DOCUMENTID, int PROPERTYCODE)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_DOC_VIEW";

                OracleParameter[] prm = new OracleParameter[] {
                   new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;  
                   new OracleParameter("P_DOCUMENTID",OracleDbType.Int32),// NUMBER;  
                   new OracleParameter("C_RECORD",OracleDbType.RefCursor)// NUMBER;
            };

                prm[0].Value = PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = DOCUMENTID;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Direction = ParameterDirection.Output;
                prm[2].Value = null;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet NCL_PROPERTY_ID_TEMP_DEL(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.NCL_PROPERTY_ID_TEMP_DEL";

                OracleParameter[] prm = new OracleParameter[] {
                        new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
                         new OracleParameter("P_DOCUMENTID",OracleDbType.Int32),// NUMBER;
                            new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
                        new OracleParameter("CUR_USER",OracleDbType.RefCursor),// NUPMS.OBJECTIONMODULE.t_cursor;
                       
                    };

                prm[0].Value = NCLPropID.PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = ID_BASIC_PROPERTY;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropID.ULBCODE;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Value = NCLPropID.DOCUMENTID;
                prm[3].Direction = ParameterDirection.Input;
                prm[4].Value = NCLPropID.P_BOOKS_PROP_APPNO;
                prm[4].Direction = ParameterDirection.Input;
                prm[5].Value = null;
                prm[5].Direction = ParameterDirection.Output;
               
               

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GetMasterDocByCategoryOrClaimType(int ULBCODE, int CATEGORYID, int ClaimTypeID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_MST_CTZ_DOC";

                OracleParameter[] prm = new OracleParameter[] {
                new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),
                new OracleParameter("P_CATEGORYID",OracleDbType.Int32,ParameterDirection.Input),
                new OracleParameter("P_ClaimTypeID",OracleDbType.Int32,ParameterDirection.Input),
                new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("C_RECORD1",OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("C_ECDOCRECORD",OracleDbType.RefCursor,ParameterDirection.Output)
                            };

                prm[0].Value = ULBCODE;
                prm[1].Value = CATEGORYID;
                prm[2].Value = ClaimTypeID;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        #region Classification Document Upload Events
        public DataSet INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP(NCLClassPropIdentification nCLClassPropIdentification)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
               new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),

              new OracleParameter("P_DOCUMENTTYPEID",OracleDbType.Int32,ParameterDirection.Input),
            
              new OracleParameter("P_DOCUMENTDETAILS",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTDATE",OracleDbType.Date,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTEXTENSION",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_SCANNEDDOCUMENT",OracleDbType.Blob,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("CUR_DOCUMENTS",OracleDbType.RefCursor,ParameterDirection.Output),
               
                    };

                prm[0].Value = nCLClassPropIdentification.PROPERTYCODE;
                prm[1].Value = nCLClassPropIdentification.P_BOOKS_PROP_APPNO;
                prm[2].Value = nCLClassPropIdentification.DOCUMENTTYPEID;
                prm[3].Value = nCLClassPropIdentification.DOCUMENTDETAILS;
                prm[4].Value = nCLClassPropIdentification.DOCUMENTNUMBER;
                prm[5].Value = nCLClassPropIdentification.DOCUMENTDATE;
                prm[6].Value = nCLClassPropIdentification.DOCUMENTEXTENSION;
                prm[7].Value = nCLClassPropIdentification.SCANNEDDOCUMENT;
                prm[8].Value = nCLClassPropIdentification.CREATEDBY;
               


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.INS_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int P_DOC_SCAN_ID, int P_BOOKS_PROP_APPNO)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_DOC_SCAN_ID",OracleDbType.Int32,ParameterDirection.Input),
               
              new OracleParameter("CUR_DOCUMENTS",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = PROPERTYCODE;
                prm[1].Value = P_BOOKS_PROP_APPNO;
                prm[2].Value = P_DOC_SCAN_ID;
               



                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.DEL_NCL_PROPERTY_CLASS_DOC_ID_BBD_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTROWID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_DOCUMENTS",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = PROPERTYCODE;
                prm[1].Value = DOCUMENTROWID;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.SEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS(int PROPERTYCATEGORYID, int CLASSIFICATIONID, int SUBCLASSIFICATIONID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_NPM_MST_CLASS_DOCUMENT_BY_CATEGORY_SUBCLASS";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCATEGORYID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYCLASSIFICATIONID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYSUBCLASSIFICATIONID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_DOCUMENTS",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = PROPERTYCATEGORYID;
                prm[1].Value = CLASSIFICATIONID;
                prm[2].Value = SUBCLASSIFICATIONID;



                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS stored procedure.");
                throw;
            }
        }
        public DataSet GetPropertySubClassByULBAndCategory(int PropCatID, int ulbcode)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.SELECT_MST_PROPSUBCLASS_BYULBANDCAT";

                OracleParameter[] prm = new OracleParameter[] {
                  new OracleParameter("P_PROPERTYCATEGORYID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
                  new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
                  new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output),// SYS_REFCURSOR;
                  new OracleParameter("C_RECORD1",OracleDbType.RefCursor,ParameterDirection.Output),// SYS_REFCURSOR;
                    };
                prm[0].Value = PropCatID;
                prm[1].Value = ulbcode;



                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GetPropertySubClassByULBAndCategory stored procedure.");
                throw;
            }
        }

        #endregion
        #region Tax Events
        public DataSet InsertBBMPPropertyTaxResponse(int UlbCode, string Json, string Response, string IpAddress, string Createdby, string oParamater)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INSERT_BBMP_PROPERTYTAX_RESPONSE";

                OracleParameter[] prm = new OracleParameter[] {
                  new OracleParameter("P_ULBCODE",OracleDbType.Int32),
                   new OracleParameter("P_JSON",OracleDbType.Clob),
                   new OracleParameter("P_RESPONSE",OracleDbType.Clob),
                   new OracleParameter("P_IPADDRESS",OracleDbType.Varchar2,20),
                   new OracleParameter("P_CREATEDBY",OracleDbType.Clob),
                  new OracleParameter(oParamater,OracleDbType.Int32)
            };
                prm[0].Value = UlbCode;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = Json;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = Response;
                prm[2].Direction = ParameterDirection.Input;

                prm[3].Value = IpAddress;
                prm[3].Direction = ParameterDirection.Input;

                prm[4].Value = Createdby;
                prm[4].Direction = ParameterDirection.Input;

                prm[5].Value = null;
                prm[5].Direction = ParameterDirection.Output;



                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GetTaxDetails(string applicationNo,long propertycode,long P_BOOKS_PROP_APPNO,string loginId)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.SEL_OFFLINE_PTAX_BY_APPLICATION";

                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64),
                      new OracleParameter("P_BOOKS_PROP_APPNO",OracleDbType.Int64),
                  new OracleParameter("P_APPLICATIONNO",OracleDbType.Varchar2),
                       new OracleParameter("P_LOGINID",OracleDbType.Varchar2),
                   new OracleParameter("CUR_TAXDATA",OracleDbType.RefCursor),
                   new OracleParameter("C_NPM_RECORD",OracleDbType.RefCursor)
                 
            };
                prm[0].Value = propertycode;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = P_BOOKS_PROP_APPNO;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = applicationNo;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Value = loginId;
                prm[3].Direction = ParameterDirection.Input;
                prm[4].Value = null;
                prm[4].Direction = ParameterDirection.Output;
                prm[5].Value = null;
                prm[5].Direction = ParameterDirection.Output;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion
        //should add InsBBMPPropertyTaxResponseDetails 
        #region Objection Events
        public DataSet InsertBBMPPropertyTaxResponse(int PROPERTYCODE, string OBJECTIONDETAILS, byte[] SCANNEDDOCUMENT, string DOCUMENTDETAILS, string CREATEDBY)
        {
            try
            {
                

                string sp_name = "OBJECTIONMODULE.INS_NCL_PROPERTY_OBJECTION_BBD_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_OBJECTIONDETAILS",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_SCANNEDDOCUMENT",OracleDbType.Blob,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTDETAILS",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("CUR_OUTPUT",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = PROPERTYCODE;
                prm[1].Value = OBJECTIONDETAILS;
                prm[2].Value = SCANNEDDOCUMENT;
                prm[3].Value = DOCUMENTDETAILS;
                prm[4].Value = CREATEDBY;



                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        #endregion

        #region eSignCode
        //will check this later.
        #endregion

   //     [DllImport(@"D:\\NameMatchJulyFinal2023.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_GetMatchScore")]
        

      
        public DataSet GetUserCitizen(string userId)
        {
            string commandText = "BBMP.getUserPwd";
            OracleParameter[] array = new OracleParameter[2]
            {
            new OracleParameter(),
            null
            };
            array[0].OracleDbType = OracleDbType.Varchar2;
            array[0].ParameterName = "p_UserId";
            array[0].Value = userId;
            array[1] = new OracleParameter();
            array[1].OracleDbType = OracleDbType.RefCursor;
            array[1].ParameterName = "cur_user";
            array[1].Direction = ParameterDirection.Output;
            return _databaseService.ExecuteDataset(commandText, array);
        }
        public int Insert_React_UserFlow(int P_BOOKS_PROP_APPNOAPPNO, int propertyCode, int currentStep, int DraftWardId, int DraftZoneId, string LoginID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.INS_USER_REACT_FLOW";

                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("P_LoginID",OracleDbType.NVarchar2),
                      new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64),
                  new OracleParameter("P_BOOK_APP_NO",OracleDbType.Int64),
                       new OracleParameter("P_CURRENT_STEP",OracleDbType.Int32),
                   new OracleParameter("P_DraftWardId",OracleDbType.Int64),
                     new OracleParameter("P_DraftZoneId",OracleDbType.Int64),

            };
                prm[0].Value = LoginID;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = propertyCode;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = P_BOOKS_PROP_APPNOAPPNO;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Value = currentStep;
                prm[3].Direction = ParameterDirection.Input;
                prm[4].Value = DraftWardId;
                prm[4].Direction = ParameterDirection.Input;
                prm[5].Value = DraftZoneId;
                prm[5].Direction = ParameterDirection.Input;
               
                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet Get_React_UserFlow(Int64 propertyCode, Int64 BOOKAPP_NO, string LoginID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.GET_USER_REACT_FLOW";

                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("P_LoginID",OracleDbType.NVarchar2),
                      new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64),
                      new OracleParameter("P_BOOK_APP_NO",OracleDbType.Int64),
                       new OracleParameter("P_CURRENT_USER",OracleDbType.RefCursor),
            };
                prm[0].Value = LoginID;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = propertyCode;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = BOOKAPP_NO;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Value = null;
                prm[3].Direction = ParameterDirection.Output;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Get_React_UserFlow stored procedure.");
                throw;
            }
        }
        public int Ins_EsignPDf(Int64 propertycode, Int64 bookspropappno, byte[] pdfFilePath, string appearance)
        {
            try
            {
                byte[] appearanceBytes = System.Text.Encoding.UTF8.GetBytes(appearance);
                string sp_name = "OBJECTIONMODULE_REACT.INS_ESIGN_PDF";

                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("V_PROPERTYCODE",OracleDbType.Int64),
                      new OracleParameter("V_BOOKS_APP_NO",OracleDbType.Int64),
                      new OracleParameter("v_PDF_FILE_CONTENT",OracleDbType.Blob),
                       new OracleParameter("V_PDF_SIGNATURE_DATA",OracleDbType.Blob),

            };
                prm[0].Value = propertycode;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = bookspropappno;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = pdfFilePath;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Value = appearanceBytes;
                prm[3].Direction = ParameterDirection.Input;


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Get_React_UserFlow stored procedure.");
                throw;
            }
        }
        public DataSet Get_ESignPdf(Int64 propertycode, Int64 bookspropappno)
        {
            try
            {
                
                string sp_name = "OBJECTIONMODULE_REACT.GET_ESIGN_PDF";

                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("V_PROPERTYCODE",OracleDbType.Int64),
                      new OracleParameter("V_BOOKS_APP_NO",OracleDbType.Int64),
                      new OracleParameter("V_PDF_OUT",OracleDbType.RefCursor),
                     

            };
                prm[0].Value = propertycode;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = bookspropappno;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = null;
                prm[2].Direction = ParameterDirection.Output;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Get_React_UserFlow stored procedure.");
                throw;
            }
        }
        public int Ins_PDF_Draft_Exception_log(string propertycode,string Propertyid,string status,string output,string ExceptionMessage)
        {
            try
            {

                string sp_name = "OBJECTIONMODULE_REACT.INS_BBD_DRAFT_EXCEPTION_REACT";

                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("V_PROPERTYCODE",OracleDbType.Varchar2),
                     new OracleParameter("V_PROPERTYID",OracleDbType.Varchar2),
                      new OracleParameter("V_SUCCESSSTATUS",OracleDbType.Varchar2),
                      new OracleParameter("V_FILEPATH",OracleDbType.Varchar2),
                      new OracleParameter("V_EXCEPTIONMESSAGE",OracleDbType.Varchar2)

            };
                prm[0].Value = propertycode;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = Propertyid;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = status;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Value = output;
                prm[3].Direction = ParameterDirection.Input;
                prm[4].Value = ExceptionMessage;
                prm[4].Direction = ParameterDirection.Input;


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Get_React_UserFlow stored procedure.");
                throw;
            }
        }
        public DataSet GetEAASTHIStatus()
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.Get_EAASTHI_Status";
                OracleParameter[] prm = new OracleParameter[] {
                      new OracleParameter("C_RECORD",OracleDbType.RefCursor),
            };
               
                prm[0].Value = null;
                prm[0].Direction = ParameterDirection.Output;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Get_EAASTHI_Status stored procedure.");
                throw;
            }
        }
        public DataSet GetEAASTHIDailyReport()
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.GetEAASTHIDailyReport";
                OracleParameter[] prm = new OracleParameter[] {
                      new OracleParameter("C_RECORD",OracleDbType.RefCursor),
            };

                prm[0].Value = null;
                prm[0].Direction = ParameterDirection.Output;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Get_EAASTHI_Status stored procedure.");
                throw;
            }
        }
        public DataSet GetEAASTHIDailyReportDetails(int wardNumber,string QueryName,int pageNo,int PageSize)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE_REACT.Get_EAASTHI_Daily_Report_Details";
                OracleParameter[] prm = new OracleParameter[] {
                     new OracleParameter("P_WARDID",OracleDbType.Int16),
                      new OracleParameter("P_QUERYNAME",OracleDbType.Varchar2),
                     new OracleParameter("P_PAGE",OracleDbType.Int16),
                     new OracleParameter("P_PAGESIZE",OracleDbType.Int16),
                      new OracleParameter("C_RECORD",OracleDbType.RefCursor),
            };

                prm[0].Value = wardNumber;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = QueryName;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = null;
                prm[2].Direction = ParameterDirection.Output;
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.GetEAASTHIDailyReport stored procedure.");
                throw;
            }
        }
        public int INS_NPM_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP(Int64 propertyCode, string? ECNUMBER, string? REGISTRATIONNUMBER, string? IS_LATEST_REGISTRATIONNO, string? LATEST_REGISTRATIONNO, string? DISTRICTNAME, string? TALUKANAME, string? VILLAGENAME, string? HOBLINAME, string? ARTICLENAME, string? EXECUTIONDATE, Int64 KAVERIECDOC_RESPONSE_ROWID, string loginID)
        {
            string sp_name = "ECDOCUMENTUPLOAD.INS_NPM_PROPERTY_KAVERIEC_PROPERTY_DETAILS_TEMP";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int64,ParameterDirection.Input),

              new OracleParameter("P_ECNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_REGISTRATIONNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_IS_LATEST_REGISTRATIONNO",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_LATEST_REGISTRATIONNO",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DISTRICTNAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_TALUKANAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_VILLAGENAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_HOBLINAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_ARTICLENAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_EXECUTIONDATE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_KAVERIECDOC_RESPONSE_ROWID",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

            prm[0].Value = propertyCode;

            prm[1].Value = ECNUMBER;
            prm[2].Value = REGISTRATIONNUMBER;
            prm[3].Value = IS_LATEST_REGISTRATIONNO;
            prm[4].Value = LATEST_REGISTRATIONNO;
            prm[5].Value = DISTRICTNAME;
            prm[6].Value = TALUKANAME;
            prm[7].Value = VILLAGENAME;
            prm[8].Value = HOBLINAME;
            prm[9].Value = ARTICLENAME;
            prm[10].Value = EXECUTIONDATE;
            prm[11].Value = KAVERIECDOC_RESPONSE_ROWID;
            prm[12].Value = loginID;

            try
            {
                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GET_ARO_BY_ZONE(int zoneid)
        {
            string sp_name = "OBJECTIONMODULE_REACT.GET_ARO_BY_ZONE";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_ZONEID",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("C_OUT",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

            prm[0].Value = zoneid;

           

            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GET_WARD_BY_ARO(int AROID)
        {
            string sp_name = "OBJECTIONMODULE_REACT.GET_WARD_BY_ARO";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_AROID",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("C_OUT",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

            prm[0].Value = AROID;

            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GET_PENDENCE_REPORT(int ZoneId, int AROId, int WARDID, string SEARCHTYPE)
        {
            string sp_name = "OBJECTIONMODULE_REACT.GET_PENDENCE_REPORT";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_ZONEID",OracleDbType.Int64,ParameterDirection.Input),
                  new OracleParameter("P_AROID",OracleDbType.Int64,ParameterDirection.Input),
                      new OracleParameter("P_WARDID",OracleDbType.Int64,ParameterDirection.Input),
                          new OracleParameter("P_SEARCHTYPE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("C_OUT",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

            prm[0].Value = ZoneId;

            prm[1].Value = AROId;

            prm[2].Value = WARDID;

            prm[3].Value = SEARCHTYPE;

            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GET_PENDENCE_REPORT_DETAILS(int WARDID, string PROPERTYID,string TYPEOFROLE, int PAGENO, int PAGECOUNT)
        {
            string sp_name = "OBJECTIONMODULE_REACT.GET_PENDENCE_REPORT_DETAILS";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_WARDID",OracleDbType.Int64,ParameterDirection.Input),
                      new OracleParameter("P_TYPEOFROLE",OracleDbType.Varchar2,ParameterDirection.Input),
               new OracleParameter("P_PROPERTYID",OracleDbType.Varchar2,ParameterDirection.Input),
                new OracleParameter("P_PAGENO",OracleDbType.Int64,ParameterDirection.Input),
                 new OracleParameter("P_PAGECOUNT",OracleDbType.Int64,ParameterDirection.Input),
              new OracleParameter("C_OUT",OracleDbType.RefCursor,ParameterDirection.Output),
                new OracleParameter("C_LOGIN_DETAILS",OracleDbType.RefCursor,ParameterDirection.Output)

                    };

            prm[0].Value = WARDID;
            prm[1].Value = TYPEOFROLE;
            prm[2].Value = PROPERTYID;
            prm[3].Value = PAGENO;
            prm[4].Value = PAGECOUNT;

            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GetECDailyReport()
        {
            string sp_name = "OBJECTIONMODULE_REACT.Get_EC_DAILY_Report";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("C_OUT",OracleDbType.RefCursor,ParameterDirection.Output) };
            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GetMutationDailyReport()
        {
            string sp_name = "OBJECTIONMODULE_REACT.Get_Mutation_DAILY_Report";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("C_OUT",OracleDbType.RefCursor,ParameterDirection.Output) };
            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GetDraftDownloaded()
        {
            string sp_name = "MUTATIONOBJECTIONMODULE_REACT.Get_Draft_Downloaded_Details";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output) };
            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet MUTATION_NOTICES(int ulbCode, int PAGENO, int PAGECOUNT)
        {
            string sp_name = "OBJECTIONMODULE_REACT.GET_MUT_NOTICES";
            OracleParameter[] prm = new OracleParameter[] {
                  new OracleParameter("P_ULBCODE",OracleDbType.Int16,ParameterDirection.Input),
                    new OracleParameter("P_PAGENO",OracleDbType.Int16,ParameterDirection.Input),
                      new OracleParameter("P_PAGECOUNT",OracleDbType.Int16,ParameterDirection.Input),
              new OracleParameter("P_OUT",OracleDbType.RefCursor,ParameterDirection.Output)
            };
            prm[0].Value = ulbCode;
            prm[1].Value = PAGENO;
            prm[2].Value = PAGECOUNT;
            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public string GetIPAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    return "Unable to retrieve HttpContext";
                }

                var clientIpAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (string.IsNullOrEmpty(clientIpAddress))
                {
                    clientIpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                }

                return clientIpAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      public  DataSet GET_Final_eKhatha_Status_Based_on_ePID(string PropertyEpid)
        {
            string sp_name = "OBJECTIONMODULE_REACT.GET_Final_eKhatha_Status_Based_on_ePID";
            OracleParameter[] prm = new OracleParameter[] {
                  new OracleParameter("P_PROPERTYEPID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output) };
            prm[0].Value = PropertyEpid;
            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public DataSet GENERATE_NEW_KHATA_PROPERTYCODE()
        {
            string sp_name = "NEW_KHATA_DETAILS.GENERATE_NEW_KHATA_PROPERTYCODE";
            OracleParameter[] prm = new OracleParameter[] {
                 
              new OracleParameter("C_RECORD",OracleDbType.RefCursor,ParameterDirection.Output)
            };
           
            try
            {
                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
        public int Insert_New_khata_details(newkhatadetails newkhata, Int64 propertycode, Int64 books_app_no, List<NUPMS_BO.EKYCDetailsBO> mainParameters1, KaveriData.DocumentDetails documentDetails,
            KaveriData.EcData ECdocumentDetails, string fromdate, string todate)
        {

            string[] procedures = {
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_MAIN_TEMP",           //0
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_SAS_APP_DATA_TEMP",   //1
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_PHOTO",                 //2
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_ADDRESS",                 //3
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_COORDINATE", //4
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_IDENTIFICATION", //5
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_BESCOM_TEMP", //6
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_OWNER", //7
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_KAVERI_DOC", //8
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_KAVERI_EC", //9
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_ODD_SITE_DETAILS", //10
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_OVERALL_RECOMMENDATION", //11
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_APPARTMENT_DETAILS", //12
                "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_MATRIX", //13
                 "NEW_KHATA_DETAILS.INS_NEW_KHATA_DIMENSION", //14
                   "NEW_KHATA_DETAILS.INS_NEW_KHATA_BUILDING_DETAILS", //15
                   "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_SITE", //16
                         "NEW_KHATA_DETAILS.NCL_NEW_KHATA_PROPERTY_KAVERI_PROPERTY_DETAILS_TEMP", //17
                    "NEW_KHATA_DETAILS.NCL_NEW_KHATA_PROPERTY_KAVERI_PARTIES_DETAILS_TEMP", //18
                    "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_OWNER_CLAIM_KAVERI_EC",//19,
                    "NEW_KHATA_DETAILS.INS_NEW_KHATA_NCL_PROPERTY_OWNER_EXEC_KAVERI_EC"//20


    };

            Dictionary<string, List<OracleParameter[]>> parametersMap = new Dictionary<string, List<OracleParameter[]>>();
            var mainParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_KRSId", OracleDbType.Int64) { Value = newkhata.KRSId },
            new OracleParameter("P_EPID", OracleDbType.Varchar2) { Value = newkhata.EPID },
            new OracleParameter("P_KhataType", OracleDbType.Varchar2) { Value = newkhata.KhataType },
            new OracleParameter("P_KhataTypeId", OracleDbType.Int32) { Value = newkhata.KhataTypeId },
            new OracleParameter("P_SourceOfAPP", OracleDbType.Varchar2) { Value = newkhata.SourceOfAPP },
            new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
            new OracleParameter("P_MasterPropertyTypeId", OracleDbType.Int32) { Value = newkhata.PropertyDetails.MasterPropertyTypeId },
            new OracleParameter("P_MasterPropertyTypeName", OracleDbType.Varchar2) { Value = newkhata.PropertyDetails.MasterPropertyTypeName },
             new OracleParameter("eaasthiWardId", OracleDbType.Varchar2) { Value = newkhata.PropertyDetails.EaasthiWardId },
                new OracleParameter("P_SASNO", OracleDbType.Varchar2) { Value = newkhata.SASNo },
        }
    };
            parametersMap.Add(procedures[0], mainParameters);

            var sasParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
             new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_SASNo", OracleDbType.Varchar2) { Value = newkhata.SASNo },
              new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber }
        }
    };
            parametersMap.Add(procedures[1], sasParameters);

            var propertyPhotoParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
          new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_PropertyPhotoBase64", OracleDbType.Blob) { Value = newkhata.PropertyDetails.PropertyPhotoBase64 },
              new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber }
        }
    };
            parametersMap.Add(procedures[2], propertyPhotoParameters);

            var propertyAddressParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_DoorOrPlotNo", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.DoorOrPlotNo },
             new OracleParameter("P_BuildingOrLand", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.BuildingOrLand },
              new OracleParameter("P_LandMark", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.LandMark },
               new OracleParameter("P_PinCode", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.PinCode },
                new OracleParameter("P_Locality", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.Locality },
                  new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber }
        }
    };
            parametersMap.Add(procedures[3], propertyAddressParameters);

            var CoordinateParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
             new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_Latitude", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.Latitude },
              new OracleParameter("P_Longitude", OracleDbType.Varchar2) { Value = newkhata.PropertyAddress.Longitude },
                new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
        }
    };
            parametersMap.Add(procedures[4], CoordinateParameters);

            var identificationParameters = new List<OracleParameter[]>();
            foreach (var identification in newkhata.UploadedDocumentsToProveAKahata)
            {
                identificationParameters.Add(new OracleParameter[]
                {
                     new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_MasterDocumentId", OracleDbType.Int32) { Value = identification.MasterDocumentId },
            new OracleParameter("P_MasterDocumentText", OracleDbType.Varchar2) { Value = identification.MasterDocumentText },
            new OracleParameter("P_MasterSubDocumentId", OracleDbType.Int32) { Value = identification.MasterSubDocumentId },
            new OracleParameter("P_MasterSubDocumentText", OracleDbType.Varchar2) { Value = identification.MasterSubDocumentText },
            new OracleParameter("P_DocumentBase64", OracleDbType.Blob) { Value = identification.DocumentBase64 },
              new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },

                });
            }
            parametersMap.Add(procedures[5], identificationParameters);

            var BescomParameters = new List<OracleParameter[]>();
            foreach (var bescomdetails in newkhata.BescomDetails)
            {
                BescomParameters.Add(new OracleParameter[]
                        {
                             new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_MeterNo", OracleDbType.Varchar2) { Value = bescomdetails.MeterNo },
            new OracleParameter("P_EscomName", OracleDbType.Varchar2) { Value = bescomdetails.EscomName  },
            new OracleParameter("P_AccountId", OracleDbType.Varchar2) { Value = bescomdetails.AccountId },
            new OracleParameter("P_RRNumber", OracleDbType.Varchar2) { Value = bescomdetails.RRNumber  },
            new OracleParameter("P_ConsumerName", OracleDbType.Varchar2) { Value = bescomdetails.ConsumerName  },
            new OracleParameter("P_Type", OracleDbType.Varchar2) { Value = bescomdetails.Type  },
            new OracleParameter("P_TrafficType", OracleDbType.Varchar2) { Value = bescomdetails.TrafficType  },
            new OracleParameter("P_BescomRawJSON", OracleDbType.Varchar2) { Value = bescomdetails.BescomRawJSON },
              new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
        });

            };
            parametersMap.Add(procedures[6], BescomParameters);

            var ownerParameters = new List<OracleParameter[]>();
            for (int i = 0; i < newkhata.OwnerData.Count(); i++)
            {
                ownerParameters.Add(new OracleParameter[]
                        {
           new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
            new OracleParameter("P_EKYCJSON", OracleDbType.Clob) { Value = newkhata.OwnerData[i].EKYCJSON },
             new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
              new OracleParameter("P_OWNERNAME", OracleDbType.Varchar2) { Value = newkhata.OwnerData[i].OwnerName },
               new OracleParameter("P_OWNERNAME_EN", OracleDbType.Varchar2) { Value = mainParameters1[i].OwnerNameEng },
                 new OracleParameter("P_IDENTIFIERNAME", OracleDbType.Varchar2) { Value = mainParameters1[i].IdentifierNameKnd},
                  new OracleParameter("P_IDENTIFIERNAME_EN", OracleDbType.Varchar2) { Value = mainParameters1[i].IdentifierNameEng},
                   new OracleParameter("P_OWNERADDRESS", OracleDbType.Varchar2) { Value = mainParameters1[i].AddressKnd},
                    new OracleParameter("P_OWNERADDRESS_EN", OracleDbType.Varchar2) { Value = mainParameters1[i].AddressEng},
                     new OracleParameter("P_OWNERPHOTO", OracleDbType.Blob) { Value = mainParameters1[i].photoBytes},
                  //   new OracleParameter("P_OWNERPHOTO", OracleDbType.Blob) { Value =  DBNull.Value },
                      new OracleParameter("P_MaskedAadhaar", OracleDbType.Varchar2) { Value = mainParameters1[i].maskedAadhaar},
                       new OracleParameter("P_OwnerGender", OracleDbType.Char) { Value = mainParameters1[i].Gender},
                        new OracleParameter("P_DATEOFBIRTH", OracleDbType.Varchar2) { Value = mainParameters1[i].DateOfBirth},
                          new OracleParameter("P_MOBILENUMBER", OracleDbType.Varchar2) { Value = newkhata.OwnerData[i].MobileNumber},
                           new OracleParameter("P_NAMEMATCHSCORE", OracleDbType.Decimal) { Value = Convert.ToDecimal(newkhata.OwnerData[i].NameMatchScore) },
                            new OracleParameter("P_VAULTREFNUMBER", OracleDbType.Varchar2) { Value = mainParameters1[i].VaultRefNumber},
                             new OracleParameter("P_AADHAARHASH", OracleDbType.Varchar2) { Value = mainParameters1[i].AadhaarHash},


        });

            };
            parametersMap.Add(procedures[7], ownerParameters);
            var kaveriDocParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
               new OracleParameter("P_REGISTRATIONNUMBER", OracleDbType.Varchar2) { Value = newkhata.KaveriDeedInformation.DeedNumber },
                new OracleParameter("P_KaveriDeedInfoRawAPIResponseJSON", OracleDbType.Clob) { Value = newkhata.KaveriDeedInformation.KaveriDeedInfoRawAPIResponseJSON },
                 new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                  new OracleParameter("P_NATUREDEED", OracleDbType.Varchar2) { Value = documentDetails.naturedeed },
                   new OracleParameter("P_APPLICATIONNUMBER", OracleDbType.Varchar2) { Value = documentDetails.applicationnumber },
                    new OracleParameter("P_REGISTRATIONDATETIME", OracleDbType.Varchar2) { Value = documentDetails.registrationdatetime },
                    }
    };
            parametersMap.Add(procedures[8], kaveriDocParameters);

            var kaveriDocPROPERTYDETAILSParameters = new List<OracleParameter[]>();
            for (int i = 0; i < documentDetails.propertyinfo.Count(); i++)

            {
                kaveriDocPROPERTYDETAILSParameters.Add(new OracleParameter[]
    {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
               new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
            new OracleParameter("P_REGISTRATIONNUMBER", OracleDbType.Varchar2) { Value = newkhata.KaveriDeedInformation.DeedNumber },
            new OracleParameter("P_PROPERTYID", OracleDbType.Varchar2) { Value = newkhata.EPID },
            new OracleParameter("P_DOCUMENTID", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].documentid },
            new OracleParameter("P_VILLAGENAME", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].villagenamee },
            new OracleParameter("P_SRONAME", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].sroname },
            new OracleParameter("P_HOBLINAME", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].hobli },
            new OracleParameter("P_ZONENAME", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].zonenamee },
            new OracleParameter("P_PROPERTYTYPE", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].propertytype },
            //new OracleParameter("P_TOTALAREA", OracleDbType.Varchar2) { Value = documentDetails.propertyinfo[i].are },
           
         
           
        });

            };
            parametersMap.Add(procedures[17], kaveriDocPROPERTYDETAILSParameters);
        
            var kaveriDocPARTIESParameters = new List<OracleParameter[]>();
            for (int i = 0; i < documentDetails.partyinfo.Count(); i++)
            {
                kaveriDocPARTIESParameters.Add(new OracleParameter[]
{
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
               new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                new OracleParameter("P_REGISTRATIONNUMBER", OracleDbType.Clob) { Value = newkhata.KaveriDeedInformation.MainDeedNumber },
                 new OracleParameter("P_PARTYNAME", OracleDbType.Varchar2) { Value = documentDetails.partyinfo[i].partyname },
                   new OracleParameter("P_IDPROOFTYPE", OracleDbType.Varchar2) { Value = documentDetails.partyinfo[i].idprooftypedesc },
                    new OracleParameter("P_IDPROOFNUMBER", OracleDbType.Varchar2) { Value = documentDetails.partyinfo[i].idproofnumber },
                 new OracleParameter("P_PARTYTYPE", OracleDbType.Varchar2) { Value = documentDetails.partyinfo[i].partytypename},
             new OracleParameter("P_ADMISSIONDATE", OracleDbType.Varchar2) { Value = documentDetails.partyinfo[i].admissiondate },
             new OracleParameter("P_EKYC_OWNERNAME", OracleDbType.Varchar2) { Value = documentDetails.partyinfo[i].EkycOwnerName },
                 
                      
                            });
    };
            parametersMap.Add(procedures[18], kaveriDocPARTIESParameters);


            var kaveriECParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
                new OracleParameter("P_ECNUMBER", OracleDbType.Varchar2) { Value = newkhata.KaveriECInformation.EncumbranceCertificate },
                 new OracleParameter("P_KaveriECInfoRawAPIResponseJSON", OracleDbType.Clob) { Value = newkhata.KaveriECInformation.KaveriECInfoRawAPIResponseJSON },
                 new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                new OracleParameter("P_REGISTRATIONNUMBER", OracleDbType.Varchar2) { Value =newkhata.KaveriECInformation.DeedNumber},
               new OracleParameter("P_EXECUTIONDATE", OracleDbType.Varchar2) { Value = ECdocumentDetails.ExecutionDate },
             new OracleParameter("P_FROMDATE", OracleDbType.Varchar2) { Value =  fromdate  },
          new OracleParameter("P_TODATE", OracleDbType.Varchar2) { Value =  todate  }
   
        }
    };
            parametersMap.Add(procedures[9], kaveriECParameters);
            var kaveriECClaimantsOwnerParameters = new List<OracleParameter[]>();
            foreach (var claim in ECdocumentDetails.Claimants)
            {
                {
                    kaveriECClaimantsOwnerParameters.Add(new OracleParameter[]
                   {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
               
               
                 new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                new OracleParameter("P_REGISTRATIONNUMBER", OracleDbType.Varchar2) { Value =newkhata.KaveriECInformation.DeedNumber},
          new OracleParameter("P_OWNERNAME", OracleDbType.Char) { Value = claim},
  new OracleParameter("P_ISCLAIMANTOREXECUTANT", OracleDbType.Char) { Value = "C" },
       
                    });
                    }
    };
            parametersMap.Add(procedures[19], kaveriECClaimantsOwnerParameters);



            var kaveriECEXECUTANTSOwnerParameters = new List<OracleParameter[]>();
            foreach (var EXEC in ECdocumentDetails.Executants)
            {
                {
                    kaveriECClaimantsOwnerParameters.Add(new OracleParameter[]
                   {
            new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
               
                 new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                new OracleParameter("P_REGISTRATIONNUMBER", OracleDbType.Varchar2) { Value =newkhata.KaveriECInformation.DeedNumber},
                  new OracleParameter("P_OWNERNAME", OracleDbType.Char) { Value = EXEC },
  new OracleParameter("P_ISCLAIMANTOREXECUTANT", OracleDbType.Char) { Value = "E" },
        
                    });
                }
            };
            parametersMap.Add(procedures[20], kaveriECEXECUTANTSOwnerParameters);

            if (newkhata.IrregularDimensionDetails != null)
            {
                var oddSiteParameters = new List<OracleParameter[]>();
                foreach (var IrregularDimensionDetails in newkhata.IrregularDimensionDetails)
                {
                    oddSiteParameters.Add(new OracleParameter[]
                    {
                 new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
                  new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                      new OracleParameter("P_SIDENUMBER", OracleDbType.Varchar2) { Value = IrregularDimensionDetails.Position },
                          new OracleParameter("P_SideFacingLenft", OracleDbType.Varchar2) { Value =IrregularDimensionDetails.SideFacingLenft },
                          new OracleParameter("P_SideFacingLenMt", OracleDbType.Varchar2) { Value = IrregularDimensionDetails.SideFacingLenMt },

                    });
                }
                parametersMap.Add(procedures[10], oddSiteParameters);
            }
            var OverallRecomendParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
       new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
                  new OracleParameter("P_Status", OracleDbType.Varchar2) { Value = newkhata.OverallRecommendation.Status },
                      new OracleParameter("P_StatusId", OracleDbType.Varchar2) { Value = newkhata.OverallRecommendation.StatusId },
                          new OracleParameter("P_StatusReason", OracleDbType.Clob) { Value =  newkhata.OverallRecommendation.StatusReason },
                             new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value =  newkhata.LoginInformation.UserMobileNumber }
        }
    };
            parametersMap.Add(procedures[11], OverallRecomendParameters);
            if (newkhata.PropertyDetails.MasterPropertyTypeId == 3)
            {
                var AppartmentParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
   new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
                 new OracleParameter("P_PBID_RowId", OracleDbType.Int64) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.PBID_RowId },
new OracleParameter("P_SuperBuiltUpAreaSqFt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.SuperBuiltUpAreaSqFt },
new OracleParameter("P_SuperBuiltUpAreaSqMt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.SuperBuiltUpAreaSqMt },
new OracleParameter("P_SuperBuiltUpAreaUnit", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.SuperBuiltUpAreaUnit },
new OracleParameter("P_CarpetUpAreaSqFt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.CarpetUpAreaSqFt },
new OracleParameter("P_CarpetUpAreaSqMt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.CarpetUpAreaSqMt },
new OracleParameter("P_CarpetUpAreaUnit", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.CarpetUpAreaUnit },
new OracleParameter("P_CommonAreaSqFt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.CommonAreaSqFt },
new OracleParameter("P_CommonAreaSqMt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.CommonAreaSqMt },
new OracleParameter("P_CommonAreaUnit", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.CommonAreaUnit },
new OracleParameter("P_NoOfCarParking", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.NoOfCarParking },
new OracleParameter("P_AreaOfCarParkingSqFt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.AreaOfCarParkingSqFt },
new OracleParameter("P_AreaOfCarParkingSqMT", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.AreaOfCarParkingSqMT },
new OracleParameter("P_TotalAreaOfCarParkingSqFt", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.TotalAreaOfCarParkingSqFt },
new OracleParameter("P_TotalAreaOfCarParkingSqMT", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.TotalAreaOfCarParkingSqMT },
new OracleParameter("P_FlatFloorType", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.FlatFloorType },
new OracleParameter("P_FloorNumber", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.FloorNumber },

new OracleParameter("P_FloorNumberText", OracleDbType.Varchar2) { Value =  newkhata.BuiltUpAreaDetailsForMultiStory.FloorNumberText},

new OracleParameter("P_FlatNo", OracleDbType.Varchar2) { Value = newkhata.BuiltUpAreaDetailsForMultiStory.FlatNo },
  new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
        }
    };
                parametersMap.Add(procedures[12], AppartmentParameters);
            }
            var MatrixParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
          new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
              new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
        }
    };
            parametersMap.Add(procedures[13], MatrixParameters);
            if (newkhata.KRSRegularShapeDetailsRequestParamater != null)
            {
                var RegularDimensionParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
          new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
              new OracleParameter("P_EastWestft", OracleDbType.Decimal) { Value = newkhata.KRSRegularShapeDetailsRequestParamater.EastWestft },
                  new OracleParameter("P_NorthSouthft", OracleDbType.Decimal) { Value = newkhata.KRSRegularShapeDetailsRequestParamater.NorthSouthft },
                      new OracleParameter("P_EastWestMt", OracleDbType.Decimal) { Value = newkhata.KRSRegularShapeDetailsRequestParamater.EastWestMt },
                          new OracleParameter("P_NorthSouthMT", OracleDbType.Decimal) { Value = newkhata.KRSRegularShapeDetailsRequestParamater.NorthSouthMT },
                           new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
        }
    };
                parametersMap.Add(procedures[14], RegularDimensionParameters);
            }
        

            if (newkhata.BWSSBNumber != null)
            {
                var BwssBParameters = new List<OracleParameter[]>();
                foreach (var BwssBDetails in newkhata.BWSSBNumber)
                {
                    BwssBParameters.Add(new OracleParameter[]
                    {
                 new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
                  new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                      new OracleParameter("P_BWSSBNUMBER", OracleDbType.Varchar2) { Value = BwssBDetails.BWSSBNumber },
                          new OracleParameter("P_SASAPPLICATIONNO", OracleDbType.Varchar2) { Value =newkhata.SASNo },
                                   new OracleParameter("C_RECORD1", OracleDbType.RefCursor) ,


                    });
                }
                parametersMap.Add(procedures[15], BwssBParameters);
            }



            var SiteParameters = new List<OracleParameter[]>
    {
        new OracleParameter[]
        {
          new OracleParameter("P_PROPERTYCODE", OracleDbType.Int64) { Value = propertycode },
            new OracleParameter("P_BOOKS_PROP_APPNO", OracleDbType.Int64) { Value = books_app_no },
              new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2) { Value = newkhata.LoginInformation.UserMobileNumber },
                            new OracleParameter("P_KAVERISITEAREA_FT", OracleDbType.Decimal) { Value = newkhata.KaveriDeedInformation.PropAreaSqft },
                                          new OracleParameter("P_KAVERISITEAREA_MT", OracleDbType.Decimal) { Value = newkhata.KaveriDeedInformation.PropAreaSqMt },
        }
    };
            parametersMap.Add(procedures[16], SiteParameters);





            try
            {
                return _databaseService.ExecuteStoredProceduresWithLoop(procedures, parametersMap, "NEW_KHATA_DETAILS.INS_NEW_EKHATA_INSERTION_ERRORLOG",newkhata.EPID,newkhata.KRSId);
            }
            catch (OracleException Ex)
            {
                throw Ex;
            }
        }
    }
}
