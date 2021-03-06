using EliteAPI.Event.Models.Abstractions;
using Newtonsoft.Json;

namespace EliteAPI.Event.Models
{
    public class InterdictionEvent : EventBase
    {
        internal InterdictionEvent() { }

        public static InterdictionEvent FromJson(string json) => JsonConvert.DeserializeObject<InterdictionEvent>(json);


        [JsonProperty("Success")]
        public bool Success { get; internal set; }

        [JsonProperty("IsPlayer")]
        public bool IsPlayer { get; internal set; }

        [JsonProperty("Interdicted")]
        public string Interdicted { get; internal set; }

        [JsonProperty("Faction")]
        public string Faction { get; internal set; }

        [JsonProperty("Power")]
        public string Power { get; internal set; }

        
    }
}