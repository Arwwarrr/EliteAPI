namespace EliteAPI.Events
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class FuelScoopInfo
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("Scooped")]
        public double Scooped { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }
    }

    public partial class FuelScoopInfo
    {
        public static FuelScoopInfo Process(string json, EliteDangerousAPI api) => api.Events.InvokeFuelScoopEvent(JsonConvert.DeserializeObject<FuelScoopInfo>(json, EliteAPI.Events.FuelScoopConverter.Settings));
    }

    public static class FuelScoopSerializer
    {
        public static string ToJson(this FuelScoopInfo self) => JsonConvert.SerializeObject(self, EliteAPI.Events.FuelScoopConverter.Settings);
    }

    internal static class FuelScoopConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
