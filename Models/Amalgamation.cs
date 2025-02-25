namespace BBMPCITZAPI.Models
{
    public class Amalgamation
    {
        public class AMalGamation_final
        {
            public Int64 PROPERTYCODE { get; set; }

            public Int64 MUTAPPLID { get; set; }
            public string? UGD { get; set; }
            public string? CORNERSITE { get; set; }
            public string? CHECKBANDI_NORTH { get; set; }
            public string? CHECKBANDI_SOUTH { get; set; }
            public string? CHECKBANDI_EAST { get; set; }
            public string? CHECKBANDI_WEST { get; set; }
          
            public string? EASTWEST { get; set; }
            public string? NORTHSOUTH { get; set; }
            public string? ODDSITE { get; set; }
            public decimal? SITEAREA { get; set; }
            public int? PROPERTYCATEGORYID { get; set; }
            public string? SURVEYNO { get; set; }
            public string? ASSESMENTNUMBER { get; set; }
            public string? LoginId { get; set; }
            public byte[]? PROPERTYPHOTO { get; set; }
            public string? AmalOrderNumber { get; set; }
            public DateTime? AmalOrderDate { get; set; }
            public string? VaultRefNumber { get; set; }
            public int UlbCode { get; set; }
            

        }
    }
}
