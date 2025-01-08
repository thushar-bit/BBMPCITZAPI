namespace BBMPCITZAPI.Models
{
    public class ObjectionModels
    {


        public class INS_NCL_PROPERTY_OBJECTORS_FINAL_SUBMIT
        {

            public int? OBJECTIONID { get; set; }
            public int? PROPERTYCODE { get; set; }
            public byte[]? SCANNEDDOCUMENTOBJECTION { get; set; }
            public byte[]? REASONDOCUMENT  { get; set; }
            public string? ISCOMMUNICATIONADDRESS   { get; set; }
            public int? REASONID { get; set; }
            public string? REASONDETAILS { get; set; }
            public int? ULBCODE  { get; set; }
            public string? NAMEDOCUMENTDETAILS  { get; set; }
            public string? REASONDOCUMENTDETAILS { get; set; }
            public string? DOCUMENTEXTENSION { get; set; }
            public string? DOORNO   { get; set; }
            public string? BUILDINGNAME{ get; set; }
            public string? AREALOCATLITY   { get; set; }
            public string? PINCODE  { get; set; }
            public string? CREATEDBY  { get; set; }
            public string? TYPEOFDOCUMENT { get; set; }


        }

        public class INS_NCL_PROPERTY_SEARCH_FINAL_SUBMIT
        {
            public Int64 Search_Req_Id { get; set; }

            public string? IsHaveAAdhaarNumber { get; set; }
            public string? IsHaveSASNumber { get; set; }
            public string? DoorNo { get; set; }
            public string? BuildingName { get; set; }
            public string? AreaOrLocality { get; set; }
            public string? Pincode { get; set; }
            public byte[]? IDDocument { get; set; }
            public string? IDCardType { get; set; }
            public string? IDCardNumber { get; set; }
            public string? MobileNumber { get; set; }
            public string? Mobiverify { get; set; }
            public string? Email { get; set; }
            public int? ZoneId { get; set; }
            public int? WardId { get; set; }
            public string? SearchName { get; set; }
            public string? SASApplicationNumber { get; set; }
            public string? LoginId { get; set; }
            public string? IsHaveOldEkhata { get; set; }
            public byte[]? OldEkhataDocument { get; set; }
        }
        public class INS_NCL_PROPERTY_MUTATION_OBJECTION_FINAL_SUBMIT
        {
            public Int64 Mutatation_Req_Id { get; set; }
            public string? PropertyEpid { get; set; }

            public string? ObjectionDocumentName{ get; set; }
            public byte[]? ObjectionDocument { get; set; }
            public string? REASONDETAILS { get; set; }
         
            public string? MobileNumber { get; set; }
            public string? Mobiverify { get; set; }
            public string? Email { get; set; }
           
            public string? LoginId { get; set; }
       
        }

    }
}
