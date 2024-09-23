namespace BBMPCITZAPI.Models
{
    public class KaveriData
    {
        public class KAVERI_API_EC_RESPONSE
        {
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
            public List<string> eCdata { get; set; }
        }
        public class ekycdata
        {
            public int OwnerNumber { get; set; }
            public string OwnerName { get; set; }
        }

        public class ECdataItem
        {
            public string applicationNumber { get; set; }
            public string propertyId { get; set; }
            public string marketValue { get; set; }
            public string consideration { get; set; }
            public string articleNamee { get; set; }
            public string east { get; set; }
            public string west { get; set; }
            public string north { get; set; }
            public string south { get; set; }
            public string area { get; set; }
            public string villageCode { get; set; }
            public string villageNamee { get; set; }
            public string hobliCode { get; set; }
            public string hobliNamee { get; set; }
            public string referenceText { get; set; }
            public List<PropNumberDetail> propNumberDetails { get; set; }
            public List<PropertySchedule> propertySchedules { get; set; }
            public List<PartyDetail> partyDetails { get; set; }
            public List<DocumentDetail> documentDetails { get; set; }
            public string correctionNote { get; set; }
            public string liabilityNote { get; set; }
        }

        public class PropNumberDetail
        {
            public string currentPropertyTypeId { get; set; }
            public string currentNumber { get; set; }
        }

        public class PropertySchedule
        {
            public string scheduleid { get; set; }
            public string description { get; set; }
        }

        public class PartyDetail
        {
            public string partyId { get; set; }
            public string partyName { get; set; }
            public string partyTypeId { get; set; }
            public string address { get; set; }
        }

        public class DocumentDetail
        {
            public string documentId { get; set; }
            public string sroCode { get; set; }
            public string executionDate { get; set; }
            public string cdNumber { get; set; }
            public string pageCount { get; set; }
            public string documentReference { get; set; }
        }
        public class ECDataDescription
        {
            public string District { get; set; }
            public string Taluka { get; set; }
            public string HobliOrTown { get; set; }
            public string Village { get; set; }
            public string SurveyNo { get; set; }
        }

        public class EcData
        {
            public List<string> Description { get; set; }
            public string DocumentValuation { get; set; }
            public string ExecutionDate { get; set; }
            public List<string> Executants { get; set; }
            public List<string> Claimants { get; set; }
            public string EkycOwnerName { get; set; }
            public string NameMatchScore { get; set; }
            public string Volume { get; set; }
            public string Book { get; set; }
            public string DocSummary { get; set; }
            public string CrossReference { get; set; }
            public string CorrectionNote { get; set; }
            public string LiabilityNote { get; set; }
        }

        public class EcDocumentDetail
        {
            public string Description { get; set; }
            public string DocumentValuation { get; set; }
            public DateTime ExecutionDate { get; set; }
            public List<string> Executants { get; set; }
            public List<string> Claimants { get; set; }
            public string Volume { get; set; }
            public string Book { get; set; }
            public string DocumentSummary { get; set; }
            public string CrossReference { get; set; }
            public string CorrectionNote { get; set; }
            public string LiabilityNote { get; set; }
        }



        public class KAVERI_API_DOC_DETAILS_RESPONSE
        {
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
            public string json { get; set; }
        }

        public class DocumentDetails
        {
            public string applicationnumber { get; set; }
            public string executedate { get; set; }
            public string pendingdocumentnumber { get; set; }
            public string finalregistrationnumber { get; set; }
            public string registrationdatetime { get; set; }
            public string pagecount { get; set; }
            public string stamparticlename { get; set; }
            public string naturedeed { get; set; }
            public string book { get; set; }
            public List<PropertyInfo> propertyinfo { get; set; }
            public List<PartyInfo> partyinfo { get; set; }
            public List<propertyschedules> propertyschedules { get; set; }
            public List<WitnessInfo> witnessinfo { get; set; }
            public string applicationType { get; set; }
            public string applicationTypeId { get; set; }
            public string presentdate { get; set; }


        }
        public class propertyschedules
        {
            public string propertyschedulespropertyid { get; set; }
            public string scheduletype { get; set; }
            public string totalarea { get; set; }
            public string scheduledescription { get; set; }
            public string scheduleparties { get; set; }
            public string bhoomisellerparty { get; set; }
            public string name { get; set; }
            public string eastboundary { get; set; }
            public string westboundary { get; set; }
            public string northboundary { get; set; }
            public string southboundary { get; set; }
        }

        public class PropertyInfo
        {
            public string applicationnumber { get; set; }
            public string propertyid { get; set; }
            public string documentid { get; set; }
            public string villagenamee { get; set; }
            public string propertytypeid { get; set; }
            public string propertytype { get; set; }
            public string sroname { get; set; }
            public string northboundary { get; set; }
            public string southboundary { get; set; }
            public string eastboundary { get; set; }
            public string westboundary { get; set; }
            public string landmark { get; set; }
            public string consideration { get; set; }
            public string marketvalue { get; set; }
            public string sroconsideration { get; set; }
            public string sromarketvalue { get; set; }
            public string cessduty { get; set; }
            public string govtduty { get; set; }
            public string additionalduty { get; set; }
            public string stampduty { get; set; }
            public string duplicatecopies { get; set; }
            public string duplicatefee { get; set; }
            public string duplicatestampduty { get; set; }
            public string duplicateregistrationfee { get; set; }
            public string valuationreport { get; set; }
            public string sronoofscanpages { get; set; }
            public string hobli { get; set; }
            public string stamparticle { get; set; }
            public string denodescription { get; set; }
            public string estampdescription { get; set; }
            public string adjudescription { get; set; }
            public string zonenamee { get; set; }
            public List<PropertySchedules> propertyschedules { get; set; }
            public List<PropertyNumberDetails> propertynumberdetails { get; set; }
        }

        public class PropertySchedules
        {
            public string propertyschedulespropertyid { get; set; }
            public string scheduletype { get; set; }
            public string totalarea { get; set; }
            public string scheduledescription { get; set; }
            public string scheduleparties { get; set; }
            public string bhoomisellerparty { get; set; }
            public string name { get; set; }
            public string eastboundary { get; set; }
            public string westboundary { get; set; }
            public string northboundary { get; set; }
            public string southboundary { get; set; }
        }

        public class PropertyNumberDetails
        {
            public string propertynumberpropertyid { get; set; }
            public string currentpropertytypeid { get; set; }
            public string propertynumbertype { get; set; }
            public string currentnumber { get; set; }
            public string hissa_no { get; set; }
            public string survey_no { get; set; }
            public string propertynumberdescription { get; set; }
        }

        public class PartyInfo
        {
            public string applicationnumber { get; set; }
            public string partyid { get; set; }
            public string partyname { get; set; }
            public string age { get; set; }
            public string address { get; set; }
            public string profession { get; set; }
            public string phonenumber { get; set; }
            public string epic { get; set; }
            public string partytypeid { get; set; }
            public string partytypename { get; set; }
            public string isexecutor { get; set; }
            public string ispresenter { get; set; }
            public string admissiondate { get; set; }
            public string section88exemption { get; set; }
            public string idprooftypedesc { get; set; }
            public string idproofnumber { get; set; }
            public string relationship { get; set; }
            public string relativename { get; set; }
            public string isorganization { get; set; }
            public string tanno { get; set; }
            public string auname { get; set; }
            public string auaddress { get; set; }
            public string salutation { get; set; }
            public string poaname { get; set; }
            public string minorguardianname { get; set; }
            public string sex { get; set; }
            public string NameMatchScore { get; set; }
            public string EkycOwnerName { get; set; }
        }

        public class WitnessInfo
        {
            public string witnessid { get; set; }
            public string name { get; set; }
            public string houseno { get; set; }
            public string address { get; set; }
            public string pincode { get; set; }
            public string age { get; set; }
            public string profession { get; set; }
            public string sex { get; set; }
            public string relation { get; set; }
            public string relativename { get; set; }
        }
    }
}
