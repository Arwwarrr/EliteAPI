namespace EliteAPI.Events
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ShipyardInfo
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("MarketID")]
        public long MarketId { get; set; }

        [JsonProperty("StationName")]
        public string StationName { get; set; }

        [JsonProperty("StarSystem")]
        public string StarSystem { get; set; }
    }

    public partial class ShipyardInfo
    {
        public static ShipyardInfo Process(string json, EliteDangerousAPI api) => api.Events.InvokeShipyardEvent(JsonConvert.DeserializeObject<ShipyardInfo>(json, EliteAPI.Events.ShipyardConverter.Settings));
    }

    public static class ShipyardSerializer
    {
        public static string ToJson(this ShipyardInfo self) => JsonConvert.SerializeObject(self, EliteAPI.Events.ShipyardConverter.Settings);
    }

    internal static class ShipyardConverter
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
