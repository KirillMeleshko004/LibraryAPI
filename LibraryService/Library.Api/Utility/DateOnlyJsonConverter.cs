using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.Api.Utility
{
   public class DateOnlyJsonConverter : JsonConverter<DateOnly?>
   {
      public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, 
         JsonSerializerOptions options)
      {
         _ = DateOnly.TryParse(reader.GetString(), out DateOnly res);

         return res;
      }

      public override void Write(Utf8JsonWriter writer, DateOnly? value, 
         JsonSerializerOptions options)
      {
         writer.WriteStringValue(value.ToString());
      }
   }
}