using Newtonsoft.Json;

namespace EliteAPI.Event.Models
{
    public class ItemsUnlocked
    {
        [JsonProperty("Name")]
        public string Name { get; internal set; }

        [JsonProperty("Name_Localised")]
        public string NameLocalised { get; internal set; }
    }
}