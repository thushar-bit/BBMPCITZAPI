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


namespace BBMPCITZAPI.Services
{
    public class BBMPBookModuleService : IBBMPBookModuleService
    {

        private readonly ILogger<BBMPCITZController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseService _databaseService;

        public BBMPBookModuleService(ILogger<BBMPCITZController> logger, IConfiguration configuration, DatabaseService databaseService)
        {
            _logger = logger;
            _configuration = configuration;
            _databaseService = databaseService;
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
        public  int Insert_PROPERTY_ADDRESS_TEMP(Insert_PROPERTY_ADDRESS_TEMP insertCITZProperty) 
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INS_UPD_NCL_PROPERTY_ADDRESS_TEMP";
            OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_STREETID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_DOORNO",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_BUILDINGNAME",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_AREAORLOCALITY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_LANDMARK",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_PINCODE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYPHOTO",OracleDbType.Blob,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYCATEGORYID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_PUID",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

            prm[0].Value = insertCITZProperty.propertyCode;
            prm[1].Value = insertCITZProperty.STREETID;
            prm[2].Value = insertCITZProperty.DOORNO;
            prm[3].Value = insertCITZProperty.BUILDINGNAME;
            prm[4].Value = insertCITZProperty.AREAORLOCALITY;
            prm[5].Value = insertCITZProperty.LANDMARK;
            prm[6].Value = insertCITZProperty.PINCODE;
            prm[7].Value = insertCITZProperty.PROPERTYPHOTO;
            prm[8].Value = insertCITZProperty.categoryId;
            prm[9].Value = insertCITZProperty.PUIDNo;
            prm[10].Value = insertCITZProperty.loginId;

           
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
                prm[17].Value = UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP.loginId;


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
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

                prm[0].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_NORTH;
                prm[2].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_SOUTH;
                prm[3].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_EAST;
                prm[4].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.CHECKBANDI_WEST;
                prm[5].Value = UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI.loginId;


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
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

                prm[0].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.CARPETAREA;
                prm[2].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.ADDITIONALAREA;
                prm[3].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.SUPERBUILTUPAREA;
                prm[4].Value = UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA.loginId;


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
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

                prm[0].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.propertyCode;
                prm[1].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.FEATUREHEADID;
                prm[2].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.FEATUREID;
                prm[3].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.BUILTYEAR;
                prm[4].Value = UPD_NCL_PROPERTY_SITE_TEMP_USAGE.loginId;


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
        public int DEL_SEL_NCL_PROP_BUILDING_TEMP(int ULBCODE, NCLBuilding NCLBLDG)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_SEL_NCL_PROP_BUILDING_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
             new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BULDINGBLOCKID",OracleDbType.Int32,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_FLOORNUMBERID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("CUR_USER",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = NCLBLDG.ULBCODE;
                prm[1].Value = NCLBLDG.PROPERTYCODE;
                prm[2].Value = NCLBLDG.BUILDINGNUMBERID;
                prm[3].Value = NCLBLDG.FLOORNUMBERID;
                prm[4].Value = null;


                return _databaseService.ExecuteNonQuery(sp_name, prm);
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
                string sp_name = "NCL.GET_NCL_TEMP_FLOOR_AREA";
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
             new OracleParameter("P_FEATUREID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_OWNUSAGEAREA",OracleDbType.Decimal,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_WOODTYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FEATUREHEADID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FLOORTYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_FLOORNUMBERID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BUILDINGUSAGETYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_ROOFTYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_ULBCODE",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BUILTYEAR",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_BUILDINGTYPEID",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("P_DEMOLISHED",OracleDbType.Int32,ParameterDirection.Input),// NUMBER;
             new OracleParameter("CUR_USER",OracleDbType.RefCursor,ParameterDirection.Output),// NUPMS.NCL_TEMP.t_cursor;
             new OracleParameter("P_RRNO",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_WATERMETERNO",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_ISWATERMETERNOAVAILABLE",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);             
             new OracleParameter("P_BULDINGBLOCKID",OracleDbType.Int32,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_BULDINGBLOCKNAME",OracleDbType.Varchar2,ParameterDirection.Input),// VARCHAR2(20);
             new OracleParameter("P_RENTEDAREA",OracleDbType.Decimal,ParameterDirection.Input),
             new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,ParameterDirection.Input)
                    };

                prm[0].Value = NCLBLDG.FEATUREID;
                prm[1].Value = NCLBLDG.PROPERTYCODE;
                prm[2].Value = NCLBLDG.ownUseArea;
                prm[3].Value = NCLBLDG.WOODTYPEID;
                prm[4].Value = NCLBLDG.FEATUREHEADID;
                prm[5].Value = NCLBLDG.FLOORTYPEID;
                prm[6].Value = NCLBLDG.FLOORNUMBERID;
                prm[7].Value = NCLBLDG.BUILDINGUSAGETYPEID;
                prm[8].Value = NCLBLDG.CREATEDBY;
                prm[9].Value = NCLBLDG.ROOFTYPEID;
                prm[10].Value = 0;
                prm[11].Value = NCLBLDG.ULBCODE;
                prm[12].Value = NCLBLDG.BUILTYEAR;
                prm[13].Value = NCLBLDG.BUILDINGTYPEID;
                prm[14].Value = NCLBLDG.DEMOLISHED;
                prm[15].Value = null;
                prm[16].Value = NCLBLDG.RRNO;
                prm[17].Value = NCLBLDG.WATERMETERNO;
                prm[18].Value = NCLBLDG.ISWATERMETERNOAVAILABLE;
                prm[19].Value = NCLBLDG.BUILDINGNUMBERID;
                prm[20].Value = NCLBLDG.BUILDINGBLOCKNAME;
                prm[21].Value = NCLBLDG.rentedArea;
                prm[22].Value = NCLBLDG.CREATEDIP;


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
                      new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input)
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
        public DataSet COPY_OWNER_FROM_BBDDRAFT_NCLTEMP(int propertyCode, int ownerNumber)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.COPY_OWNER_FROM_BBDDRAFT_NCLTEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("C_OWNERRECORD1",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_OWNERRECORD2",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = propertyCode;
                prm[1].Value = ownerNumber;

                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet DEL_SEL_NCL_PROP_OWNER_TEMP(int propertyCode, int ownerNumber)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_SEL_NCL_PROP_OWNER_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("C_OWNERRECORD1",OracleDbType.RefCursor,ParameterDirection.Output),
              new OracleParameter("C_OWNERRECORD2",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = propertyCode;
                prm[1].Value = ownerNumber;

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
                  new OracleParameter("P_OWNERNUMBER",OracleDbType.Int32),// NUPMS.NCL_TEMP.T_CURSOR;
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
                string sp_name = "NCL_TEMP.NCL_PROPERTY_RIGHTS_TEMP_INS";

                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
              new OracleParameter("P_RIGHTS",OracleDbType.Varchar2,2000),// VARCHAR2(2000);
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
              new OracleParameter("CUR_USER",OracleDbType.RefCursor)// SYS_REFCURSOR;
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
                prm[5].Direction = ParameterDirection.Output;
                prm[5].Value = null;


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
                string sp_name = "NCL_TEMP.NCL_PROPERTY_RIGHTS_TEMP_UPD";

                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,20),// VARCHAR2(20);
              new OracleParameter("P_RIGHTS",OracleDbType.Varchar2,2000),// VARCHAR2(2000);
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
              new OracleParameter("CUR_USER",OracleDbType.RefCursor),// DATE;
              new OracleParameter("P_PROPERTYRIGHTSID",OracleDbType.Int32),// NUMBER;
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
                prm[5].Direction = ParameterDirection.Output;
                prm[5].Value = null;
                prm[6].Direction = ParameterDirection.Input;
                prm[6].Value = NCLPropRight.PROPERTYRIGHTSID;


                return _databaseService.ExecuteNonQuery(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public int NCL_PROPERTY_RIGHTS_TEMP_DEL(int RIGHTSID, int ID_BASIC_PROPERTY, int ULBCODE, int PROPERTYCODE)
        {
            try
            {
                string sp_name = "CITIZEN.NCL_PROPERTY_RIGHTS_TEMP_DEL";

                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;              
              new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;              
              new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
              new OracleParameter("CUR_USER",OracleDbType.RefCursor),// T_CURSOR;
              new OracleParameter("P_PROPERTYRIGHTSID",OracleDbType.Int32)// NUMBER;
            };

                prm[0].Value = PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = ULBCODE;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Direction = ParameterDirection.Input;
                prm[2].Value = ID_BASIC_PROPERTY;
                prm[3].Direction = ParameterDirection.Output;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = RIGHTSID;


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
        public int InsertNCLPropertyID( int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID)
        {
            try
            {
                string sp_name = "CITIZEN.NCL_PROPERTY_ID_TEMP_INS";

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
                         new OracleParameter("P_CREATEDIP",OracleDbType.Varchar2,25),
                         new OracleParameter("CUR_USER",OracleDbType.RefCursor)// SYS_REFCURSOR;
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
                prm[10].Value = NCLPropID.CREATEDIP;
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
                string sp_name = "CITIZEN.GET_DOC_VIEW";

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
        public DataSet DeleteToNclPropIdTemp(int ID_BASIC_PROPERTY, NCLPropIdentification NCLPropID)
        {
            try
            {
                string sp_name = "CITIZEN.NCL_PROPERTY_ID_TEMP_DEL";

                OracleParameter[] prm = new OracleParameter[] {
                        new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_ID_BASIC_PROPERTY",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("P_ULBCODE",OracleDbType.Int32),// NUMBER;
                        new OracleParameter("CUR_USER",OracleDbType.RefCursor),// NUPMS.NCL_TEMP.t_cursor;
                        new OracleParameter("P_DOCUMENTID",OracleDbType.Int32)// NUMBER;
                    };

                prm[0].Value = NCLPropID.PROPERTYCODE;
                prm[0].Direction = ParameterDirection.Input;
                prm[1].Value = ID_BASIC_PROPERTY;
                prm[1].Direction = ParameterDirection.Input;
                prm[2].Value = NCLPropID.ULBCODE;
                prm[2].Direction = ParameterDirection.Input;
                prm[3].Direction = ParameterDirection.Output;
                prm[3].Value = null;
                prm[4].Direction = ParameterDirection.Input;
                prm[4].Value = NCLPropID.DOCUMENTID;

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
                new OracleParameter("C_RECORD1",OracleDbType.RefCursor,ParameterDirection.Output)
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
        public DataSet INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTTYPEID, int CLASSIFICATIONID, int SUBCLASSIFICATIONID, string DOCUMENTDETAILS, string DOCUMENTNUMBER, string DOCUMENTDATE, string DOCUMENTEXTENSION, byte[] SCANNEDDOCUMENT, string CREATEDBY)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.INS_NCL_PROPERTY_DOC_BBD_CLASS_TEMP";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCODE",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTTYPEID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_CLASSIFICATIONID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_SUBCLASSIFICATIONID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTDETAILS",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTNUMBER",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTDATE",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_DOCUMENTEXTENSION",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("P_SCANNEDDOCUMENT",OracleDbType.Blob,ParameterDirection.Input),
              new OracleParameter("P_CREATEDBY",OracleDbType.Varchar2,ParameterDirection.Input),
              new OracleParameter("CUR_DOCUMENTS",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = PROPERTYCODE;
                prm[1].Value = DOCUMENTTYPEID;
                prm[2].Value = CLASSIFICATIONID;
                prm[3].Value = SUBCLASSIFICATIONID;
                prm[4].Value = DOCUMENTDETAILS;
                prm[5].Value = DOCUMENTNUMBER;
                prm[6].Value = DOCUMENTDATE;
                prm[7].Value = DOCUMENTEXTENSION;
                prm[8].Value = SCANNEDDOCUMENT;
                prm[9].Value = CREATEDBY;


                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP(int PROPERTYCODE, int DOCUMENTROWID)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.DEL_NCL_PROPERTY_DOC_BBD_CLASS_TEMP";
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
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
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
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }
        public DataSet GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS(int CLASSIFICATIONID, int SUBCLASSIFICATIONID1, int SUBCLASSIFICATIONID2)
        {
            try
            {
                string sp_name = "OBJECTIONMODULE.GET_NPM_MST_CLASS_DOCUMENT_CLASSANDSUBCLASS";
                OracleParameter[] prm = new OracleParameter[] {
              new OracleParameter("P_PROPERTYCLASSIFICATIONID",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYSUBCLASSIFICATIONID1",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("P_PROPERTYSUBCLASSIFICATIONID2",OracleDbType.Int32,ParameterDirection.Input),
              new OracleParameter("CUR_DOCUMENTS",OracleDbType.RefCursor,ParameterDirection.Output)
                    };

                prm[0].Value = CLASSIFICATIONID;
                prm[1].Value = SUBCLASSIFICATIONID1;
                prm[2].Value = SUBCLASSIFICATIONID2;



                return _databaseService.ExecuteDataset(sp_name, prm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
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
                _logger.LogError(ex, "Error occurred while executing OBJECTIONMODULE.Insert_PROPERTY_ADDRESS_TEMP stored procedure.");
                throw;
            }
        }

        #endregion
        #region Tax Events
        public DataSet InsertBBMPPropertyTaxResponse(int UlbCode, string Json, string Response, string IpAddress, string Createdby, string oParamater)
        {
            try
            {
                string sp_name = "CITIZEN.INSERT_BBMP_PROPERTYTAX_RESPONSE";

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

        [DllImport(@"D:\\NameMatchJulyFinal2023.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_GetMatchScore")]
        public static extern float CPlus_NameMatchJulyFinal2023(string n1, string n2);

        public float Fn_CPlus_NameMatchJulyFinal2023(string n1_input, string n2_input)
        {
            float NameMatchPercentageResult = 0;
            try
            {
                NameMatchPercentageResult = CPlus_NameMatchJulyFinal2023(n1_input, n2_input);
            }
            catch (Exception ex)
            {
                NameMatchPercentageResult = 0;
            }
            return NameMatchPercentageResult;
        }
    }
}
