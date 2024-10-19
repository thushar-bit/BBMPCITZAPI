using Newtonsoft.Json;

namespace BBMPCITZAPI.Models
{
    public class CustomDateTimeConverter : JsonConverter
    {
        private readonly string _dateFormat = "yyyyMMddTHH:mm:ss";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
            {
                return DateTime.MinValue;
            }

            return DateTime.ParseExact(reader.Value.ToString(), _dateFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime date = (DateTime)value;
            writer.WriteValue(date.ToString(_dateFormat));
        }
    }

}
