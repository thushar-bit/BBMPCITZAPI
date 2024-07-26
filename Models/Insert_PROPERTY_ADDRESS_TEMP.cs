namespace BBMPCITZAPI.Models
{
    public class Insert_PROPERTY_ADDRESS_TEMP
    {
        public  int? propertyCode { get; set; }
        public int? STREETID { get; set; }
        public string? DOORNO { get; set; }
        public string? BUILDINGNAME { get; set; }
        public string? AREAORLOCALITY { get; set; }
        public string? LANDMARK { get; set; }
        public string? PINCODE { get; set; }
        public byte[]? PROPERTYPHOTO { get; set; }
        public int? categoryId { get; set; }
        public string? PUIDNo { get; set; }
        public string? loginId { get; set; }
        public int? EIDAPPNO { get; set; }
        public string? Latitude { get; set; }   
            public string? Longitude { get; set; }
    }
    public class UPD_NCL_PROPERTY_SITE_DIMENSION_TEMP 
    {
        public int? propertyCode { get; set; }
        public string? SITEORBUILDING { get; set; }
        public string? EVENORODDSITE { get; set; }
         public decimal? SITEAREA { get; set; }
         public decimal? SITEAREAFT { get; set; }
         public decimal? BUILDINGAREA { get; set; }
         public decimal? BUILDINGAREAFT { get; set; }
          public string? EASTWEST { get; set; }
         public string? NORTHSOUTH { get; set; }
         public decimal? EWODDSITE1FT { get; set; }
         public decimal? EWODDSITE2FT { get; set; }
         public decimal? EWODDSITE3FT { get; set; }
         public decimal? EWODDSITE4FT { get; set; }
         public decimal? NSODDSITE1FT { get; set; }
         public decimal? NSODDSITE2FT { get; set; }
         public decimal? NSODDSITE3FT { get; set; }
         public decimal? NSODDSITE4FT { get; set; }
        public string? loginId { get; set; }
        public int? EIDAPPNO { get; set; }  
    }
    public class UPD_NCL_PROPERTY_MAIN_TEMP_CHECKBANDI
    {
       public int? propertyCode { get; set; }
        public string? CHECKBANDI_NORTH { get; set; }
        public string? CHECKBANDI_SOUTH { get; set; }
        public string? CHECKBANDI_EAST { get; set; }
        public string? CHECKBANDI_WEST { get; set; }
        public string? loginId { get; set; }
        public int? EIDAPPNO { get; set; }
    }
    public class UPD_NCL_PROPERTY_APARTMENT_TEMP_AREA 
    {
        public int? propertyCode { get; set; }
        public decimal? CARPETAREA { get; set; }
        public decimal? ADDITIONALAREA { get; set; }
        public decimal? SUPERBUILTUPAREA { get; set; }
        public string? loginId { get; set; }
        public int? EIDAPPNO { get; set; }
    }
    public class UPD_NCL_PROPERTY_SITE_TEMP_USAGE 
    {
        public int? propertyCode { get; set; }
        public int? FEATUREHEADID { get; set; }
        public int? FEATUREID { get; set; }
        public int? BUILTYEAR { get; set; }
        public string? loginId { get; set; }
        public int? EIDAPPNO { get; set; }
    }
    public class NCLBuilding
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public int? FLOORNUMBERID { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public int? BUILDINGUSAGETYPEID { get; set; }// NUMBER;
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public int? WOODTYPEID { get; set; }// NUMBER;
        public decimal? AREA { get; set; }// NUMBER;
        public int? PROPERTYCODE { get; set; }//NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public int? FLOORTYPEID { get; set; }// NUMBER;
        public int? FLOORID { get; set; }// NUMBER;
        public int? ROOFTYPEID { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        public int? FEATUREHEADID { get; set; }// NUMBER;
        public int? FEATUREID { get; set; }// NUMBER;
        public int? BUILTYEAR { get; set; }// NUMBER;
        public int? BUILDINGTYPEID { get; set; }// NUMBER;
        public int? DEMOLISHED { get; set; }// NUMBER;
        public string? RRNO { get; set; }// VARCHAR2(50);
        public string? WATERMETERNO { get; set; }// VARCHAR2(50);
        public decimal? PLOTAREAOWNERSHARE_NOS { get; set; }// NUMBER;
        public decimal? PLOTAREAOWNERSHARE_AREA { get; set; }// NUMBER;
        public decimal? PLOTAREAOWNERSHARE_FRACTION { get; set; }// NUMBER;
        public string? ISWATERMETERNOAVAILABLE { get; set; }
        public int? BUILDINGNUMBERID { get; set; }// NUMBER;
        public int? BUILDINGBLOCKID { get; set; }// NUMBER;
        public string? BUILDINGBLOCKNAME { get; set; }
        public string? CREATEDIP { get; set; }
        public decimal? ownUseArea { get; set; }
        public decimal? rentedArea { get; set; }
        public int? EIDAPPNO { get; set; }

    }
    public class NCLAPARTMENT
    {

        public decimal? PLOTAREAOWNERSHARE_AREA { get; set; }// NUMBER;
        public decimal? SUPERBUILTUPAREA { get; set; }// NUMBER;
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public decimal ADDITIONALAREA { get; set; }// NUMBER;
        public decimal PLOTAREAOWNERSHARE_NOS { get; set; }// NUMBER;
        public decimal CARPETAREA { get; set; }// NUMBER;
        public int? APARTMENTID { get; set; }// NUMBER;
        public DateTime? UPDATEDON { get; set; }// DATE;
        public decimal? PLOTAREAOWNERSHARE_FRACTION { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public char? PARKINGAVAILABLE { get; set; }// CHAR(1);
        public int? PARKINGUNITS { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public string? BLOCKNUMBER { get; set; }// VARCHAR2(20);
        public string? FLATNO { get; set; }// VARCHAR2(20);
        public decimal? PARKINGAREA { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        //--------added by SOORAJ 24/oct/2016
        public int? FLOORTYPEID { get; set; }// NUMBER;
        public int? FEATUREHEADID { get; set; }// NUMBER;
        public int? FEATUREID { get; set; }// NUMBER;
        public int? ROOFTYPEID { get; set; }// NUMBER;
        public int? WOODTYPEID { get; set; }// NUMBER;
        public string? RRNO { get; set; }// VARCHAR2(250);
        public int? BUILDINGTYPEID { get; set; }//NUMBER, 
        public int? YEAROFCONSTRUCTION { get; set; }//NUMBER
        public int? FLOORNUMBERID { get; set; }// NUMBER;
        public int? BUILDINGUSAGETYPEID { get; set; }//NUMBER, 
        public int? BHKID { get; set; } //number
        public string? CREATEDIP { get; set; }
        public int? EIDAPPNO { get; set; }

    }
    public class NCLPropOwner
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public int? OWNERNUMBER { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public string? MOBILENUMBER { get; set; }// VARCHAR2(10);
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public string? OWNERNAME { get; set; }// VARCHAR2(500);
        public string? IDENTIFIERNAME { get; set; }// VARCHAR2(20);
        public int? IDENTIFIERTYPE { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        public string? OWNERADDRESS { get; set; }// VARCHAR2(1000);
        public int? CITIZENID { get; set; }// NUMBER;
        public string? COMPANYNAME { get; set; }
        public string? ISCOMPANY { get; set; }
        public int? PROPERTYTYPE { get; set; }
        public string? CREATEDIP { get; set; }
        public int? EIDAPPNO { get; set; }
    }
    public class NCLPropOwnerID
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public int? OWNERNUMBER { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public string? OWNERIDENTITYID { get; set; }// NUMBER;
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public string? OWNERIDENTITYSLNO { get; set; }// VARCHAR2(100);
        public int? IDENTITYTYPEID { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        public string? AADHAARNO { get; set; }// VARCHAR2(20);
        public string? ENCRYPT { get; set; }// VARCHAR2(3);
        public int? EIDAPPNO { get; set; }
    }
    public class NCLPropOwnerIDDoc
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public int? OWNERNUMBER { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public int? OWNERIDENTITYID { get; set; }// NUMBER;
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public byte[]? SCANNEDIDENEITY { get; set; }// BFILE;
        public int? EIDAPPNO { get; set; }

    }
    public class NCLPropOwnerPhoto
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public int? OWNERNUMBER { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public byte[]? OWNERPHOTO { get; set; }// BFILE;
        public int? PHOTOID { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        public string? UPDATEDBYE { get; set; }// VARCHAR2(20);
        public int? EIDAPPNO { get; set; }
    }
    public class NCLPropRights
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public int? PROPERTYRIGHTSID { get; set; }// NUMBER;
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public string? RIGHTS { get; set; }// VARCHAR2(2000);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public int? ULBCODE { get; set; }// NUMBER;

        public int? EIDAPPNO { get; set; }
    }
    public class NCLPropIdentification
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public string? ORDERNUMBER { get; set; }// VARCHAR2(200);
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public string? UPDATEDBY { get; set; }// VARCHAR2(20);
        public string? DOCUMENTEXTENSION { get; set; }// VARCHAR2(100);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public DateTime? CREATEDON { get; set; }// DATE;
        public string? DOCUMENTDETAILS { get; set; }// VARCHAR2(2000);
        public byte[]? SCANNEDDOCUMENT { get; set; }// BFILE;
        public int? DOCUMENTID { get; set; }// NUMBER;
        public DateTime? ORDERDATE { get; set; }// DATE;
        public int? DOCUMENTTYPEID { get; set; }// NUMBER;
        public int? ULBCODE { get; set; }// NUMBER;
        public int? MUTAPPLID { get; set; }// NUMBER;
        public string? CREATEDIP { get; set; }
        public int? EIDAPPNO { get; set; }
    }
    public class NCLClassPropIdentification
    {
        public DateTime? UPDATEDON { get; set; }// DATE;
        public string? DOCUMENTNUMBER { get; set; }// VARCHAR2(200);
        public string? CREATEDBY { get; set; }// VARCHAR2(20);
        public string? DOCUMENTEXTENSION { get; set; }// VARCHAR2(100);
        public int? PROPERTYCODE { get; set; }// NUMBER;
        public string? DOCUMENTDETAILS { get; set; }// VARCHAR2(2000);
       public byte[]? SCANNEDDOCUMENT { get; set; }// BFILE;
        public int? DOCUMENTID { get; set; }// NUMBER;
        public DateTime? DOCUMENTDATE { get; set; }// DATE;
        public int? DOCUMENTTYPEID { get; set; }// NUMBER;
        public int? CLASSIFICATIONID { get; set; }
        public int? SUBCLASSIFICATIONID { get; set; }

        public string? CREATEDIP { get; set; }
        public int? EIDAPPNO { get; set; }
    }

}
