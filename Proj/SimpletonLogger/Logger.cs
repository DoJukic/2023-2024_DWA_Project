using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RESTful_Service_Module.Systems
{
    public class Logger
    {
        public class Log
        {
            private static readonly JsonSerializerOptions logSerializationOptions = new(JsonSerializerDefaults.General)
            {
                Converters =
                {
                    TooManyUtils.IsoDateTimeOffsetConverter.Singleton
                },
            };

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            [JsonPropertyName("id")]
            public String? ID { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            [JsonPropertyName("info")]
            public String? Info { get; set; }

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            [JsonPropertyName("date_time_offset")]
            public DateTimeOffset? DateTime { get; set; }

            public static Log? FromJson(string json) => JsonSerializer.Deserialize<Log>(json, logSerializationOptions);
            public static string ToJson(Log obj) => JsonSerializer.Serialize(obj);
        }

        public class LogList
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            [JsonPropertyName("logs")]
            public List<Log>? logs { get; set; }

            public static LogList? FromJson(string json) => JsonSerializer.Deserialize<LogList>(json);
            public static string ToJson(LogList obj) => JsonSerializer.Serialize(obj);
        }

        public void AddLog(string info)
        {

        }

        public void GetLogOptions()
        {

        }
    }
}
