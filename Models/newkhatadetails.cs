namespace BBMPCITZAPI.Models
{
    public class newkhatadetails
    {
       
       
            public int KRSId { get; set; }
            public string SASNo { get; set; }
            public string EPID { get; set; }
            public string KhataType { get; set; }
            public string KhataTypeId { get; set; }
           public int HasPropertyTaxPaidButNoKhata { get; set; }

            public string ApplicationType { get; set; }

            public string PropertyTypeGovtOrPrivate { get; set; }
            public string DimensionType { get; set; }

            public string SourceOfAPP { get; set; }
            public LoginInformation LoginInformation { get; set; }
            public PropertyDetails1 PropertyDetails { get; set; }
            public PropertyAddress PropertyAddress { get; set; }
            public TaxInfo TaxInfo { get; set; }
            public List<OwnerData> OwnerData { get; set; }
            public KaveriDeedInformation KaveriDeedInformation { get; set; }
            public KaveriECInformation KaveriECInformation { get; set; }
            public List<BescomDetails> BescomDetails { get; set; }
            public AppartmentDetails BuiltUpAreaDetailsForMultiStory { get; set; }
            public List<UploadedDocument> UploadedDocumentsToProveAKahata { get; set; }
            public OverallRecommendation OverallRecommendation { get; set; }
            public RegularShape KRSRegularShapeDetailsRequestParamater { get; set; }
            public List<IrregularDimensionDetail> IrregularDimensionDetails { get; set; }
            public List<BWSSBNumber1> BWSSBNumber { get; set; }
            
        
    }
    public class LoginInformation
    {
        public string UserMobileNumber { get; set; }
    }

    public class PropertyDetails1
    {
        public int MasterPropertyCategoryId { get; set; }
        public string MasterPropertyCategoryName { get; set; }
        public string PropertyUseTypeName { get; set; }
        public string PropertyUseTypeId { get; set; }
        public string BDAPropertyExist { get; set; }
        public string BBMPZoneId { get; set; }
        public string BBMPWardNumber { get; set; }
        public string BBMPWardId { get; set; }
        public string BBMPStreet { get; set; }
        public string BBMPStreetId { get; set; }
        public string EaasthiZoneId { get; set; }
        public string EaasthiWardId { get; set; }
        public string EaasthiWardNumber { get; set; }
        public string EaasthiWardName { get; set; }
        public string EaasthiStreetName { get; set; }
        public string EaasthiStreetId { get; set; }
        public string EaasthiStreetNumber { get; set; }
        public string PropStreetId { get; set; }
        public string AdditionalStreet { get; set; }
        public string PropertyPhotoURL { get; set; }
        public byte[] PropertyPhotoBase64 { get; set; }
    }

    public class PropertyAddress
    {
        public string AdditionalStreet { get; set; }
        public string DoorOrPlotNo { get; set; }
        public string BuildingOrLand { get; set; }
        public string LandMark { get; set; }
        public string PinCode { get; set; }
        public string Locality { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class TaxInfo
    {
        public string Ulbcode { get; set; }
        public string Propertyid { get; set; }
        public List<object> Tax { get; set; }
        public object TaxInfoRAWJSON { get; set; }
    }

    public class EKYCData
    {
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Co { get; set; }
        public string Country { get; set; }
        public string Dist { get; set; }
        public string House { get; set; }
        public string Street { get; set; }
        public string Lm { get; set; }
        public string Loc { get; set; }
        public string Pc { get; set; }
        public string Po { get; set; }
        public string State { get; set; }
        public string Subdist { get; set; }
        public string Vtc { get; set; }
        public string Lang { get; set; }
    }

    public class LocalKYCData
    {
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Co { get; set; }
        public string Country { get; set; }
        public string Dist { get; set; }
        public string House { get; set; }
        public string Street { get; set; }
        public string Lm { get; set; }
        public string Loc { get; set; }
        public string Pc { get; set; }
        public string Po { get; set; }
        public string State { get; set; }
        public string Subdist { get; set; }
        public string Vtc { get; set; }
        public string Lang { get; set; }
    }
    public class KaveriDeedInformation
    {
        public bool IsDeedBefore01042004 { get; set; }
        public string DeedNumber { get; set; }
        public string MainDeedNumber { get; set; }
        public string PropertyDoorSiteNo { get; set; }
        public string Ward { get; set; }
        public string ZoneName { get; set; }
        public string Hobli { get; set; }
        public string SubRegister { get; set; }
        public string RegistrationDistrict { get; set; }
        public string East { get; set; }
        public string West { get; set; }
        public string South { get; set; }
        public string North { get; set; }
        public double PropAreaSqft { get; set; }
        public double PropAreaSqMt { get; set; }
        public double BuildingAreaSqft { get; set; }
        public double BuildingAreaSqMT { get; set; }
        public int PreferenceID { get; set; }
        public string PerferenceDesc { get; set; }
        public string PreferencePropAreaValueInSqFt { get; set; }
        public string PreferencePropAreaValueInSqMt { get; set; }
        public string KaveriDeedInfoRawAPIResponseJSON { get; set; }
        public string KaveriDeedDocumentbase64 { get; set; }
        public byte[]  KaveriDocuemntBase64Before01042004 {get;set;}
    }

    public class KaveriECInformation
    {
        public string EncumbranceCertificateBefore01042004 { get; set; }

        public string EncumbranceCertificate { get; set; }
        public string DeedNumber { get; set; }
        public string   EncumbranceCertificateDocuemntURLBefore01042004 { get; set; }
        public string  EncumbranceCertificateDocuemntURLAfter01042004 { get; set; }
        public byte[] EncumbranceCertificateDocuemntBase64Before01042004 { get; set; }
        public byte[] EncumbranceCertificateDocuemntBase64After01042004 { get; set; }
        public string KaveriECInfoRawAPIResponseJSON { get; set; }
        public string MainDeedNumber { get; set; }
    }

    public class BescomDetails
    {
        public int BescomrowId { get; set; }
        public string MeterNo { get; set; }
        public string EscomName { get; set; }
        public string AccountId { get; set; }
        public string RRNumber { get; set; }
        public string ConsumerName { get; set; }
        public string Type { get; set; }
        public string TrafficType { get; set; }
        public string BescomRawJSON { get; set; }
    }

    public class UploadedDocument
    {
        public int MasterDocumentId { get; set; }
        public int EasthiDocumentId { get; set; }
        public int EasthiMasterSubDocumentId { get; set; }
        public string MasterDocumentText { get; set; }
        public int MasterSubDocumentId { get; set; }
        public string MasterSubDocumentText { get; set; }
        public string DocumentURL { get; set; }
        public byte[] DocumentBase64 { get; set; }
    }

    public class OverallRecommendation
    {
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string StatusReason { get; set; }
    }

    public class IrregularDimensionDetail
    {
        public int DimensionID { get; set; }
        public double SideFacingLenft { get; set; }
        public double SideFacingLenMt { get; set; }
        public int Position { get; set; }
    }

    public class BWSSBNumber1
    {
        public string BWSSBNumber { get; set; }
    }
    public class AppartmentDetails
    {
        public int PBID_RowId { get; set; }
        public string SuperBuiltUpAreaSqFt { get; set; }
        public string SuperBuiltUpAreaSqMt { get; set; }
        public string SuperBuiltUpAreaUnit { get; set; }
        public string CarpetUpAreaSqFt { get; set; }
        public string CarpetUpAreaSqMt { get; set; }
        public string CarpetUpAreaUnit { get; set; }
        public string CommonAreaSqFt { get; set; }
        public string CommonAreaSqMt { get; set; }
        public string CommonAreaUnit { get; set; }
        public string NoOfCarParking { get; set; }
        public string AreaOfCarParkingSqFt { get; set; }
        public string AreaOfCarParkingSqMT { get; set; }
        public string TotalAreaOfCarParkingSqFt { get; set; }
        public string TotalAreaOfCarParkingSqMT { get; set; }
        public string FlatFloorType { get; set; }
        public string FloorNumber { get; set; }
        public string FloorNumberText { get; set; }
        public string FlatNo { get; set; }
     
    }
    public class RegularShape
    {
        public int RegularShapeId { get; set; }
        public decimal EastWestft { get; set; }
        public decimal NorthSouthft { get; set; }
        public decimal EastWestMt { get; set; }
        public decimal NorthSouthMT { get; set; }
        public string AdditionalInfo { get; set; }
    
    }
    public class OwnerData
    {
        public string OwnerName { get; set; }
        public string NameAsInAadhaar { get; set; }
        public string AadhaarNo { get; set; }
        public string AadhaarVeriStatus { get; set; }
        public string NameMatchScore { get; set; }
        public string EKYCJSON { get; set; }
        public string MobileNumber { get; set; }
        //public EKYCData EKYCData { get; set; }
        //public LocalKYCData LocalKYCData { get; set; }
        //public byte[] Photo { get; set; }
    }

}

